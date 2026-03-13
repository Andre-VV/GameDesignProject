using UnityEngine;

public interface IWeapon : IDamageDealer
{
    string WeaponName { get; }
    WeaponType WeaponType { get; }
    void OnEquip(GameObject owner);
    void OnUnequip();
}
