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
    public GameObject OtherPlayer { get; private set; }

    private Collider2D playerCollider;
    private int playerLayerMask;
    private Vector2 groundVector = Vector2.down;

    private RaycastHit2D groundCast;
    [SerializeField] private bool isGrounded = false;
    [SerializeField] private bool isOnLeftWall = false;
    [SerializeField] private bool isOnRightWall = false;
    [SerializeField] private bool isOnPlatform = false;
    [SerializeField] private bool goingAgainstWall = false;
    private Rigidbody2D platformRb;

    // Start is called before the first frame update
    void Start()
    {
        PlayerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerLayerMask = 1 << gameObject.layer;
        groundVector *= PlayerRb.gravityScale;
        platformRb = transform.parent.GetComponent<Rigidbody2D>();

        GameObject[] playerList = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObj in playerList)
        {
            if (playerObj.name != name) OtherPlayer = playerObj;
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
        gravityToggleAction.started += OnGravityToggle;
    }

    void OnDisable()
    {
        moveAction.started -= OnMove;
        moveAction.canceled -= OnMove;
        moveAction.performed -= OnMove;
        jumpAction.started -= OnJump;
        gravityToggleAction.started -= OnGravityToggle;

        moveAction.Disable();
        jumpAction.Disable();
        gravityToggleAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        groundCast = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, groundVector, 0.05f, playerLayerMask);
        isGrounded = groundCast;
        isOnLeftWall = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, Vector2.left, 0.2f, playerLayerMask);
        isOnRightWall = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, Vector2.right, 0.2f, playerLayerMask);
        goingAgainstWall = (moveInput.x == -1 && isOnLeftWall) || (moveInput.x == 1 && isOnRightWall);

        if (PlayerRb.velocity.y * groundVector.y > maxFallSpeed) PlayerRb.gravityScale = 0;
        else
        {
            if (PlayerRb.velocity.y * -groundVector.y >= 0) PlayerRb.gravityScale = baseGravity * -groundVector.y;
            else PlayerRb.gravityScale = fallGravityMultiplier * baseGravity * -groundVector.y;
        }

    }


    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {
        // Horizontal Movement
        if (moveInput.x != 0 || isGrounded)
        {
            float velCheck = moveSpeed * moveInput.x;
            
            // For relative velocity with respect to the platform the player is on
            if ((isGrounded && groundCast.collider.CompareTag("Platform")) || (isOnPlatform && goingAgainstWall))
            {
                velCheck += platformRb.velocity.x;
            }

            float velDelta = velCheck - PlayerRb.velocity.x;
            
            Vector2 forceReq = new(PlayerRb.mass * velDelta / Time.fixedDeltaTime, 0);

            // Conserving momentum from speed-up
            if (Math.Abs(platformRb.velocity.x - PlayerRb.velocity.x) > Math.Abs(moveSpeed)) forceReq *= speedSustain;

            // Mimic air drag
            if (!isGrounded && !isOnPlatform) forceReq *= airAdjustMultiplier;

            PlayerRb.AddForce(forceReq);
        }

        // Sliding on wall
        if (goingAgainstWall && ((PlayerRb.velocity.y * groundVector.y) > (maxFallSpeed * wallSlideSlow)))
        {
            float velDelta = (maxFallSpeed * wallSlideSlow * groundVector.y) - PlayerRb.velocity.y;
            Vector2 forceReq = new(0, PlayerRb.mass * velDelta / Time.fixedDeltaTime);
            PlayerRb.AddForce(forceReq);
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
            if (isGrounded)
            {
                Vector2 jumpVec = jumpMult * -groundVector / Time.fixedDeltaTime;
                PlayerRb.AddForce(jumpVec + platformRb.velocity);
            }

            // Wall Jump Functionality in Progress
            // else if (IsOnLeftWall || IsOnRightWall)
            // {
            //     Vector2 sideDir = Vector2.zero;
            //     if (IsOnLeftWall) sideDir += Vector2.left;
            //     if (IsOnRightWall) sideDir += Vector2.right;

            //     JustWallJumped = true;
            //     jumpVector = 0.5f * jumpMult * (-groundVector - sideDir).normalized;
            //     playerRb.AddForce(jumpVector, ForceMode2D.Impulse);
            // }
        }
    }

    private void OnGravityToggle(InputAction.CallbackContext context)
    {
        if (HelpPopupController.isPaused) return;
        if (context.phase == InputActionPhase.Started)
        {
            isGrounded = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, groundVector, 0.05f, playerLayerMask);

            int otherPlayerLayerMask = 1 << OtherPlayer.layer;
            Vector3 otherPlayerSize = OtherPlayer.GetComponent<Collider2D>().bounds.size;
            bool otherPlayerGrounded = Physics2D.BoxCast(OtherPlayer.transform.position, otherPlayerSize, 0, -groundVector, 0.05f, otherPlayerLayerMask);

            if (groundCast || otherPlayerGrounded)
            {
                PlayerRb.gravityScale *= -1;
                groundVector *= -1;
            }
        }
    }
}