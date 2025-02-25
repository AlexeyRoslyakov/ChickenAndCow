using UnityEngine;

public class NestVisibilityController : MonoBehaviour
{
    public GameObject frontWall;

    private void Start()
    {
        if (frontWall != null)
        {
            frontWall.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (frontWall != null)
            {
                frontWall.SetActive(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (frontWall != null)
            {
                frontWall.SetActive(true);
            }
        }
    }
}