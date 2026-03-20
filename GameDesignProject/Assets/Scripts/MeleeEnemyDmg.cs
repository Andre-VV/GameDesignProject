using System.Threading;
using UnityEngine;

public class MeleeEnemyDmg : MonoBehaviour, IDamageDealer
{
    [SerializeField] float damage = 20f;
    [SerializeField] float attackRate = 1f;
    [SerializeField] LayerMask targetLayers;
    public float Damage => damage;
    public float AttackRate => attackRate;
    public LayerMask TargetLayers => targetLayers;

    private float attackTimer = 0;

    // Add a reusable collider array for OverlapCollider
    private readonly Collider2D[] colliders = new Collider2D[8];

    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    public void Attack()
    {
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
            return;
        }

        var info = new DamageInfo { Amount = damage, Source = gameObject };

        int hitCount = Physics2D.OverlapCollider(GetComponent<Collider2D>(), new ContactFilter2D { layerMask = targetLayers }, colliders);
        attackTimer = attackRate;
        for (int i = 0; i < hitCount; i++)
        {
            colliders[i].GetComponent<IDamageable>()?.TakeDamage(info);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayers & (1 << other.gameObject.layer)) != 0)
            Attack();
    }
}
