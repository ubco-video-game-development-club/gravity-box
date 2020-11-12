using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TriangleEnemy : Enemy
{
    [SerializeField] private float launchForce = 5.0f;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player == null)
        {
            Debug.LogWarning("Object with tag \"Player\" not found.");
            return;
        }

        Vector2 toPlayer = (player.transform.position - this.transform.position).normalized;
        GetComponent<Rigidbody2D>().AddForce(toPlayer * launchForce, ForceMode2D.Impulse);
    }
}
