using UnityEngine;

public class Btn_Pause : BaseButton
{
    protected override void OnClick()
    {
        GameManager.Instance.OnClickPause();

    }
}
