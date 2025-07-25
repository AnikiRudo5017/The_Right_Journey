using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  // Thêm để sử dụng Slider
using UnityEngine.SceneManagement;  // Thêm cho sceneLoaded

public abstract class PlayerController : MonoBehaviour
{
    [Header("Level")]
    public int level;
    [Header("Pointrank")]
    public int pointRank;

    [Header("Setup")]
    public int hp;
    public int maxHP = 100;
    public int armor;
    public int maxArmor = 50;
    public int attackDame = 10;

    [Header("Movement")]
    public float moveSpeed=3.5f;
    public Vector2 movementInput;
    private float horizontal;
    private float vertical;

    [Header("Physics")]
    public Rigidbody2D rb;
    public Collider2D col;
    public SpriteRenderer spriteRenderer;

    [Header("Animation")]
    public Animator anim;

    [Header("UI Bars")]
    public Slider hpBar;  // Slider hiển thị máu (HP)
    public Slider armorBar;  // Slider hiển thị giáp (Armor)
    public Slider expSlider;

    [Header("Armor Regen")]
    public float armorRegenInterval = 5f;  // Thời gian hồi giáp (5s)
    public int armorRegenAmount = 1;  // Số giáp hồi mỗi lần

    public bool isFacingRight = true;
    public Joystick joystick;
    protected bool isAttacking = false;
    protected float lastAttackTime;
    private float lastArmorRegenTime;

    // Biến để theo dõi thay đổi (để tránh set slider liên tục)
    private int lastHp;
    private int lastArmor;

    [Header("Lấy dữ liệu từ save")]
    GameSaveManager saveManager;


    [Header("LeverSystem")]
    public PlayerLevel levelInfo = new PlayerLevel();

    private static PlayerController instance;

    [Header("dash")]
    public float dashDistance = 3f;
    public float dashDuration = 0.15f;
    private bool isDashing = false;
    public float dashForce = 70f;  // Lực dash (tùy chỉnh, thay speed bằng force cho trơn tru)



    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);  // Giữ nhân vật
        }
        else
        {
            Destroy(gameObject);  // Xóa duplicate
            return;
        }

        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        lastAttackTime = 0f;

        // Thêm debug để kiểm tra rb
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D not found on " + gameObject.name + ". Please attach Rigidbody2D component.");
        }
    }

    void Start()
    {
        saveManager = FindFirstObjectByType<GameSaveManager>();
        // Khởi tạo slider khi bắt đầu game
        if (hpBar != null)
        {
            hpBar.maxValue = maxHP;
            hpBar.value = hp;
        }
        if (armorBar != null)
        {
            armorBar.maxValue = maxArmor;
            armorBar.value = armor;
        }

        lastArmorRegenTime = Time.time;  // Khởi tạo thời gian hồi giáp
        lastHp = hp;  // Theo dõi ban đầu
        lastArmor = armor;


        StartCoroutine(LoadDATAFromSave());
    }

    void Update()
    { 
    
        HandleInput();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movementInput = new Vector2(horizontal, vertical);
            DashPressed();
        }

        // Hồi giáp tự động mỗi 5s
        if (Time.time - lastArmorRegenTime >= armorRegenInterval)
        {
            RestoreArmor(armorRegenAmount);
            lastArmorRegenTime = Time.time;
            Debug.Log("Armor regenerated: " + armorRegenAmount + ", Current armor: " + armor);  // Debug để kiểm tra regen
        }

        UpdateDisplayEXP();
        Invoke("setMovespeed", 6f);
    }

    public void setMovespeed()
    {
        moveSpeed = 3.5f;
    }
    protected virtual void LateUpdate()
    {
        // Đồng bộ slider chỉ khi giá trị thay đổi (tối ưu, tránh set mỗi frame)
        if (hpBar != null && hp != lastHp)
        {
            hpBar.maxValue = maxHP;
            hpBar.value = hp;
            lastHp = hp;
        }
        if (armorBar != null && armor != lastArmor)
        {
            armorBar.maxValue = maxArmor;
            armorBar.value = armor;
            lastArmor = armor;
        }
    }

    protected virtual void FixedUpdate()
    {
        HandleMovement();
        if (isAttacking)
        {
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;  // Dừng di chuyển khi tấn công
            }
            return;
        }
        Vector2 velocity = movementInput.normalized * moveSpeed;
        if (rb != null)
        {
            rb.linearVelocity = velocity;
        }
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
    public void DashPressed()
    {
        Dash();
    }


    protected virtual void Attack()
    {
        PerformAttack();
    }

    protected abstract void PerformAttack();

    protected abstract void UseSkill1();
    protected abstract void UseSkill2();
    public void Dash()
    {
        moveSpeed = 6f;
    }

   

    protected virtual void TakeDamage(int damage)
    {
        int remainingDamage = damage;

        // Giảm giáp trước
        if (armor > 0)
        {
            if (remainingDamage >= armor)
            {
                remainingDamage -= armor;
                armor = 0;
            }
            else
            {
                armor -= remainingDamage;
                remainingDamage = 0;
            }
        }

        // Giảm máu nếu còn damage thừa
        if (remainingDamage > 0)
        {
            hp = Mathf.Max(0, hp - remainingDamage);  // Đảm bảo hp không âm
        }

        // Cập nhật slider
        if (hpBar != null) hpBar.value = hp;
        if (armorBar != null) armorBar.value = armor;

        if (hp <= 0)
        {
            anim.SetTrigger("Die");
            StartCoroutine(DieAfterAnimation(1f));  // Giả sử animation Die kéo dài 1s, điều chỉnh nếu cần
        }
        else
        {
            anim.SetTrigger("Hurt");
        }
    }

    public virtual void RestoreHP(int amount)
    {
        hp = Mathf.Min(maxHP, hp + amount);  // Hồi máu, không vượt maxHP
    }

    public virtual void RestoreArmor(int amount)
    {
        armor = Mathf.Min(maxArmor, armor + amount);  // Hồi giáp, không vượt maxArmor
    }

    public void Flip()
    {
        isFacingRight = !isFacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private System.Collections.IEnumerator DieAfterAnimation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);  // Destroy nhân vật sau animation Die
    }


    public virtual void GainExpFromEnemy(float percent)  // hàm trừ máu gọi vào các nhân vật
    {
        float expToGain = levelInfo.maxEXP * percent;
        bool leveledUp = levelInfo.AddExp(expToGain);

        if (leveledUp)
        {
            // Gọi các chức năng khi lên level (buff máu, mở kỹ năng...)
            maxHP = maxHP + level * 20;         // mỗi cấp +20 HP
            maxArmor = maxArmor + level * 5;    // mỗi cấp +5 Armor
            attackDame = attackDame + level * 3; // mỗi cấp +3 damage
            Debug.Log("Lên level! Level mới: " + levelInfo.currentLevel);
        }
    }
    public virtual void UpdateDisplayEXP()  // hiển thị exp
    {
        expSlider.maxValue = levelInfo.maxEXP;
        expSlider.value = levelInfo.currentEXP;

    }

    public virtual IEnumerator LoadDATAFromSave()
    {
        yield return new WaitForSeconds(1.5f);
        if (NeworLoad.newGAME == false)
        {
            level = saveManager.GetPlayerdata().level;
        }
        else
        {
            level = 1;
        }
    }

    public IEnumerator AutoSave()
    {
        return null;
    }
}