using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private int armor;
    [SerializeField] private int maxArmor;

    public int Health => health;
    public int MaxHealth => maxHealth;
    public int MaxArmor => armor;
    public int Armor => armor;

    private void Start()
    {
        Initilaze();
    }
    public int SetHealth()
    {
        return health = maxHealth;
    }
    public int GetMaxHealth() { return maxHealth; }
    public int SetArmor() { return armor = maxArmor; }

    public int GetHealth() { return health; }
    public int GetArmor() { return armor; }
    public int GetMaxArmor() { return armor; }
    public void UpdateStats(int health, int maxHealth,int armor,int MaxArmor)
    {
        this.health = health;
        this.maxHealth = maxHealth;
        this.armor = armor;
        this.maxArmor = MaxArmor;
        Debug.Log($"{this.health} {this.maxHealth} {this.armor}  {this.maxArmor} ");
    }

    private void Initilaze()
    {
        SetHealth();
        GetMaxHealth();
        SetArmor();
        GetMaxArmor();
    }
   
}
