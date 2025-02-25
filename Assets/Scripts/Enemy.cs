using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2;
    public Transform target;
    private Transform playerTarget;
    public Transform bulletSpawnPoint;
    public GameObject bullet;
    public Animator legsAnimator;
    private Animator animator;

    public float fireRate;
    private float timerFire;
    private bool isShoot;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        playerTarget = GameObject.FindGameObjectWithTag("Player").transform;
    }

    
    void Update()
    {
       //Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Aim();
        timerFire += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (timerFire >= fireRate)
            {
                Shoot();
                isShoot = true;
                timerFire = 0;
            }
            else { animator.SetBool("isShoot", false); }
        }
    }
    void Move(float x, float y)
    {
        transform.position += new Vector3(x, y).normalized * Time.deltaTime * speed;
        Debug.Log("X is:" + x);
        Debug.Log("leg animator  is:" + legsAnimator.GetBool("isWalk"));

        if (x != 0 || y != 0)
        { legsAnimator.SetBool("isWalk", true); }
        else { legsAnimator.SetBool("isWalk", false); }

    }
    void Aim()
    {
        Vector3 targetPos = playerTarget.position;
        targetPos.z = target.position.z;
        target.position = targetPos;
        Vector3 dir = target.position - bulletSpawnPoint.position;
        transform.right = dir;
        transform.right = dir;
    }
    private void Shoot()
    {
        animator.SetBool("isShoot", true);
        GameObject newBullet = Instantiate(bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        Destroy(newBullet, 2f);
        //animator.SetBool("isShoot", false);
    }
}
