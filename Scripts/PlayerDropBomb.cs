using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDropBomb : MonoBehaviour
{
    public GameObject bombPrefab;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DropBomb();
        }
    }

    private void DropBomb()
    {
        if (bombPrefab)
        {
            Instantiate(bombPrefab,
                new Vector3(Mathf.Round(transform.position.x + 0.5f) - 0.5f, bombPrefab.transform.position.y, Mathf.RoundToInt(transform.position.z + 0.5f) - 0.5f),
                bombPrefab.transform.rotation);
        }
    }
}
