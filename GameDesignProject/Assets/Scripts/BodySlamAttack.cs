using UnityEngine;


public class BodySlamAttack : MonoBehaviour, IDamageDealer
{
    [SerializeField] float damage = 30f;
    [SerializeField] float cooldown = 2f;
    [SerializeField] float knockbackForce = 5f;
    [SerializeField] LayerMask targetLayers;

    public float Damage => damage;
    public float AttackRate => cooldown;
    public LayerMask TargetLayers => targetLayers;

    private float cooldownTimer;
    private Collider2D col;

    void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    public void Attack()
    {
        if (cooldownTimer > 0f) return;

        var results = new Collider2D[16];
        var filter = new ContactFilter2D();
        filter.SetLayerMask(targetLayers);
        filter.useTriggers = false;

        int count = col.Overlap(filter, results);
        for (int i = 0; i < count; i++)
        {
            var damageable = results[i].GetComponent<IDamageable>();
            if (damageable == null) continue;

            Vector2 knockDir = (results[i].transform.position - transform.position).normalized;
            var info = new DamageInfo
            {
                Amount = damage,
                Source = gameObject,
                KnockbackDir = knockDir,
                KnockbackForce = knockbackForce
            };
            damageable.TakeDamage(info);
        }

        cooldownTimer = cooldown;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayers & (1 << other.gameObject.layer)) != 0)
            Attack();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((targetLayers & (1 << collision.gameObject.layer)) != 0)
            Attack();
    }
}
