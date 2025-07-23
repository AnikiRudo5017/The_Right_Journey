using UnityEngine;
using DG.Tweening;
namespace NamTT
{
    public class BtnOnClickSettings : BaseButton
    {

        protected override void OnClick()
        {
            UIManager.instance.OnOpenSettingPanel();
        }
    }
}