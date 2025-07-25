using UnityEngine;

public class DemonBullet : MonoBehaviour
{
    public int damage = 1;
    public float lifeTime = 5f;
    public GameObject damageZonePrefab;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        if (GetComponent<Rigidbody2D>() != null && GetComponent<Rigidbody2D>().linearVelocity.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent<PlayerController>(out var health))
        {
            health.TakeDamage(damage);
            if (damageZonePrefab != null)
            {
                Instantiate(damageZonePrefab, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
