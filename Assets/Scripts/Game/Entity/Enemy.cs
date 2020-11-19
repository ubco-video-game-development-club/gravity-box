using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float Health { get { return currentHealth; } }

    [SerializeField] private float maxHealth = 10.0f;
    [SerializeField] private HealthBar healthBarPrefab;
    [SerializeField] private float healthBarYOffset = -0.5f;
    [SerializeField] private int pointsValue = 75;
    [SerializeField] private UnityEvent onDeath = new UnityEvent();

    private float currentHealth;

    protected void Awake()
    {
        currentHealth = maxHealth;

        // Initialize health bar
        Vector3 healthBarSpawn = transform.position + Vector3.up * healthBarYOffset;
        HealthBar healthBar = Instantiate(healthBarPrefab, healthBarSpawn, Quaternion.identity, HUD.Singleton.HealthBarParent);
        
    }

    public void TakeDamage(float damage)
    {
        if(damage < 0f) return; //Can't to negative damage
        currentHealth -= damage;

        if (currentHealth <= 0.0f)
        {
            GameManager.ScoreSystem.AddScore(pointsValue);
            onDeath.Invoke();
            Destroy(gameObject);
        }
    }

    public void AddOnDeathListener(UnityAction listener)
    {
        onDeath.AddListener(listener);
    }

    public void RemoveOnDeathListener(UnityAction listener)
    {
        onDeath.RemoveListener(listener);
    }

}
