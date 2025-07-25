using UnityEngine;

public abstract class PlayerController : MonoBehaviour
{
    [Header("Level")]
    public int level = 1;

    [Header("Setup")]
    public int hp;
    public int maxHP;
    public int attackDame;

    [Header("Movement")]
    public float moveSpeed;
    private Vector2 movementInput;
    private float horizontal;
    private float vertical;

    [Header("Physics")]
    private Rigidbody2D rb;
    public Collider2D col;
    public SpriteRenderer spriteRenderer;

    [Header("Animation")]
    public Animator anim;

    private bool isFacingRight = true;
    public Joystick joystick;
    protected bool isAttacking = false;
    protected float lastAttackTime;

    void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (col == null) col = GetComponent<Collider2D>();
        if (anim == null) anim = GetComponentInChildren<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        lastAttackTime = 0f;
    }

    protected virtual void Update()
    {
        HandleInput();
    }

    protected virtual void FixedUpdate()
    {
        HandleMovement();
        if (isAttacking)
        {
            rb.linearVelocity = Vector2.zero;  // Dừng di chuyển khi tấn công
            return;
        }
        Vector2 velocity = movementInput.normalized * moveSpeed;
        rb.linearVelocity = velocity;
    }

    protected virtual void HandleInput()
    {
        horizontal = joystick.Horizontal;
        vertical = joystick.Vertical;
        // Không cần kiểm tra phím nữa, sử dụng UI Buttons để gọi Attack() và UseNomalSkill()
    }

    protected virtual void HandleMovement()
    {
        movementInput = new Vector2(horizontal, vertical);

        if (movementInput.magnitude > 0.1f)
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }

        if (movementInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (movementInput.x < 0 && isFacingRight)
        {
            Flip();
        }
    }

    // Method public để UI Button gọi tấn công
    public void AttackButtonPressed()
    {
        Attack();
    }

    // Method public để UI Button gọi kỹ năng
    public void Skill1ButtonPressed()
    {
        UseSkill1();
    }
    public void Skill2ButtonPressed()
    {
        UseSkill2();
    }

    protected virtual void Attack()
    {
        PerformAttack();
    }

    protected abstract void PerformAttack();

    protected abstract void UseSkill1();
    protected abstract void UseSkill2();

    protected virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(1, damage);
        hp = Mathf.Max(0, hp - actualDamage);
        if (hp <= 0)
        {
            anim.SetTrigger("Die");
            // Thêm logic chết (ví dụ: Destroy(gameObject) sau animation Die)
        }
        else
        {
            anim.SetTrigger("Hurt");
        }
    }

    public virtual void RestoreHP(int amount)
    {
        hp = Mathf.Min(maxHP, hp + amount);
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}