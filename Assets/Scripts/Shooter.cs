using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public float speed = 2;
    public Transform target;
    public Transform bulletSpawnPoint;
    public GameObject bullet;
    public Animator legsAnimator;
    public Animator bodyAnimator;
    public float minTargetDistance = 3f;
    [SerializeField] int life = 100;

    private Rigidbody2D rb2D;
    private string currentState;

    public float fireRate;
    private float timerFire;
    public WeaponType currentWeapon;
    private bool isShooting;
    private bool isDead;

    private const string PLAYER_KNIFE_IDLE = "Kinfe_Idle_anim";
    private const string PLAYER_GUN_IDLE = "Gun_Idle";
    private const string PLAYER_SHOTGUN_IDLE = "Shotgun_Idle_anim";
    private const string PLAYER_GUN_WALK = "Gun_Move_anim";
    private const string PLAYER_SHOTGUN_WALK = "Shotgun_Move_anim";
    private const string PLAYER_KNIFE_WALK = "Knife_Move_anim";
    private const string PLAYER_KNIFE_SHOOT = "Knife_attack_anim";
    private const string PLAYER_GUN_SHOOT = "Gun_Shoot";
    private const string PLAYER_SHOTGUN_SHOOT = "Shotgun_Shot_anim";

    public enum WeaponType
    {
        Knife,
        Pistol,
        Shotgun,
        Rifle,
        Flashlight
    }

    void Start()
    {
        currentWeapon = WeaponType.Pistol;
        rb2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ChooseWeapon();
    }

    void FixedUpdate()
    {
        Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Aim();

        timerFire += Time.deltaTime;
        if (Input.GetMouseButton(0) && timerFire >= fireRate && !isShooting)
        {
            timerFire = 0;
            StartCoroutine(Shoot());
        }
    }

    void Move(float x, float y)
    {
        Vector2 movement = new Vector2(x, y).normalized * speed;
        rb2D.velocity = movement;
        bool isMoving = movement.magnitude > 0;
        legsAnimator.SetBool("isWalk", isMoving);
        bodyAnimator.SetBool("isWalk", isMoving);
    }

    void PlayLegsWalkingAnimation()
    {
        switch (currentWeapon)
        {
            case WeaponType.Knife:
                legsAnimator.Play(PLAYER_KNIFE_WALK);
                break;
            case WeaponType.Pistol:
                legsAnimator.Play(PLAYER_GUN_WALK);
                break;
            case WeaponType.Shotgun:
                legsAnimator.Play(PLAYER_SHOTGUN_WALK);
                break;
            default:
                legsAnimator.Play("Idle");
                break;
        }
    }

    string GetIdleAnimationForWeapon()
    {
        switch (currentWeapon)
        {
            case WeaponType.Knife: return PLAYER_KNIFE_IDLE;
            case WeaponType.Pistol: return PLAYER_GUN_IDLE;
            case WeaponType.Shotgun: return PLAYER_SHOTGUN_IDLE;
            default: return "Idle";
        }
    }

    void Aim()
    {
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0f;

        Vector3 direction = targetPos - transform.position;
        float distance = direction.magnitude;

        if (distance < minTargetDistance)
        {
            targetPos = transform.position + direction.normalized * minTargetDistance;
        }

        target.position = targetPos;

        // Rotate the player to face the target
        Vector3 dir = target.position - bulletSpawnPoint.position;
        transform.right = dir;
    }

    private IEnumerator Shoot()
    {
        isShooting = true;


        bodyAnimator.Play(GetShootAnimation());
        if (currentWeapon != WeaponType.Knife)
        {
            GameObject newBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Destroy(newBullet, 2f);
        }

        yield return new WaitForSeconds(GetAnimationClipLength(GetShootAnimation()));
        isShooting = false;
        ResetAttackState();
    }

    public void ResetAttackState()
    {
        if (rb2D.velocity.magnitude > 0)
        {
            bodyAnimator.Play(GetWalkAnimation());
        }
        else
        {
            bodyAnimator.Play(GetIdleAnimation());
        }
    }

    private string GetShootAnimation()
    {
        switch (currentWeapon)
        {
            case WeaponType.Knife:
                return PLAYER_KNIFE_SHOOT;
            case WeaponType.Pistol:
                return PLAYER_GUN_SHOOT;
            case WeaponType.Shotgun:
                return PLAYER_SHOTGUN_SHOOT;
            default:
                return PLAYER_GUN_SHOOT;
        }
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
        switch (currentWeapon)
        {
            case WeaponType.Knife:
                return PLAYER_KNIFE_WALK;
            case WeaponType.Pistol:
                return PLAYER_GUN_WALK;
            case WeaponType.Shotgun:
                return PLAYER_SHOTGUN_WALK;
            default:
                return PLAYER_GUN_WALK;
        }
    }

    private string GetIdleAnimation()
    {
        switch (currentWeapon)
        {
            case WeaponType.Knife:
                return PLAYER_KNIFE_IDLE;
            case WeaponType.Pistol:
                return PLAYER_GUN_IDLE;
            case WeaponType.Shotgun:
                return PLAYER_SHOTGUN_IDLE;
            default:
                return PLAYER_GUN_IDLE;
        }
    }

    void ChooseWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ChangeWeapon(WeaponType.Knife);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChangeWeapon(WeaponType.Pistol);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChangeWeapon(WeaponType.Shotgun);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChangeWeapon(WeaponType.Rifle);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ChangeWeapon(WeaponType.Flashlight);
        }
    }

    void ChangeWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
        if (rb2D.velocity.magnitude > 0)
        {
            PlayLegsWalkingAnimation();
            bodyAnimator.Play(GetWalkAnimation());
        }
        else
        {
            bodyAnimator.Play(GetIdleAnimationForWeapon());
        }

        Debug.Log("Weapon changed to: " + currentWeapon);
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