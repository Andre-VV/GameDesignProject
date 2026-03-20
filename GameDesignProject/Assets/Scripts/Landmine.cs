using UnityEngine;

public class Landmine : MonoBehaviour, IDamageDealer
{
    [SerializeField] float damage = 50f;
    [SerializeField] float radius = 2f;
    [SerializeField] LayerMask targetLayers;

    public float Damage => damage;
    public float AttackRate => 0f;
    public LayerMask TargetLayers => targetLayers;

    public void Attack()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayers);
        var info = new DamageInfo { Amount = damage, Source = gameObject };
        foreach (var hit in hits)
            hit.GetComponent<IDamageable>()?.TakeDamage(info);
        Debug.Log("Impact");
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((targetLayers & (1 << other.gameObject.layer)) != 0)
            Attack();
    }
}
