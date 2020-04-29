using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDropBomb : NetworkBehaviour
{
    public GameObject bombPrefab;

    void Start()
    {

    }

    void Update()
    {
        if (this.isLocalPlayer && Input.GetKeyDown(KeyCode.Space))
        {
            CmdDropBomb();
        }
    }

    [Command]
    private void CmdDropBomb()
    {
        if (bombPrefab)
        {
            if (NetworkServer.active)
            {
                NetworkServer.Spawn(Instantiate(bombPrefab,
                 new Vector3(Mathf.Round(transform.position.x + 0.5f) - 0.5f, bombPrefab.transform.position.y, Mathf.RoundToInt(transform.position.z + 0.5f) - 0.5f),
                 bombPrefab.transform.rotation));
            }
        }
    }
}
