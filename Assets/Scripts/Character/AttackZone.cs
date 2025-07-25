using UnityEngine;

public class AttackZone : MonoBehaviour
{
    private float damage;
    private float lifeTime = 0.1f;
    public void Initialize(int damage)
    {
        this.damage = damage;
        GetComponent<CircleCollider2D>().isTrigger = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        //Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, attackRange);
        //damageTimer -= Time.deltaTime;
        //if (damageTimer <= 0f)
        //{
        //    foreach (var enemy in enemies)
        //    {
        //        if (enemy.tag == "enemy")
        //        {
        //            var target = enemy.GetComponent<EnemyBase>();
        //            if (target != null)
        //            {
        //                target.TakeDame(damagePerHalfSecond);
        //                damageTimer = 0.5f;
        //            }
        //        }

        //    }
        //}
    }
}
