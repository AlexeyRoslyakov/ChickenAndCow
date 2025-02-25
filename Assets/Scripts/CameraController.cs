using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public float offset;
    public float speed=1;
    // Start is called before the first frame update


    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 playerPos = player.position + transform.forward * offset;
        transform.position = Vector3.Lerp(transform.position, playerPos, speed);
    }
}
