using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;

public class PowerUp : NetworkBehaviour
{
    public enum PowerUpType
    {
        Fire,
        Bomb,
        Speed,
        Kick
    }

    public PowerUpType type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            NetworkServer.Destroy(gameObject);
        }
    }
}
