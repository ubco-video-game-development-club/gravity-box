using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TriangleEnemy : Enemy
{
    [SerializeField] private float launchForce = 5.0f;
    [SerializeField] private float constantVelocity = 10.0f;
    [SerializeField] private int damage = 1;
    private Rigidbody2D rig;

    new protected void Awake()
    {
        base.Awake();
        rig = GetComponent<Rigidbody2D>();
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
    }
    
    void OnCollisionEnter2D(Collision2D col)
    {
        if(col.transform.TryGetComponent<Player>(out Player player))
        {
            player.TakeDamage(1);
        }
    }
}
