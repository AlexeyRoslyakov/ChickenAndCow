using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrelScript : MonoBehaviour
{
    public GameObject vfx;
   
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("bullet"))
        {
            GameObject newExplode = Instantiate(vfx, transform.position, transform.rotation);
            Destroy(newExplode, 2f);
          Destroy(gameObject,0.2f);
        }
    }
}