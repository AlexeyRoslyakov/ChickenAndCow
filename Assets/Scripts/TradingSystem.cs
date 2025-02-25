using UnityEngine;
using TMPro;

public class TradingSystem : MonoBehaviour
{
    public static TradingSystem Instance { get; private set; }
    [Header("Trading UI")] public GameObject tradingWindow;
    public TextMeshProUGUI milkQuantityText;
    public TextMeshProUGUI milkPriceText;
    public TextMeshProUGUI milkProductText;

    public TextMeshProUGUI woodQuantityText;
    public TextMeshProUGUI woodPriceText;
    public TextMeshProUGUI woodProductText;

    public TextMeshProUGUI stoneQuantityText;
    public TextMeshProUGUI stonePriceText;
    public TextMeshProUGUI stoneProductText;

    public TextMeshProUGUI foodQuantityText;
    public TextMeshProUGUI foodPriceText;
    public TextMeshProUGUI foodProductText;

    public TextMeshProUGUI eggsQuantityText;
    public TextMeshProUGUI eggsPriceText;
    public TextMeshProUGUI eggsProductText;

    public TextMeshProUGUI totalMoneyText;

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

    private void PopulateTradingUI()
    {
        var gm = GameManager.Instance;

        // Milk
        Debug.Log($"Milk Quantity: {gm.milk.quantity}");
        milkQuantityText.text = gm.milk.quantity.ToString();
        milkPriceText.text = gm.milk.price.ToString();
        milkProductText.text = gm.milk.GetProductValue().ToString();

        // Wood
        Debug.Log($"Wood Quantity: {gm.wood.quantity}");
        woodQuantityText.text = gm.wood.quantity.ToString();
        woodPriceText.text = gm.wood.price.ToString();
        woodProductText.text = gm.wood.GetProductValue().ToString();

        // Stone
        Debug.Log($"Stone Quantity: {gm.stone.quantity}");
        stoneQuantityText.text = gm.stone.quantity.ToString();
        stonePriceText.text = gm.stone.price.ToString();
        stoneProductText.text = gm.stone.GetProductValue().ToString();

        // Food
        Debug.Log($"Food Quantity: {gm.food.quantity}");
        foodQuantityText.text = gm.food.quantity.ToString();
        foodPriceText.text = gm.food.price.ToString();
        foodProductText.text = gm.food.GetProductValue().ToString();

        // Eggs
        Debug.Log($"Eggs Quantity: {gm.eggs.quantity}");
        eggsQuantityText.text = gm.eggs.quantity.ToString();
        eggsPriceText.text = gm.eggs.price.ToString();
        eggsProductText.text = gm.eggs.GetProductValue().ToString();

        // Total
        totalMoneyText.text = gm.CalculateTotalTradeValue().ToString();
    }

    public void OpenTradingWindow()
    {
        PopulateTradingUI();
        tradingWindow.SetActive(true);
        GameObject.FindWithTag("Player").GetComponent<Player2Dmovement>().DisableMovement();
    }

    public void CloseTradingWindow()
    {
        tradingWindow.SetActive(false);
        GameObject.FindWithTag("Player").GetComponent<Player2Dmovement>().EnableMovement();
    }

    public void OnYesButtonPressed()
    {
        GameManager.Instance.SellAllResources();
        CloseTradingWindow();
    }

    public void OnNoButtonPressed()
    {
        CloseTradingWindow();
    }
}