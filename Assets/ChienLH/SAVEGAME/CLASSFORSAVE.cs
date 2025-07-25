
using System;

[System.Serializable]
public class PlayerData
{
    public string userId;
    public string playerName;
    public int pointRank;
    public int gold;
    public long timestamp;
    public int level;

    public PlayerData() { }

    public PlayerData(string userId, string playerName, int gold, int pointRank,int level)
    {
        this.userId = userId;
        this.playerName = playerName;
        this.gold = gold;
        this.timestamp = System.DateTimeOffset.Now.ToUnixTimeSeconds();
        this.pointRank = pointRank;
        this.level = level;
    }
}


[System.Serializable]
public class PlayerLevel
{
    public int currentLevel = 1;
    public float currentEXP = 0;
    public float maxEXP = 100;

    // Hàm tăng EXP
    public bool AddExp(float amount)
    {
        currentEXP += amount;
        if (currentEXP >= maxEXP)
        {
            LevelUp();
            return true; // lên cấp
        }
        return false;
    }

    private void LevelUp()
    {
        currentLevel++;
        currentEXP -= maxEXP;
        maxEXP *= 1.2f; // tăng yêu cầu EXP theo cấp
        // Gọi sự kiện tăng cấp tại đây nếu cần

    }
}


public static class NeworLoad
{
    public static bool newGAME =true;
}