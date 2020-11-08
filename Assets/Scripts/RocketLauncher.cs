﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour
{
    [SerializeField] private Rigidbody2D playerBody;
    [SerializeField] private Rocket rocketPrefab;
    [SerializeField] private float rocketCooldown = 0.5f;
    [SerializeField] private float recoilStrength = 1;

    private bool canFireRocket = true;
    private Vector2 mouseDir = Vector2.right;

    private new Transform transform;

    void Awake()
    {
        // Make sure the player reference is set, otherwise default to the root GameObject.
        CheckPlayer();

        transform = GetComponent<Transform>();
    }

    void Update()
    {
        // Update mouse direction
        mouseDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        // Fire rocket on click
        if (Input.GetButton("Fire1") && canFireRocket)
        {
            // Start rocket cooldown
            StartCoroutine(RocketCooldown());

            // Spawn rocket
            Rocket rocket = Instantiate(rocketPrefab, transform.position, Quaternion.identity);
            rocket.Direction = mouseDir;

            // Apply recoil force to player
            playerBody.AddForce(recoilStrength * -mouseDir, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Look at the mouse position
        transform.rotation = Quaternion.FromToRotation(Vector2.right, mouseDir);
    }

    private IEnumerator RocketCooldown()
    {
        canFireRocket = false;
        yield return new WaitForSeconds(rocketCooldown);
        canFireRocket = true;
    }

    private void CheckPlayer()
    {
        if (playerBody == null)
        {
            Debug.Log("Warning: Player reference not assigned to Rocket Launcher. Defaulting to root GameObject.");
            Rigidbody2D rootRB2D;
            if (transform.root.TryGetComponent<Rigidbody2D>(out rootRB2D))
            {
                playerBody = rootRB2D;
            }
            else
            {
                Debug.Log("Error: Failed to find valid root GameObject. Make sure the rocket launcher is a child of the player, and that the player has a Rigidbody2D component.");
            }
        }
    }
}
