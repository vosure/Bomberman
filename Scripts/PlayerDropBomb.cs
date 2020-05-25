using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerDropBomb : NetworkBehaviour
{
    public GameObject bombPrefab;

    private Player player;

    public int bombsAvailable = 1;
    private float dropCooldown = 3.0f;

    private float timeStamp;

    void Start()
    {
        player = GetComponent<Player>();
        bombsAvailable = player.bombs;
    }

    void Update()
    {
        if (this.isLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (bombsAvailable > 0)
                {
                    bombsAvailable = bombsAvailable - 1 < 0 ? 0 : bombsAvailable - 1;
                    CmdDropBomb();
                    StartCoroutine(StartCooldown());
                }
            }
        }
    }

    public IEnumerator StartCooldown()
    {
        float timeStamp = Time.time + dropCooldown;
        while (timeStamp >= Time.time)
        {
            yield return null;
        }
        bombsAvailable = bombsAvailable + 1 < player.bombs ? bombsAvailable + 1 : player.bombs;
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
