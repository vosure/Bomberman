using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public PlayerCamera playerCamera;

    private Rigidbody rigidBody;
    private Animator animator;
    private Player player;

    private bool fliped;

    void Start()
    {
        if (Mathf.Sign(transform.position.z) == 1)
            fliped = true;

        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    void Update()
    {
        UpdateMovement();
    }

    private void UpdateMovement()
    {
        animator.SetBool("Walking", false);

        if (Input.GetKey(KeyCode.W))
        {
            if (!fliped)
                MoveUp();
            else
                MoveDown();
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (!fliped)
                MoveDown();
            else
                MoveUp();
        }

        if (Input.GetKey(KeyCode.A))
        {
            if (!fliped)
                MoveLeft();
            else
                MoveRight();
        }

        if (Input.GetKey(KeyCode.D))
        {
            if (!fliped)
                MoveRight();
            else
                MoveLeft();
        }
    }

    private void MoveUp()
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, player.movementSpeed);
        transform.rotation = Quaternion.Euler(0, 0, 0);
        playerCamera.UpdateRotation();
        animator.SetBool("Walking", true);
    }

    private void MoveDown()
    {
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, -player.movementSpeed);
        transform.rotation = Quaternion.Euler(0, 180, 0);
        playerCamera.UpdateRotation();

        animator.SetBool("Walking", true);
    }

    private void MoveLeft()
    {
        rigidBody.velocity = new Vector3(-player.movementSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
        transform.rotation = Quaternion.Euler(0, 270, 0);
        playerCamera.UpdateRotation();

        animator.SetBool("Walking", true);
    }

    private void MoveRight()
    {
        rigidBody.velocity = new Vector3(player.movementSpeed, rigidBody.velocity.y, rigidBody.velocity.z);
        transform.rotation = Quaternion.Euler(0, 90, 0);
        playerCamera.UpdateRotation();

        animator.SetBool("Walking", true);
    }
}
