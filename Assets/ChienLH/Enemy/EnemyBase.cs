using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public abstract class EnemyBase : MonoBehaviour
{
    [Header("Movement & Combat")]
    public float moveSpeed = 3f;
    public float chaseRange = 6f;
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;
    public int damageAmount = 1;

    [Header("Health")]
    public int maxHealth = 3;

    [Header("UI - Health Bar")]
    public Slider healthBar;
    public float healthLerpSpeed = 5f;

    protected int currentHealth;
    protected Transform player;
    protected float lastAttackTime;
    protected bool isDead = false;
    protected bool isAttacking = false;
    protected Animator animator;

    protected virtual void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.minValue = 0f;
            healthBar.maxValue = 1f;
            healthBar.value = 1f;
        }
    }

    protected virtual void Update()
    {
        if (isDead || player == null)
            return;
        float distance = Vector2.Distance(transform.position, player.position);

        if (isAttacking)
        {
            animator.SetBool("run", false);
        }
        else if (distance <= attackRange)
        {
            animator.SetBool("run", false);
            Attack();
        }
        else if (distance <= chaseRange)
        {
            animator.SetBool("run", true);
            ChaseTowardsEdge(distance);
        }
        else
        {
            animator.SetBool("run", false);
        }
        if (healthBar != null)
        {
            float target = (float)currentHealth / maxHealth;
            healthBar.value = Mathf.Lerp(healthBar.value, target, Time.deltaTime * healthLerpSpeed);
        }
        if (!isDead && player != null)
        {
            float directionX = player.position.x - transform.position.x;
            Flip(directionX);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player") &&
            collision.collider.TryGetComponent<PlayerHealth>(out var health))
        {
            health.TakeDamage(damageAmount);
        }
    }

    public virtual void TakeDamageEnemy(int amount)
    {
        if (isDead)
            return;

        currentHealth -= amount;
        animator.SetTrigger("hit");
        if (healthBar != null)
            healthBar.value = (float)currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(HitStunRoutine());
        }
    }

    protected IEnumerator HitStunRoutine()
    {
        isAttacking = true;
        yield return new WaitForSeconds(0.3f);
        isAttacking = false;
    }

    protected virtual void Die()
    {
        isDead = true;
        animator.SetTrigger("die");

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 1f);
    }

    protected void ChaseTowardsEdge(float currentDistance)
    {
        if (isAttacking) return;

        Vector3 dir = (player.position - transform.position).normalized;
        float desiredDist = currentDistance - attackRange;
        float moveStep = moveSpeed * Time.deltaTime;
        float step = Mathf.Min(moveStep, desiredDist);

        transform.position += dir * step;
        Flip(dir.x);
    }

    protected void Flip(float dirX)
    {
        if (dirX > 0) transform.localScale = Vector3.one;
        else if (dirX < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    public void Initialize(Transform playerTransform)
    {
        player = playerTransform;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
    public abstract void Attack();
    public int CurrentHealth => currentHealth;
}
