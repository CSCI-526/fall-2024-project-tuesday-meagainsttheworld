using UnityEngine;
using UnityEngine.InputSystem;
//New Code by Ziang Qin
using UnityEngine.Tilemaps;

public class PlayerController_K : MonoBehaviour
{
    [SerializeField] private float moveVal = 0f;
    public float moveSpeed = 5;
    public float jumpImpulse = 10;

    private Rigidbody2D playerRb;
    public GameObject otherPlayer;

    private int playerLayerMask;
    private Vector2 groundVector = Vector2.down;

    [SerializeField] private bool IsGrounded = true;
    [SerializeField] private bool IsOnLeftWall = false;
    [SerializeField] private bool IsOnRightWall = false;

    //New Code by Ziang Qin
    public Tilemap tilemap;  // Reference to the Tilemap
    private string passingTile = "color tiles_10";
    private string killingTile = "color tiles_32";
    public int numPlayersPassed;
    private int added = 0;

    // public GameManager gameManager;  Reference to GameManager

    /// Awake is called when the script instance is being loaded.
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerLayerMask = 1 << gameObject.layer;
        groundVector *= playerRb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        IsGrounded = Physics2D.BoxCast(transform.position, GetComponent<Collider2D>().bounds.size, 0, groundVector, 0.05f, playerLayerMask);
        IsOnLeftWall = Physics2D.BoxCast(transform.position, GetComponent<Collider2D>().bounds.size, 0, Vector2.left, 0.2f, playerLayerMask);
        IsOnRightWall = Physics2D.BoxCast(transform.position, GetComponent<Collider2D>().bounds.size, 0, Vector2.right, 0.2f, playerLayerMask);
        CheckDestination();
    }

    //New Code by Ziang Qin
    void CheckDestination()
    {
        // Get the player's current position
        Vector3 playerPosition = transform.position;

        if (tilemap == null) return;
        // Convert player's world position to tilemap grid position
        Vector3Int tilePosition = tilemap.WorldToCell(playerPosition);

        // Get the tile at the player's position
        TileBase currentTile = tilemap.GetTile(tilePosition);
        // Check if the tile is not null and has the target name
        if (currentTile != null && currentTile.name == passingTile && added == 0)
        {
            Debug.Log("Player1LevelPassed");
            numPlayersPassed++;
            added = 1;
            // Add your logic here (e.g., apply effects or trigger events)
        }
        if (currentTile != null && currentTile.name == killingTile)
        {
            Debug.Log("You are dead. Restart the game");
        }
        if (numPlayersPassed == 2) Debug.Log("Player1LevelPassed");
    }


    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    void FixedUpdate()
    {
        // Check if the game is active before allowing movement
        // if (!gameManager.isGameActive) return;
        if((!IsOnLeftWall || moveVal != -1) && (!IsOnRightWall || moveVal != 1))
        {
            playerRb.velocity = new Vector2(moveVal * moveSpeed, playerRb.velocity.y);
        }
    }

    void OnMove(InputValue val)
    {
        moveVal = val.Get<Vector2>().x;
    }

    void OnJump()
    {
        // if (!gameManager.isGameActive) return; // Prevent jumping input if the game hasn't started
        //check if alive too
        if (IsGrounded) playerRb.AddForce(jumpImpulse * -groundVector, ForceMode2D.Impulse);
    }

    void OnGravityToggle()
    {
        IsGrounded = Physics2D.Raycast(transform.position, groundVector, GetComponent<Collider2D>().bounds.extents.y + 0.05f, playerLayerMask);

        Vector3 otherPlayerPos = otherPlayer.transform.position;
        float otherPlayerExY = otherPlayer.GetComponent<Collider2D>().bounds.extents.y;
        bool otherPlayerGrounded = Physics2D.Raycast(otherPlayerPos, -groundVector, otherPlayerExY + 0.05f, 1 << otherPlayer.layer);
        if (IsGrounded || otherPlayerGrounded)
        {
            playerRb.gravityScale *= -1;
            groundVector *= -1;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GrowPowerup"))
        {
            transform.localScale = transform.localScale.x < 2 ? new Vector3(2, 2, 1) : transform.localScale;
            Debug.Log("Grow Activated");
        }
        if (other.CompareTag("ShrinkPowerup"))
        {
            transform.localScale = transform.localScale.x > 0.5 ? new Vector3(0.5f, 0.5f, 1): transform.localScale;
            Debug.Log("Shrink Activated");
        }
    }
}