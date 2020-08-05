using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour
{
    public GameObject explosionPrefab;
    public LayerMask collisionMask;

    public GameObject[] powerUpPrefabs;

    private bool exploded = false;

    private int explosions;

    void Start()
    {
        Invoke("CmdExplode", 3f);
    }

    [Command]
    void CmdExplode()
    {
        AudioManager.instance.PlaySound("Explosion", transform.position);

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
                    hit.collider.gameObject.GetComponent<Box>().DestroyAndSpawnNewOne();

                    CmdSpawnPowerUpAtPosition(hit.collider.gameObject.transform.position);
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

    [Command]
    private void CmdSpawnPowerUpAtPosition(Vector3 position)
    {
        Vector3 newPosition = new Vector3(position.x, -0.3f, position.z);
        if (Utils.ShouldSpawn(GameSettings.powerUpChance))
        {
            int random = Random.Range(0, 3);
            NetworkServer.Spawn(Instantiate(powerUpPrefabs[random], newPosition, Quaternion.identity));
        }
    }

    public void SetExplosionsCount(int explosions)
    {
        this.explosions = explosions;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))
        {
            CancelInvoke("CmdExplode");
            CmdExplode();
        }
    }

}
