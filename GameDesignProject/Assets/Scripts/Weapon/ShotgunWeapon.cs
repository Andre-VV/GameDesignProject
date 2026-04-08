using UnityEngine;
using Weapon.CombatTypes;

public class ShotgunWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private string weaponName = "Shotgun";
    [SerializeField] private bool supportsHoldFire = false;
    [SerializeField] private float damagePerPellet = 10f;
    [SerializeField] private float fireRate = 0.8f;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 12f;
    [SerializeField] private float projectileLifeTime = 1.0f;
    [SerializeField] private int pelletCount = 3;
    [SerializeField] private float spreadAngle = 10f;

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

        SpawnPellets(origin, direction.normalized);
        lastFireTime = Time.time;
        return true;
    }

    private bool CanFire()
    {
        return Time.time >= lastFireTime + fireRate;
    }

    private void SpawnPellets(Vector2 origin, Vector2 baseDirection)
    {
        int safePelletCount = Mathf.Max(1, pelletCount);
        float startAngle = -spreadAngle * (safePelletCount - 1) * 0.5f;

        for (int i = 0; i < safePelletCount; i++)
        {
            float angleOffset = startAngle + (spreadAngle * i);
            Vector2 pelletDirection = RotateDirection(baseDirection, angleOffset);
            SpawnProjectile(origin, pelletDirection);
        }
    }

    private Vector2 RotateDirection(Vector2 direction, float degrees)
    {
        float radians = degrees * Mathf.Deg2Rad;
        float sin = Mathf.Sin(radians);
        float cos = Mathf.Cos(radians);

        return new Vector2(
            direction.x * cos - direction.y * sin,
            direction.x * sin + direction.y * cos);
    }

    private void SpawnProjectile(Vector2 origin, Vector2 direction)
    {
        Projectile projectileInstance = Instantiate(projectilePrefab, origin, Quaternion.identity);
        projectileInstance.Initialize(
            direction,
            damagePerPellet,
            projectileSpeed,
            projectileLifeTime,
            targetLayers,
            owner);
    }
}
