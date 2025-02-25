using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    private Animator doorAnimator;
    private bool isPlayerInZone = false;

    private void Start()
    {
        doorAnimator = GetComponent<Animator>();
        if (doorAnimator == null)
        {
            Debug.LogError("Animator component not found on the door GameObject.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && doorAnimator != null)
        {
            isPlayerInZone = true;
            doorAnimator.SetTrigger("Open"); // Trigger the door to open
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && doorAnimator != null)
        {
            isPlayerInZone = false;
            doorAnimator.SetTrigger("Close"); // Trigger the door to close
        }
    }
}