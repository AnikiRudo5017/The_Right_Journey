using UnityEngine;

public class Btn_BackHome : BaseButton
{
    protected override void OnClick()
    {
        GameManager.Instance.UIManager.BackHome();
    }
}
