using UnityEngine;

public class Trigger : MonoBehaviour
{
    public GameObject Wall;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            Wall.SetActive(false);
        }
            
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Box"))
        {
            Wall.SetActive(true);
        }
            
    }
}
