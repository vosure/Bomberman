using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject player;

    private Vector3 offset;

    private void Start()
    {
        offset = transform.position - player.transform.position;
    }

    private void LateUpdate()
    {
        float x = player.transform.position.x;// - offset.x;
        float z = player.transform.position.z;// - offset.z;

        transform.position = new Vector3(x, transform.position.y, z);
    }
}
