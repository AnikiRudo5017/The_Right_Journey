using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardItem : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI rankText;
    public TextMeshProUGUI playerNameText;
    public TextMeshProUGUI goldText;
    public Image backgroundImage;

    [Header("Rank Colors")]
    public Color firstPlaceColor = Color.yellow;
    public Color secondPlaceColor = Color.gray;
    public Color thirdPlaceColor = new Color(0.8f, 0.5f, 0.2f);
    public Color defaultColor = Color.white;

    public void SetupItem(int rank, string userId, string playerName, int gold)
    {
        // Hiển thị thông tin cơ bản
        rankText.text = $"#{rank}";
        playerNameText.text = playerName;
        goldText.text = FormatGoldAmount(gold);

        // Thiết lập màu sắc theo rank
        SetupRankAppearance(rank);

        // Highlight nếu là player hiện tại
        if (IsCurrentPlayer(userId))
        {
            HighlightCurrentPlayer();
        }
    }

    private void SetupRankAppearance(int rank)
    {
        Color rankColor = defaultColor;

        switch (rank)
        {
            case 1:
                rankColor = firstPlaceColor;
                rankText.text = "🥇 #1";
                break;
            case 2:
                rankColor = secondPlaceColor;
                rankText.text = "🥈 #2";
                break;
            case 3:
                rankColor = thirdPlaceColor;
                rankText.text = "🥉 #3";
                break;
            default:
                rankColor = defaultColor;
                rankText.text = $"#{rank}";
                break;
        }

        // Áp dụng màu sắc
        if (backgroundImage != null)
        {
            backgroundImage.color = new Color(rankColor.r, rankColor.g, rankColor.b, 0.3f);
        }

        if (rankText != null)
        {
            rankText.color = rankColor;
        }
    }

    private string FormatGoldAmount(int gold)
    {
        if (gold >= 1000000)
        {
            return $"{gold / 1000000f:F1}M";
        }
        else if (gold >= 1000)
        {
            return $"{gold / 1000f:F1}K";
        }
        else
        {
            return gold.ToString();
        }
    }

    private bool IsCurrentPlayer(string userId)
    {
        var auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        return auth.CurrentUser != null && auth.CurrentUser.UserId == userId;
    }

    private void HighlightCurrentPlayer()
    {
        // Highlight background cho player hiện tại
        if (backgroundImage != null)
        {
            backgroundImage.color = new Color(0f, 1f, 0f, 0.5f);
        }

        // Thêm text indicator
        if (playerNameText != null)
        {
            playerNameText.text += " (Bạn)";
            playerNameText.fontStyle = FontStyles.Bold;
        }
    }
}