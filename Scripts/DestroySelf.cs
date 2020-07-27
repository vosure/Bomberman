using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class DestroySelf : MonoBehaviour
{
    public float delay = 3.0f;

    private void Start ()
    {
        Debug.Log("DESTROYED BOX HAS BEEN SPAWNED");
        Invoke("DestroyAfterDelay", delay);
    }

    private void DestroyAfterDelay()
    {
        NetworkServer.Destroy(gameObject);
    }
}
