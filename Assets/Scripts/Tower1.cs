using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    float timer, fereRate;
    [SerializeField] Transform playertarget;
    [SerializeField] float range;

    // Update is called once per frame
    void Update()
    {
        if (playertarget == null) return;
        if (Vector2.Distance(playertarget.transform.position, transform.position) < range)
        {
            transform.up = playertarget.position - transform.position;
        }
    }
}