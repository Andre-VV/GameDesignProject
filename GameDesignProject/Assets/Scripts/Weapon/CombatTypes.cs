using UnityEngine;

namespace Weapon.CombatTypes
{
    public enum DamageType { Physical, Fire }
    public enum WeaponType { Melee, Ranged, Area }
    public enum EnemyType { Normal, Fast, Tank, Boss }

    public struct DamageInfo
    {
        public float Amount;
        public DamageType Type;
        public GameObject Source;
        public Vector2 KnockbackDir;
        public float KnockbackForce;
    }

    public struct DeathInfo
    {
        public GameObject Victim;
        public GameObject Killer;
        public DamageInfo FinalDamage;
    }
}
