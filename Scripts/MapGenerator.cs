using UnityEngine;
using UnityEngine.Networking;

public class MapGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public Material[] wallMaterials;

    public GameObject floorPrefab;
    public Material[] floorMaterials;

    public GameObject obstaclePrefab;
    public Material[] obstacleMaterials;

    public GameObject boxPrefab;
    public Material[] boxMaterials;

    public Vector2 mapSize;

    public bool generateWalls;
    public bool generateObstacles;
    public bool generateBoxes;

    //NOTE(vosure): Do I actually need that?!
    [Range(0, 1)]
    public float outlinePersent;

    [Range(0, 100)]
    public float objectsOnMapPercent = 100.0f;

    private Transform mapHolder;

    public GameObject[] playerPositions;

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
                SetRandomMaterial(floorPrefab, floorMaterials);
                GameObject newTile = Instantiate(floorPrefab, tilePosition, Quaternion.Euler(Vector3.right * 90));
                newTile.transform.localScale = Vector3.one * (1 - outlinePersent);
                newTile.transform.parent = mapHolder;

                InstantiateOject(x, y);
            }
        }

        if (generateWalls)
        {
            GenerateWallsAround();
        }

        //SetPlayerPositions();
    }

    private void SetRandomMaterial(GameObject prefab, Material[] materials)
    {
        prefab.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
    }

    private void SetRandonRotation(GameObject prefab)
    {
        float[] zAxisDegree = { 0.0f, 90.0f, 180.0f, 270.0f } ;
        prefab.transform.rotation = Quaternion.Euler(00.0f, zAxisDegree[Random.Range(0, 4)], 0);
    }

    private void InstantiateOject(int x, int y)
    {
        //TODO(vosure): Collapse
        Vector3 position = new Vector3(-mapSize.x / 2.0f + 0.5f + x, 0.425f, -mapSize.y / 2.0f + 0.5f + y);
        if (x == 0 || x == mapSize.x || y == 0 || y == mapSize.y)
        {
            if ((y >= 2 && y <= mapSize.y - 2) || (x >= 2 && x <= mapSize.x - 2))
            {
                if (Random.Range(0.0f, 100.0f) > (100 - objectsOnMapPercent))
                {
                    //SetRandomMaterial(boxPrefab, boxMaterials);
                    GameObject obj = Instantiate(boxPrefab, position, Quaternion.identity);
                    SetRandonRotation(obj);
                    obj.transform.parent = mapHolder;
                }
            }
        }
        else if (y % 2 != 0)
        {
            if (x % 2 != 0)
            {
                if (generateObstacles) //NOTE(vosure) just for better customization, delete later
                {
                    if (Random.Range(0.0f, 100.0f) > (100 - objectsOnMapPercent))
                    {
                        SetRandomMaterial(obstaclePrefab, obstacleMaterials);
                        GameObject obj = Instantiate(obstaclePrefab, position, Quaternion.identity);
                        obj.transform.parent = mapHolder;
                    }
                }
            }
            else
            {
                if (generateBoxes)
                {
                    if (Random.Range(0.0f, 100.0f) > (100 - objectsOnMapPercent))
                    {
                        //SetRandomMaterial(boxPrefab, boxMaterials);
                        GameObject obj = Instantiate(boxPrefab, position, Quaternion.identity);
                        SetRandonRotation(obj);
                        obj.transform.parent = mapHolder;
                    }
                }
            }
        }
        else
        {
            if (generateBoxes)
            {
                if (Random.Range(0.0f, 100.0f) > (100 - objectsOnMapPercent))
                {

                    //SetRandomMaterial(boxPrefab, boxMaterials);
                    GameObject obj = Instantiate(boxPrefab, position, Quaternion.identity);
                    SetRandonRotation(obj);
                    obj.transform.parent = mapHolder;
                }
            }
        }
    }


    private void GenerateWallsAround()
    {
        Vector3 bottomLeft = new Vector3(-mapSize.x / 2.0f - 0.5f, 0.5f, -mapSize.y / 2.0f - 0.5f);
        Vector3 bottomRight = new Vector3(mapSize.x / 2.0f + 1.5f, 0.5f, -mapSize.y / 2.0f - 0.5f);
        Vector3 topLeft = new Vector3(-mapSize.x / 2.0f - 0.5f, 0.5f, mapSize.y / 2.0f + 1.5f);
        Vector3 topRight = new Vector3(mapSize.x / 2.0f + 1.5f, 0.5f, mapSize.y / 2.0f + 1.5f);

        GenerateWall(bottomLeft, bottomRight, wallPrefab);
        GenerateWall(bottomRight, topRight, wallPrefab);
        GenerateWall(topRight, topLeft, wallPrefab);
        GenerateWall(topLeft, bottomLeft, wallPrefab);

        playerPositions[0].transform.position = bottomLeft + new Vector3(1.0f, 0.0f, 1.0f);
        playerPositions[1].transform.position = bottomRight + new Vector3(-1.0f, 0.0f, 1.0f);
        playerPositions[2].transform.position = topLeft + new Vector3(1.0f, 0.0f, -1.0f);
        playerPositions[3].transform.position = topRight + new Vector3(-1.0f, 0.0f, -1.0f);
    }

    //TODO(vosure):Should be generated on server!
    private void GenerateWall(Vector3 From, Vector3 To, GameObject prefab)
    {
        int x = (int)From.x;
        int z = (int)From.z;
        for (int i = 0; i < mapSize.x + 2; i++)
        {
            Vector3 position = Vector3.Lerp(From, To, i / (mapSize.x + 2));
            SetRandomMaterial(prefab, wallMaterials);
            GameObject newBlock = Instantiate(prefab, position, Quaternion.identity);
            newBlock.transform.parent = mapHolder;
        }
    }
}
