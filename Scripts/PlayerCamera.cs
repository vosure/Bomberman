using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    private Vector3 rotation;

    private void Awake()
    {
        float rotationY = Mathf.Sign(transform.position.z) == 1 ? 180.0f : 0.0f;
        rotation = new Vector3(75.0f, rotationY, 0.0f);
        transform.transform.eulerAngles = rotation;
    }

    public void UpdateRotation()
    {
        transform.transform.eulerAngles = rotation;
    }
}
