using System.Collections;
using UnityEngine;

public class GoblinArcShooter : EnemyBase
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public int bulletCount = 5;
    public float arcAngle = 60f;
    public float bulletSpeed = 8f;

    public override void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown || isAttacking) return;
        lastAttackTime = Time.time;
        ArcAttack();
    }

    private void ArcAttack()
    {
        if (bulletPrefab == null || firePoint == null || player == null) return;

        Vector3 baseDir = (player.position - firePoint.position).normalized;
        float startAngle = -arcAngle / 2f;
        float deltaAngle = bulletCount > 1 ? arcAngle / (bulletCount - 1) : 0f;
        for (int i = 0; i < bulletCount; i++)
        {
            float currentAngle = startAngle + deltaAngle * i;
            Vector3 dir = Quaternion.Euler(0, 0, currentAngle) * baseDir;

            GameObject b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
            if (b.TryGetComponent<Rigidbody2D>(out var rb))
                rb.linearVelocity = dir * bulletSpeed;

            if (b.TryGetComponent<Bullet>(out var bullet))
                bullet.damage = damageAmount;
        }

        animator.SetTrigger("atk");
    }
}
