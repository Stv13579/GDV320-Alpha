using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a class that gets attached to the player
/// it allows the player to move around with wasd or arrow keys
/// there are also features to this character controller such as
/// friction, gravity, sliding, jumping, coyote time, head bobbing, and camera shaking
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    PlayerLook lookScript;
    CharacterController cController;

    [SerializeField]
    float gravity;
    [SerializeField]
    float jumpSpeed;
    [SerializeField]
    float acceleration;
    [SerializeField]
    AnimationCurve frictionCurve = AnimationCurve.Linear(0.0f, 0.1f, 1.0f, 1.0f);
    [SerializeField]
    float movementSpeed, maxMovementSpeed = 13.0f, baseMaxMovementSpeed = 13.0f;

    StatModifier.FullStat speed = new StatModifier.FullStat(0.0f);

    [SerializeField]
    float coyoteTime;
    float currentCoyoteTime;

    Vector3 velocity;

    [SerializeField]
    float airStraffMod;

    bool isGrounded = false;

    float headBobTimer = 0.0f;
    float headBobFrequency = 1.0f;
    float headBobAmplitude = 0.02f;
    // the default position of the head
    float headBobNeutral = 0.80f;
    float headBobMinSpeed = 0.1f;
    float headBobBlendSpeed = 4.0f;
    [SerializeField] 
    AnimationCurve headBobBlendCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    float headBobMultiplier = 0.0f;
    Vector3 oldPos;
    Vector3 newPos;

    // sound
    float randIndexTimer = 0.0f;
    AudioManager audioManager;

    // head shaking
    bool isHeadShaking;

    // FOV change
    [SerializeField]
    float initialFOV = 90.0f;
    [SerializeField]
    float increasedFOVMoving = 92.0f;

    // increase movement speed over time
    [SerializeField]
    float moveTime = 0.0f;
    [SerializeField]
    AnimationCurve moveAnimaCurve;

    [SerializeField]
    LayerMask environmentLayer;

    //sliding parameters
    bool willSlideOnSlopes = true;
    [SerializeField]
    float slopeSpeed;
    Vector3 hitPointNormal;
    bool isSliding
    {
        get
        {
            // if player is grounded and the raycast is hitting the ground
            if (cController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2.5f, environmentLayer))
            {
                //store the normal
                hitPointNormal = slopeHit.normal;
                // returns if the slope is greater then slopelimit
                return Vector3.Angle(hitPointNormal, Vector3.up) > cController.slopeLimit;
            }
            else
            {
                return false;
            }
        }
    }
    float maxVeloMagnitude = 1.5f;

    [SerializeField]
    Transform cameraHolder;
    Vector3 cameraHolderTargetPos;
    float cameraHolderTargetAngle;
    float currentCameraHolderAngle;
    [SerializeField]
    float cameraShakePosPunchLerp = 8.0f;
    [SerializeField]
    float cameraShakePosLerp = 16.0f;
    [SerializeField]
    float cameraShakeAnglePunchLerp = 20.0f;
    [SerializeField]
    float cameraShakeAngleLerp = 40.0f;
    // how deep the drop is (camera pos)
    [SerializeField]
    float cameraShakeDrop = 0.1f;
    // how deep the dip is (camera angle)
    [SerializeField]
    float cameraShakeDip = 25.0f;

    [SerializeField]
    float collisionForce = 0.5f;

    bool ableToMove = true;
    public void SetAbleToMove(bool tempAbleToMove) { ableToMove = tempAbleToMove; }

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        lookScript = this.gameObject.GetComponent<PlayerLook>();
        cController = this.gameObject.GetComponent<CharacterController>();
        speed.baseValue = baseMaxMovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        maxMovementSpeed = StatModifier.UpdateValue(speed);
        if (ableToMove)
        {
            PlayerMoving();
            UpdateCameraShake();
        }
    }

    // function for the camera shake when landing
    // lerps the camera pos and camera angle to their targets
    // then lerps from target to zero
    // quick and dirty way of getting smooth bumping
    void UpdateCameraShake()
    {
        cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, cameraHolderTargetPos, cameraShakePosLerp * Time.deltaTime);
        currentCameraHolderAngle = Mathf.Lerp(currentCameraHolderAngle, cameraHolderTargetAngle, cameraShakeAngleLerp * Time.deltaTime);

        cameraHolderTargetPos = Vector3.Lerp(cameraHolderTargetPos, Vector3.zero, cameraShakePosPunchLerp * Time.deltaTime);
        cameraHolderTargetAngle = Mathf.Lerp(cameraHolderTargetAngle, 0.0f, cameraShakeAnglePunchLerp * Time.deltaTime);

        lookScript.SetBumpTilt(currentCameraHolderAngle);
    }

    // manages all of the player movement e.g. jumping, coyote time, sliding, friction, gravity
    void PlayerMoving()
    {
        // reads players input 
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // converting the players input into a vector 3 and timings it by the players look direction
        Vector3 inputMove = new Vector3(x, 0.0f, z);
        Vector3 realMove = Quaternion.Euler(0.0f, lookScript.GetSpin(), 0.0f) * inputMove;
        realMove.Normalize();

        float tempAirStraffMod = 1.0f;
        // when jumping and moving to that direction
        if (!isGrounded)
        {
            tempAirStraffMod = airStraffMod;
            isHeadShaking = true;
        }

        // if on a slope bigger then the slope limit
        // push play off the slope
        if (willSlideOnSlopes && isSliding)
        {
            velocity += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z).normalized * slopeSpeed;
            //isGrounded = false;
        }

        // as the player gains velo when coming off the ramp
        // i want to cap the velocity so that the player doesnt go flying
        // so i apply friction to the player
        if(velocity.magnitude > maxVeloMagnitude)
        {
            velocity -= velocity.normalized * tempAirStraffMod * acceleration * frictionCurve.Evaluate(velocity.magnitude) * Time.deltaTime;
        }


        // friction
        // we store the y velocity
        float cacheY = velocity.y;
        // set the y velocity to be 0
        velocity.y = 0;
        // movement for the player
        velocity += realMove * tempAirStraffMod * acceleration * Time.deltaTime;
        // friction for the players x and z axis
        velocity -= velocity.normalized * tempAirStraffMod * acceleration * frictionCurve.Evaluate(velocity.magnitude) * Time.deltaTime;
        // we give back the y velocity
        velocity.y = cacheY;

        Jumping();

        // gravity on the player
        velocity.y -= gravity * Time.deltaTime;
        // getting the position before the player moves (headbobbing)
        oldPos = transform.position;
        // moving the player on screen
        cController.Move(velocity * movementSpeed * Time.deltaTime);
        // getting the position after the player moves (headbobbing)
        newPos = transform.position;

        // collision when touching the roof
        if ((cController.collisionFlags & CollisionFlags.Above) != 0)
        {
            velocity.y = -0.75f;
        }

        CoyoteTime();
        
        // this is to stop the players sound if the player is up close to the 
        RaycastHit hit;
        if (Physics.Raycast(cController.transform.position, transform.forward, out hit, 1.0f, environmentLayer))
        {
            return;
        }

        // plays random foot step sound effects
        randIndexTimer -= Time.deltaTime;
        int randomSoundIndex = Random.Range(0, 4);
        if (isGrounded == true && (Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S)) ||
           (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
        {
            if (randIndexTimer <= 0.0f)
            {
                if (audioManager)
                {
                    audioManager.StopSFX($"Player Running {randomSoundIndex + 1}");
                    audioManager.PlaySFX($"Player Running {randomSoundIndex + 1}");
                }
                randIndexTimer = 0.37f;
            }

        }
        HeadBobbing();
        MovingCurve();
    }

    // there is the players movement based on a curve
    // if the player moves for a certain amount of time then their acceleration increase
    void MovingCurve()
    {
        if (((cController.collisionFlags & CollisionFlags.Below) != 0) && Mathf.Abs(velocity.x) > 0.1f || Mathf.Abs(velocity.z) > 0.1f)
        {
            moveTime += Time.deltaTime;
            moveTime = Mathf.Clamp(moveTime, 0, 3);
            if (movementSpeed >= maxMovementSpeed && Input.GetKey(KeyCode.W))
            {
                lookScript.GetCamera().fieldOfView += increasedFOVMoving * Time.deltaTime;
            }
        }
        else
        {
            lookScript.GetCamera().fieldOfView -= initialFOV * Time.deltaTime;
            moveTime = 0.0f;
        }
        moveAnimaCurve.RemoveKey(1);
        moveAnimaCurve.RemoveKey(0);

        moveAnimaCurve.AddKey(new Keyframe(0, maxMovementSpeed - 1));

        moveAnimaCurve.AddKey(new Keyframe(3, maxMovementSpeed));
        movementSpeed = moveAnimaCurve.Evaluate(moveTime);

        if (lookScript.GetCamera().fieldOfView >= increasedFOVMoving)
        {
            lookScript.GetCamera().fieldOfView = increasedFOVMoving;
        }
        if (lookScript.GetCamera().fieldOfView <= initialFOV)
        {
            lookScript.GetCamera().fieldOfView = initialFOV;
        }

        // if players y velo reaches a certain height then increase the fov
        if (velocity.y < -25.0f && lookScript.GetCamera().transform.rotation.x > 20)
        {
            lookScript.GetCamera().fieldOfView += increasedFOVMoving * Time.deltaTime;
        }
    }

    // jump function
    void Jumping()
    {
        // checks if player is on the ground and if player has press space
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = jumpSpeed;
            isHeadShaking = true;
            isGrounded = false;
        }
    }

    // CoyoteTime function
    // gives player time to jump when they have just fall off a platform
    void CoyoteTime()
    {
        // collision detection for player
        if ((cController.collisionFlags & CollisionFlags.Below) != 0)
        {
            isGrounded = true;
            velocity.y = -1.0f;
            currentCoyoteTime = coyoteTime;
            if (isHeadShaking == true)
            {
                if (audioManager)
                {
                    audioManager.StopSFX("Player Landing");
                    audioManager.PlaySFX("Player Landing");
                }
                CameraShake();
                isHeadShaking = false;
            }
        }
        // coyoteTime
        if (currentCoyoteTime > 0.0f)
        {
            currentCoyoteTime -= Time.deltaTime;
        }
        else if (currentCoyoteTime < 0.0f)
        {
            isGrounded = false;
            currentCoyoteTime = coyoteTime;
        }
    }

    // HeadBobbing Function
    // blends headbobbing on and off smoothly
    void HeadBobbing()
    {
        // getting the difference between the oldpos and newpos
        Vector3 frameMove = newPos - oldPos;
        Vector2 planarFrameMove = new Vector2(frameMove.x, frameMove.z);
        headBobTimer += planarFrameMove.magnitude;

        // how fast the player is moving
        Vector2 planarFrameVelocity = planarFrameMove;
        planarFrameVelocity.x /= Time.deltaTime;
        planarFrameVelocity.y /= Time.deltaTime;

        // for blending back to the neutral position of the head
        if (isGrounded && planarFrameVelocity.magnitude > headBobMinSpeed)
        {
            headBobMultiplier += headBobBlendSpeed * Time.deltaTime;
            headBobMultiplier = Mathf.Min(1.0f, headBobMultiplier);
        }
        else
        {
            headBobMultiplier -= headBobBlendSpeed * Time.deltaTime;
            headBobMultiplier = Mathf.Max(0.0f, headBobMultiplier);

            if (headBobMultiplier <= 0.0f)
            {
                headBobTimer = 0.0f;
            }
        }

        // movement for the head bobbing
        Vector3 localPos = lookScript.GetCamera().transform.localPosition;
        float headBobMove = (Mathf.Cos(headBobTimer * headBobFrequency) - 1.0f) * headBobAmplitude;
        localPos.y = headBobNeutral + (headBobMove * headBobBlendCurve.Evaluate(headBobMultiplier));
        lookScript.GetCamera().transform.localPosition = localPos;
    }

    // Camera Shake function
    // this drops the camerea when play hits ground
    void CameraShake()
    {
        cameraHolderTargetPos = new Vector3(0, -cameraShakeDrop, 0);
        cameraHolderTargetAngle = cameraShakeDip;
    }


    void OnTriggerStay(Collider other)
    {
        // collision with the enemy
        if (other.gameObject.layer == 8)
        {
            Vector3 dir = other.transform.position - this.transform.position;
            dir.y = 0;
            dir = dir.normalized;

	        other.gameObject.GetComponentInParent<Rigidbody>().AddForce(dir * collisionForce, ForceMode.Impulse);
        }
    }

    public StatModifier.FullStat GetSpeedStat()
    {
        return speed;
    }
}

