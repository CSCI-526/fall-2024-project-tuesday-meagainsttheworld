using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public List<Trap> traps = new List<Trap>();

    private int currentTrapIndex = 0;

    private Renderer trapRenderer;
    public Color selectedColor = Color.red;
    private Color originalColor;
    public GameManager gameManager; // Reference to GameManager

    // Start is called before the first frame update
    void Start()
    {
        // Trap[] allTraps = FindObjectsOfType<Trap>();
        // // Get only the controllable traps
        // foreach (Trap trap in allTraps)
        // {
        //     if (trap.isControllable)
        //     {
        //         controllableTraps.Add(trap);
        //     }
        // }
        // traps.AddRange(FindObjectsOfType<Trap>());
        if (traps.Count > 0)
        {
            SelectTrap(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is active before allowing trap controls
        if (!gameManager.isGameActive) return;

        // Switch to the previous trap by pressing "," 
        if (Input.GetKeyDown(KeyCode.Comma))
        {
            SwitchTrap(-1);
        }
        // Switch to the next trap with by pressing "." 
        if (Input.GetKeyDown(KeyCode.Period))
        {
            SwitchTrap(1);
        }

        if (traps.Count > 0)
        {
            MoveTrap(currentTrapIndex);
            // Press Shift to use the trap's skill
            if (Input.GetKey(KeyCode.RightShift))
            {
                traps[currentTrapIndex].ActivateSkillTimed();
            }
            else
            {
                traps[currentTrapIndex].DeactivateSkill();
            }
        }
    }
    void SwitchTrap(int direction)
    {
        DeselectTrap(currentTrapIndex);
        currentTrapIndex = (currentTrapIndex + direction + traps.Count) % traps.Count;
        SelectTrap(currentTrapIndex);
    }

    void SelectTrap(int index)
    {
        Debug.Log($"Trap {traps[index].name} is selected.");
        // add visual change
        trapRenderer = traps[index].GetComponent<Renderer>();
        originalColor = trapRenderer.material.color;
        trapRenderer.material.color = selectedColor;

    }

    void DeselectTrap(int index)
    {
        Debug.Log($"Trap {traps[index].name} is deselected.");
        // add visual change
        trapRenderer.material.color = originalColor;
    }

    void MoveTrap(int index)
    {
        Vector2 moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            moveDirection.x = -1; // left
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            moveDirection.x = 1; // right
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            moveDirection.y = 1; // Move up
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            moveDirection.y = -1; // Move down
        }
        traps[index].Move(moveDirection);
    }
}
