using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    [Header("FireBase Reference")]
    FirebaseApp app;
    public DatabaseReference dataBase;
    public FirebaseAuth auth;
    private string currentUserID;

    public PlayerData currentPlayerData;
    private List<PlayerData> leaderboardData = new List<PlayerData>();
    private void Start()
    {
        dataBase = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                app = Firebase.FirebaseApp.DefaultInstance;
                auth = FirebaseAuth.DefaultInstance;
                dataBase = FirebaseDatabase.DefaultInstance.RootReference;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                UnityEngine.Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
            }

        });
          // Lắng nghe thay đổi auth state
        auth.StateChanged += OnAuthStateChanged;
    }

    public void OnAuthStateChanged(object sender, EventArgs eventArgs)    // Kiểm tra sự kiện của người đăng nhập
    {
        if (auth.CurrentUser != null)
        {
            currentUserID = auth.CurrentUser.UserId;   //Gán user ID vào 1 biến string
            LoadPlayerDataFromLeaderboard();
        }
        else
        {
            currentUserID = null;
            
        }
    }
        public async Task LoadPlayerDataFromLeaderboard()
       {
        Debug.Log("Đang tải dữ liệu người chơi từ leaderboard...");


        leaderboardData.Clear();



        if (string.IsNullOrEmpty(currentUserID)) return;

       try
        {
            var snapshot = await dataBase.Child("leaderboard").Child(currentUserID).GetValueAsync();
            var snapshot2 = await dataBase.Child("leaderboard").GetValueAsync();

            if (snapshot2.Exists)
            {
                //foreach (var child in snapshot2.Children)  // cái này để lấy dữ liệu trên kia nhét vào 1 list sau đó sắp sếp  
                //{
                //    string json = child.GetRawJsonValue();
                //    PlayerData playerdata = JsonUtility.FromJson<PlayerData>(json);
                //    leaderboardData.Add(playerdata);
                //}



                if (snapshot.Exists)
                {
                    string json = snapshot.GetRawJsonValue();
                    currentPlayerData = JsonUtility.FromJson<PlayerData>(json);  // gán dữ liệu vào 1 dic

                    Debug.Log($"Tải thành công dữ liệu của user {currentUserID}: Gold = {currentPlayerData.gold}, Rank = {currentPlayerData.pointRank}");
                }
                else
                {
                    Debug.LogWarning($"Không tìm thấy dữ liệu cho user {currentUserID} trong leaderboard.");
                    currentPlayerData = null;
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Lỗi khi tải dữ liệu người chơi: " + ex.Message);
        }
    }
    public PlayerData GetPlayerdata()
    {
        return currentPlayerData;
    }
    public List<PlayerData> ListLeaderBoard()
    {
        return leaderboardData;
    }
    public async Task<bool> SavePlayerDataToLeaderboard(PlayerData playerData) // hàm này dùng để savegame
    {
        if (playerData == null || string.IsNullOrEmpty(playerData.userId))
        {
            Debug.LogWarning("Dữ liệu không hợp lệ để lưu.");
            return false;
        }

        try
        {
            // Cập nhật timestamp trước khi lưu
            playerData.timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            string json = JsonUtility.ToJson(playerData);
            await dataBase.Child("leaderboard").Child(playerData.userId).SetRawJsonValueAsync(json);

            Debug.Log("Đã lưu dữ liệu thành công cho user: " + playerData.userId);
            return true;
        }
        catch (Exception ex)
        {
            Debug.LogError("Lỗi khi lưu dữ liệu leaderboard: " + ex.Message);
            return false;
        }
    }

    public async Task<bool> NEWGAME(string PlayerName)
    {
        if (string.IsNullOrEmpty(currentUserID)) return false;

        try {
            PlayerData playerdatanew = new PlayerData()
            {
                playerName = PlayerName,
                pointRank = 0,
                gold = 0,
                timestamp = 0,
                level=1

            };
            string json = JsonUtility.ToJson(playerdatanew);

            await dataBase.Child("leaderboard").Child(currentUserID).SetRawJsonValueAsync(json);
            Debug.Log("JSON to save: " + json);

            return true;
        }

        catch(Exception ex) {
            Debug.Log("Lỗi lưu game: " + ex.Message);
            return false;

        }

    }

    public async Task LoadLeaderboard()
    {
      await   dataBase.Child("leaderboard").GetValueAsync().ContinueWith(task =>
        {
            if (task.IsCompletedSuccessfully)
            {
                var snapshot = task.Result;
               

                foreach (var child in snapshot.Children)
                {
                    var data = child.Value as Dictionary<string, object>;

                    leaderboardData.Add(new PlayerData
                    {
                        userId = data["userId"].ToString(),
                        gold = int.Parse(data["gold"].ToString()),
                        pointRank = int.Parse(data["pointRank"].ToString())
                    });
                }

                // Sắp xếp theo pointRank giảm dần
                leaderboardData = leaderboardData.OrderByDescending(p => p.pointRank).ToList();
               
                // In ra console để test
                for (int i = 0; i < leaderboardData.Count; i++)
                {
                    Debug.Log($"#{i + 1}: {leaderboardData[i].userId} - {leaderboardData[i].pointRank} điểm");
                }
            }
        });
    }


    //public async Task<bool> DeletePlayerDataFromLeaderboard()
    //{
    //    if (string.IsNullOrEmpty(currentUserID)) return false;

    //    try
    //    {
    //        await dataBase.Child("leaderboard").Child(currentUserID).Child("gold").RemoveValueAsync();
    //        await dataBase.Child("leaderboard").Child(currentUserID).Child("playerName").RemoveValueAsync();
    //        await dataBase.Child("leaderboard").Child(currentUserID).Child("pointRank").RemoveValueAsync();
    //        await dataBase.Child("leaderboard").Child(currentUserID).Child("timestamp").RemoveValueAsync();
    //        Debug.Log($"Đã xóa dữ liệu của user {currentUserID} khỏi leaderboard.");
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.LogError("Lỗi khi xóa dữ liệu khỏi leaderboard: " + ex.Message);
    //        return false;
    //    }
    //}

}