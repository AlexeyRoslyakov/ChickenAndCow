using System.Collections;
using UnityEngine;

public enum ResourceType
{
    Wood,
    Stone,
    Food,
    Eggs,
    Milk
}

public class ResourceInteraction : MonoBehaviour
{
    public ResourceType resourceType;
    public int resourceAmount = 1;
    private bool playerInRange;

    [HideInInspector] public Nest nest;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
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
        if (playerInRange && Input.GetMouseButtonDown(0))
        {
            InteractWithResource();
        }
    }

    private void InteractWithResource()
    {
        var player = GameObject.FindWithTag("Player").GetComponent<Player2Dmovement>();
        if (player != null)
        {
            player.PlayInteractionAnimation(resourceType);
        }

        
        StartCoroutine(AddResourceAfterAnimation());
    }

    private IEnumerator AddResourceAfterAnimation()
    {
        yield return new WaitForSeconds(0.5f); 

        switch (resourceType)
        {
            case ResourceType.Wood:
                GameManager.Instance.AddWood(resourceAmount);
                break;
            case ResourceType.Stone:
                GameManager.Instance.AddStone(resourceAmount);
                break;
            case ResourceType.Food:
                GameManager.Instance.AddFood(resourceAmount);
                break;
            case ResourceType.Eggs:
                GameManager.Instance.AddEggs(resourceAmount);
                break;
            case ResourceType.Milk:
                GameManager.Instance.AddMilk(resourceAmount);
                break;
        }

        if (nest != null)
        {
            nest.EggCollected();
        }

        Destroy(gameObject);
    }
}