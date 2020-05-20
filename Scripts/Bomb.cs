using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour
{
    public AudioClip[] explosionSound;
    public GameObject explosionPrefab;
    public LayerMask collisionMask;

    public GameObject powerUpPrefab;

    private bool exploded = false;

    public int explosions;

    void Start()
    {
        Invoke("CmdExplode", 3f);
    }

    [Command]
    void CmdExplode()
    {
        AudioSource.PlayClipAtPoint(explosionSound[Random.Range(0, 5)], transform.position);

        if (NetworkServer.active)
            NetworkServer.Spawn(Instantiate(explosionPrefab, transform.position, Quaternion.identity));


        CmdCreateExplosions(Vector3.forward);
        CmdCreateExplosions(Vector3.right);
        CmdCreateExplosions(Vector3.back);
        CmdCreateExplosions(Vector3.left);

        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
        transform.Find("Collider").gameObject.SetActive(false);
        NetworkServer.Destroy(gameObject);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))
        {
            CancelInvoke("CmdExplode");
            CmdExplode();
        }
    }

    [Command]
    private void CmdCreateExplosions(Vector3 direction)
    {
        for (int i = 1; i < explosions + 1; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit, i, collisionMask);

            if (!hit.collider)
            {
                NetworkServer.Spawn(Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation));
            }
            else
            {
                if (hit.collider.tag == "Box")
                {
                    SpawnPowerUpAtPosition(hit.collider.gameObject.transform.position);
                    NetworkServer.Destroy(hit.collider.gameObject);
                    break;
                }
                if (hit.collider.tag == "PowerUp")
                {
                    NetworkServer.Destroy(hit.collider.gameObject);
                    break;
                }
            }
        }
    }

    public void SpawnPowerUpAtPosition(Vector3 position)
    {
        if (Random.Range(1, 100) <= GameSettings.powerUpChance)
        {
            int random = Random.Range(1, 3);
            GameObject powerUp = Instantiate(powerUpPrefab, position, powerUpPrefab.transform.rotation);
            switch (random)
            {
                case 1:
                    {
                        powerUp.GetComponent<PowerUp>().CreateFirePowerUp();
                        break;
                    }
                case 2:
                    {
                        powerUp.GetComponent<PowerUp>().CreateSpeedPowerUp();
                        break;
                    }
                case 3:
                    {
                        powerUp.GetComponent<PowerUp>().CreateBombPowerUp();
                        break;
                    }
                case 4:
                    {
                        powerUp.GetComponent<PowerUp>().CreateKickPowerUp();
                        break;
                    }
            }
            NetworkServer.Spawn(powerUp);
        }
    }
}
