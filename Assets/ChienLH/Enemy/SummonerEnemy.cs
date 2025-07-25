using System.Collections;
using UnityEngine;

public class SummonerEnemy : EnemyBase
{
    [Header("Summon Settings")]
    public GameObject minionPrefab;
    public int minionCount = 3;
    public float summonCooldown = 10f;
    public float spawnRadius = 3f;

    [Header("Evade Settings")]
    public float evadeSpeed = 5f;
    public float evadeRange = 4f;

    private float lastSummonTime;

    protected override void Update()
    {
        if (Evade())
            return;
        base.Update();
        if (Time.time >= lastSummonTime + summonCooldown && !isAttacking)
        {
            lastSummonTime = Time.time;
            StartCoroutine(SummonCoroutine());
        }
    }
    private bool Evade()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, evadeRange);
        Vector2 evadeDir = Vector2.zero;

        foreach (var col in hits)
        {
            if (col.CompareTag("Bullet"))
            {
                Vector2 away = (Vector2)transform.position - (Vector2)col.transform.position;
                evadeDir += away.normalized;
            }
        }
        if (evadeDir == Vector2.zero && player != null)
        {
            float distToPlayer = Vector2.Distance(transform.position, player.position);
            if (distToPlayer < evadeRange)
            {
                Vector2 away = (Vector2)transform.position - (Vector2)player.position;
                evadeDir = away.normalized;
            }
        }

        if (evadeDir == Vector2.zero)
            return false;

        transform.position += (Vector3)(evadeDir.normalized * evadeSpeed * Time.deltaTime);
        Flip(evadeDir.x);
        isAttacking = true;
        return true;
    }

    private IEnumerator SummonCoroutine()
    {
        isAttacking = true;
        animator.SetTrigger("summon");

        yield return new WaitForSeconds(0.3f);

        for (int i = 0; i < minionCount; i++)
        {
            Vector2 offset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPos = transform.position + new Vector3(offset.x, offset.y, 0f);
            GameObject minion = Instantiate(minionPrefab, spawnPos, Quaternion.identity);
            if (minion.TryGetComponent<EnemyBase>(out var enemyBase))
            {
                enemyBase.Initialize(player);
            }
        }

        isAttacking = false;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, evadeRange);
    }

    public override void Attack()
    {
        
    }
}
