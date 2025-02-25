using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestSound : MonoBehaviour
{
    AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        audio.pitch = 1.0f;
        audio.volume = 1.0f;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            audio.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            audio.Pause();
        }
    }
}