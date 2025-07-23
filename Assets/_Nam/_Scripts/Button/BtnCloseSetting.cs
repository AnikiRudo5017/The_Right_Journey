using UnityEngine;



public class BtnCloseSetting : BaseButton
{
    protected override void OnClick()
    {
        UIManager.instance.OnCloseSettingpanel();
    }
}

