using System.Collections;
using UnityEngine;

public class DemonBoss : EnemyBase
{
    [Header("Spit Attack")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float arcAngle = 45f;
    public float bulletSpeed = 2f;
    public float preSpitDelay = 0.3f;

    [Header("Teleport Skill")]
    public float teleportCooldown = 5f;
    public float teleportDistanceFromPlayer = 1f;
    public float teleportYOffset = 1f;

    private float lastTeleportTime;

    public override void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown || isAttacking)
            return;

        if (Time.time >= lastTeleportTime + teleportCooldown && Random.value < 0.3f)
        {
            StartCoroutine(TeleportRoutine());
        }
        else
        {
            lastAttackTime = Time.time;
            StartCoroutine(SpitRoutine());
        }
    }


    private IEnumerator SpitRoutine()
    {
        isAttacking = true;
        animator.SetTrigger("atk");

        yield return new WaitForSeconds(preSpitDelay);

        int bulletCount = Random.Range(1, 11);

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

        isAttacking = false;
    }

    private IEnumerator TeleportRoutine()
    {
        isAttacking = true;
        lastTeleportTime = Time.time;

        animator.SetTrigger("tele_out");
        yield return new WaitForSeconds(1f);

        Vector3 dirToPlayer = (player.position - transform.position).normalized;
        Vector3 targetPos = player.position - dirToPlayer * teleportDistanceFromPlayer;
        targetPos.y += teleportYOffset;

        transform.position = targetPos;

        animator.SetTrigger("tele_in");
        yield return new WaitForSeconds(1f);

        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        DrawArcGizmo();
    }

    private void DrawArcGizmo()
    {
        if (firePoint == null || player == null) return;

        Gizmos.color = Color.red;

        Vector3 baseDir = (player.position - firePoint.position).normalized;
        float startAngle = -arcAngle / 2f;
        float radius = 1.5f;
        int segments = 20;

        Vector3 prev = firePoint.position + Quaternion.Euler(0, 0, startAngle) * baseDir * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = startAngle + (arcAngle / segments) * i;
            Vector3 next = firePoint.position + Quaternion.Euler(0, 0, angle) * baseDir * radius;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
}
