using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameObject wall;


    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered by: " + other.gameObject.name);
        if (other.gameObject.CompareTag("Player"))
        {
            wall.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            wall.SetActive(true);
        }
    }
}
