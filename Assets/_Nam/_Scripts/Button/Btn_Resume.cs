using UnityEngine;


public class Btn_Resume : BaseButton
{
    protected override void OnClick()
    {
       GameManager.Instance.UIManager.OnClosePausePanel();
    }
}
