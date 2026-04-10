using TMPro;
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

    public AudioSource FireSound;

    public string WeaponName => weaponName;
    public WeaponType WeaponType => WeaponType.Ranged;
    public bool SupportsHoldFire => supportsHoldFire;

    public TextMeshProUGUI WeaponDisplay;

    public TextMeshProUGUI AmmoDisplay;

    public int AmmoCount = 6; // -1 for infinite ammo

    private int currentAmmo;

    public void OnEquip(GameObject owner)
    {
        this.owner = owner;
        currentAmmo = AmmoCount;
        if (WeaponDisplay == null)
        {
            WeaponDisplay = GameObject.Find("Weapon2Name").GetComponent<TextMeshProUGUI>();
        }
        if (AmmoDisplay == null)
        {
            AmmoDisplay = GameObject.Find("Weapon2Ammo").GetComponent<TextMeshProUGUI>();
        }
        if (WeaponDisplay != null)
        {
            WeaponDisplay.text = $"{WeaponName}";
        }
        if (AmmoDisplay != null)
        {
            if (AmmoCount < 0)
            {
                AmmoDisplay.text = "Inf/Inf";
            }
            else
            {
                AmmoDisplay.text = $"{currentAmmo} / {AmmoCount}";
            }

        }
    }

    public void OnUnequip()
    {
        owner = null;
        if (WeaponDisplay != null)
            WeaponDisplay.text = "";

        if (AmmoDisplay != null)
            AmmoDisplay.text = "";
    }

    public bool TryFire(Vector2 origin, Vector2 direction)
    {
        if (!CanFire()) return false;
        if (projectilePrefab == null) return false;
        if (direction.sqrMagnitude <= 0.0001f) return false;
        if (!HasAmmo()) return false;

        PlaySound();
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

    private void PlaySound()
    {
        if(FireSound == null)
        {
            FireSound = GameObject.Find("shotgunFireSound").GetComponent<AudioSource>();
        }
        FireSound.Play();

    }

    private bool HasAmmo()
    {
        if (currentAmmo == 0)
            return false;
        if (currentAmmo > 0)
        {
            currentAmmo = currentAmmo - 1;
            if (AmmoDisplay != null)
            {
                AmmoDisplay.text = $"{currentAmmo} / {AmmoCount}";
            }
        }
        return true;
    }
}
