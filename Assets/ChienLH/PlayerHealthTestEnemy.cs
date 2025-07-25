using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    [Tooltip("Máu tối đa của người chơi")]
    public int maxHealth = 5;
    [Tooltip("Thời gian bất động (hit stun) sau khi chịu sát thương")]
    public float hitStunDuration = 0.3f;

    [Header("UI (optional)")]
    [Tooltip("Text hiển thị máu, nếu có")]
    public Text healthText; // Thay vì Slider

    private int currentHealth;
    private bool isDead = false;
    private bool isHitStunned = false;
    private Animator animator;

    void Awake()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();

        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (isDead || isHitStunned)
            return;

        int actualDamage = Mathf.Max(1, damage);
        currentHealth = Mathf.Max(0, currentHealth - actualDamage);

        UpdateHealthUI();

        animator.SetTrigger("Hurt");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(HitStunRoutine());
        }
    }

    public void RestoreHealth(int amount)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Min(maxHealth, currentHealth + amount);
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        if (healthText != null)
        {
            healthText.text = $"HP: {currentHealth}/{maxHealth}";
        }
    }

    private IEnumerator HitStunRoutine()
    {
        isHitStunned = true;
        yield return new WaitForSeconds(hitStunDuration);
        isHitStunned = false;
    }

    private void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");

        var col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject, 1f);
    }

    public bool IsDead()
    {
        return isDead;
    }
}
