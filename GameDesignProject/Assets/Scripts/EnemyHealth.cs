using System;
using UnityEngine;
using Weapon.CombatTypes;


public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 60f;
    [SerializeField] private float currentHealth;
    [SerializeField] private EnemyType enemyType = EnemyType.Normal;
    private DamageInfo lastDamageInfo;


    public float MaxHealth => maxHealth;

    //properties -- getter section
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0f;
    public EnemyType EnemyType => enemyType;

    public event Action<float, float> OnHealthChanged;
    public event Action<DeathInfo> OnDeath;

    public EnemyDeathPickupSpawner pickupSpawner;


    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(DamageInfo damage)
    {
        if (IsDead) return;

        currentHealth = Mathf.Max(0f, currentHealth - damage.Amount);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        lastDamageInfo = damage;

        if (IsDead)
            Die();
    }

    private void Die()
    {
        if (pickupSpawner != null)
        {
            pickupSpawner.spawnRandomPickup();
        }

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
