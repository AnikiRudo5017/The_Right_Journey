using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    [Header("Skill 1 UI")]
    public Button skill1Button;  // Button icon skill1
    public Image skill1CooldownMask;  // Mask vuông trắng quay đếm ngược
    public TextMeshProUGUI skill1CooldownText;  // Text đếm giây

    [Header("Skill 2 UI")]
    public Button skill2Button;  // Button icon skill2
    public Image skill2CooldownMask;  // Mask vuông trắng quay đếm ngược
    public TextMeshProUGUI skill2CooldownText;  // Text đếm giây

    public Warrior warrior;  // Gán Warrior để lấy cooldown

    private float skill1Remaining = 0f;
    private float skill2Remaining = 0f;

    void Awake()
    {
        DontDestroyOnLoad(this);  // Làm Canvas persist
    }
    void Update()
    {
        // Skill1: Ban đầu fillAmount = 0 (icon bình thường, không mask)
        if (skill1Remaining > 0)
        {
            skill1Remaining -= Time.deltaTime;
            skill1CooldownMask.fillAmount = skill1Remaining / warrior.skill1Cooldown;  // Quay đếm ngược (từ 1 xuống 0)
            skill1CooldownText.text = Mathf.CeilToInt(skill1Remaining).ToString();
            skill1Button.interactable = false;  // Disable button trong cooldown
        }
        else
        {
            skill1CooldownMask.fillAmount = 0;  // Ẩn mask, icon bình thường
            skill1CooldownText.text = "";
            skill1Button.interactable = true;
        }

        // Skill2: Tương tự
        if (skill2Remaining > 0)
        {
            skill2Remaining -= Time.deltaTime;
            skill2CooldownMask.fillAmount = skill2Remaining / warrior.skill2Cooldown;
            skill2CooldownText.text = Mathf.CeilToInt(skill2Remaining).ToString();
            skill2Button.interactable = false;
        }
        else
        {
            skill2CooldownMask.fillAmount = 0;
            skill2CooldownText.text = "";
            skill2Button.interactable = true;
        }
    }

    public void StartSkill1Cooldown()
    {
        skill1Remaining = warrior.skill1Cooldown;
    }

    public void StartSkill2Cooldown()
    {
        skill2Remaining = warrior.skill2Cooldown;
    }
}