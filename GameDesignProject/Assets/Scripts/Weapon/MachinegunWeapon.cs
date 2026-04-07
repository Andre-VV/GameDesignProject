using UnityEngine;
using Weapon.CombatTypes;

public class MachinegunWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private string weaponName = "Machinegun";
    [SerializeField] private bool supportsHoldFire = true;
    [SerializeField] private float damage = 7f;
    [SerializeField] private float fireRate = 0.12f;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 18f;
    [SerializeField] private float projectileLifeTime = 1.2f;

    private GameObject owner;
    private float lastFireTime;

    public string WeaponName => weaponName;
    public WeaponType WeaponType => WeaponType.Ranged;
    public bool SupportsHoldFire => supportsHoldFire;

    public void OnEquip(GameObject owner)
    {
        this.owner = owner;
    }

    public void OnUnequip()
    {
        owner = null;
    }

    public bool TryFire(Vector2 origin, Vector2 direction)
    {
        if (!CanFire()) return false;
        if (projectilePrefab == null) return false;
        if (direction.sqrMagnitude <= 0.0001f) return false;

        SpawnProjectile(origin, direction.normalized);
        lastFireTime = Time.time;
        return true;
    }

    private bool CanFire()
    {
        return Time.time >= lastFireTime + fireRate;
    }

    private void SpawnProjectile(Vector2 origin, Vector2 direction)
    {
        Projectile projectileInstance = Instantiate(projectilePrefab, origin, Quaternion.identity);
        projectileInstance.Initialize(direction, damage, projectileSpeed, projectileLifeTime, targetLayers, owner);
    }
}
