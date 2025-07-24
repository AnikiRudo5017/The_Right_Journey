using UnityEngine;

public class GameSaveManager : MonoBehaviour
{
    [Header("FireBase Reference")]
    FirebaseApp app;
    public DatabaseReference dataBase;
    public FirebaseAuth auth;
    private string currentUserID;
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

        }
          // Lắng nghe thay đổi auth state
        auth.StateChanged += OnAuthStateChanged;
    }

    public void OnAuthStateChanged(object sender, EventArgs eventArgs)    // Kiểm tra sự kiện của người đăng nhập
    {
        if (auth.CurrentUser != null)
        {
            currentUserID = auth.CurrentUser.UserId;   //Gán user ID vào 1 biến string
            LoadAllSaveSlots();
        }
        else
        {
            currentUserID = null;
            saveSlot.Clear();
        }
    }
    public async Task LoadLeaderboardData()
    {
        Debug.Log("Đang tải dữ liệu bảng xếp hạng...");

        try
        {

            var snapshot = await dataBase.Child("leaderboard").GetValueAsync();
            leaderboardData.Clear();

            if (snapshot.Exists)
            {
                foreach (var childSnapshot in snapshot.Children)
                {
                    string json = childSnapshot.GetRawJsonValue();
                    PlayerData data = JsonUtility.FromJson<PlayerData>(json);
                    leaderboardData[data.userId] = data;
                }

                Debug.Log("Tải leaderboard thành công!");
            }
            else
            {
                Debug.Log("Không có dữ liệu leaderboard.");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError("Lỗi tải leaderboard: " + ex.Message);
        }
    }


}