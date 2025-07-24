using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseLeaderboard : MonoBehaviour
{
    [Header("Firebase References")]
    public FirebaseAuth auth;
    public DatabaseReference database;

    [Header("UI Components")]
    public GameObject leaderboardPanel;
    public Transform leaderboardContent;
    public GameObject leaderboardItemPrefab;
    public Button refreshButton;
    public Button closeButton;

    [Header("Player Info UI")]
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI playerGoldText;
    public TextMeshProUGUI playerRankText;
    public Button saveScoreButton;
    public TMP_InputField goldInputField; // For testing

    [System.Serializable]
    public class PlayerData
    {
        public string userId;
        public string playerName;
        public int gold;
        public long timestamp;

        public PlayerData() { }

        public PlayerData(string userId, string playerName, int gold)
        {
            this.userId = userId;
            this.playerName = playerName;
            this.gold = gold;
            this.timestamp = System.DateTimeOffset.Now.ToUnixTimeSeconds();
        }
    }

    private List<PlayerData> leaderboardData = new List<PlayerData>();
    private PlayerData currentPlayerData;

    void Start()
    {
        // Initialize Firebase
        auth = FirebaseAuth.DefaultInstance;
        database = FirebaseDatabase.DefaultInstance.RootReference;

        // Setup UI events
        refreshButton.onClick.AddListener(LoadLeaderboard);
        closeButton.onClick.AddListener(() => leaderboardPanel.SetActive(false));
        saveScoreButton.onClick.AddListener(SavePlayerScore);

        // Load leaderboard on start
        LoadLeaderboard();
        LoadCurrentPlayerData();
    }

    // Lưu điểm số của player hiện tại
    public void SavePlayerScore()
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("Người chơi chưa đăng nhập!");
            return;
        }

        string userId = auth.CurrentUser.UserId;
        string playerName = auth.CurrentUser.Email; // Hoặc DisplayName

        // Lấy số vàng từ input field (hoặc từ game data)
        int gold = 0;
        if (goldInputField != null && !string.IsNullOrEmpty(goldInputField.text))
        {
            int.TryParse(goldInputField.text, out gold);
        }
        else
        {
            // Lấy từ game data thực tế
            gold = GetCurrentPlayerGold();
        }

        PlayerData playerData = new PlayerData(userId, playerName, gold);

        StartCoroutine(SavePlayerDataToFirebase(playerData));
    }

    // Lưu dữ liệu player lên Firebase
    private IEnumerator SavePlayerDataToFirebase(PlayerData playerData)
    {
        var task = database.Child("leaderboard").Child(playerData.userId).SetValueAsync(playerData);

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi lưu dữ liệu: " + task.Exception);
        }
        else
        {
            Debug.Log("Lưu điểm thành công!");
            LoadLeaderboard(); // Refresh leaderboard
        }
    }

    // Load toàn bộ leaderboard
    public void LoadLeaderboard()
    {
        StartCoroutine(LoadLeaderboardFromFirebase());
    }

    private IEnumerator LoadLeaderboardFromFirebase()
    {
        var task = database.Child("leaderboard").OrderByChild("gold").GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsFaulted)
        {
            Debug.LogError("Lỗi load leaderboard: " + task.Exception);
        }
        else
        {
            DataSnapshot snapshot = task.Result;
            leaderboardData.Clear();

            foreach (var child in snapshot.Children)
            {
                var playerData = JsonUtility.FromJson<PlayerData>(child.GetRawJsonValue());
                if (playerData != null)
                {
                    leaderboardData.Add(playerData);
                }
            }

            // Sắp xếp theo vàng giảm dần
            leaderboardData = leaderboardData.OrderByDescending(p => p.gold).ToList();

            DisplayLeaderboard();
        }
    }

    // Hiển thị leaderboard trên UI
    private void DisplayLeaderboard()
    {
        // Xóa các item cũ
        foreach (Transform child in leaderboardContent)
        {
            Destroy(child.gameObject);
        }

        // Tạo item mới cho mỗi player
        for (int i = 0; i < leaderboardData.Count; i++)
        {
            GameObject item = Instantiate(leaderboardItemPrefab, leaderboardContent);
            LeaderboardItem itemScript = item.GetComponent<LeaderboardItem>();

            if (itemScript != null)
            {
                itemScript.SetupItem(i + 1, leaderboardData[i]);
            }
        }

        UpdateCurrentPlayerInfo();
    }

    // Load thông tin player hiện tại
    private void LoadCurrentPlayerData()
    {
        if (auth.CurrentUser == null) return;

        string userId = auth.CurrentUser.UserId;
        StartCoroutine(LoadCurrentPlayerDataFromFirebase(userId));
    }

    private IEnumerator LoadCurrentPlayerDataFromFirebase(string userId)
    {
        var task = database.Child("leaderboard").Child(userId).GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (!task.IsFaulted && task.Result.Exists)
        {
            currentPlayerData = JsonUtility.FromJson<PlayerData>(task.Result.GetRawJsonValue());
            UpdateCurrentPlayerInfo();
        }
    }

    // Cập nhật thông tin player hiện tại trên UI
    private void UpdateCurrentPlayerInfo()
    {
        if (currentPlayerData != null)
        {
            playerNameText.text = currentPlayerData.playerName;
            playerGoldText.text = currentPlayerData.gold.ToString();

            // Tìm rank của player hiện tại
            int rank = leaderboardData.FindIndex(p => p.userId == currentPlayerData.userId) + 1;
            playerRankText.text = rank > 0 ? $"#{rank}" : "Chưa xếp hạng";
        }
    }

    // Lấy số vàng hiện tại của player (implement theo game của bạn)
    private int GetCurrentPlayerGold()
    {
        // Thay bằng logic thực tế của game
        // Ví dụ: return GameManager.Instance.playerGold;
        return Random.Range(1000, 10000); // Demo random
    }

    // Mở/đóng leaderboard panel
    public void ToggleLeaderboard()
    {
        leaderboardPanel.SetActive(!leaderboardPanel.activeInHierarchy);
        if (leaderboardPanel.activeInHierarchy)
        {
            LoadLeaderboard();
        }
    }

    // Cập nhật điểm số theo thời gian thực (optional)
    public void StartRealtimeUpdates()
    {
        database.Child("leaderboard").ValueChanged += OnLeaderboardChanged;
    }

    public void StopRealtimeUpdates()
    {
        database.Child("leaderboard").ValueChanged -= OnLeaderboardChanged;
    }

    private void OnLeaderboardChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError("Database error: " + args.DatabaseError.Message);
            return;
        }

        // Refresh leaderboard when data changes
        LoadLeaderboard();
    }

    void OnDestroy()
    {
        StopRealtimeUpdates();
    }
}