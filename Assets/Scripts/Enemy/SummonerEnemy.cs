using System.Collections;
using UnityEngine;

public class SummonerEnemy : EnemyBase
{
    public GameObject minionPrefab;
    public int minionCount = 3;
    public float summonCooldown = 10f;
    public float spawnRadius = 3f;

    private float lastSummonTime;

    protected override void Update()
    {
        base.Update();

        if (Time.time >= lastSummonTime + summonCooldown && !isAttacking)
        {
            lastSummonTime = Time.time;
            StartCoroutine(SummonCoroutine());
        }
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
    }
    public override void Attack() { }
}
