using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        Fire,
        Bomb,
        Speed,
        Kick
    }

    public Texture2D fireTexture;
    public Texture2D bombTexture;
    public Texture2D speedTexture;
    public Texture2D bombKickTexture;

    public PowerUpType type;

    public void CreateFirePowerUp()
    {
        GetComponent<Renderer>().material.mainTexture = fireTexture;
        type = PowerUpType.Fire;
    }
    public void CreateSpeedPowerUp()
    {
        GetComponent<Renderer>().material.mainTexture = speedTexture;
        type = PowerUpType.Speed;
    }
    public void CreateBombPowerUp()
    {
        GetComponent<Renderer>().material.mainTexture = bombTexture;
        type = PowerUpType.Bomb;
    }
    public void CreateKickPowerUp()
    {
        GetComponent<Renderer>().material.mainTexture = bombKickTexture;
        type = PowerUpType.Kick;
    }
}
