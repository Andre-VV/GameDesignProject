using System;
using UnityEngine;

public interface IDamageable
{
    float MaxHealth { get; }
    float CurrentHealth { get; }
    bool IsDead { get; }
    void TakeDamage(DamageInfo damage);
    event Action<float, float> OnHealthChanged; // (current, max)
    event Action OnDeath;

}
