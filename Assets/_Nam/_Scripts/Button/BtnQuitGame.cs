using UnityEngine;

public class BtnQuitGame : BaseButton
{
    protected override void OnClick()
    {
        GameManager.Instance.AudioManager.Play("ButtonClick");
        Application.Quit();
    }
}
