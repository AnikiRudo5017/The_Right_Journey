using UnityEngine;


public class Btn_Resume : BaseButton
{
    protected override void OnClick()
    {
        GameManager.Instance.AudioManager.Play("ButtonClick");
        GameManager.Instance.UIManager.OnClosePausePanel();
    }
}
