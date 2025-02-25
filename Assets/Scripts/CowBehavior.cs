using UnityEngine;
using System.Collections;

public class CowBehavior : MonoBehaviour
{
    [Header("Movement Settings")] public float moveRadius = 3f;
    public float moveSpeed = 1f;

    [Header("Idle Time Range")] public float minIdleTime = 5f;
    public float maxIdleTime = 10f;

    private Vector3 targetPosition;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isMoving;
    private bool isFacingRight = true;
    private bool playerInRange;
    private bool hasMilk = true;


    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        GameManager.Instance.RegisterCow(this);
        StartCoroutine(CowBehaviorRoutine());
    }

    private IEnumerator CowBehaviorRoutine()
    {
        while (true) // Change to player !isDead
        {
            float idleTime = Random.Range(minIdleTime, maxIdleTime);
            animator.SetBool("isMoving", false);
            yield return new WaitForSeconds(idleTime);

            targetPosition = GetRandomPositionWithinRadius();

            animator.SetBool("isMoving", true);
            isMoving = true;

            while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
            {
                MoveTowardsTarget();
                yield return null;
            }

            isMoving = false;
        }
    }

    private Vector3 GetRandomPositionWithinRadius()
    {
        Vector2 randomPoint = Random.insideUnitCircle * moveRadius;
        return transform.position + new Vector3(randomPoint.x, randomPoint.y, 0);
    }

    private void MoveTowardsTarget()
    {
        Vector2 direction = (targetPosition - transform.position).normalized;

        Vector2 newPosition = rb.position + direction * moveSpeed * Time.deltaTime;
        rb.MovePosition(newPosition);

        if (direction.x > 0 && !isFacingRight)
        {
            FlipSprite();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            FlipSprite();
        }
    }

    private void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (hasMilk)
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

    public bool TryCollectMilk()
    {
        if (hasMilk && playerInRange)
        {
            hasMilk = false;
            return true;
        }

        return false;
    }

    public void ReplenishMilk()
    {
        hasMilk = true;
    }
}