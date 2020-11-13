﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TriangleEnemy : Enemy
{
    [SerializeField] private float launchForce = 5.0f;
    [SerializeField] private float constantVelocity = 10.0f;
    [SerializeField] private int damage = 1;
    [SerializeField] private GameObject deathParticles;
    private Rigidbody2D rig;

    new protected void Awake()
    {
        base.Awake();
        rig = GetComponent<Rigidbody2D>();
        this.AddOnDieListener(OnDie);
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.LogWarning("Object with tag \"Player\" not found.");
            return;
        }

        Vector2 toPlayer = (player.transform.position - this.transform.position).normalized;
        rig.AddForce(toPlayer * launchForce, ForceMode2D.Impulse);
    }

    void FixedUpdate()
    {
        Vector2 direction = rig.velocity.normalized;
        rig.velocity = direction * constantVelocity;
        transform.rotation = Quaternion.FromToRotation(Vector2.up, direction);
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(damage);
        }
    }

    public void OnDie()
    {
        Instantiate(deathParticles, transform.position, Quaternion.identity);
    }

}
