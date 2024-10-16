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
    [Range(0, 1)] public float airAdjustMultiplier = 0.2f;

    [SerializeField] private InputAction moveAction;
    [SerializeField] private InputAction jumpAction;
    [SerializeField] private InputAction gravityToggleAction;

    private Rigidbody2D playerRb;
    public GameObject OtherPlayer { get; private set; }

    private Collider2D playerCollider;
    private int playerLayerMask;
    private Vector2 groundVector = Vector2.down;

    [SerializeField] private bool IsGrounded = true;
    [SerializeField] private bool IsOnLeftWall = false;
    [SerializeField] private bool IsOnRightWall = false;

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<Collider2D>();
        playerLayerMask = 1 << gameObject.layer;
        groundVector *= playerRb.gravityScale;

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
        IsGrounded = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, groundVector, 0.05f, playerLayerMask);
        IsOnLeftWall = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, Vector2.left, 0.05f, playerLayerMask);
        IsOnRightWall = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, Vector2.right, 0.05f, playerLayerMask);

        if (playerRb.velocity.y * groundVector.y > maxFallSpeed) playerRb.gravityScale = 0;
        else
        {
            if (playerRb.velocity.y * -groundVector.y >= 0) playerRb.gravityScale = baseGravity * -groundVector.y;
            else playerRb.gravityScale = fallGravityMultiplier * baseGravity * -groundVector.y;
        }

    }


    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {
        // Horizontal Movement
        if (moveInput.x != 0 || IsGrounded)
        {
            float velDelta = moveSpeed * moveInput.x - playerRb.velocity.x;
            Vector2 forceReq = new(playerRb.mass * velDelta / Time.fixedDeltaTime, 0);

            if (IsGrounded) playerRb.AddForce(forceReq);
            else playerRb.AddForce(airAdjustMultiplier * forceReq);
        }

        // Sliding on wall
        if (((moveInput.x == -1 && IsOnLeftWall) || (moveInput.x == 1 && IsOnRightWall)) &&
            ((playerRb.velocity.y * groundVector.y) > (maxFallSpeed * wallSlideSlow)))
        {
            float velDelta = (maxFallSpeed * wallSlideSlow * groundVector.y) - playerRb.velocity.y;
            Vector2 forceReq = new(0, playerRb.mass * velDelta / Time.fixedDeltaTime);
            playerRb.AddForce(forceReq);
        }
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        if (HelpPopupController.isPaused) return;
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (HelpPopupController.isPaused) return;
        if (context.phase == InputActionPhase.Started)
        {
            float jumpMult = Mathf.Sqrt(jumpHeight * Mathf.Abs(Physics2D.gravity.y * baseGravity) * 2) * playerRb.mass;
            if (IsGrounded)
            {
                Vector2 jumpVec = jumpMult * (-groundVector + moveInput * 0.1f).normalized;
                playerRb.AddForce(jumpVec, ForceMode2D.Impulse);
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
            IsGrounded = Physics2D.BoxCast(transform.position, playerCollider.bounds.size, 0, groundVector, 0.05f, playerLayerMask);

            int otherPlayerLayerMask = 1 << OtherPlayer.layer;
            Vector3 otherPlayerSize = OtherPlayer.GetComponent<Collider2D>().bounds.size;
            bool otherPlayerGrounded = Physics2D.BoxCast(OtherPlayer.transform.position, otherPlayerSize, 0, -groundVector, 0.05f, otherPlayerLayerMask);

            if (IsGrounded || otherPlayerGrounded)
            {
                playerRb.gravityScale *= -1;
                groundVector *= -1;
            }
        }
    }
}