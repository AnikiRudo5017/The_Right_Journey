using UnityEngine;

public class Warrior : PlayerController
{
    [Header("Attack")]
    public GameObject attackZonePrefab;  // Prefab effect tấn công
    public Transform pointAttack;
    public LayerMask enemyMask;
    public float attackRange = 1f;
    public float attackCooldown = 0.15f;

    void Start()
    {
        lastAttackTime = 0f;
    }

    protected override void PerformAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        isAttacking = true;  // Dừng di chuyển
        anim.SetTrigger("Attack");  // Trigger animation

        int damage = attackDame;

        // Tìm kẻ địch trong phạm vi
        Collider2D[] enemies = Physics2D.OverlapCircleAll(pointAttack.position, attackRange, enemyMask);
        foreach (var enemyCollider in enemies)
        {
            if (enemyCollider.CompareTag("enemy"))
            {
                if (attackZonePrefab != null)
                {
                    GameObject atkZone = Instantiate(attackZonePrefab, pointAttack.position, Quaternion.identity);
                    Destroy(atkZone, 0.1f);
                }
                //Giả sử code enemy có hàm TakeDamage
                //CharacterController enemy = enemyCollider.GetComponent<CharacterController>();
                //if (enemy != null)
                //{
                //    enemy.TakeDamage(damage);
                //}
            }
        }

        // Reset isAttacking sau khi animation kết thúc
        StartCoroutine(ResetAttackState(0.2f));  // Điều chỉnh nếu animation dài hơn
    }

    private System.Collections.IEnumerator ResetAttackState(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    protected override void UseNomalSkill()
    {
        // Chưa triển khai
        anim.SetTrigger("Skill");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (pointAttack != null)
        {
            Gizmos.DrawWireSphere(pointAttack.position, attackRange);
        }
    }
}