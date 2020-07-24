using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Box : MonoBehaviour
{
    public GameObject destroyedBoxPrefab;

    public void Destroy()
    {

        NetworkServer.Spawn(Instantiate(destroyedBoxPrefab, gameObject.transform.position, Quaternion.identity));
        Destroy(gameObject);
    }
}

