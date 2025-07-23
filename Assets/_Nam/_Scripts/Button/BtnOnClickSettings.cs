using UnityEngine;
using DG.Tweening;

public class BtnOnClickSettings : BaseButton
{

    protected override void OnClick()
    {
        UIManager.instance.OnOpenSettingPanel();
    }
}
