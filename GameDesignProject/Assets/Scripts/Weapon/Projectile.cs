using UnityEngine;
using Weapon.CombatTypes;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;
    [SerializeField] private float lifeTime = 1.5f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private LayerMask targetLayers;

    private Rigidbody2D rb;
    private GameObject attackSource;
    private Vector2 moveDirection;
    private float spawnTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
    }

    /// <summary>
    /// Applies the runtime values for this projectile and starts its movement.
    /// </summary>
    /// <param name="direction">Normalized travel direction for the projectile.</param>
    /// <param name="damage">Damage dealt when the projectile hits a valid target.</param>
    /// <param name="speed">Movement speed applied to the projectile rigidbody.</param>
    /// <param name="lifeTime">How long the projectile can exist before it is destroyed.</param>
    /// <param name="targetLayers">Reserved layer filter for future hit validation.</param>
    /// <param name="attackSource">The object that spawned this projectile.</param>
    public void Initialize(
        Vector2 direction,
        float damage,
        float speed,
        float lifeTime,
        LayerMask targetLayers,
        GameObject attackSource)
    {
        moveDirection = direction.normalized;
        this.damage = damage;
        this.speed = speed;
        this.lifeTime = lifeTime;
        this.targetLayers = targetLayers;
        this.attackSource = attackSource;

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        rb.linearVelocity = moveDirection * this.speed;
        spawnTime = Time.time;
    }

    /// <summary>
    /// Destroys the projectile when its lifetime has expired.
    /// </summary>
    private void Update()
    {
        if (Time.time >= spawnTime + lifeTime)
            DestroyProjectile();
    }

    /// <summary>
    /// Applies damage on contact and then destroys the projectile.
    /// </summary>
    /// <param name="other">The collider that entered this projectile's trigger.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!ShouldHit(other)) return;

        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable == null)
            damageable = other.GetComponentInParent<IDamageable>();

        if (damageable != null)
        {
            DamageInfo damageInfo = new DamageInfo
            {
                Amount = damage,
                Source = attackSource
            };

            damageable.TakeDamage(damageInfo);
        }

        DestroyProjectile();
    }

    /// <summary>
    /// Returns whether the projectile should respond to the given collider.
    /// </summary>
    /// <param name="other">The collider being checked for a hit response.</param>
    /// <returns>False when the collider belongs to the attack source; otherwise true.</returns>
    private bool ShouldHit(Collider2D other)
    {
        if (attackSource != null && (other.gameObject == attackSource || other.transform.IsChildOf(attackSource.transform)))
            return false;

        Projectile otherProjectile = other.GetComponent<Projectile>();
        if (otherProjectile == null)
            otherProjectile = other.GetComponentInParent<Projectile>();

        if (otherProjectile != null && attackSource != null && otherProjectile.attackSource == attackSource)
            return false;

        return true;
    }

    /// <summary>
    /// Removes the projectile from the scene.
    /// </summary>
    private void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
