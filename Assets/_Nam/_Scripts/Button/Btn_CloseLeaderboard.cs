using UnityEngine;

public class Btn_CloseLeaderboard : BaseButton
{
    protected override void OnClick()
    {
       GameManager.Instance.UIManager.OnCloseLeaderBoard();
    }
}
