using UnityEngine;

public class Btn_LeaderBoard : BaseButton
{
    protected override void OnClick()
    {
        GameManager.Instance.UIManager.OnOpenLeaderBoard();
    }
}
