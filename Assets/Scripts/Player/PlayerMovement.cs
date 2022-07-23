using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerLook lookScript;
    private CharacterController cController;

    [Header("General Movement Values")]
    [SerializeField]
    private float gravity;
    [SerializeField]
    private float jumpSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private AnimationCurve frictionCurve = AnimationCurve.Linear(0, 0.1f, 1, 1);
    [SerializeField]
    private float movementSpeed;
    [SerializeField]
    private float maxMovementSpeed = 15.0f;
    [SerializeField]
    private float coyoteTime;
    private float currentCoyoteTime;

    public float movementMulti = 1.0f;

    [Header("Character velocity")]
    private Vector3 velocity;

    [SerializeField]
    private float airStraffMod;

    [Header("Checks")]
    private bool isGrounded = false;

    [Header("Head Bobbing")]
    private float headBobTimer = 0.0f;
    private float headBobFrequency = 1.0f;
    private float headBobAmplitude = 0.02f;
    // the default position of the head
    private float headBobNeutral = 0.80f;
    private float headBobMinSpeed = 0.1f;
    private float headBobBlendSpeed = 4.0f;
    [SerializeField] private AnimationCurve headBobBlendCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    private float headBobMultiplier = 0.0f;
    private Vector3 oldPos;
    private Vector3 newPos;

    public bool ableToMove = true;

    // sound
    float randIndexTimer = 0.0f;
    private AudioManager audioManager;

    // head shaking
    private bool isHeadShaking;

    // FOV change
    [SerializeField]
    private float initialFOV = 90.0f;
    [SerializeField]
    private float increasedFOVMoving = 92.0f;

    // increase movement speed over time
    [SerializeField]
    private float moveTime = 0.0f;
    [SerializeField]
    private AnimationCurve moveAnimaCurve;

    public LayerMask environmentLayer;

    //sliding parameters
    [Header("Slope")]
    private bool willSlideOnSlopes = true;
    [SerializeField]
    private float slopeSpeed;
    private Vector3 hitPointNormal;
    private float slopeSpeedTimer;
    private bool isSlidng
    {
        get
        {
            // if player is grounded and the raycast is hitting the ground
            if (cController.isGrounded && Physics.Raycast(transform.position, Vector3.down, out RaycastHit slopeHit, 2.5f))
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
    private float maxVeloMagnitude = 1.5f;
    [Header("camera shake")]
    public Transform cameraHolder;
    public Vector3 cameraHolderTargetPos;
    public float cameraHolderTargetAngle;
    public float currentCameraHolderAngle;
    public float cameraShakePosPunchLerp = 8.0f;
    public float cameraShakePosLerp = 16.0f;
    public float cameraShakeAnglePunchLerp = 20.0f;
    public float cameraShakeAngleLerp = 40.0f;
    // how deep the drop is (camera pos)
    public float cameraShakeDrop = 0.1f;
    // how deep the dip is (camera angle)
    public float cameraShakeDip = 25.0f;


    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        lookScript = this.gameObject.GetComponent<PlayerLook>();
        cController = this.gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ableToMove)
        {
            PlayerMoving();
            UpdateCameraShake();
        }
    }
    void UpdateCameraShake()
    {
        // how fast green moves (parabolar shape)
        cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, cameraHolderTargetPos, cameraShakePosLerp * Time.deltaTime);
        currentCameraHolderAngle = Mathf.Lerp(currentCameraHolderAngle, cameraHolderTargetAngle, cameraShakeAngleLerp * Time.deltaTime);

        // how fast red moves 
        cameraHolderTargetPos = Vector3.Lerp(cameraHolderTargetPos, Vector3.zero, cameraShakePosPunchLerp * Time.deltaTime);
        cameraHolderTargetAngle = Mathf.Lerp(cameraHolderTargetAngle, 0.0f, cameraShakeAnglePunchLerp * Time.deltaTime);

        // red faster green follows red

        lookScript.bumpTilt = currentCameraHolderAngle;
    }
    private void PlayerMoving()
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
        if (willSlideOnSlopes && isSlidng)
        {
            velocity += new Vector3(hitPointNormal.x, -hitPointNormal.y, hitPointNormal.z).normalized * slopeSpeed;
            isGrounded = false;
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
        cController.Move(velocity * movementSpeed * movementMulti * Time.deltaTime);
        // getting the position after the player moves (headbobbing)
        newPos = transform.position;

        // collision when touching the roof
        if ((cController.collisionFlags & CollisionFlags.Above) != 0)
        {
            velocity.y = -0.75f;
        }

        CoyoteTime();

        RaycastHit hit;
        if (Physics.Raycast(cController.transform.position, transform.forward, out hit, 1.0f, environmentLayer))
        {
            return;
        }

        randIndexTimer -= Time.deltaTime;
        int randomSoundIndex = Random.Range(0, 4);
        if (isGrounded == true && (Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.S)) ||
           (Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
        {
            if (randIndexTimer <= 0.0f)
            {
                if (randomSoundIndex == 0)
                {
                    audioManager.Stop("Player Running 1");
                    audioManager.Play("Player Running 1");
                }
                else if (randomSoundIndex == 1)
                {
                    audioManager.Stop("Player Running 2");
                    audioManager.Play("Player Running 2");
                }
                else if (randomSoundIndex == 2)
                {
                    audioManager.Stop("Player Running 3");
                    audioManager.Play("Player Running 3");
                }
                else
                {
                    audioManager.Stop("Player Running 4");
                    audioManager.Play("Player Running 4");
                }
                randIndexTimer = 0.37f;
            }

        }
        HeadBobbing();
        MovingCurve();
    }
    // the is the players movement based on a curve
    // if the player moves for a certain amount of time then their acceleration increase
    private void MovingCurve()
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

    private void Jumping()
    {
        // checks if player is on the ground and if player has press space
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = jumpSpeed;
            isHeadShaking = true;
            isGrounded = false;
        }
    }

    private void CoyoteTime()
    {
        // collision detection for player
        if ((cController.collisionFlags & CollisionFlags.Below) != 0)
        {
            isGrounded = true;
            velocity.y = -1.0f;
            currentCoyoteTime = coyoteTime;
            if (isHeadShaking == true)
            {
                audioManager.Stop("Player Landing");
                audioManager.Play("Player Landing");
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

    private void HeadBobbing()
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

    private void CameraShake()
    {
        cameraHolderTargetPos = new Vector3(0, -cameraShakeDrop, 0);
        cameraHolderTargetAngle = cameraShakeDip;
    }
}

