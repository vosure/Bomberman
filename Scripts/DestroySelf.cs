using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DestroySelf : MonoBehaviour
{
    public float delay = 3.0f;

    private void Start ()
    {
        Invoke("DestroyAfterDelay", delay);
    }

    private void DestroyAfterDelay()
    {
        NetworkServer.Destroy(gameObject);
    }
}
