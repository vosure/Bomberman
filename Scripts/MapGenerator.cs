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

    public GameObject decorationPrefab;
    public GameObject[] decorationObjects;

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

    public int decorationAreaSize = 4;

    //TODO(vosure): change to one dimensional array
    private MapObject[,] mapGrid;

    void Start()
    {
        //NOTE(vosure): Probably I should use int width and height, instead of Vector2 mapSize!?
        GenerateMap();
    }

    public void GenerateMap()
    {
        mapGrid = new MapObject[(int)mapSize.x + 1, (int)mapSize.y + 1];
        string holderName = "Generated Map";
        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        mapHolder = new GameObject(holderName).transform;
        mapHolder.parent = transform;

        for (int x = 0; x <= mapSize.x; x++)
        {
            for (int y = 0; y <= mapSize.y; y++)
            {
                InstantiateOject(x, y);
            }
        }

        if (generateWalls)
        {
            GenerateWallsAround();
        }

        //SetPlayerPositions();

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {
                switch (mapGrid[x, y].type)
                {
                    //TODO(vosure): Add flowers!
                    case (MapObjectType.Box):
                        {
                            GameObject obj = Instantiate(boxPrefab, mapGrid[x, y].position, Quaternion.identity);
                            SetRandonRotation(obj);
                            obj.transform.parent = mapHolder;

                            Vector3 floorPosition = new Vector3(mapGrid[x, y].position.x, 0, mapGrid[x, y].position.z);
                            SetRandomMaterial(floorPrefab, floorMaterials);
                            GameObject newTile = Instantiate(floorPrefab, floorPosition, Quaternion.Euler(Vector3.right * 90));
                            newTile.transform.localScale = Vector3.one * (1 - outlinePersent);
                            newTile.transform.parent = mapHolder;

                            break;
                        }
                    case (MapObjectType.Obstacle):
                        {
                            SetRandomMaterial(obstaclePrefab, obstacleMaterials);
                            GameObject obj = Instantiate(obstaclePrefab, mapGrid[x, y].position, Quaternion.identity);
                            obj.transform.parent = mapHolder;

                            Vector3 floorPosition = new Vector3(mapGrid[x, y].position.x, 0, mapGrid[x, y].position.z);
                            SetRandomMaterial(floorPrefab, floorMaterials);
                            GameObject newTile = Instantiate(floorPrefab, floorPosition, Quaternion.Euler(Vector3.right * 90));
                            newTile.transform.localScale = Vector3.one * (1 - outlinePersent);
                            newTile.transform.parent = mapHolder;

                            break;
                        }
                    case (MapObjectType.Empty):
                        {
                            Vector3 floorPosition = new Vector3(mapGrid[x, y].position.x, 0, mapGrid[x, y].position.z);
                            SetRandomMaterial(floorPrefab, floorMaterials);
                            GameObject newTile = Instantiate(floorPrefab, floorPosition, Quaternion.Euler(Vector3.right * 90));
                            newTile.transform.localScale = Vector3.one * (1 - outlinePersent);
                            newTile.transform.parent = mapHolder;

                            break;
                        }
                    case (MapObjectType.Decoration):
                        {
                            GameObject decorationBlock = Instantiate(decorationPrefab, mapGrid[x, y].position, Quaternion.identity);
                            decorationBlock.transform.parent = mapHolder;

                            if (Random.value < 0.5)
                            {
                                Vector3 decorationOjbectPositions = new Vector3(mapGrid[x, y].position.x, 1.0f, mapGrid[x, y].position.z);
                                //TODO(vosure): Fix zero elements array!
                                GameObject decorationOjbect = Instantiate(decorationObjects[Random.Range(0, decorationObjects.Length)], decorationOjbectPositions, Quaternion.identity);
                                decorationOjbect.transform.parent = mapHolder;
                            }
                            break;
                        }
                }
            }
        }
    }

    private void SetRandomMaterial(GameObject prefab, Material[] materials)
    {
        prefab.GetComponent<Renderer>().material = materials[Random.Range(0, materials.Length)];
    }

    private void SetRandonRotation(GameObject prefab)
    {
        float[] zAxisDegree = { 0.0f, 90.0f, 180.0f, 270.0f };
        prefab.transform.rotation = Quaternion.Euler(00.0f, zAxisDegree[Random.Range(0, 4)], 0);
    }

    private void InstantiateOject(int x, int y)
    {
        //TODO(vosure): Collapse
        Vector3 position = new Vector3(-mapSize.x / 2.0f + 0.5f + x, 0.5f, -mapSize.y / 2.0f + 0.5f + y);
        if (x >= decorationAreaSize && x <= mapSize.x - decorationAreaSize && y >= decorationAreaSize && y <= mapSize.y - decorationAreaSize)
        {
            if (x == decorationAreaSize || x == mapSize.x - decorationAreaSize || y == decorationAreaSize || y == mapSize.y - decorationAreaSize)
            {
                if ((y >= 2 + decorationAreaSize && y <= mapSize.y - 2 - decorationAreaSize) || (x >= 2 + decorationAreaSize && x <= mapSize.x - 2 - decorationAreaSize))
                {
                    if (Random.Range(0.0f, 100.0f) > (100 - objectsOnMapPercent))
                    {
                        //SetRandomMaterial(boxPrefab, boxMaterials);
                        //GameObject obj = Instantiate(boxPrefab, position, Quaternion.identity);
                        //SetRandonRotation(obj);
                        //obj.transform.parent = mapHolder;

                        mapGrid[x, y] = new MapObject(position, MapObjectType.Box, true);
                    }
                }
                else
                {
                    mapGrid[x, y] = new MapObject(position, MapObjectType.Empty, true);
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
                            //SetRandomMaterial(obstaclePrefab, obstacleMaterials);
                            //GameObject obj = Instantiate(obstaclePrefab, position, Quaternion.identity);
                            //obj.transform.parent = mapHolder;
                            mapGrid[x, y] = new MapObject(position, MapObjectType.Obstacle, true);
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
                            //GameObject obj = Instantiate(boxPrefab, position, Quaternion.identity);
                            //SetRandonRotation(obj);
                            //obj.transform.parent = mapHolder;
                            mapGrid[x, y] = new MapObject(position, MapObjectType.Box, true);
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
                        //GameObject obj = Instantiate(boxPrefab, position, Quaternion.identity);
                        //SetRandonRotation(obj);
                        //obj.transform.parent = mapHolder;
                        mapGrid[x, y] = new MapObject(position, MapObjectType.Box, true);
                    }
                }
            }
        }
        else
        {
            mapGrid[x, y] = new MapObject(position, MapObjectType.Decoration, true);
        }
    }


    private void GenerateWallsAround()
    {
        Vector3 bottomLeft = new Vector3(-mapSize.x / 2.0f - 0.5f + decorationAreaSize, 0.5f, -mapSize.y / 2.0f - 0.5f + decorationAreaSize);
        Vector3 bottomRight = new Vector3(mapSize.x / 2.0f + 1.5f - decorationAreaSize, 0.5f, -mapSize.y / 2.0f - 0.5f + decorationAreaSize);
        Vector3 topLeft = new Vector3(-mapSize.x / 2.0f - 0.5f + decorationAreaSize, 0.5f, mapSize.y / 2.0f + 1.5f - decorationAreaSize);
        Vector3 topRight = new Vector3(mapSize.x / 2.0f + 1.5f - decorationAreaSize, 0.5f, mapSize.y / 2.0f + 1.5f - decorationAreaSize);

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
            Vector3 position = Vector3.Lerp(From, To, i / (mapSize.x - decorationAreaSize * 2 + 2));

            //SetRandomMaterial(prefab, wallMaterials);
            //GameObject newBlock = Instantiate(prefab, position, Quaternion.identity);
            //newBlock.transform.parent = mapHolder;
        }
    }

    public enum MapObjectType
    {
        Empty,
        Obstacle,
        Box,
        Wall,
        Decoration,
    }

    public struct MapObject
    {
        public Vector3 position;
        public MapObjectType type;
        public bool hasFloor;

        public MapObject(Vector3 position, MapObjectType type, bool hasFloor)
        {
            this.position = position;
            this.type = type;
            this.hasFloor = hasFloor;
        }
    }
}


