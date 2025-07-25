using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class LeaderBoardUI : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject leaderboardPanel;
    public GameObject rowPrefab; // Gán prefab 1 dòng
    public Transform contentHolder; // Gán Content của Scroll View

    public GameSaveManager saveManager;
    List<PlayerData> playerDataList;
    private async Task Start()
    {
        saveManager=FindFirstObjectByType<GameSaveManager>();
        //await UpdateLeaderBoard();
    }

    
    public async Task UpdateLeaderBoard()
    {
     await  saveManager.LoadLeaderboard();
        playerDataList = saveManager.ListLeaderBoard();
       RefreshUI();
      

    }
    private void RefreshUI()
    {
        // Xoá các dòng cũ
        foreach (Transform child in contentHolder)
        {
            Destroy(child.gameObject);
        }

        // Tạo dòng mới từ danh sách
        for (int i = 0; i < playerDataList.Count; i++)
        {
            var data = playerDataList[i];
            GameObject row = Instantiate(rowPrefab, contentHolder);

            // Lấy TMP_Text con của dòng
            TMP_Text[] texts = row.GetComponentsInChildren<TMP_Text>();
            texts[0 ].text = $"#{i + 1}";                     // Rank
            texts[1].text = data.userId;                    // Name
            texts[2].text = data.pointRank.ToString();      // Score
        }
    }
    public void ShowLeaderboard()
    {
        leaderboardPanel.SetActive(true);
    }

    public void HideLeaderboard()
    {
        leaderboardPanel.SetActive(false);
    }
}
