using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public float Health { get { return currentHealth; } }

    [SerializeField] private float maxHealth = 10.0f;
    [SerializeField] private UnityEvent onDie;

    private float currentHealth;

    void Update() 
    {
        if (currentHealth <= 0.0f)
        {
            onDie.Invoke();
            Destroy(gameObject);
        }
    }

    public void AddOnDieListener(UnityAction listener)
    {
        onDie.AddListener(listener);
    }

    public void RemoveOnDieListener(UnityAction listener)
    {
        onDie.RemoveListener(listener);
    }

}
