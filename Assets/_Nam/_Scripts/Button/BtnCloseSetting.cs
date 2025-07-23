using UnityEngine;



public class BtnCloseSetting : BaseButton
{
    protected override void OnClick()
    {
        GameManager.Instance.AudioManager.Play("ButtonClick");
        GameManager.Instance.UIManager.OnCloseSettingpanel();
    }
}

