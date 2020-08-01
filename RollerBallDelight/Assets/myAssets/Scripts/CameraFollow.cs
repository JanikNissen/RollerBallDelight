using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject player;
    private Vector3 offset;

    public void Start()
    {
        offset = transform.position - player.transform.position;
    }

    public void LateUpdate()
    {
        transform.position = player.transform.position + offset;
    }
}
