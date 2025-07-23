using UnityEngine;

public class Slime : EnemyBase
{
    public GameObject mediumSlimePrefab;
    public GameObject smallSlimePrefab;
    public int splitsLeft = 2;

    public override void Attack() { }

    public override void TakeDamage(int amount)
    {
        if (isDead) return;
        animator.SetTrigger("hit");
        if (splitsLeft > 1)
            Split(mediumSlimePrefab, splitsLeft - 1);
        else if (splitsLeft > 0)
            Split(smallSlimePrefab, splitsLeft - 1);
        else
            Die();
        Destroy(gameObject);
    }

    private void Split(GameObject prefab, int nextSplits)
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject go = Instantiate(prefab, transform.position, Quaternion.identity);
            if (go.TryGetComponent<Slime>(out var slime))
                slime.splitsLeft = nextSplits;
            if (go.TryGetComponent<Rigidbody2D>(out var rb))
                rb.AddForce(Random.insideUnitCircle.normalized * 2f, ForceMode2D.Impulse);
        }
    }
}