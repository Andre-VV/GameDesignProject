using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponHandler : MonoBehaviour
{
    [SerializeField] private Transform slot1Mount;
    [SerializeField] private Transform slot2Mount;

    // Legacy fallback so existing scene references do not become unusable immediately.
    [SerializeField, HideInInspector] private MonoBehaviour weaponSlot1Source;
    [SerializeField, HideInInspector] private MonoBehaviour weaponSlot2Source;

    private IWeapon weaponSlot1;
    private IWeapon weaponSlot2;
    private Camera mainCamera;
    private bool isSecondaryFireHeld;

    private void Awake()
    {
        mainCamera = Camera.main;
        slot1Mount = ResolveMount(slot1Mount, weaponSlot1Source);
        slot2Mount = ResolveMount(slot2Mount, weaponSlot2Source);

        weaponSlot1 = FindMountedWeapon(slot1Mount);
        weaponSlot2 = FindMountedWeapon(slot2Mount);

        weaponSlot1?.OnEquip(gameObject);
        weaponSlot2?.OnEquip(gameObject);
    }

    private void Update()
    {
        if (weaponSlot2 != null && weaponSlot2.SupportsHoldFire && Mouse.current.rightButton.isPressed)
        {
            TryFireWeapon(weaponSlot2);
        }

    }

    private void OnDisable()
    {
        isSecondaryFireHeld = false;
    }

    public void OnFirePrimary(InputValue value)
    {
        if (!value.isPressed || weaponSlot1 == null)
            return;

        TryFireWeapon(weaponSlot1);
    }

    public void OnFireSecondary(InputValue value)
    {
        isSecondaryFireHeld = value.isPressed;

        if (!value.isPressed || weaponSlot2 == null)
            return;

        TryFireWeapon(weaponSlot2);
    }

    public bool EquipSecondaryWeaponPrefab(GameObject weaponTemplate)
    {
        if (weaponTemplate == null || slot2Mount == null)
            return false;

        UnequipCurrentSecondaryWeapon();
        DestroyMountedChildren(slot2Mount);

        GameObject weaponInstance = Instantiate(weaponTemplate, slot2Mount);
        weaponInstance.transform.localPosition = Vector3.zero;
        weaponInstance.transform.localRotation = Quaternion.identity;
        weaponInstance.transform.localScale = Vector3.one;

        IWeapon newWeapon = FindMountedWeapon(weaponInstance.transform);
        if (newWeapon == null)
        {
            Destroy(weaponInstance);
            return false;
        }

        weaponSlot2 = newWeapon;
        weaponSlot2.OnEquip(gameObject);
        return true;
    }

    public void EquipSecondaryWeapon(IWeapon weapon)
    {
        weaponSlot2?.OnUnequip();
        weaponSlot2 = weapon;
        weaponSlot2?.OnEquip(gameObject);
    }

    private void TryFireWeapon(IWeapon weapon)
    {
        weapon.TryFire((Vector2)transform.position, GetAimDirection());
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
        Vector2 direction = mouseWorldPosition - transform.position;

        if (direction.sqrMagnitude <= 0.0001f)
            return Vector2.right;

        return direction.normalized;
    }

    private Transform ResolveMount(Transform mount, MonoBehaviour legacySource)
    {
        if (mount != null)
            return mount;

        return legacySource != null ? legacySource.transform : null;
    }

    private IWeapon FindMountedWeapon(Transform mount)
    {
        if (mount == null)
            return null;

        MonoBehaviour[] behaviours = mount.GetComponentsInChildren<MonoBehaviour>(true);
        foreach (MonoBehaviour behaviour in behaviours)
        {
            if (behaviour is IWeapon weapon)
                return weapon;
        }

        return null;
    }

    private void UnequipCurrentSecondaryWeapon()
    {
        if (weaponSlot2 == null)
            return;

        weaponSlot2.OnUnequip();

        if (weaponSlot2 is MonoBehaviour weaponBehaviour)
        {
            if (slot2Mount != null && weaponBehaviour.transform == slot2Mount)
                Destroy(weaponBehaviour);
            else
                Destroy(weaponBehaviour.gameObject);
        }

        weaponSlot2 = null;
    }

    private void DestroyMountedChildren(Transform mount)
    {
        if (mount == null)
            return;

        for (int i = mount.childCount - 1; i >= 0; i--)
        {
            Destroy(mount.GetChild(i).gameObject);
        }
    }
}
