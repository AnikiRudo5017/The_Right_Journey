
[System.Serializable]
public class PlayerData
{
    public string userId;
    public string playerName;
    public int pointRank;
    public int gold;
    public long timestamp;

    public PlayerData() { }

    public PlayerData(string userId, string playerName, int gold, int pointRank)
    {
        this.userId = userId;
        this.playerName = playerName;
        this.gold = gold;
        this.timestamp = System.DateTimeOffset.Now.ToUnixTimeSeconds();
        this.pointRank = pointRank;
        
    }
}

public static class NeworLoad
{
    public static bool newGAME =true;
}