using UnityEngine;
using Weapon.CombatTypes;

public interface IWeapon
{
    string WeaponName { get; }
    WeaponType WeaponType { get; }
    bool SupportsHoldFire { get; }

    void OnEquip(GameObject owner);
    void OnUnequip();
    bool TryFire(Vector2 origin, Vector2 direction);
}
