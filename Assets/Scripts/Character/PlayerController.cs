using UnityEngine;
using UnityEngine.Rendering;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public abstract class PlayerController : MonoBehaviour
{
    [Header("SetUp")]
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        // Xử lý vật lý di chuyển
        Vector2 velocity = movementInput.normalized * moveSpeed;
        rb.linearVelocity = velocity;
    }
    protected virtual void HandleInput()
    {
        horizontal = joystick.Horizontal;
        vertical = joystick.Vertical;
        if (Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            UseNomalSkill();
        }
    }
    protected virtual void HandleMovement()
    {
        movementInput = new Vector2(horizontal, vertical);

        // Xử lý animation
        if (movementInput.magnitude > 0.1f)  // Ngưỡng tránh giật
        {
            anim.SetBool("isRun", true);
        }
        else
        {
            anim.SetBool("isRun", false);
        }

        // Lật mặt nhân vật
        if (movementInput.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (movementInput.x < 0 && isFacingRight)
        {
            Flip();
        }
    }
    protected virtual void Attack()
    {
        PerformAttack();
    }

    protected abstract void PerformAttack();

    protected abstract void UseNomalSkill();
    protected virtual void TakeDamage(int damage)
    {
        int actualDamage = Mathf.Max(1, damage);
        hp = Mathf.Max(0, hp - actualDamage);  // Đảm bảo hp không âm
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

    public virtual void RestoreHP(int amount)  //Khôi phục HP  gọi trong các hàm của item
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
