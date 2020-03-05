using UnityEngine;
using System.Collections;

public class GlobalStateManager : MonoBehaviour
{
    private int deadPlayers = 0;
    private int deadPlayerNumber = -1;

    public void PlayerDied(int playerNumber)
    {
        deadPlayers++;

        if (deadPlayers == 1)
        {
            deadPlayerNumber = playerNumber;
            Invoke("CheckPlayersDeath", 0.3f);
        }
    }

    public void CheckPlayerDeath()
    {
        if (deadPlayers == 1)
        {
            if (deadPlayerNumber == 1)
            {
                // TODO(vosure): Should be GUI
                Debug.Log("Player 2 is the winner!");
            }
            else
            {
                // TODO(vosure): Should be GUI
                Debug.Log("Player 1 is the winner!");
            }
        }
        else
        {
            // TODO(vosure): Should be GUI
            Debug.Log("Draw!");
        }
    }

}
