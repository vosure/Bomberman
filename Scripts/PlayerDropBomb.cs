using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDropBomb : NetworkBehaviour
{
    public GameObject bombPrefab;

    private Player player;

    private int bombsAvailable = 1;
    private float dropCooldown = 3.0f;
    public bool shouldCooldown = false;
    bool canDrop = false;

    private float timeStamp;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        if (this.isLocalPlayer)
        {
            if (bombsAvailable > 0)
            {
                canDrop = true;
            }
            else
            {
                canDrop = false;
            }
            if (shouldCooldown)
            {
                if (timeStamp <= Time.time)
                {

                    bombsAvailable = bombsAvailable + 1 < player.bombs ? bombsAvailable + 1 : player.bombs;
                    if (bombsAvailable == player.bombs)
                        shouldCooldown = false;
                }

            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (canDrop)
                {
                    bombsAvailable = bombsAvailable - 1 < 0 ? 0 : bombsAvailable - 1;
                    CmdDropBomb();
                    timeStamp = Time.time + dropCooldown;
                    shouldCooldown = true;
                }
            }
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
                GameObject bomb = Instantiate(bombPrefab, bombPosition, bombPrefab.transform.rotation);
                bomb.GetComponent<Bomb>().explosions = player.explosions;

                NetworkServer.Spawn(bomb);
            }
        }
    }
}
