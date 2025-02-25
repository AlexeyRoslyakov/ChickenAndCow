using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankAIMove : MonoBehaviour
{
    [SerializeField] int upSpeed = 1;
    [SerializeField] int rightSpeed = 1;
    [SerializeField] int speedRotation = 10;
    [SerializeField] int rotateForce = 50;
    [SerializeField] int moveVertical = 1;
    [SerializeField] int moveHorizontal = 0;
    float changeInterval = 5000f;
    float timeElapsed;


    // Update is called once per frame
    void Update()
    {
        TimeToMove();
        // Moviement(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        MoveTank(moveHorizontal, moveVertical);
    }

    void MoveTank(float x, float y)
    {
        transform.Rotate(Vector3.forward, -x * Time.deltaTime * rotateForce);
        Vector3 direction = Vector3.zero;
        direction.y = y;
        transform.position += transform.up * y * Time.deltaTime * rightSpeed;
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

    void TimeToMove()
    {
        timeElapsed += Time.deltaTime * 1000;
        Debug.Log(timeElapsed);
        Debug.Log(Time.deltaTime);
        if (timeElapsed >= changeInterval)
        {
            moveHorizontal = Random.Range(-1, 2);
        }

        timeElapsed = 0;
    }
}