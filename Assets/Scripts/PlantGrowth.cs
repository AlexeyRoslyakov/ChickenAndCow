using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    [Header("Plant Sprites")] public Sprite phase1Sprite;
    public Sprite phase2Sprite;
    public Sprite phase3Sprite;
    public Sprite phase4Sprite;


    public ResourceType resourceType;
    private int currentPhase = 1;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("PlantGrowth requires a SpriteRenderer component.");
            return;
        }

        UpdatePlantSprite();
        GameManager.Instance.RegisterPlant(this);
    }


    public void AdvanceGrowthPhase()
    {
        if (currentPhase < 4)
        {
            currentPhase++;
            Debug.Log($"Plant {name} advanced to phase {currentPhase}");
            UpdatePlantSprite();
        }
        else
        {
            Debug.Log($"Plant {name} is already fully grown.");
        }
    }

    private void UpdatePlantSprite()
    {
        switch (currentPhase)
        {
            case 1:
                spriteRenderer.sprite = phase1Sprite;
                break;
            case 2:
                spriteRenderer.sprite = phase2Sprite;
                break;
            case 3:
                spriteRenderer.sprite = phase3Sprite;
                break;
            case 4:
                spriteRenderer.sprite = phase4Sprite;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentPhase == 4 && other.CompareTag("Player"))
        {
            
           
                HarvestPlant(other.gameObject);
            
            
        }
    }

    private void HarvestPlant(GameObject player)
    {
        var playerScript = player.GetComponent<Player2Dmovement>();

        if (playerScript != null)
        {
            playerScript.PlayInteractionAnimation(resourceType);
            playerScript.GetTired(1);

            GameManager.Instance.AddFood(6);
            var bed = GetComponentInParent<PlantBed>();
            if (bed != null)
            {
                bed.ResetBed();
            }

            Destroy(gameObject);
            Debug.Log($"{resourceType} harvested and plant removed from the garden.");
        }
        else
        {
            Debug.LogError("Player2Dmovement script is missing on the Player object.");
        }
    }
}