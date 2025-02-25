using UnityEngine;

public class PlantBed : MonoBehaviour
{
    [Header("Plant Settings")]
    public GameObject wheatPlantPrefab;  
    public GameObject tomatoPlantPrefab;
    public Transform plantSpawnPoint;
   
    private bool playerInRange = false; 
    private bool hasPlant; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Check if the player has seeds and show the action icon
            if (GameManager.Instance.HasSeeds())
            {
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
        
        if (playerInRange && Input.GetMouseButtonDown(0))
        {
            PlantSeed();
        }
    }

    private void PlantSeed()
    {
        var gm = GameManager.Instance;
        var player = GameObject.FindWithTag("Player").GetComponent<Player2Dmovement>();
       
        if (hasPlant)
        {
            Debug.Log("A plant already exists in this bed.");
            return;
        }

        if (gm.HasWheatSeeds())
        {
            gm.SpendWheatSeeds(1);
            Instantiate(wheatPlantPrefab, plantSpawnPoint.position, Quaternion.identity);
            if (player != null)
            {
                player.PlayInteractionAnimation(ResourceType.Food); 
            }
            hasPlant = true;
            Debug.Log("Planted Wheat!");
        }
        else if (gm.HasTomatoSeeds())
        {
            gm.SpendTomatoSeeds(1); 
            Instantiate(tomatoPlantPrefab, plantSpawnPoint.position, Quaternion.identity);
            if (player != null)
            {
                player.PlayInteractionAnimation(ResourceType.Wood); 
            }
            hasPlant = true;
            Debug.Log("Planted Tomato!");
        }
        else
        {
            Debug.LogWarning("No seeds to plant!");
        }
        

        
        GameManager.Instance.HideActionIcon();
    }
    public void ResetBed()
    {
        hasPlant = false;
        Debug.Log("Garden bed reset. Ready for planting.");
    }
}