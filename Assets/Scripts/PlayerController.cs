using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.2f;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    public float moveSpeed;
    public float jumpImpulse;
    private Rigidbody2D playerRb;

    private Vector2 moveInput;
    private BoxCollider2D touchingCol;
    public bool IsMoving { get; private set; }
    
    //find ground
    [SerializeField] private bool _isGrounded = true;
    public bool IsGrounded { 
        get{return _isGrounded;}
        private set{_isGrounded = value;} }

    //find wall
    [SerializeField] private bool _isOnWall = true;
    private BoxCollider2D wallCheckCollider;
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
    public bool IsOnWall { 
        get{return _isOnWall;}
        private set{_isOnWall = value;} }

    public bool _isFacingRight = true;
    public bool IsFacingRight { 
        get{return _isFacingRight;}
        private set
        {
            if(_isFacingRight != value){
                transform.localScale *= new Vector2(-1,1);}
            _isFacingRight = value;
        }
    } 

    public GameManager gameManager; // Reference to GameManager

    /// Awake is called when the script instance is being loaded.
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        touchingCol = GetComponent<BoxCollider2D>();
        wallCheckCollider = transform.Find("WallCheck").GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {
        // Check if the game is active before allowing movement
        if (!gameManager.isGameActive) return;

        if(!IsOnWall){
            playerRb.velocity = new Vector2(moveInput.x * moveSpeed, playerRb.velocity.y);
        }
        
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = wallCheckCollider.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (!gameManager.isGameActive) return; // Prevent movement input if the game hasn't started
        moveInput = context.ReadValue<Vector2>();        
        IsMoving = moveInput != Vector2.zero;
        SetFacingDirection(moveInput);
    }

    private void SetFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !IsFacingRight){
            IsFacingRight = true;
        }else if(moveInput.x < 0 && IsFacingRight){
            IsFacingRight = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!gameManager.isGameActive) return; // Prevent jumping input if the game hasn't started
        //check if alive too
        if(context.started && IsGrounded)
        {
            playerRb.velocity = new Vector2(playerRb.velocity.x, jumpImpulse);
            

        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Flag"))
        {
            HandleFlagTrigger(other);
        }
    }
    private void HandleFlagTrigger(Collider2D other)
    {
        // Your logic for when the player touches the flag
        Debug.Log("Flag was triggered!");
        gameManager.HandleFlag(other);
        // Additional logic...
    }

}