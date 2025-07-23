using UnityEngine;

public class BtnQuitGame : BaseButton
{
    protected override void OnClick()
    {
       Application.Quit();
    }
}
