using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoviement : MonoBehaviour
{
    [SerializeField] int upSpeed = 1;
    [SerializeField] int rightSpeed = 1;
    [SerializeField] int speedRotation = 10;
    [SerializeField] int rotateForce = 10;
    Rigidbody2D rb;
    public GameObject tower;
    Animator towerAnimator;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        towerAnimator = tower.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        TowerRotation();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(transform.position + transform.up * upSpeed * Input.GetAxis("Vertical") * Time.fixedDeltaTime);
        rb.SetRotation(rb.rotation + rotateForce * -Input.GetAxis("Horizontal") * Time.fixedDeltaTime);
    }

    void MoveTank(float x, float y)
    {
        transform.Rotate(Vector3.forward, -x * Time.deltaTime * rotateForce);
        Vector3 direction = Vector3.zero;
        direction.y = y;
        transform.position += transform.up * y * Time.deltaTime * rightSpeed;
    }

    void TowerRotation()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePosition - tower.transform.position;
        direction.z = 0;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        tower.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        if (Input.GetMouseButtonDown(0))
        {
            towerAnimator.SetBool("IsShooting", true);
        }
        else towerAnimator.SetBool("IsShooting", false);
    }

    void Moviement(float x, float y)
    {
        //rotate
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Rotate(Vector3.forward, 5 * Time.deltaTime * speedRotation);
        }

        Vector3 direction = Vector3.zero;
        direction.y = y;
        direction.x = x;
        transform.position += direction.normalized * Time.deltaTime * rightSpeed;
    }
}