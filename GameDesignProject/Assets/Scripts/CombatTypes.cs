using UnityEngine;

public enum DamageType { Physical, Fire }

public enum WeaponType { Melee, Ranged, Area }

public struct DamageInfo
{
    public float Amount;
    public DamageType Type;
    public GameObject Source;
    public Vector2 KnockbackDir;
    public float KnockbackForce;
}
