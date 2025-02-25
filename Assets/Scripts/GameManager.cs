using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance

    [Header("UI Elements")] public TextMeshProUGUI statusText;
    public TextMeshProUGUI milkText;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI stoneText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI eggsText;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI wheatSeedCountText;
    public TextMeshProUGUI tomatoSeedCountText;
    public GameObject actionIcon;
    public GameObject sleepIcon;
    public GameObject happyIcon;
    public GameObject tiredIcon;
    public GameObject deadIcon;
    public GameObject winScreen;
    public GameObject gameOverScreen;
    public GameObject wheatSeedIcon;
    public GameObject tomatoSeedIcon;

    [Header("Audio Sources")] public AudioSource backgroundMusicSource;
    public AudioSource sleepMusicSource;
    public AudioSource gameOverMusicSource;

    [Header("Audio Clips")] public AudioClip backgroundMusic;
    public AudioClip sleepMusic;
    public AudioClip gameOverMusic;

    [Header("Resource Management")] public ResourceData milk = new ResourceData("Milk", 10);
    public ResourceData wood = new ResourceData("Wood", 5);
    public ResourceData stone = new ResourceData("Stone", 3);
    public ResourceData food = new ResourceData("Food", 8);
    public ResourceData eggs = new ResourceData("Eggs", 12);

    [Header("Trading System")] public GameObject tradingScreen;
    public GameObject yesButton;
    public GameObject noButton;

    private int milkCount;
    private int woodCount;
    private int stoneCount;
    private int foodCount;
    private int eggsCount;
    private int playerMoney = 0;
    private int wheatSeedCount = 0;   
    private int tomatoSeedCount = 0;

    private List<Nest> nests = new List<Nest>();
    private List<CowBehavior> cows = new List<CowBehavior>();
    private List<PlantGrowth> plants = new List<PlantGrowth>();


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
        PlayBackgroundMusic();
        UpdateMilkUI();
        UpdateWoodUI();
        UpdateStoneUI();
        UpdateFoodUI();
        UpdateEggsUI();
        HideActionIcon();
        HideSleepIcon();
        HideWinScreen();
        HideGameOverScreen();
        HideTradingScreen();
        UpdateMoneyUI();
        UpdateSeedUI();
    }
    public int GetPlayerMoney()
    {
        return playerMoney;
    }
    public void SpendMoney(int amount)
    {
        if (playerMoney >= amount)
        {
            playerMoney -= amount;
            UpdateMoneyUI();
            Debug.Log($"Spent {amount} money. Remaining: {playerMoney}");
        }
        else
        {
            Debug.LogError("Attempted to spend more money than available!");
        }
    }


    public void RegisterNest(Nest nest)
    {
        if (!nests.Contains(nest))
        {
            nests.Add(nest);
        }
    }

    public void RegisterCow(CowBehavior cow)
    {
        if (!cows.Contains(cow))
        {
            cows.Add(cow);
        }
    }
    public void RegisterPlant(PlantGrowth plant)
    {
        if (!plants.Contains(plant))
        {
            plants.Add(plant);
            Debug.Log($"Plant registered: {plant.name}");
        }
        else
        {
            Debug.Log($"Plant already registered: {plant.name}");
        }
    }

    public bool GameOver { get; private set; } = false;

    public void AddMoney(int amount)
    {
        playerMoney += amount;
        Debug.Log($"Added {amount} money. Current total: {playerMoney}");
        UpdateMoneyUI();
        CheckWinCondition();
    }

    public void CheckWinCondition()
    {
        if (playerMoney >= 1000) // make a variable later
        {
            HandlePlayerWin();
        }
    }

    private void HandlePlayerWin()
    {
        Debug.Log("Player has won!");
        GameOver = true; // Set the game over flag
        ShowWinScreen();
        DisablePlayerControls();
    }

    public void ShowWinScreen()
    {
        winScreen.SetActive(true);
    }

    public void HideWinScreen()
    {
        winScreen.SetActive(false);
    }

    public void UpdateMoneyUI()
    {
        if (moneyText == null)
        {
            Debug.LogError("MoneyText is not assigned in the Inspector!");
            return;
        }

        moneyText.text = playerMoney.ToString();
        Debug.Log($"MoneyText updated to: {moneyText.text}");
    }

    public void NotifySleepComplete()
    {
        foreach (Nest nest in nests)
        {
            nest.ReplenishEgg();
        }

        foreach (CowBehavior cow in cows)
        {
            cow.ReplenishMilk();
        }
        foreach (var plant in plants)
        {
            Debug.Log($"Advancing growth phase for plant: {plant.name}");
            plant.AdvanceGrowthPhase();
        }
        
        PurchaseSystem.Instance.RestockGoods();
    }

    public void PlayBackgroundMusic()
    {
        backgroundMusicSource.clip = backgroundMusic;
        backgroundMusicSource.loop = true;
        backgroundMusicSource.Play();
    }

    public void PlaySleepMusic()
    {
        backgroundMusicSource.Stop();
        sleepMusicSource.clip = sleepMusic;
        sleepMusicSource.loop = true;
        sleepMusicSource.Play();
    }

    public void StopSleepMusic()
    {
        sleepMusicSource.Stop();
        PlayBackgroundMusic();
    }

    public void AddMilk(int amount)
    {
        milk.quantity += amount;
        UpdateMilkUI();
    }

    public void AddWood(int amount)
    {
        wood.quantity += amount;
        UpdateWoodUI();
    }

    public void AddStone(int amount)
    {
        stone.quantity += amount;
        UpdateStoneUI();
    }

    public void AddFood(int amount)
    {
        food.quantity += amount;
        UpdateFoodUI();
    }

    public void AddEggs(int amount)
    {
        eggs.quantity += amount;
        UpdateEggsUI();
    }

    private void UpdateMilkUI()
    {
        milkText.text = milk.quantity.ToString();
    }

    private void UpdateWoodUI() => woodText.text = wood.quantity.ToString();
    private void UpdateStoneUI() => stoneText.text = stone.quantity.ToString();
    private void UpdateFoodUI() => foodText.text = food.quantity.ToString();
    private void UpdateEggsUI() => eggsText.text = eggs.quantity.ToString();
    public bool HasSeeds()
    {
        return wheatSeedCount > 0 || tomatoSeedCount > 0;
    }

    public bool HasWheatSeeds()
    {
        return wheatSeedCount > 0;
    }

    public bool HasTomatoSeeds()
    {
        return tomatoSeedCount > 0;
    }
    public void AddWheatSeeds(int amount)
    {
        wheatSeedCount += amount;
        UpdateSeedUI();
    }

    public void AddTomatoSeeds(int amount)
    {
        tomatoSeedCount += amount;
        UpdateSeedUI();
    }

    public void SpendWheatSeeds(int amount)
    {
        wheatSeedCount = Mathf.Max(0, wheatSeedCount - amount); // Prevent negative values
        UpdateSeedUI();
    }

    public void SpendTomatoSeeds(int amount)
    {
        tomatoSeedCount = Mathf.Max(0, tomatoSeedCount - amount); // Prevent negative values
        UpdateSeedUI();
    }

    private void UpdateSeedUI()
    {
        // Update wheat seed UI
        if (wheatSeedCount > 0)
        {
            wheatSeedIcon.SetActive(true);
            wheatSeedCountText.text = wheatSeedCount.ToString();
        }
        else
        {
            wheatSeedIcon.SetActive(false);
        }

        // Update tomato seed UI
        if (tomatoSeedCount > 0)
        {
            tomatoSeedIcon.SetActive(true);
            tomatoSeedCountText.text = tomatoSeedCount.ToString();
        }
        else
        {
            tomatoSeedIcon.SetActive(false);
        }
    }

    public void ShowActionIcon() => actionIcon?.SetActive(true);

    public void HideActionIcon()
    {
        if (actionIcon == null)
        {
            Debug.LogError("Action Icon reference is missing or has been destroyed.");
            return;
        }

        actionIcon.SetActive(false);
    }

    public void ShowSleepIcon() => sleepIcon?.SetActive(true);
    public void HideSleepIcon() => sleepIcon?.SetActive(false);

    public void UpdatePlayerStatus(string status, GameObject iconToEnable)
    {
        statusText.text = status;
        happyIcon.SetActive(false);
        tiredIcon.SetActive(false);
        deadIcon.SetActive(false);
        iconToEnable.SetActive(true);
    }

    public void ShowGameOverScreen()
    {
        gameOverScreen.SetActive(true);
        GameOver = true;
    }

    public void HideGameOverScreen()
    {
        gameOverScreen.SetActive(false);
    }


    public void HideTradingScreen()
    {
        tradingScreen.SetActive(false);
    }

    public void PlayGameOverMusic()
    {
        backgroundMusicSource.Stop();
        sleepMusicSource.Stop();

        gameOverMusicSource.clip = gameOverMusic;
        gameOverMusicSource.loop = true;
        gameOverMusicSource.Play();
    }

    public int CalculateTotalTradeValue()
    {
        return milk.GetProductValue() +
               wood.GetProductValue() +
               stone.GetProductValue() +
               food.GetProductValue() +
               eggs.GetProductValue();
    }

    public void SellAllResources()
    {
        int totalTradeValue = CalculateTotalTradeValue();
        Debug.Log($"Total trade value: {totalTradeValue}");

        AddMoney(totalTradeValue);

        milk.quantity = 0;
        wood.quantity = 0;
        stone.quantity = 0;
        food.quantity = 0;
        eggs.quantity = 0;

        Debug.Log("Resources reset to 0");

        UpdateMilkUI();
        UpdateWoodUI();
        UpdateStoneUI();
        UpdateFoodUI();
        UpdateEggsUI();

        Debug.Log("Main UI updated after selling resources.");
    }
    

    private void DisablePlayerControls()
    {
        var player = GameObject.FindWithTag("Player").GetComponent<Player2Dmovement>();
        if (player != null)
        {
            player.SetDead(true);
        }
    }
}


[System.Serializable]
public class ResourceData
{
    public string name;
    public int quantity;
    public int price;

    public ResourceData(string name, int price)
    {
        this.name = name;
        this.quantity = 0; // Default quantity is zero
        this.price = price;
    }

    public int GetProductValue()
    {
        return quantity * price;
    }
    
}