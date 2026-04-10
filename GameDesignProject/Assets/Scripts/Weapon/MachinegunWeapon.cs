using TMPro;
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

    public AudioSource FireSound;

    private GameObject owner;
    private float lastFireTime;

    public string WeaponName => weaponName;
    public WeaponType WeaponType => WeaponType.Ranged;
    public bool SupportsHoldFire => supportsHoldFire;

    public TextMeshProUGUI WeaponDisplay;

    public TextMeshProUGUI AmmoDisplay;

    public int AmmoCount = 100; // -1 for infinite ammo

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

    private void PlaySound()
    {
        if (FireSound == null)
        {
            FireSound = GameObject.Find("MachinegunSound").GetComponent<AudioSource>();
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
