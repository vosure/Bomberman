using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager singleton;
    public GameObject sceneCamera;

    private void Awake()
    {
        if (singleton != null)
        {
            Debug.Log("More than one GameManager");
        }
        else
        {
            singleton = this;
        }
    }

    public void SetSceneCameraActice(bool isActive)
    {
        if (sceneCamera == null)
            return;

        sceneCamera.SetActive(isActive);
    }

    private const string PLAYER_ID_PREFIX = "Player";

    private static Dictionary<string, Player> players = new Dictionary<string, Player>();

    public static void RegisterPlayer(string netID, Player player)
    {
        string playerID = PLAYER_ID_PREFIX + netID;
        players.Add(playerID, player);
        player.transform.name = playerID;
    }

    public static void DeRegisterPlayer(string playerID)
    {
        players.Remove(playerID);
    }

    public static Player GetPlayer(string playerID)
    {
        return players[playerID];
    }
}
