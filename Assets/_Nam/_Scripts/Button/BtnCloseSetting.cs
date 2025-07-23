using UnityEngine;


namespace NamTT
{
    public class BtnCloseSetting : BaseButton
    {
        protected override void OnClick()
        {
            UIManager.instance.OnCloseSettingpanel();
        }
    }
}
