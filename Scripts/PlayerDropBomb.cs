using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDropBomb : NetworkBehaviour
{
    public GameObject bombPrefab;

    private Player player;

    void Start()
    {
        player = GetComponent<Player>();
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
                Vector3 bombPosition = new Vector3(Mathf.Round(transform.position.x + 0.5f) - 0.5f, bombPrefab.transform.position.y, Mathf.RoundToInt(transform.position.z + 0.5f) - 0.5f);
                GameObject bomb  = Instantiate(bombPrefab, bombPosition, bombPrefab.transform.rotation);
                bomb.GetComponent<Bomb>().explosions = player.explosions;

                NetworkServer.Spawn(bomb);
            }
        }
    }
}
