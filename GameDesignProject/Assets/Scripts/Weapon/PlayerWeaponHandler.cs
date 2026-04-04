using UnityEngine;
using UnityEngine.InputSystem;
using Weapon.CombatTypes;


public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private MonoBehaviour weaponSlot1Source;
    [SerializeField] private MonoBehaviour weaponSlot2Source;

    private IWeapon weaponSlot1;
    private IWeapon weaponSlot2;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        weaponSlot1 = weaponSlot1Source as IWeapon;
        weaponSlot2 = weaponSlot2Source as IWeapon;

        weaponSlot1?.OnEquip(gameObject);
        weaponSlot2?.OnEquip(gameObject);
    }

    public void OnFirePrimary(InputValue value)
    {
        if (!value.isPressed || weaponSlot1 == null) return;
        weaponSlot1.TryFire((Vector2)transform.position, GetAimDirection());
    }

    public void OnFireSecondary(InputValue value)
    {
        if (!value.isPressed || weaponSlot2 == null) return;
        weaponSlot2.TryFire((Vector2)transform.position, GetAimDirection());
    }

    private Vector2 GetAimDirection()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (mainCamera == null || Mouse.current == null)
            return Vector2.right;

        Vector3 mouseScreenPosition = Mouse.current.position.ReadValue();
        mouseScreenPosition.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);
        Vector2 direction = (mouseWorldPosition - transform.position);

        if (direction.sqrMagnitude <= 0.0001f)
            return Vector2.right;

        return direction.normalized;
    }

    public void EquipSecondaryWeapon(IWeapon weapon)
    {
        weaponSlot2?.OnUnequip();
        weaponSlot2 = weapon;
        weaponSlot2?.OnEquip(gameObject);
    }
}
