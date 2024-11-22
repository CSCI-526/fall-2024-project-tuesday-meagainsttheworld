using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public PlayerStats stats;
    [SerializeField] private Vector2 moveInput;

    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction gravityToggleAction;

    [SerializeField, Range(0, 120)] private int jumpBuffer = 20;
    [SerializeField, Range(0, 120)] private int gravityToggleBuffer = 20;

    [SerializeField] private float wallJumpRecovery = 0;

    [field: SerializeField] public bool IsGrounded { get; private set; }
    [SerializeField] private bool isOnLeftWall = false;
    [SerializeField] private bool isOnRightWall = false;
    [SerializeField] private bool isOnPlatform = false;
    [SerializeField] private bool isTouchingPlatform = false;
    [SerializeField] private bool goingAgainstWall = false;
    [SerializeField] private bool justWallJumped = false;
    [SerializeField] private bool isInWell = false;

    public Rigidbody2D PlayerRb { get; private set; }
    public TrailRenderer PlayerTrail { get; private set; }
    public PlayerController OtherPlayer { get; private set; }

    private Collider2D playerCollider;
    private int playerLayerMask;
    private int baseLayerNum;
    private Vector2 groundVector = Vector2.down;
    private RaycastHit2D groundCast;
    private int jumpBufferLeft = 0;
    private int gravityToggleBufferLeft = 0;
    private int wallJumpDir = 0;
    private float currMaxFallSpeed = 0;
    private float relativeFallVel = 0;
    private Rigidbody2D platformRb;
    private readonly static string gravityCounterURL = "https://docs.google.com/forms/d/12FMEMZWAwOvY1Bp0RQirwPzLJTtQAcXGL1Svm6RgX6o/formResponse";

    void Start()
    {
        if (!stats) stats = Resources.Load<PlayerStats>("Default Stats");

        PlayerRb = GetComponent<Rigidbody2D>();
        PlayerTrail = GetComponent<TrailRenderer>();
        playerCollider = GetComponent<Collider2D>();
        baseLayerNum = gameObject.layer;
        playerLayerMask = (1 << baseLayerNum) | (1 << 8); // 8 is Gray's layer
        groundVector *= PlayerRb.gravityScale;

        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObj in playerList)
        {
            if (playerObj.name != name) OtherPlayer = playerObj.GetComponent<PlayerController>();
        }
    }

    void OnEnable()
    {
        moveAction.Enable();
        jumpAction.Enable();
        gravityToggleAction.Enable();

        moveAction.started += OnMove;
        moveAction.canceled += OnMove;
        moveAction.performed += OnMove;
        jumpAction.started += OnJump;
        jumpAction.canceled += OnJump;
        gravityToggleAction.started += OnGravityToggle;
    }

    void OnDisable()
    {
        moveAction.started -= OnMove;
        moveAction.canceled -= OnMove;
        moveAction.performed -= OnMove;
        jumpAction.started -= OnJump;
        jumpAction.canceled -= OnJump;
        gravityToggleAction.started -= OnGravityToggle;

        moveAction.Disable();
        jumpAction.Disable();
        gravityToggleAction.Disable();
    }

    void Update()
    {
        playerLayerMask = (1 << gameObject.layer) | (1 << 8);
        groundCast = Physics2D.BoxCast(playerCollider.bounds.center, playerCollider.bounds.size, 0, groundVector, 0.05f, playerLayerMask);
        isOnLeftWall = Physics2D.Raycast(playerCollider.bounds.center, Vector2.left, 0.01f + transform.localScale.x * 0.5f, playerLayerMask);
        isOnRightWall = Physics2D.Raycast(playerCollider.bounds.center, Vector2.right, 0.01f + transform.localScale.x * 0.5f, playerLayerMask);
        
        IsGrounded = groundCast;

        if (IsGrounded)
        {
            isOnPlatform = groundCast.collider.CompareTag("Platform");
            justWallJumped = false;
        }

        if (jumpBufferLeft > 0) ExecuteJump();

        if (gravityToggleBufferLeft > 0) ExecuteGravityToggle();

        goingAgainstWall = (moveInput.x == -1 && isOnLeftWall) || (moveInput.x == 1 && isOnRightWall);

        currMaxFallSpeed = goingAgainstWall ? stats.maxFallSpeed * (1 - stats.wallSlideSlow) : stats.maxFallSpeed;

        relativeFallVel = PlayerRb.velocity.y * groundVector.y;
    }


    void FixedUpdate()
    {
        // Horizontal Movement
        if (moveInput.x != 0 || IsGrounded)
        {
            // For relative velocity with respect to the platform the player is on
            float platformVel = !isInWell && isOnPlatform ? platformRb.velocity.x : 0;
            
            float velCheck = stats.moveSpeed * moveInput.x + platformVel;

            float velDelta = velCheck - PlayerRb.velocity.x;

            Vector2 forceReq = new(PlayerRb.mass * velDelta / Time.fixedDeltaTime, 0);

            // Conserving momentum from speed-up
            if (Math.Abs(PlayerRb.velocity.x) > (stats.moveSpeed + Math.Abs(platformVel))) forceReq *= stats.momentumResist;

            // Wall Jump direction change delay
            if (justWallJumped && wallJumpDir != moveInput.x) forceReq *= wallJumpRecovery;

            // Mimic air drag
            if (!IsGrounded) forceReq *= stats.airAdjustMultiplier;

            PlayerRb.AddForce(forceReq);
        }

        // Clamp fall speed
        if (relativeFallVel >= currMaxFallSpeed)
        {
            PlayerRb.gravityScale = 0;
            float velDelta = (currMaxFallSpeed - relativeFallVel) * groundVector.y;
            Vector2 forceReq = new(0, PlayerRb.mass * velDelta / Time.fixedDeltaTime);
            PlayerRb.AddForce(forceReq);
        }
        else
        {
            float relativeYVel = -relativeFallVel;
            if (relativeYVel >= stats.airHangThreshold) PlayerRb.gravityScale = stats.baseGravity * -groundVector.y;
            else
            {
                if (Math.Abs(relativeYVel) < stats.airHangThreshold) PlayerRb.gravityScale = 0.5f * stats.baseGravity * -groundVector.y;
                else 
                {
                    // jumping = false;
                    PlayerRb.gravityScale = stats.fallGravityMultiplier * stats.baseGravity * -groundVector.y;
                }
            }
        }

        // Wall jump Lerp
        if (justWallJumped)
        {
            wallJumpRecovery = Mathf.Lerp(wallJumpRecovery, 1, stats.wallJumpLerp);
            if (wallJumpRecovery > stats.wallJumpRecoveryThreshold) justWallJumped = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Well")) 
        {
            PlayerRb.gravityScale = 0;
            gameObject.layer = 9;
            isInWell = true;
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Well")) 
        {
            PlayerRb.gravityScale = 0;
            gameObject.layer = 9;
            isInWell = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Well"))
        {
            gameObject.layer = baseLayerNum;
            isInWell = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        isTouchingPlatform = other.gameObject.CompareTag("Platform");
        if (isTouchingPlatform) platformRb = other.transform.GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay2D(Collision2D other)
    {
        Vector3 boxSize = playerCollider.bounds.size * 0.35f;
        float travelDist = boxSize.x * 0.5f;
        bool upCheck = Physics2D.BoxCast(playerCollider.bounds.center, boxSize, 0, Vector2.up, travelDist, playerLayerMask);
        bool downCheck = Physics2D.BoxCast(playerCollider.bounds.center, boxSize, 0, Vector2.down, travelDist, playerLayerMask);
        bool leftCheck = Physics2D.BoxCast(playerCollider.bounds.center, boxSize, 0, Vector2.left, travelDist, playerLayerMask);
        bool rightCheck = Physics2D.BoxCast(playerCollider.bounds.center, boxSize, 0, Vector2.right, travelDist, playerLayerMask);

        if ((upCheck && downCheck) || (leftCheck && rightCheck))
        {
            StartCoroutine(GetComponent<TrapHit>().PlayDeath());
            GetComponent<TrapHit>().SendDeath();
            Debug.Log("Crushed");
        }

        isTouchingPlatform = other.gameObject.CompareTag("Platform");
    }

    void OnCollisionExit2D(Collision2D other)
    {
        isTouchingPlatform = false;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (HelpPopupController.isPaused) return;

        if (context.phase == InputActionPhase.Started)
        {
            jumpBufferLeft = jumpBuffer;
        }
    }

    private void ExecuteJump()
    {
        if (IsGrounded || isOnLeftWall || isOnRightWall)
        {
            float jumpMult = Mathf.Sqrt(stats.jumpHeight * Mathf.Abs(Physics2D.gravity.y * stats.baseGravity) * 2) * PlayerRb.mass / Time.fixedDeltaTime;
            Vector2 jumpVec;
            if (IsGrounded)
            {
                jumpVec = jumpMult * -groundVector;
                PlayerRb.AddForce(jumpVec);
            }
            else
            {
                wallJumpDir = 0;
                if (isOnLeftWall) wallJumpDir += 1;
                if (isOnRightWall) wallJumpDir -= 1;

                jumpVec = 1.2f * jumpMult * (Quaternion.AngleAxis(wallJumpDir * 30, groundVector.y * Vector3.forward) * -groundVector);
                PlayerRb.velocity = new(PlayerRb.velocity.x, 0);
                PlayerRb.AddForce(jumpVec);
                justWallJumped = true;
                wallJumpRecovery = 0;
            }
            jumpBufferLeft = 0;
        }
        else jumpBufferLeft--;
    }

    private void OnGravityToggle(InputAction.CallbackContext context)
    {
        if (HelpPopupController.isPaused) return;
        if (context.phase == InputActionPhase.Started)
        {
            gravityToggleBufferLeft = gravityToggleBuffer;
        }
    }

    private void ExecuteGravityToggle()
    {
        if (IsGrounded || OtherPlayer.IsGrounded)
        {
            PlayerRb.gravityScale *= -1;
            groundVector *= -1;
            gravityToggleBufferLeft = 0;

            SendGravityChangePosition();
        }
        else gravityToggleBufferLeft--;
    }

    public void SendGravityChangePosition()
    {
        WWWForm form = new();

        // Session ID
        form.AddField("entry.143696455", DataCollection.SessionID);
        // Current build name
        form.AddField("entry.1196517597", DataCollection.buildNo);
        // Level
        form.AddField("entry.87724549", SceneManager.GetActiveScene().name);
        // Player X coordinate at gravity toggle location
        form.AddField("entry.1671942587", transform.position.x.ToString());
        // Player Y coordinate at gravity toggle location
        form.AddField("entry.622429265", transform.position.y.ToString());

        Debug.Log(name + " gravity shift loc: " + transform.position);

        DataCollection.Post(gravityCounterURL, form);
    }
}