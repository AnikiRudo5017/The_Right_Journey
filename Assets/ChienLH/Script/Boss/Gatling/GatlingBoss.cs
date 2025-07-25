using System.Collections;
using UnityEngine;

public class BossGatling : EnemyBase
{
    [Header("Common Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 8f;

    [Header("Gatling 1")]
    public int burstCount = 10;
    public float preShootDelay = 0.3f;
    public float burstInterval = 0.1f;
    public float gatlingSpreadAngle = 10f;

    [Header("Gatling 2")]
    public int gatlingBulletCount = 36;
    public float gatlingInterval = 0.05f;
    public float gatlingCooldown = 8f;

    [Header("Melee Settings")]
    public float meleeRange = 2f;
    public float meleeAngle = 60f;
    public float meleeDamageDelay = 0.2f;
    public float meleeRecoverTime = 0.3f;

    private float lastRadialTime;

    public override void Attack()
    {
        if (isAttacking) return;

        float now = Time.time;
        Vector2 toPlayer = player.position - transform.position;
        float dist = toPlayer.magnitude;

        if (dist <= meleeRange)
        {
            isAttacking = true;
            StartCoroutine(MeleeAttack());
            return;
        }

        if (now >= lastRadialTime + gatlingCooldown)
        {
            lastRadialTime = now;
            lastAttackTime = now;
            StartCoroutine(RadialSpread());
            return;
        }

        if (now >= lastAttackTime + attackCooldown)
        {
            lastAttackTime = now;
            StartCoroutine(GatlingBurst());
        }
    }

    private IEnumerator MeleeAttack()
    {
        animator.SetTrigger("melee");

        yield return new WaitForSeconds(meleeDamageDelay);

        Vector2 toPlayer = player.position - transform.position;
        if (toPlayer.magnitude <= meleeRange)
        {
            if (player.TryGetComponent<PlayerHealth>(out var health))
            {
                health.TakeDamage(damageAmount);
            }
        }

        yield return new WaitForSeconds(meleeRecoverTime);
        isAttacking = false;
    }

    private IEnumerator GatlingBurst()
    {
        isAttacking = true;
        animator.SetTrigger("gatling1");

        yield return new WaitForSeconds(preShootDelay);
        for (int i = 0; i < burstCount; i++)
        {
            SpawnWithSpread();
            yield return new WaitForSeconds(burstInterval);
        }

        isAttacking = false;
    }

    private IEnumerator RadialSpread()
    {
        isAttacking = true;
        animator.SetTrigger("gatling2");

        float angleStep = 360f / gatlingBulletCount;
        yield return PerformRadial(angleStep, 0f);
        yield return new WaitForSeconds(gatlingInterval);
        yield return PerformRadial(angleStep, angleStep * 0.5f);

        isAttacking = false;
    }

    private IEnumerator PerformRadial(float angleStep, float startOffset)
    {
        float angle = startOffset;
        for (int i = 0; i < gatlingBulletCount; i++)
        {
            float rad = Mathf.Deg2Rad * angle;
            Vector2 dir = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
            Spawn(dir);
            angle += angleStep;
            yield return new WaitForSeconds(gatlingInterval);
        }
    }

    private void SpawnWithSpread()
    {
        if (bulletPrefab == null || firePoint == null || player == null) return;
        Vector2 dir = (player.position - firePoint.position).normalized;
        float randomAngle = Random.Range(-gatlingSpreadAngle, gatlingSpreadAngle);
        dir = Quaternion.Euler(0, 0, randomAngle) * dir;
        Spawn(dir);
    }

    private void Spawn(Vector2 direction)
    {
        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        if (b.TryGetComponent<Rigidbody2D>(out var rb))
            rb.linearVelocity = direction * bulletSpeed;
        if (b.TryGetComponent<Bullet>(out var bullet))
            bullet.damage = damageAmount;
    }
}
