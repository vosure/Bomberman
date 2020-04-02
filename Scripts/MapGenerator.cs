using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject wallPrefab; //TODO(vosure): Name environmnet objects and save it somewhere as prefabs
    public GameObject floorPrefab;
    public GameObject obstaclePrefab;
    public GameObject boxPrefab;

    public Vector2 mapSize;

    public bool generateWalls;
    public bool generateObstacles;
    public bool generateBoxes;

    //NOTE(vosure): Do I actually need that?!
    [Range(0, 1)]
    public float outlinePersent;

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

        for (int x = 0; x <= mapSize.x; x++)
        {
            for (int y = 0; y <= mapSize.y; y++) //NOTE(vosure): Y is Z ? WTF?!
            {
                Vector3 tilePosition = new Vector3(-mapSize.x / 2.0f + 0.5f + x, 0, -mapSize.y / 2.0f + 0.5f + y);
                GameObject newTile = Instantiate(floorPrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
                newTile.transform.localScale = Vector3.one * (1 - outlinePersent);
                newTile.transform.parent = mapHolder;

                GameObject obj = InstantiateOject(x, y);
            }
        }

        if (generateWalls)
        {
            GenerateWallsAround();
        }
    }

    private GameObject InstantiateOject(int x, int y)
    {
        Vector3 position = new Vector3(-mapSize.x / 2.0f + 0.5f + x, 0.5f, -mapSize.y / 2.0f + 0.5f + y);
        if (x == 0 || x == mapSize.x || y == 0 || y == mapSize.y)
        {
            //TODO(vosure): Boxes everywhere except corners of the map
        }
        else if (y % 2 != 0)
        {
            if (x % 2 != 0)
            {
                if (generateObstacles) //NOTE(vosure) just for better customization, delete later
                {
                    GameObject obj = Instantiate(obstaclePrefab, position, Quaternion.identity);
                    obj.transform.parent = mapHolder;
                    return obj;
                }
            }
            else
            {
                if (generateBoxes)
                {
                    GameObject obj = Instantiate(boxPrefab, position, Quaternion.identity);
                    obj.transform.parent = mapHolder;
                    return obj;
                }
            }
        }
        else
        {
            if (generateBoxes)
            {
                GameObject obj = Instantiate(boxPrefab, position, Quaternion.identity);
                obj.transform.parent = mapHolder;
                return obj;
            }
        }

        return null;
    }


    private void GenerateWallsAround()
    {
        Vector3 bottomLeft = new Vector3(-mapSize.x / 2.0f - 0.5f, 0.5f, -mapSize.y / 2.0f - 0.5f);
        Vector3 bottomRight = new Vector3(mapSize.x / 2.0f + 1.5f, 0.5f, -mapSize.y / 2.0f - 0.5f);
        Vector3 topLeft = new Vector3(-mapSize.x / 2.0f - 0.5f, 0.5f, mapSize.y / 2.0f + 1.5f);
        Vector3 topRight = new Vector3(mapSize.x / 2.0f + 1.5f, 0.5f, mapSize.y / 2.0f + 1.5f);

        GenerateWall(bottomLeft, bottomRight);
        GenerateWall(bottomRight, topRight);
        GenerateWall(topRight, topLeft);
        GenerateWall(topLeft, bottomLeft);
    }

    private void GenerateWall(Vector3 From, Vector3 To)
    {
        int x = (int)From.x;
        int z = (int)From.z;
        for (int i = 0; i <= mapSize.x; i++)
        {
            Vector3 position = Vector3.Lerp(From, To, i / (mapSize.x + 1));
            GameObject newBlock = Instantiate(wallPrefab, position, Quaternion.identity);
            newBlock.transform.parent = mapHolder;
        }
    }
}
