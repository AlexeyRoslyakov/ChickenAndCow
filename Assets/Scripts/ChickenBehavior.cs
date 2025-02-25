using UnityEngine;
using System.Collections;

public class ChickenBehavior : MonoBehaviour
{
    [Header("Movement Settings")] public float roamRadius = 5f;
    public float moveSpeed = 1.5f;
    public float fleeSpeed = 20f;

    [Header("Idle Time Range")] public float minIdleTime = 3f;
    public float maxIdleTime = 5f;

    private Vector3 targetPosition;
    private Animator animator;
    private Rigidbody2D rb;
    private bool isMoving;
    private bool isFacingRight = true;
    private bool isFleeing = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(ChickenRoamRoutine());
    }

    private IEnumerator ChickenRoamRoutine()
    {
        while (true)
        {
            if (!isFleeing)
            {
                float idleTime = Random.Range(minIdleTime, maxIdleTime);
                animator.SetBool("isMoving", false);
                yield return new WaitForSeconds(idleTime);

                targetPosition = GetRandomPositionWithinRadius(roamRadius);
                animator.SetBool("isMoving", true);
                isMoving = true;

                while (Vector3.Distance(transform.position, targetPosition) > 0.1f && !isFleeing)
                {
                    MoveTowardsTarget(moveSpeed);
                    yield return null;
                }

                isMoving = false;
            }

            yield return null;
        }
    }

    private Vector3 GetRandomPositionWithinRadius(float radius)
    {
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        return transform.position + new Vector3(randomPoint.x, randomPoint.y, 0);
    }

    private void MoveTowardsTarget(float speed)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;
        Vector2 newPosition = rb.position + direction * speed * Time.deltaTime;
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
            isFleeing = true;
            animator.SetBool("isFleeing", true);
            targetPosition = GetRandomPositionAwayFromPlayer(other.transform.position, roamRadius * 2);
            StartCoroutine(FleeRoutine());
        }
    }

    private Vector3 GetRandomPositionAwayFromPlayer(Vector3 playerPosition, float fleeDistance)
    {
        Vector3 fleeDirection = (transform.position - playerPosition).normalized;
        return transform.position + fleeDirection * fleeDistance + (Vector3) Random.insideUnitCircle * roamRadius;
    }

    private IEnumerator FleeRoutine()
    {
        float fleeTime = Random.Range(1f, 3f);
        float timer = 0f;

        while (timer < fleeTime)
        {
            MoveTowardsTarget(fleeSpeed);
            timer += Time.deltaTime;
            yield return null;
        }

        isFleeing = false;
        animator.SetBool("isFleeing", false);
    }
}