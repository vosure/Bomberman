using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Box : MonoBehaviour
{
    public GameObject destroyedBoxPrefab;

    public void DestroyAndSpawnNewOne()
    {
        NetworkServer.Spawn(Instantiate(destroyedBoxPrefab, gameObject.transform.position, Quaternion.identity));
        NetworkServer.Destroy(gameObject);
    }
}

