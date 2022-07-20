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
    private float coyoteTime;
    private float currentCoyoteTime;

    [SerializeField]
    private float fallTimer;
    private float currentFallTimer;

    public float movementMulti = 1.0f;
    [SerializeField]
    private float movementSpeed = 10.0f;

    [Header("Character velocity")]
    private Vector3 velocity;
    private Vector3 storedJumpVelo;

    [SerializeField]
    private float airStraffMod;

    [Header("Checks")]
    private bool isGrounded = false;

    [Header("Head Bobbing")]
    private float headBobTimer = 0.0f;
    private float headBobFrequency = 1.0f;
    private float headBobAmplitude = 0.1f;
    // the default position of the head
    private float headBobNeutral = 0.80f;
    private float headBobMinSpeed = 0.1f;
    private float headBobBlendSpeed = 4.0f;
    [SerializeField] private AnimationCurve headBobBlendCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);
    private float headBobMultiplier = 0.0f;
    private Vector3 oldPos;
    private Vector3 newPos;

    public bool ableToMove = true;

    float randIndexTimer = 0.0f;

    private AudioManager audioManager;

    private Transform cameraTransform;

    private bool isHeadShaking;

    [SerializeField]
    private float initialFOV = 90.0f;

    [SerializeField]
    private float increasedFOVMoving = 100.0f;

    [SerializeField]
    private float moveTime = 0.0f;
    [SerializeField]
    private AnimationCurve moveAnimaCurve;

    public LayerMask environmentLayer;
    // Start is called before the first frame update
    void Start()
    {
        currentFallTimer = fallTimer;
        cameraTransform = this.gameObject.GetComponentInChildren<Camera>().transform;
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
        }
    }
    
    private void PlayerMoving()
    {
        // reads players input 
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Jumping();
        // converting the players input into a vector 3 and timings it by the players look direction
        Vector3 inputMove = new Vector3(x, 0.0f, z);
        Vector3 realMove = Quaternion.Euler(0.0f, lookScript.GetSpin(), 0.0f) * inputMove;
        realMove.Normalize();

        float tempAirStraffMod = 1.0f;
        // when jumping and moving to that direction
        if (!isGrounded)
        {
            tempAirStraffMod = airStraffMod;
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
            if (isHeadShaking == true)
            {
                StartCoroutine(Shake(0.1f, 1.0f));
                isHeadShaking = false;
            }
            velocity.y = -1.0f;
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
        FallingFunctions();
    }

    private void FallingFunctions()
    {
        // if players y velo reaches a certain height then increase the fov
        if (velocity.y < -25.0f)
        {
            lookScript.GetCamera().fieldOfView += increasedFOVMoving * Time.deltaTime;
        }
    }

    // the is the players movement based on a curve
    // if the player moves for a certain amount of time then their acceleration increase
    private void MovingCurve()
    {
        if (((cController.collisionFlags & CollisionFlags.Below) != 0) && Mathf.Abs(velocity.x) > 0.1f || Mathf.Abs(velocity.z) > 0.1f)
        {
            moveTime += Time.deltaTime;
            moveTime = Mathf.Clamp(moveTime, 0, 3);
            if (movementSpeed >= 12.0f)
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
            currentFallTimer = fallTimer;
            isGrounded = true;
            storedJumpVelo = velocity;
            velocity.y = -1.0f;
            currentCoyoteTime = coyoteTime;
            if (isHeadShaking == true)
            {
                StartCoroutine(Shake(0.1f, 1.0f));
                audioManager.Stop("Player Landing");
                audioManager.Play("Player Landing");
                isHeadShaking = false;
            }
        }
        // coyoteTime
        if (currentCoyoteTime > 0)
        {
            currentCoyoteTime -= Time.deltaTime;
        }
        else if (currentCoyoteTime < 0)
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

        // to get how much the head moves per frame
        Vector2 planarFrameVelocity = planarFrameMove;
        planarFrameVelocity.x /= Time.deltaTime;
        planarFrameVelocity.y /= Time.deltaTime;

        // for blending back to the neutral position of the head
        if (isGrounded && planarFrameVelocity.magnitude > headBobMinSpeed)
        {
            if (headBobMultiplier <= 0.0f)
            {
                headBobMultiplier = 1.0f;
            }
            else
            {
                headBobMultiplier += headBobBlendSpeed * Time.deltaTime;
                headBobMultiplier = Mathf.Min(1.0f, headBobMultiplier);
            }
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

    private IEnumerator Shake(float duration, float magnitude)
    {
        while (duration > 0)
        {
            float x = Random.Range(-1.0f, 1.0f) * magnitude;

            cameraTransform.localEulerAngles += new Vector3(x, 0, 0);

            duration -= Time.deltaTime;

            yield return null;
        }

    }
}

