using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject wall;


    private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            wall.SetActive(false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            wall.SetActive(true);
        }
    }
}
