using UnityEngine;
using System.Collections;

public class DisableTriggerOnPlayerExit : MonoBehaviour
{

    public void OnTriggerExit (Collider other)
    {
        if (other.gameObject.CompareTag ("Player"))
        {
            Debug.Log("disable bomb collider");
            GetComponent<Collider> ().isTrigger = false;
        }
    }
}
