using UnityEngine;
using System.Collections;

/// <summary>
/// This script makes sure that a bomb can be laid down at the player's feet without causing buggy movement when the player walks away.
/// It disables the trigger on the collider, essentially making the object solid.
/// </summary>
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
