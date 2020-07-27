using UnityEngine;
using UnityEngine.Networking;

public class MapGenerator : MonoBehaviour
{
    public GameObject[] floorPrefabs;

    public GameObject[] obstaclePrefabs;

    public GameObject boxPrefab;

    public GameObject[] decorationBlockPrefabs;
    public GameObject[] decorationObjects;

    public Vector2 mapSize;

    //NOTE(vosure): Do I actually need that?!
    [Range(0, 1)]
    public float outlinePersent;

    [Range(0, 100)]
    public float objectsOnMapPercent = 100.0f;


    public GameObject[] playerPositions;

    public int decorationAreaSize = 4;

    public bool spawnDecorations = false;

    private Transform mapHolder;
    //TODO(vosure): change to one dimensional array
    private MapObject[,] mapGrid;


    void Start()
    {
        //NOTE(vosure): Probably I should use int width and height, instead of Vector2 mapSize!?
        //GenerateMap();
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

        SetPlayerPositions();

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

                            

                            SpawnFloorOnPosition(mapGrid[x, y].position);

                            break;
                        }
                    case (MapObjectType.Obstacle):
                        {
                            //SetRandomMaterial(obstaclePrefab, obstacleMaterials);
                            GameObject obj = Instantiate(obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)], mapGrid[x, y].position, Quaternion.identity);
                            obj.transform.parent = mapHolder;

                            SpawnFloorOnPosition(mapGrid[x, y].position);

                            break;
                        }
                    case (MapObjectType.Empty):
                        {
                            SpawnFloorOnPosition(mapGrid[x, y].position);

                            break;
                        }
                    case (MapObjectType.Decoration):
                        {
                            if (spawnDecorations)
                            {
                                GameObject decorationBlock = Instantiate(decorationBlockPrefabs[Random.Range(0, decorationBlockPrefabs.Length)], mapGrid[x, y].position, Quaternion.identity);
                                decorationBlock.transform.parent = mapHolder;

                                if (Random.value < 0.5)
                                {
                                    Vector3 decorationOjbectPositions = new Vector3(mapGrid[x, y].position.x, 1.0f, mapGrid[x, y].position.z);
                                    //TODO(vosure): Fix zero elements array!
                                    GameObject decorationOjbect = Instantiate(decorationObjects[Random.Range(0, decorationObjects.Length)], decorationOjbectPositions, Quaternion.identity);
                                    decorationOjbect.transform.parent = mapHolder;
                                }
                            }
                            break;
                        }
                }
            }
        }
    }

    private void SpawnFloorOnPosition(Vector3 position)
    {
        Vector3 floorPosition = new Vector3(position.x, 0.0f, position.z);
        GameObject newTile = Instantiate(floorPrefabs[Random.Range(0, floorPrefabs.Length)], floorPosition, Quaternion.Euler(Vector3.right * 90));
        newTile.transform.localScale = Vector3.one * (1.0f - outlinePersent);
        newTile.transform.parent = mapHolder;
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
                    if (ShouldSpawn())
                    {
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
                    if (ShouldSpawn())
                    {
                        mapGrid[x, y] = new MapObject(new Vector3(position.x, 0.0f, position.z), MapObjectType.Obstacle, true);
                    }

                }
                else
                {

                    if (ShouldSpawn())
                    {
                        mapGrid[x, y] = new MapObject(position, MapObjectType.Box, true);
                    }

                }
            }
            else
            {

                if (ShouldSpawn())
                {
                    mapGrid[x, y] = new MapObject(position, MapObjectType.Box, true);
                }
            }
        }
        else
        {
            mapGrid[x, y] = new MapObject(position, MapObjectType.Decoration, true);
        }
    }

    private bool ShouldSpawn()
    {
        return (Random.Range(0.0f, 100.0f) > (100 - objectsOnMapPercent));
    }

    private void SetPlayerPositions()
    {
        Vector3 bottomLeft = new Vector3(-mapSize.x / 2.0f - 0.5f + decorationAreaSize, 0.5f, -mapSize.y / 2.0f - 0.5f + decorationAreaSize);
        Vector3 bottomRight = new Vector3(mapSize.x / 2.0f + 1.5f - decorationAreaSize, 0.5f, -mapSize.y / 2.0f - 0.5f + decorationAreaSize);
        Vector3 topLeft = new Vector3(-mapSize.x / 2.0f - 0.5f + decorationAreaSize, 0.5f, mapSize.y / 2.0f + 1.5f - decorationAreaSize);
        Vector3 topRight = new Vector3(mapSize.x / 2.0f + 1.5f - decorationAreaSize, 0.5f, mapSize.y / 2.0f + 1.5f - decorationAreaSize);

        playerPositions[0].transform.position = bottomLeft + new Vector3(1.0f, 0.0f, 1.0f);
        playerPositions[1].transform.position = bottomRight + new Vector3(-1.0f, 0.0f, 1.0f);
        playerPositions[2].transform.position = topLeft + new Vector3(1.0f, 0.0f, -1.0f);
        playerPositions[3].transform.position = topRight + new Vector3(-1.0f, 0.0f, -1.0f);
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