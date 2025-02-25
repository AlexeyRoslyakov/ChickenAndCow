using UnityEngine;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class Player2Dmovement : MonoBehaviour
{
    [Header("Player Settings")] public float speed = 2;
    public float maxLife = 5;
    public float maxEnergy = 18;
    [SerializeField] HeartBar heartBar;
    [SerializeField] EnergyBar energyBar;
    public float currentLife;
    public float currentEnergy;

    [Header("Audio Settings")] [SerializeField]
    private AudioSource actionAudioSource;

    [SerializeField] private AudioClip wateringSound;
    [SerializeField] private AudioClip diggingSound;
    [SerializeField] private AudioClip choppingSound;


    private bool isMoving;
    private bool isActing;
    private bool isSleeping;
    private bool isDead;
    private bool isMovementDisabled = false;
    private Rigidbody2D rb2D;
    private Animator bodyAnimator;
    AudioSource audioSource;

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        bodyAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        currentLife = maxLife;
        currentEnergy = maxEnergy;
        UpdatePlayerStatus();
    }

    void Update()
    {
        if (isDead || isSleeping)
        {
            rb2D.velocity = Vector2.zero;
            return;
        }

        Action();

        if (Input.GetMouseButtonDown(0))
        {
            TryInteract();
        }
    }

    private void FixedUpdate()
    {
        if (isDead || isSleeping || isMovementDisabled)
        {
            rb2D.velocity = Vector2.zero;
            return;
        }

        Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }


    void Move(float x, float y)
    {
        Vector2 movement = new Vector2(x, y).normalized * speed;
        rb2D.velocity = movement;

        isMoving = movement.magnitude > 0;
        bodyAnimator.SetBool("isMoving", isMoving);
        bodyAnimator.SetFloat("xAxis", x);
        bodyAnimator.SetFloat("yAxis", y);
    }
    public void DisableMovement()
    {
        isMovementDisabled = true;
    }

    public void EnableMovement()
    {
        isMovementDisabled = false;
    }

    // Used for test functions
    void Action()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isSleeping) //Test button
        {
            Debug.Log("Action");
            isActing = true;

            // Decrease energy if in active mode
            if (currentEnergy > 0)
            {
                GetTired(1);
            }
            else
            {
                GetDamage(1);
            }
        }

        if (Input.GetKeyDown(KeyCode.R)) //Test button
        {
            GetDamage(1);
        }
    }

    public void PlayInteractionAnimation(ResourceType resourceType)
    {
        if (currentEnergy > 0)
        {
            GetTired(1);
        }
        else
        {
            GetDamage(1);
        }

        switch (resourceType)
        {
            case ResourceType.Wood:
                bodyAnimator.SetTrigger("Gatito_Chop_Down");
                actionAudioSource.PlayOneShot(choppingSound);
                break;
            case ResourceType.Stone:
                bodyAnimator.SetTrigger("Gatito_Dig_Down");
                actionAudioSource.PlayOneShot(diggingSound);
                break;
            case ResourceType.Food:
            case ResourceType.Eggs:
                bodyAnimator.SetTrigger("Gatito_Water_Down");
                actionAudioSource.PlayOneShot(wateringSound);
                break;
        }
    }

    private void TryInteract()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 1f);
        foreach (Collider2D collider in hitColliders)
        {
            CowBehavior cow = collider.GetComponent<CowBehavior>();
            if (cow != null && cow.TryCollectMilk())
            {
                GameManager.Instance.AddMilk(1);
                Debug.Log("Milk collected!");
                return;
            }
        }
    }

    public void EnterSleepMode()
    {
        isSleeping = true;
        rb2D.velocity = Vector2.zero; // Stop player movement
        Debug.Log("Player is now sleeping");
    }

    public void ExitSleepMode()
    {
        isSleeping = false;
        Debug.Log("Player woke up");
        GameManager.Instance.NotifySleepComplete();
    }

    public void RestoreEnergy(float amount)
    {
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        energyBar.UpdateEnergyBarUI(maxEnergy, currentEnergy);
        UpdatePlayerStatus();
    }


    public void GetDamage(float amount)
    {
        currentLife -= amount;
        currentLife = Mathf.Max(currentLife, 0);
        heartBar.UpdateHeartBarUI(maxLife, currentLife);
        UpdatePlayerStatus();
    }

    public void GetTired(float amount)
    {
        currentEnergy -= amount;
        currentEnergy = Mathf.Max(currentEnergy, 0);
        energyBar.UpdateEnergyBarUI(maxEnergy, currentEnergy);
        UpdatePlayerStatus();
    }

    public bool IsDead
    {
        get { return isDead; }
    }

    public void SetDead(bool value)
    {
        isDead = value;
    }

    private void UpdatePlayerStatus()
    {
        if (currentLife <= 0)
        {
            HandlePlayerDeath();
        }
        else if (currentEnergy <= 3)
        {
            GameManager.Instance.UpdatePlayerStatus("Tired", GameManager.Instance.tiredIcon);
        }
        else
        {
            GameManager.Instance.UpdatePlayerStatus("Good", GameManager.Instance.happyIcon);
        }
    }

    private void HandlePlayerDeath()
    {
        isDead = true;
        rb2D.velocity = Vector2.zero;
        bodyAnimator.enabled = false;
        GameManager.Instance.UpdatePlayerStatus("Dead", GameManager.Instance.deadIcon);
        GameManager.Instance.ShowGameOverScreen();
        GameManager.Instance.PlayGameOverMusic();
    }
}