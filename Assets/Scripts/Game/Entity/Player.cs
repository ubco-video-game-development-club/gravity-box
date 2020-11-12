using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [System.Serializable] public class OnHealthChangedEvent : UnityEvent<int> { }
    [System.Serializable] public class OnDeathEvent : UnityEvent { }

    public int MaxHealth { get { return maxHealth; } }
    [SerializeField] private int maxHealth = 5;

    [SerializeField] private float invincibilityFrame = 0.5f;
    [SerializeField] private OnHealthChangedEvent onHealthChanged;
    [SerializeField] private OnDeathEvent onDeath;

    private int currentHealth = 0;
    private bool isInvincible = false;

    private new Rigidbody2D rigidbody2D;
    private YieldInstruction invincibilityFrameInstruction;

    void Awake()
    {
        onHealthChanged = new OnHealthChangedEvent();
        rigidbody2D = GetComponent<Rigidbody2D>();
        invincibilityFrameInstruction = new WaitForSeconds(invincibilityFrame);
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            onDeath.Invoke();
        }

        StartCoroutine(InvincibilityFrame());

        onHealthChanged.Invoke(currentHealth);
    }

    private IEnumerator InvincibilityFrame()
    {
        isInvincible = true;
        yield return invincibilityFrameInstruction;
        isInvincible = false;
    }

    public void ApplyKnockback(Vector2 knockbackForce)
    {
        rigidbody2D.AddForce(knockbackForce);
    }

    public void AddHealthChangedListener(UnityAction<int> call) 
    {
        onHealthChanged.AddListener(call);
    }

    public void RemoveHealthChangedListener(UnityAction<int> call) 
    {
        onHealthChanged.RemoveListener(call);
    }

    public void AddDeathListener(UnityAction call) 
    {
        onDeath.AddListener(call);
    }

    public void RemoveDeathListener(UnityAction call) 
    {
        onDeath.RemoveListener(call);
    }
}
