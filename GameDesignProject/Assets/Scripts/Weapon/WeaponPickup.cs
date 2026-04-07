using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] private GameObject weaponTemplate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerWeaponHandler weaponHandler = other.GetComponent<PlayerWeaponHandler>();
        if (weaponHandler == null)
            weaponHandler = other.GetComponentInParent<PlayerWeaponHandler>();

        if (weaponHandler == null)
            return;

        if (weaponHandler.EquipSecondaryWeaponPrefab(weaponTemplate))
            Destroy(gameObject);
    }
}
