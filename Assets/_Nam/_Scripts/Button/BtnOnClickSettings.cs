using UnityEngine;
using DG.Tweening;

public class BtnOnClickSettings : BaseButton
{

    protected override void OnClick()
    {
        GameManager.Instance.AudioManager.Play("ButtonClick");
        GameManager.Instance.UIManager.OnOpenSettingPanel();
        
    }
}
