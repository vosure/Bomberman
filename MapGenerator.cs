using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject wallPrefab; //TODO(vosure): Name environmnet objects and save it somewhere as prefabs
    public GameObject obstaclePrefab;
    public GameObject boxPrefab;

    public int width = 64;
    public int height = 64;
    public float blockSize = 2.0f;



    private Transform mapHolder;

    void Start()
    {
        //GenerateMap();
    }

    public void GenerateMap()
    {
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        int mapSizeHalved = (int)(width / 2.0f);

        GenerateWallsAround(mapSizeHalved);
        GenerateObstacles(mapSizeHalved);
    }

    private void GenerateObstacles(int mapSizeHalved)
    {
        for (float i = -mapSizeHalved + blockSize; i < mapSizeHalved; i+=2)
        {
            for (float j = -mapSizeHalved + blockSize; j < mapSizeHalved; j+=2)
            {
                if (j % 4 == 0 && i % 4 == 0)
                {
                    GameObject newBlock = Instantiate(obstaclePrefab, new Vector3(i, 0.0f, j), Quaternion.identity) as GameObject;
                    newBlock.transform.parent = mapHolder;
                }
            }
        }
    }

    private void GenerateWallsAround(int mapSizeHalved)
    {
        Vector3 bottomLeft = new Vector3(-mapSizeHalved, 0.0f, -mapSizeHalved);
        Vector3 bottomRight = new Vector3(mapSizeHalved, 0.0f, -mapSizeHalved);
        Vector3 topLeft = new Vector3(-mapSizeHalved, 0.0f, mapSizeHalved);
        Vector3 topRight = new Vector3(mapSizeHalved, 0.0f, mapSizeHalved);

        GenerateWall(bottomLeft, bottomRight);
        GenerateWall(bottomRight, topRight);
        GenerateWall(topRight, topLeft);
        GenerateWall(topLeft, bottomLeft);
    }

    private void GenerateWall(Vector3 From, Vector3 To)
    {
        int x = (int)From.x;//-32 32
        int z = (int)From.z;//-32 -32
        int mapSizeHalved = (int)(width / 2.0f);
        for (int i = 0; i <= mapSizeHalved; i++)
        {
            Vector3 position = Vector3.Lerp(From, To, i / (float)mapSizeHalved);
            GameObject newBlock = Instantiate(wallPrefab, position, Quaternion.identity) as GameObject;
            newBlock.transform.parent = mapHolder;
        }
    }
}
