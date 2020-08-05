using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
    public static void ShowFPS()
    {
        Debug.Log("FPS - " + 1.0f / Time.deltaTime);
    }

    public static void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }

    public static bool ShouldSpawn(float chance)
    {
        return (Random.Range(0.0f, 100.0f) > (100.0f - chance));
    }
}
