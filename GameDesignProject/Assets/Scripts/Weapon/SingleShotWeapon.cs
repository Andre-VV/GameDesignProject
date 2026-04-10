using UnityEngine;
using Weapon.CombatTypes;
using TMPro;

public class SingleShotWeapon : MonoBehaviour, IWeapon
{
    [SerializeField] private string weaponName = "Single Shot";
    [SerializeField] private bool supportsHoldFire = false;
    [SerializeField] private float damage = 25f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private float projectileSpeed = 15f;
    [SerializeField] private float projectileLifeTime = 1.5f;

    private GameObject owner;
    private float lastFireTime;

    public AudioSource FireSound;

    public string WeaponName => weaponName;
    public WeaponType WeaponType => WeaponType.Ranged;
    public bool SupportsHoldFire => supportsHoldFire;

    public TextMeshProUGUI WeaponDisplay;

    public TextMeshProUGUI AmmoDisplay;

    public int AmmoCount = -1; // -1 for infinite ammo

    private int currentAmmo;

    public void OnEquip(GameObject owner)
    {
        this.owner = owner;
        currentAmmo = AmmoCount;
        if (WeaponDisplay != null)
        {
            WeaponDisplay.text = $"{WeaponName}";
        }
        if(AmmoDisplay != null)
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
    }

    public bool TryFire(Vector2 origin, Vector2 direction)
    {
        if (!CanFire()) return false;
        if (projectilePrefab == null) return false;
        if (direction.sqrMagnitude <= 0.0001f) return false;
        if (!HasAmmo()) return false;


        FireSound.Play();
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
