using UnityEngine;

public class Warrior : PlayerController
{
    [Header("Attack")]
    public GameObject attackZonePrefab;  // Prefab effect tấn công
    public Transform pointAttack;  // Điểm gốc tấn công (Transform con của nhân vật)
    public LayerMask enemyMask;
    public float attackRange = 1f;
    public float attackCooldown = 0.5f;

    [Header("Skill 1")]
    public GameObject skill1EffectPrefab;  // Prefab effect skill1
    public float skill1Range = 2f;  // Phạm vi skill (tùy chỉnh)
    public float skill1Cooldown = 5f;  // Thời gian cooldown skill
    public Transform Skill1Point;
    private float lastSkill1Time;

    [Header("Skill 2")]
    public GameObject skill2EffectPrefab;  // Prefab effect vụ nổ (gán prefab với animation vụ nổ)
    public float skill2Range = 3f;  // Phạm vi vụ nổ (tùy chỉnh)
    public float skill2Cooldown = 10f;  // Thời gian cooldown skill2 (tùy chỉnh)
    private float lastSkill2Time;

    [Header("UI Reference")]
    public SkillUI skillUI;

    void Start()
    {
        lastAttackTime = 0f;  // Khởi tạo cooldown
        lastSkill1Time = 0f;
    }

    protected override void PerformAttack()
    {
        if (Time.time - lastAttackTime < attackCooldown) return;
        lastAttackTime = Time.time;

        isAttacking = true;  // Dừng di chuyển
        anim.SetTrigger("Attack");  // Trigger animation

        int damage = attackDame + 5;

        // Luôn Instantiate effect attackZone làm con của pointAttack
        if (attackZonePrefab != null && pointAttack != null)
        {
            GameObject atkZone = Instantiate(attackZonePrefab, pointAttack.position, Quaternion.identity, pointAttack);  // Set parent = pointAttack
            Destroy(atkZone, 0.1f);  // Destroy sau thời gian ngắn
            Debug.Log("AttackZone instantiated as child of PointAttack at position: " + pointAttack.position);  // Debug để kiểm tra
        }
        else
        {
            Debug.LogWarning("attackZonePrefab or point_attack is null! Check Inspector.");  // Cảnh báo nếu chưa gán
        }

        // Tìm và gây damage cho enemy
        Collider2D[] enemies = Physics2D.OverlapCircleAll(pointAttack.position, attackRange, enemyMask);
        foreach (var enemyCollider in enemies)
        {
            if (enemyCollider.CompareTag("enemy"))
            {
                //PlayerController enemy = enemyCollider.GetComponent<PlayerController>();
                //if (enemy != null)
                //{
                //    enemy.TakeDamage(damage);
                //    Debug.Log("Damage applied to enemy: " + damage);  // Debug để kiểm tra
                //}
            }
        }
        StartCoroutine(ResetAttackState(0.3f));
    }

    private System.Collections.IEnumerator ResetAttackState(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    protected override void UseSkill1()
    {
        if (Time.time - lastSkill1Time < skill1Cooldown) return;
        lastSkill1Time = Time.time;

        isAttacking = true;  // Dừng di chuyển (dùng chung isAttacking)
        anim.SetTrigger("Skill");  // Trigger animation Skill

        int skillDamage = attackDame + 10;  // Damage skill (tùy chỉnh)

        // Instantiate effect Skill1 làm con của pointAttack
        if (skill1EffectPrefab != null && pointAttack != null)
        {
            GameObject skillEffect = Instantiate(skill1EffectPrefab, Skill1Point.position, Quaternion.identity, Skill1Point);
            Destroy(skillEffect, 0.5f);  // Tồn tại 0.5s
            Debug.Log("Skill1Effect instantiated as child of PointAttack");
        }
        else
        {
            Debug.LogWarning("skill1EffectPrefab or pointAttack is null!");
        }

        // Tìm và gây damage cho enemy trong phạm vi skill (tương tự tấn công)
        Collider2D[] enemies = Physics2D.OverlapCircleAll(pointAttack.position, skill1Range, enemyMask);
        foreach (var enemyCollider in enemies)
        {
            if (enemyCollider.CompareTag("enemy"))
            {
                //PlayerController enemy = enemyCollider.GetComponent<PlayerController>();
                //if (enemy != null)
                //{
                //    enemy.TakeDamage(skillDamage);
                //}
            }
        }
        if (skillUI != null)
        {
            skillUI.StartSkill1Cooldown();  // Bắt đầu hiển thị cooldown
        }
        StartCoroutine(ResetAttackState(0.3f));  // Reset sau 0.3s (dùng chung coroutine)
    }
    protected override void UseSkill2()
    {
        if (Time.time - lastSkill2Time < skill2Cooldown) return;
        lastSkill2Time = Time.time;

        isAttacking = true;  // Dừng di chuyển
        anim.SetTrigger("Skill");  // Trigger animation Skill (hoặc "Skill2" nếu có parameter riêng)

        int skill2Damage = attackDame + 15;  // Damage vụ nổ (tùy chỉnh)

        // Instantiate effect vụ nổ tại vị trí nhân vật, làm con của nhân vật (hoặc pointAttack)
        if (skill2EffectPrefab != null && transform != null)
        {
            GameObject explosionEffect = Instantiate(skill2EffectPrefab, transform.position, Quaternion.identity, transform);
            Destroy(explosionEffect, 0.5f);  // Tồn tại 0.5s
            Debug.Log("Skill2 Explosion Effect instantiated as child of player at position: " + transform.position);
        }
        else
        {
            Debug.LogWarning("skill2EffectPrefab or transform is null!");
        }

        // Gây damage cho enemy trong phạm vi vụ nổ
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, skill2Range, enemyMask);
        foreach (var enemyCollider in enemies)
        {
            if (enemyCollider.CompareTag("enemy"))
            {
                //PlayerController enemy = enemyCollider.GetComponent<PlayerController>();
                //if (enemy != null)
                //{
                //    enemy.TakeDamage(skill2Damage);
                //}
            }
        }
        if (skillUI != null)
        {
            skillUI.StartSkill2Cooldown();  // Bắt đầu hiển thị cooldown
        }
        StartCoroutine(ResetAttackState(0.5f));
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