using System.Collections;
using UnityEngine;

public class BedInteraction : MonoBehaviour
{
    private Player2Dmovement playerScript;
    private bool playerInRange;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerScript = other.GetComponent<Player2Dmovement>();

            if (playerScript != null && playerScript.currentEnergy <= 3)
            {
                playerInRange = true;
                GameManager.Instance.ShowActionIcon(); 
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            GameManager.Instance.HideActionIcon(); 
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetMouseButtonDown(0) && playerScript.currentEnergy <= 3)
        {
            StartCoroutine(SleepRoutine());
        }
    }

    private IEnumerator SleepRoutine()
    {
        playerScript.EnterSleepMode(); 
        GameManager.Instance.HideActionIcon();
        GameManager.Instance.ShowSleepIcon();

        GameManager.Instance.PlaySleepMusic(); 

        while (playerScript.currentEnergy < playerScript.maxEnergy)
        {
            playerScript.RestoreEnergy(1);
            yield return new WaitForSeconds(1f);
        }

        GameManager.Instance.HideSleepIcon();
        GameManager.Instance.StopSleepMusic(); 
        playerScript.ExitSleepMode(); 
    }
}