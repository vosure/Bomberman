using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject blockPrefab; //TODO(vosure): Name environmnet objects and save it somewhere as prefabs

    void Start()
    {
        GenerateMap();
    }

    //NOTE(vosure) Temp
    const int width = 64;
    const int height = 64;

    public void GenerateMap()
    {
        for (int i = -(width / 2); i < width / 2; i++)
        {
            GameObject wallBlock = Instantiate(blockPrefab, new Vector3(i, 0.0f, -32.0f), Quaternion.identity);
        }
        for (int i = -(width / 2); i < width / 2; i++)
        {
            GameObject wallBlock = Instantiate(blockPrefab, new Vector3(32, 0.0f, i), Quaternion.identity);
        }
        for (int i = (width / 2); i > -(width / 2); i++)
        {
            GameObject wallBlock = Instantiate(blockPrefab, new Vector3(i, 0.0f, 32.0f), Quaternion.identity);
        }
        for (int i = (width / 2); i > -(width / 2); i++)
        {
            GameObject wallBlock = Instantiate(blockPrefab, new Vector3(-32.0f, 0.0f, -32.0f), Quaternion.identity);
        }

    }
}
