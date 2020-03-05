using UnityEngine;
using System.Collections;

public class DisableTriggerOnPlayerExit : MonoBehaviour
{

    public void OnTriggerExit (Collider other)
    {
        if (other.gameObject.CompareTag ("Player"))
        { // When the player exits the trigger area
            GetComponent<Collider> ().isTrigger = false; // Disable the trigger
        }
    }
}
