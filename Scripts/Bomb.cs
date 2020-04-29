using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class Bomb : NetworkBehaviour
{
    public AudioClip[] explosionSound;
    public GameObject explosionPrefab;
    public LayerMask collisionMask;

    private bool exploded = false;

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
        //TODO(vosure): Get number of explosions from player script, can be increased by power up
        for (int i = 1; i < 3; i++)
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
                    NetworkServer.Destroy(hit.collider.gameObject);
                    break;
                }
            }
        }

    }
}
