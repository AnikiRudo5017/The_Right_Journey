using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PoiSonZone : MonoBehaviour
{
    public float duration = 4f;
    public int damage = 1;
    public float tickInterval = 1f;

    private CircleCollider2D col;

    private void Awake()
    {
        col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
    }

    private void OnEnable()
    {
        StartCoroutine(DamageRoutine());
    }

    private IEnumerator DamageRoutine()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, col.radius * transform.localScale.x);
            foreach (var hit in hits)
            {
                if (hit.CompareTag("Player") && hit.TryGetComponent<PlayerHealth>(out var health))
                {
                    health.TakeDamage(damage);
                }
            }
            yield return new WaitForSeconds(tickInterval);
            elapsed += tickInterval;
        }
        Destroy(gameObject);
    }
    private void OnDrawGizmosSelected()
    {
        if (col == null) col = GetComponent<CircleCollider2D>();
        Gizmos.color = Color.green;
        float r = col.radius * transform.localScale.x;
        Gizmos.DrawWireSphere(transform.position, r);
    }
}
