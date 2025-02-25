using UnityEngine;
using TMPro;

public class PurchaseSystem : MonoBehaviour
{
    public static PurchaseSystem Instance { get; private set; }
    [Header("Purchase UI")]
    
    public GameObject purchaseWindow;             
    public TextMeshProUGUI wheatStockText;      
    public TextMeshProUGUI wheatPriceText;       
    public TextMeshProUGUI wheatCartText;        
    public TextMeshProUGUI wheatTotalText;        
    public TextMeshProUGUI tomatoStockText;
    public TextMeshProUGUI tomatoPriceText;
    public TextMeshProUGUI tomatoCartText;
    public TextMeshProUGUI tomatoTotalText;

    public TextMeshProUGUI chickenStockText;
    public TextMeshProUGUI chickenPriceText;
    public TextMeshProUGUI chickenCartText;
    public TextMeshProUGUI chickenTotalText;

    public TextMeshProUGUI cowStockText;
    public TextMeshProUGUI cowPriceText;
    public TextMeshProUGUI cowCartText;
    public TextMeshProUGUI cowTotalText;

    public TextMeshProUGUI totalAmountText;       
    public TextMeshProUGUI playerMoneyText;     

    [Header("Stock Settings")]
    public int initialWheatStock = 5;
    public int initialTomatoStock = 5;
    public int initialChickenStock = 1;
    public int initialCowStock = 1;

    private int wheatStock;
    private int tomatoStock;
    private int chickenStock;
    private int cowStock;

    private int wheatCart = 0;
    private int tomatoCart = 0;
    private int chickenCart = 0;
    private int cowCart = 0;

    private int wheatPrice = 10;
    private int tomatoPrice = 10;
    private int chickenPrice = 50;
    private int cowPrice = 100;
    
    
    private int chickenPurchases = 0; 

    private int totalCost = 0;
    [Header("Cow Purchase Settings")]
    public GameObject cowPrefab;      
    public Transform cowSpawnPoint; 
    [Header("Chicken Purchase Settings")]
    public GameObject chickenPrefab;       // Reference to the chicken prefab
    public Transform chickenSpawnPoint;   // Location to spawn the chicken
    public GameObject[] chickenNests;  
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        // Ensure the purchase window is closed at the start
        ClosePurchaseWindow();
        wheatStock = initialWheatStock;
        tomatoStock = initialTomatoStock;
        chickenStock = initialChickenStock;
        cowStock = initialCowStock;
    }

    private void PopulatePurchaseUI()
    {
        // Update stock and price information
        wheatStockText.text = wheatStock.ToString();
        wheatPriceText.text = wheatPrice.ToString();
        wheatCartText.text = wheatCart.ToString();
        wheatTotalText.text = (wheatCart * wheatPrice).ToString();

        tomatoStockText.text = tomatoStock.ToString();
        tomatoPriceText.text = tomatoPrice.ToString();
        tomatoCartText.text = tomatoCart.ToString();
        tomatoTotalText.text = (tomatoCart * tomatoPrice).ToString();

        chickenStockText.text = chickenStock.ToString();
        chickenPriceText.text = chickenPrice.ToString();
        chickenCartText.text = chickenCart.ToString();
        chickenTotalText.text = (chickenCart * chickenPrice).ToString();

        cowStockText.text = cowStock.ToString();
        cowPriceText.text = cowPrice.ToString();
        cowCartText.text = cowCart.ToString();
        cowTotalText.text = (cowCart * cowPrice).ToString();
        
        
        totalCost = (wheatCart * wheatPrice) + (tomatoCart * tomatoPrice) +
                    (chickenCart * chickenPrice) + (cowCart * cowPrice);
        totalAmountText.text = totalCost.ToString();
        playerMoneyText.text = GameManager.Instance.GetPlayerMoney().ToString();
    }

    public void OpenPurchaseWindow()
    {
        PopulatePurchaseUI();
        purchaseWindow.SetActive(true);
        GameObject.FindWithTag("Player").GetComponent<Player2Dmovement>().DisableMovement();
    }

    public void ClosePurchaseWindow()
    {
        ResetCart();
        purchaseWindow.SetActive(false);
        GameObject.FindWithTag("Player").GetComponent<Player2Dmovement>().EnableMovement();
    }

    private void ResetCart()
    {
        wheatCart = 0;
        tomatoCart = 0;
        chickenCart = 0;
        cowCart = 0;
    }

    public void AdjustWheatCart(int adjustment)
    {
        wheatCart = Mathf.Clamp(wheatCart + adjustment, 0, wheatStock);
        PopulatePurchaseUI();
    }

    public void AdjustTomatoCart(int adjustment)
    {
        tomatoCart = Mathf.Clamp(tomatoCart + adjustment, 0, tomatoStock);
        PopulatePurchaseUI();
    }

    public void AdjustChickenCart(int adjustment)
    {
        chickenCart = Mathf.Clamp(chickenCart + adjustment, 0, chickenStock);
        PopulatePurchaseUI();
    }

    public void AdjustCowCart(int adjustment)
    {
        cowCart = Mathf.Clamp(cowCart + adjustment, 0, cowStock);
        PopulatePurchaseUI();
    }

    public void ConfirmPurchase()
    {
        if (GameManager.Instance.GetPlayerMoney() < totalCost)
        {
            Debug.Log("Not enough money!");
            return;
        }

        
        GameManager.Instance.SpendMoney(totalCost);
        wheatStock -= wheatCart;
        tomatoStock -= tomatoCart;
        chickenStock -= chickenCart;
        cowStock -= cowCart;
        GameManager.Instance.AddWheatSeeds(wheatCart);
        GameManager.Instance.AddTomatoSeeds(tomatoCart);
        
        for (int i = 0; i < cowCart; i++)
        {
            SpawnCow();
        }
        for (int i = 0; i < chickenCart; i++)
        {
            SpawnChickenAndActivateNest();
        }


        Debug.Log("Purchase successful!");

        // Close the purchase window
        ClosePurchaseWindow();
    }
    private void SpawnCow()
    {
        if (cowPrefab != null && cowSpawnPoint != null)
        {
            Instantiate(cowPrefab, cowSpawnPoint.position, Quaternion.identity);
            Debug.Log("Cow spawned at: " + cowSpawnPoint.position);
        }
        else
        {
            Debug.LogWarning("Cow prefab or spawn point is not set in the inspector.");
        }
    }
    private void SpawnChickenAndActivateNest()
    {
        // Spawn chicken
        if (chickenPrefab != null && chickenSpawnPoint != null)
        {
            Instantiate(chickenPrefab, chickenSpawnPoint.position, Quaternion.identity);
            Debug.Log("Chicken spawned at: " + chickenSpawnPoint.position);
        }
        else
        {
            Debug.LogWarning("Chicken prefab or spawn point is not set in the inspector.");
        }

        // Activate the next available nest
        foreach (var nest in chickenNests)
        {
            if (!nest.activeSelf)
            {
                nest.SetActive(true);
                Debug.Log($"Nest {nest.name} activated.");
                return;
            }
        }

        Debug.LogWarning("No more nests available to activate.");
    }
    public void RestockGoods()
    {
        // Reset goods to their initial stock levels
        wheatStock = initialWheatStock;
        tomatoStock = initialTomatoStock;

        // Only restock chickens if fewer than 2 have been purchased
        if (chickenPurchases < initialChickenStock)
        {
            chickenStock = initialChickenStock - chickenPurchases;
        }

        cowStock = initialCowStock;

        Debug.Log("Goods restocked after sleep.");
        PopulatePurchaseUI();
    }
}
