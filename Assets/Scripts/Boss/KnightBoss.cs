using System.Collections;
using UnityEngine;

public class KnightBoss : EnemyBase
{
    [Header("Melee Attack")]
    public Collider2D attackHitbox;
    public float attackDuration = 0.3f;

    [Header("Dash Skill")]
    public float dashSpeed = 10f;
    public float dashDuration = 0.4f;
    public float dashCooldown = 5f;
    private float lastDashTime;
    private Vector2 currentDashDirection;

    [Header("Summon Settings")]
    public GameObject meleeEnemyPrefab;
    public Transform[] summonPoints;
    private bool hasSummoned = false;

    public override void Attack()
    {
        if (isAttacking) return;

        float distance = Vector2.Distance(transform.position, player.position);

        // Dash nếu cooldown xong và player không quá gần
        if (Time.time - lastDashTime >= dashCooldown && distance > attackRange + 0.5f)
        {
            StartCoroutine(DashRoutine());
        }
        else if (distance <= attackRange)
        {
            StartCoroutine(MeleeAttackRoutine());
        }
    }

    private IEnumerator MeleeAttackRoutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        animator.SetTrigger("atk");

        yield return new WaitForSeconds(0.1f);
        attackHitbox.enabled = true;

        yield return new WaitForSeconds(attackDuration);

        attackHitbox.enabled = false;
        isAttacking = false;
    }

    private IEnumerator DashRoutine()
    {
        isAttacking = true;
        lastDashTime = Time.time;

        // Phase 1: Charge
        animator.SetTrigger("hit"); // 👈 cần animation charge
        yield return new WaitForSeconds(0.5f);

        // Phase 2: Dash
        animator.SetTrigger("dash"); // 👈 cần animation dash
        currentDashDirection = (player.position - transform.position).normalized;

        float elapsed = 0f;
        while (elapsed < dashDuration)
        {
            transform.position += (Vector3)(currentDashDirection * dashSpeed * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Phase 3: Cooldown ngắn
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }

    public override void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        animator.SetTrigger("hit");

        if (!hasSummoned && currentHealth <= maxHealth / 2)
        {
            hasSummoned = true;
            SummonAllies();
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(HitStunRoutine());
        }
    }

    private void SummonAllies()
    {
        foreach (Transform point in summonPoints)
        {
            Instantiate(meleeEnemyPrefab, point.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Đánh thường
        if (attackHitbox.enabled && other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerHealth>(out var health))
            {
                health.TakeDamage(damageAmount);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Gây damage khi dash
        if (isAttacking && animator.GetCurrentAnimatorStateInfo(0).IsName("dash"))
        {
            if (collision.collider.CompareTag("Player"))
            {
                if (collision.collider.TryGetComponent<PlayerHealth>(out var health))
                    health.TakeDamage(damageAmount);
            }
        }
    }
}
