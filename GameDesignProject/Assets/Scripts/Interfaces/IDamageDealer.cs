using UnityEngine;

public interface IDamageDealer
{
    float Damage { get; }
    float AttackRate { get; }
    LayerMask TargetLayers { get; }
    void Attack();
}
