using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private float speed = 1;
    [SerializeField] private float explosionStrength = 1;
    [SerializeField] private float explosionRadius = 1;

    public Vector2 Direction
    {
        get { return direction; }
        set
        {
            direction = value.normalized;
            ApplyVelocity(direction * speed);
        }
    }
    private Vector2 direction = Vector2.right;

    private new Rigidbody2D rigidbody2D;

    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void ApplyVelocity(Vector2 velocity)
    {
        transform.rotation = Quaternion.FromToRotation(Vector2.right, velocity);
        rigidbody2D.velocity = velocity;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        // Apply explosion to all targets within range
        Vector3 explosionPos = transform.position;
        Collider2D[] targets = Physics2D.OverlapCircleAll(explosionPos, explosionRadius);
        foreach (Collider2D target in targets)
        {
            Debug.Log(target.name);

            Rigidbody2D targetBody;
            if (target.TryGetComponent<Rigidbody2D>(out targetBody))
            {
                Vector2 forceOffset = targetBody.transform.position - explosionPos;
                Vector2 forceDir = forceOffset.normalized;
                float forceDist = forceOffset.magnitude;
                float forceScale = 1 - (forceDist / explosionRadius);
                targetBody.AddForce(forceScale * forceDir * explosionStrength, ForceMode2D.Impulse);
            }
        }

        // Spawn explosion effect
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);

        // Self destruct
        Destroy(gameObject);
    }
}
