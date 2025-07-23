using System.Collections;
using UnityEngine;

public class DeathMageBoss : EnemyBase
{
    [Header("Fireball")]
    public GameObject fireballPrefab;
    public float fireballCooldown = 1f;

    [Header("Ground AoE")]
    public GameObject fireAoEPrefab;
    public float aoeCooldown = 5f;
    public float aoeWarnTime = 1f; // ánh sáng trên ground

    public override void Attack()
    {
        // Fireball thường xuyên
        if (Time.time >= lastAttackTime + fireballCooldown)
        {
            lastAttackTime = Time.time;
            ShootFireball();
        }
        // AoE khi cooldown đủ và máu < 75%
        if (Time.time >= aoeCooldown && currentHealth <= maxHealth * 0.75f)
        {
            StartCoroutine(DoGroundAoE());
        }
    }

    private void ShootFireball()
    {
        animator.SetTrigger("cast");
        Vector3 dir = (player.position - transform.position).normalized;
        GameObject proj = Instantiate(fireballPrefab, transform.position, Quaternion.identity);
        proj.GetComponent<Rigidbody2D>().linearVelocity = dir * 5f;
    }

    private IEnumerator DoGroundAoE()
    {
        aoeCooldown = Time.time + aoeCooldown;
        animator.SetTrigger("aoe");
        // Warn effect: có thể bật sprite flash trên ground tại vị trí player
        Vector3 pos = player.position;
        // chờ warning
        yield return new WaitForSeconds(aoeWarnTime);
        Instantiate(fireAoEPrefab, pos, Quaternion.identity);
    }
}
