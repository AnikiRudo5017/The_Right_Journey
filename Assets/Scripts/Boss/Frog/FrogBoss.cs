using Chien;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FrogBoss : EnemyBase
{
    [Header("Melee Attack")]
    public Collider2D meleeHitbox;
    public float meleeDuration = 0.3f;

    [Header("Poison Zone Settings")]
    public GameObject poisonPrefab;
    public float poisonCooldown = 8f;
    public float spawnDistance = 1.5f;
    public int poisonCount = 3;

    private float lastPoisonTime;

    protected override void Update()
    {
        base.Update();

        if (!isDead && Time.time - lastPoisonTime >= poisonCooldown && !isAttacking)
        {
            StartCoroutine(PoisonRoutine());
        }
    }

    public override void Attack()
    {
        if (Time.time - lastAttackTime < attackCooldown || isAttacking)
            return;
        
        StartCoroutine(MeleeRoutine());
    }

    private IEnumerator MeleeRoutine()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        animator.SetTrigger("atk");

        yield return new WaitForSeconds(0.1f);

        meleeHitbox.enabled = true;

        yield return new WaitForSeconds(meleeDuration);

        meleeHitbox.enabled = false;
        isAttacking = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (meleeHitbox.enabled && other.CompareTag("Player"))
        {
            if (other.TryGetComponent<PlayerController>(out var health))
            {
                health.TakeDamage(damageAmount);
            }
        }
    }

    private IEnumerator PoisonRoutine()
    {
        isAttacking = true;
        animator.SetTrigger("poison");

        yield return new WaitForSeconds(2f);

        float angleStep = 360f / poisonCount;

        for (int i = 0; i < poisonCount; i++)
        {
            float angle = i * angleStep;
            Vector3 offset = Quaternion.Euler(0, 0, angle) * Vector3.right * spawnDistance;
            Vector3 spawnPos = transform.position + offset;

            Instantiate(poisonPrefab, spawnPos, Quaternion.identity);
        }

        lastPoisonTime = Time.time;
        isAttacking = false;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();
        Gizmos.color = Color.green;
        if (poisonCount > 0)
        {
            float angleStep = 360f / poisonCount;
            for (int i = 0; i < poisonCount; i++)
            {
                float angle = i * angleStep;
                Vector3 offset = Quaternion.Euler(0, 0, angle) * Vector3.right * spawnDistance;
                Gizmos.DrawWireSphere(transform.position + offset, 0.5f);
            }
        }
        if (meleeHitbox != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(meleeHitbox.bounds.center, meleeHitbox.bounds.size);
        }
    }
}
