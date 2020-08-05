using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
        }
        else
        {
            GetComponent<Player>().SetupPlayer();
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        string netID = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netID, player);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    private void OnDisable()
    {
        if (isLocalPlayer)
        {
            GameManager.singleton.SetSceneCameraActice(true);
        }

        GameManager.DeRegisterPlayer(transform.name);
    }
}
