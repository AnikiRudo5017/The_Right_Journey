using System.Collections;
using UnityEngine;

public class CatBoss : EnemyBase
{
    [Header("Ranges")]
    [Tooltip("Khoảng cách tối đa để dùng melee attack")]
    public float meleeRange = 1.5f;
    [Tooltip("Khoảng cách tối đa để dùng Gatling")]
    public float gatlingRange = 5f;

    [Header("Melee Hitbox")]
    public Collider2D meleeHitbox;
    public float meleeActiveTime = 0.3f;

    [Header("Gatling Gun (Skill Attack)")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int burstCount = 5;
    public float burstInterval = 0.15f;
    public float bulletSpeed = 8f;
    public float gatlingCooldown = 4f;
    private float lastGatlingTime = -Mathf.Infinity;

    [Header("Summon Robot (Optional)")]
    public GameObject robotPrefab;
    public int robotsToSummon = 2;
    private bool isSummoning = false;

    protected override void Start()
    {
        base.Start();
        if (meleeHitbox != null)
            meleeHitbox.enabled = false;
    }

    public override void Attack()
    {
        if (isAttacking) return;

        float dist = Vector2.Distance(player.position, transform.position);

        // 1. Dùng Gatling nếu trong range và hết cooldown
        if (dist <= gatlingRange && Time.time >= lastGatlingTime + gatlingCooldown)
        {
            StartCoroutine(DoGatlingBurst());
            lastGatlingTime = Time.time;
            return;
        }

        // 2. Dùng Melee nếu ở gần
        if (dist <= meleeRange)
        {
            StartCoroutine(DoMelee());
            return;
        }

        // 3. Nếu ngoài tầm tấn công → EnemyBase sẽ xử lý di chuyển
    }

    private IEnumerator DoMelee()
    {
        isAttacking = true;
        animator.SetTrigger("atk");

        yield return new WaitForSeconds(0.2f); // delay khớp animation

        meleeHitbox.enabled = true;
        yield return new WaitForSeconds(meleeActiveTime);
        meleeHitbox.enabled = false;

        yield return new WaitForSeconds(0.3f); // tạm nghỉ sau đánh
        isAttacking = false;
    }

    private IEnumerator DoGatlingBurst()
    {
        isAttacking = true;
        animator.SetTrigger("gatling");

        yield return new WaitForSeconds(0.3f); // delay bắt đầu animation

        for (int i = 0; i < burstCount; i++)
        {
            SpawnBullet();
            yield return new WaitForSeconds(burstInterval);
        }

        yield return new WaitForSeconds(0.5f); // nghỉ sau Gatling
        isAttacking = false;
    }

    private void SpawnBullet()
    {
        if (bulletPrefab == null || firePoint == null || player == null) return;

        Vector3 dir = (player.position - firePoint.position).normalized;
        float randomAngle = Random.Range(-10f, 10f);
        dir = Quaternion.Euler(0, 0, randomAngle) * dir;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if (bullet.TryGetComponent<Rigidbody2D>(out var rb))
            rb.linearVelocity = dir * bulletSpeed;

        if (bullet.TryGetComponent<Bullet>(out var bulletScript))
            bulletScript.damage = damageAmount;
    }

    private IEnumerator SummonRobots()
    {
        if (isSummoning) yield break;
        isSummoning = true;

        animator.SetTrigger("summon");

        for (int i = 0; i < robotsToSummon; i++)
        {
            Vector3 spawnPos = transform.position + (Vector3)Random.insideUnitCircle * 1.5f;
            Instantiate(robotPrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(0.3f);
        }

        isSummoning = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isAttacking && meleeHitbox.enabled && other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerHealth>(out var health))
                health.TakeDamage(damageAmount);
        }
    }
}
