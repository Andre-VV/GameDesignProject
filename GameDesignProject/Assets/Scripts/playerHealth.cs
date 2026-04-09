using System;
using UnityEngine;
using Weapon.CombatTypes;
using TMPro;


public class playerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    [SerializeField]  private float currentHealth;

    private DamageInfo lastDamageInfo;

    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0;

    public event Action<float, float> OnHealthChanged;
    public event Action<DeathInfo> OnDeath;

    //Display health in UI
    public TextMeshProUGUI healthText;


    void Start()
    {
        currentHealth = maxHealth;
        // Initialize health display
        healthText.text = currentHealth.ToString();
    }

    // Update is called once per frame
    public void TakeDamage(DamageInfo damage)
    {
        Debug.Log("Player took damage");
        if (IsDead) return;
        currentHealth = Mathf.Max(0, currentHealth - damage.Amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        lastDamageInfo = damage;
        if (IsDead) Die();

        // Update health display
        healthText.text = currentHealth.ToString();

    }

    private void Die()
    {
        Debug.Log("you are killed");
        DeathInfo deathInfo = new DeathInfo
        {
            Victim = gameObject,
            Killer = lastDamageInfo.Source,
            FinalDamage = lastDamageInfo
        };
        
        OnDeath?.Invoke(deathInfo);
        Destroy(gameObject);
    }
}
