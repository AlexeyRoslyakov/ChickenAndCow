using UnityEngine;

public class ChestInteraction : MonoBehaviour
{
    public enum ChestType
    {
        Selling,  // Chest for selling goods
        Buying    // Chest for buying goods
    }

    public ChestType chestType;
    private bool playerInRange;
   

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !GameManager.Instance.GameOver)
        {
            playerInRange = true;
            GameManager.Instance.ShowActionIcon();
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
        if (playerInRange && Input.GetMouseButtonDown(0) && !GameManager.Instance.GameOver)
        {
            OpenAppropriateWindow();
        }
    }

    private void OpenAppropriateWindow()
    {
        switch (chestType)
        {
            case ChestType.Selling:
                TradingSystem.Instance.OpenTradingWindow();
                break;
            case ChestType.Buying:
                PurchaseSystem.Instance.OpenPurchaseWindow();
                break;
        }
    }
}