using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Vector2 moveInput;
    [Range(1, 20)] public float moveSpeed = 10;
    [Range(1, 10)] public float jumpHeight = 4;
    [Range(1, 100)] public float maxFallSpeed = 25;
    [Range(0, 1)] public float wallSlideSlow = 0.1f;
    [Range(1, 10)] public float baseGravity = 1;
    [Range(1, 10)] public float fallGravityMultiplier = 1;
    [Range(0, 1)] public float speedSustain = 0.1f;
    [Range(0, 1)] public float airAdjustMultiplier = 0.2f;

    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction gravityToggleAction;

    public Rigidbody2D PlayerRb { get; private set; }
    public TrailRenderer PlayerTrail { get; private set; }
    public PlayerController OtherPlayer { get; private set; }

    private Collider2D playerCollider;
    private int playerLayerMask;
    private Vector2 groundVector = Vector2.down;

    private RaycastHit2D groundCast;
    [field: SerializeField] public bool IsGrounded { get; private set; }
    [SerializeField] private bool isOnLeftWall = false;
    [SerializeField] private bool isOnRightWall = false;
    [SerializeField] private bool isOnPlatform = false;
    [SerializeField] private bool goingAgainstWall = false;
    [SerializeField] private bool justWallJumped = false;
    [SerializeField] private float wallJumpForce = 0;
    private int wallJumpDir = 0;
    private Rigidbody2D platformRb;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRb = GetComponent<Rigidbody2D>();
        PlayerTrail = GetComponent<TrailRenderer>();
        playerCollider = GetComponent<Collider2D>();
        playerLayerMask = 1 << gameObject.layer;
        groundVector *= PlayerRb.gravityScale;
        platformRb = transform.parent.GetComponent<Rigidbody2D>();

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

    // Update is called once per frame
    void Update()
    {
        groundCast = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, groundVector, 0.05f, playerLayerMask);
        IsGrounded = groundCast;
        isOnLeftWall = Physics2D.Raycast(transform.position, Vector2.left, 0.2f + transform.localScale.x * 0.5f, playerLayerMask);
        isOnRightWall = Physics2D.Raycast(transform.position, Vector2.right, 0.2f + transform.localScale.x * 0.5f, playerLayerMask);

        if (IsGrounded) justWallJumped = false;

        goingAgainstWall = (moveInput.x == -1 && isOnLeftWall) || (moveInput.x == 1 && isOnRightWall);

        if (PlayerRb.velocity.y * groundVector.y > maxFallSpeed) PlayerRb.gravityScale = 0;
        else
        {
            if (PlayerRb.velocity.y * -groundVector.y > 0) PlayerRb.gravityScale = baseGravity * -groundVector.y;
            else PlayerRb.gravityScale = fallGravityMultiplier * baseGravity * -groundVector.y;
        }

    }


    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {
        // Horizontal Movement
        if (moveInput.x != 0 || IsGrounded)
        {
            float velCheck = moveSpeed * moveInput.x;

            // For relative velocity with respect to the platform the player is on
            if ((IsGrounded && groundCast.collider.CompareTag("Platform")) || (isOnPlatform && goingAgainstWall))
            {
                velCheck += platformRb.velocity.x;
            }

            float velDelta = velCheck - PlayerRb.velocity.x;

            Vector2 forceReq = new(PlayerRb.mass * velDelta / Time.fixedDeltaTime, 0);

            // Conserving momentum from speed-up
            if (Math.Abs(platformRb.velocity.x - PlayerRb.velocity.x) > Math.Abs(moveSpeed)) forceReq *= speedSustain;

            // Wall Jump direction change delay
            if (justWallJumped && wallJumpDir != moveInput.x) forceReq *= wallJumpForce;

            // Mimic air drag
            if (!IsGrounded) forceReq *= airAdjustMultiplier;

            PlayerRb.AddForce(forceReq);
        }

        // Sliding on wall
        if (goingAgainstWall && ((PlayerRb.velocity.y * groundVector.y) > (maxFallSpeed * wallSlideSlow)))
        {
            float velDelta = (maxFallSpeed * wallSlideSlow * groundVector.y) - PlayerRb.velocity.y;
            Vector2 forceReq = new(0, PlayerRb.mass * velDelta / Time.fixedDeltaTime);
            PlayerRb.AddForce(forceReq);
        }

        // Rotate in midair
        if (!IsGrounded && !goingAgainstWall)
        {
            float rotateVal = Math.Abs(PlayerRb.velocity.x) < 0.1 * moveSpeed ? 0 : Math.Sign(PlayerRb.velocity.x);
            PlayerRb.angularVelocity = rotateVal * groundVector.y * 180;
        }

        if (justWallJumped)
        {
            wallJumpForce = Mathf.Lerp(wallJumpForce, 1, 0.01f);
            if (wallJumpForce > 0.5) justWallJumped = false;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            platformRb = other.transform.GetComponent<Rigidbody2D>();
            isOnPlatform = true;
        }
    }

    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            platformRb = transform.parent.GetComponent<Rigidbody2D>();
            isOnPlatform = false;
        }
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
            float jumpMult = Mathf.Sqrt(jumpHeight * Mathf.Abs(Physics2D.gravity.y * baseGravity) * 2) * PlayerRb.mass;
            if (IsGrounded)
            {
                Vector2 jumpVec = jumpMult * -groundVector / Time.fixedDeltaTime;
                PlayerRb.AddForce(jumpVec);
            }
            else if (isOnLeftWall || isOnRightWall)
            {
                wallJumpDir = 0;
                if (isOnLeftWall) wallJumpDir += 1;
                if (isOnRightWall) wallJumpDir -= 1;

                Vector2 jumpVec = 1.2f * jumpMult * (Quaternion.AngleAxis(wallJumpDir * 30, groundVector.y * Vector3.forward) * -groundVector) / Time.fixedDeltaTime;
                PlayerRb.velocity = new(PlayerRb.velocity.x, 0);
                PlayerRb.AddForce(jumpVec);
                justWallJumped = true;
                wallJumpForce = 0;
            }
        }
        // Jump Cancel in progress
        // if (context.phase == InputActionPhase.Canceled)
        // {

        // }
    }

    private void OnGravityToggle(InputAction.CallbackContext context)
    {
        if (HelpPopupController.isPaused) return;
        if (context.phase == InputActionPhase.Started)
        {
            if (IsGrounded || OtherPlayer.IsGrounded)
            {
                PlayerRb.gravityScale *= -1;
                groundVector *= -1;
            }
        }
    }
}