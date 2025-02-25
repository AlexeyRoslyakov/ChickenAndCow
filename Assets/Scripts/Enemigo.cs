using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] Transform playerTarget;
    [SerializeField] float speed;
    [SerializeField] float range;
    [SerializeField] List<Transform> waypoints;
    int currentWaypoint;


    // Update is called once per frame
    void Update()
    {
        if (playerTarget == null) return;

        if (Vector2.Distance(playerTarget.transform.position, transform.position) < range)
        {
            Vector3 dir = playerTarget.position - transform.position;
            transform.position += dir.normalized * Time.deltaTime * speed;
            Move(dir);
        }
        else
        {
            if (waypoints.Count > 0)
            {
                Vector3 dir = waypoints[currentWaypoint].transform.position - transform.position;
                //transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypoint].position, Time.deltaTime);
                Move(dir);
                if (dir.magnitude <= 0.1f)
                {
                    currentWaypoint++;
                    if (currentWaypoint > waypoints.Count)
                    {
                        currentWaypoint = 0;
                    }
                }
            }
        }
    }

    void Move(Vector3 direction)
    {
        transform.position += direction.normalized * Time.deltaTime * speed;
    }
}