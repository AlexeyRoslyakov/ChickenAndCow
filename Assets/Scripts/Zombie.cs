using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Zombie : MonoBehaviour
{
    public float walkSpeed = 2;
    public Transform playerTarget;
    public Animator bodyAnimator;
    public float minTargetDistance = 3f;
    [SerializeField] int life = 100;
    [SerializeField] float range;
    private Rigidbody2D rb2D;
    private string currentState;
    public float attackRate;
    private float timerFire;
    private bool isAttacking;
    private bool isDead;
    private const string ZOMBIE_IDLE = "Zombie_idle_anim";
    private const string ZOMBIE_WALK = "Zombie_walk_anim";
    private const string ZOMBIE_ATTACK = "Zombie_attack_anim";

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Move(1, 0);
        Aim();
        timerFire += Time.deltaTime;
        if (Input.GetMouseButton(0) && timerFire >= attackRate && !isAttacking)
        {
            timerFire = 0;
            StartCoroutine(Attack());
        }
    }

    void Move(float x, float y)
    {
        if (playerTarget == null) return;
        if (Vector2.Distance(playerTarget.transform.position, transform.position) < range)
        {
            Vector3 dir = playerTarget.position - transform.position;
            transform.position += dir * Time.deltaTime * walkSpeed;
            bool isMoving = dir.magnitude > 0;
        }
    }

    void Aim()
    {
        if (playerTarget == null) return;
        if (Vector2.Distance(playerTarget.transform.position, transform.position) < range)
        {
            transform.right = playerTarget.position - transform.position;
        }
    }

    private IEnumerator Attack()
    {
        isAttacking = true;
        bodyAnimator.Play(GetAttackAnimation());
        yield return new WaitForSeconds(GetAnimationClipLength(GetAttackAnimation()));
        isAttacking = false;
        ResetAttackState();
    }

    public void ResetAttackState()
    {
        if (rb2D.velocity.magnitude > 0)
        {
            bodyAnimator.Play(GetWalkAnimation()); // Return to walking animation
        }
        else
        {
            bodyAnimator.Play(GetIdleAnimation()); // Return to idle animation
        }
    }

    private string GetAttackAnimation()
    {
        return ZOMBIE_ATTACK;
    }

    private float GetAnimationClipLength(string animationName)
    {
        RuntimeAnimatorController ac = bodyAnimator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }

        return 0f;
    }

    private string GetWalkAnimation()
    {
        return ZOMBIE_WALK;
    }

    private string GetIdleAnimation()
    {
        return ZOMBIE_IDLE;
    }

    public void GetDamage(int damage)
    {
        life -= damage;
        Debug.Log($"HP : {life}");
        if (life < 0)
        {
            Debug.Log($"Dead");
            isDead = true;
        }
    }
}