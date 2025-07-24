using System.Collections;
using UnityEngine;

public class GoblinShooter : EnemyBase
{
    [Header("Ranged Attack")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int burstCount = 3;
    public float burstInterval = 0.2f;
    public float bulletSpeed = 8f;
    public float preShootDelay = 0.3f;

    public override void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown || isAttacking) return;
        lastAttackTime = Time.time;
        StartCoroutine(ShootBurst());
    }

    private IEnumerator ShootBurst()
    {
        isAttacking = true;
        animator.SetTrigger("atk");
        yield return new WaitForSeconds(preShootDelay);
        for (int i = 0; i < burstCount; i++)
        {
            SpawnBullet();
            yield return new WaitForSeconds(burstInterval);
        }

        isAttacking = false;
    }

    private void SpawnBullet()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector3 dir = (player.position - firePoint.position).normalized;

        float randomAngle = Random.Range(-10f, 10f);
        dir = Quaternion.Euler(0, 0, randomAngle) * dir;

        GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);

        if (b.TryGetComponent<Rigidbody2D>(out var rb))
            rb.linearVelocity = dir * bulletSpeed;

        if (b.TryGetComponent<Bullet>(out var bullet))
            bullet.damage = damageAmount;
    }
}
