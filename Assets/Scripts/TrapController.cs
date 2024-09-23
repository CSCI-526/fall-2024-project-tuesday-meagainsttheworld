using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public List<Trap> controllableTraps = new List<Trap>();

    private int currentTrapIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        Trap[] allTraps = FindObjectsOfType<Trap>();
        // Get only the controllable traps
        foreach (Trap trap in allTraps)
        {
            if (trap.isControllable)
            {
                controllableTraps.Add(trap);
            }
        }
        if (controllableTraps.Count > 0)
        {
            SelectTrap(0);
        }
    }

    // Update is called once per frame
    void Update()
    {
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



    }
    void SwitchTrap(int direction)
    {
        DeselectTrap(currentTrapIndex);
        currentTrapIndex = (currentTrapIndex + direction + controllableTraps.Count) % controllableTraps.Count;
        SelectTrap(currentTrapIndex);
    }

    void SelectTrap(int index)
    {
        Debug.Log($"Trap {controllableTraps[index].name} is selected.");
        // add visual change
    }

    void DeselectTrap(int index)
    {
        Debug.Log($"Trap {controllableTraps[index].name} is deselected.");
        // add visual change
    }
}
