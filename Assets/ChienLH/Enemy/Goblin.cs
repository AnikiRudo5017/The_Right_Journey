using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Goblin : EnemyBase
{
    [Header("Melee Hitbox")]
    public Collider2D attackHitbox;
    public float attackDuration = 0.3f;

    public override void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown || isAttacking)
            return;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attackHitbox.enabled && other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out var health))
            {
                health.TakeDamage(damageAmount);
            }
        }
    }
}
