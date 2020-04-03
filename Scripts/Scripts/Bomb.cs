using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;

public class Bomb : MonoBehaviour
{
    public AudioClip explosionSound;
    public GameObject explosionPrefab;
    public LayerMask collisionMask;
    private bool exploded = false;

    void Start()
    {
        Invoke("Explode", 3f);
    }

    void Explode()
    {
        AudioSource.PlayClipAtPoint(explosionSound, transform.position);


        Instantiate(explosionPrefab, transform.position, Quaternion.identity);


        StartCoroutine(CreateExplosions(Vector3.forward));
        StartCoroutine(CreateExplosions(Vector3.right));
        StartCoroutine(CreateExplosions(Vector3.back));
        StartCoroutine(CreateExplosions(Vector3.left));

        GetComponent<MeshRenderer>().enabled = false;
        exploded = true;
        transform.Find("Collider").gameObject.SetActive(false);
        Destroy(gameObject, .3f);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!exploded && other.CompareTag("Explosion"))
        {
            CancelInvoke("Explode");
            Explode();
        }
    }

    private IEnumerator CreateExplosions(Vector3 direction)
    {
        //TODO(vosure): Get number of explosions from player script, can be increased by power up
        for (int i = 1; i < 3; i++)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position + new Vector3(0, .5f, 0), direction, out hit, i, collisionMask);

            if (!hit.collider)
            {
                Instantiate(explosionPrefab, transform.position + (i * direction), explosionPrefab.transform.rotation);
            }
            else
            {
                if (hit.collider.tag == "Box")
                {
                    Destroy(hit.collider.gameObject);
                    break;
                }
            }

            yield return new WaitForSeconds(.05f);
        }

    }
}
