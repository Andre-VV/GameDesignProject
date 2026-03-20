using System;
using UnityEngine;

public class playerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField]  private float currentHealth;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;

    public event Action<float, float> OnHealthChanged;
    public event Action OnDeath;

    void Start() => currentHealth = maxHealth;

    // Update is called once per frame
    public void TakeDamage(DamageInfo damage)
    {
        Debug.Log("Player took damage");
        if (IsDead) return;
        currentHealth = Mathf.Max(0, currentHealth - damage.Amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        if (IsDead) Die();
        
    }

    private void Die()
    {
        Debug.Log("you are killed");
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
