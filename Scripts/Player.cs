using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    public PlayerCamera playerCamera;

    public int explosions = 1;
    public float movementSpeed = 2.5f;
    public int bombs = 1;
    public bool canKick = false;

    public LayerMask collisionMask;

    [SyncVar]
    private bool isDead = false;
    public bool IsDead
    {
        get { return isDead; }
        protected set { isDead = value; }
    }

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectsOnDeaths;

    //[SerializeField]
    //private GameObject deathEffect;

    //[SerializeField]
    //private GameObject spawnEffect;

    private bool firstSetup = true;

    public GameObject canvas;

    public Text bombText;
    public Text speedText;
    public Text fireText;
    public Text kickText;

    private const RigidbodyConstraints moveAlongX =
        RigidbodyConstraints.FreezeRotationX |
        RigidbodyConstraints.FreezeRotationY |
        RigidbodyConstraints.FreezeRotationZ |
        RigidbodyConstraints.FreezePositionY |
        RigidbodyConstraints.FreezePositionZ;

    private const RigidbodyConstraints moveAlongZ =
        RigidbodyConstraints.FreezeRotationX |
        RigidbodyConstraints.FreezeRotationY |
        RigidbodyConstraints.FreezeRotationZ |
        RigidbodyConstraints.FreezePositionY |
        RigidbodyConstraints.FreezePositionX;

    private void Start()
    {
        if (isLocalPlayer)
            canvas.SetActive(true);
    }

    private void Update()
    {
        if (canKick)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, transform.forward, out hit, 1.0f, collisionMask);

            if (hit.collider)
            {
                if (hit.collider.gameObject.CompareTag("Bomb"))
                {
                    GameObject bomb = hit.collider.gameObject;
                    bomb.GetComponentInParent<Rigidbody>().isKinematic = false;
                    KickBomb(bomb);
                }
            }
        }

        UpdadateUI();
    }

    void UpdadateUI()
    {
        bombText.text = "Кол-во бомб - " + bombs;
        fireText.text = "Сила взрыва - " + explosions;
        speedText.text = "Скорость передвижения - " + movementSpeed;
        kickText.text = "Возможность пинать -" + (canKick ? "Да" : "Нет");
    }

    private void KickBomb(GameObject bomb)
    {
        bool freezeX = transform.forward.z == 1.0f || (transform.forward.z == -1.0f) ? true : false;
        bomb.GetComponentInParent<Rigidbody>().constraints = freezeX ? moveAlongZ : moveAlongX;
    }

    public void SetupPlayer()
    {
        if (isLocalPlayer)
        {
            GameManager.singleton.SetSceneCameraActice(false);
        }

        CmdBroadcastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }

            firstSetup = false;
        }


        SetDefaults();
    }

    [ClientRpc]
    public void RpcTakeDamage()
    {
        Die();
    }

    [ClientRpc]
    public void RpcApplyPowerUp(int powerUpType)
    {
        switch (powerUpType)
        {
            case 0:
                {
                    explosions++;
                    break;
                }
            case 1:
                {
                    bombs++;
                    GetComponent<PlayerDropBomb>().bombsAvailable++;
                    break;
                }
            case 2:
                {
                    movementSpeed += 0.5f;
                    break;
                }
            case 3:
                {
                    canKick = true;
                    break;
                }
        }
    }

    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameObjectsOnDeaths.Length; i++)
        {
            disableGameObjectsOnDeaths[i].SetActive(false);
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        //GameObject deathEffectInstance = Instantiate(deathEffect, transform.position, Quaternion.identity);
        //Destroy(deathEffectInstance, 3.0f);

        if (isLocalPlayer)
        {
            GameManager.singleton.SetSceneCameraActice(true);
        }

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2.0f);

        Transform startPosition = NetworkManager.singleton.GetStartPosition();
        transform.position = startPosition.position;
        transform.rotation = startPosition.rotation;

        yield return new WaitForSeconds(0.1f);

        SetupPlayer();
        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        for (int i = 0; i < disableGameObjectsOnDeaths.Length; i++)
        {
            disableGameObjectsOnDeaths[i].SetActive(true);
        }

        Collider collider = GetComponent<Collider>();
        if (collider != null)
            collider.enabled = true;

        //playerCamera.UpdateRotation();

        //GameObject spawnEffectInstance = Instantiate(spawnEffect, transform.position, Quaternion.identity);
        //Destroy(spawnEffectInstance, 3.0f);
    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.collider.CompareTag("Bomb"))
    //    {
    //        Debug.Log("Hit the bomb");
    //    }
    //}

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Explosion"))
        {
            GameManager.GetPlayer(gameObject.name).RpcTakeDamage();
            Debug.Log("hit by explosion");
        }
        if (other.CompareTag("PowerUp"))
        {
            PowerUp powerUp = other.gameObject.GetComponent<PowerUp>();
            NetworkServer.Destroy(other.gameObject);

            Debug.Log("Took Power Up");

            GameManager.GetPlayer(gameObject.name).RpcApplyPowerUp((int)powerUp.type);
        }
        //if (other.CompareTag("Bomb"))
        //{
        //    Debug.Log("position on the bomb" + other.gameObject.GetComponentInParent<Transform>().position);
        //}
    }
}
