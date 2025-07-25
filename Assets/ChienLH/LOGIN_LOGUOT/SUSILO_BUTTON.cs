using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SiSuLoButton : MonoBehaviour
{
    public GameObject dk_UI;
    public GameObject dn_UI;
    public GameObject quenMK;

    public SuSiLoFIREBASE siSuLoManager;
    public TMP_InputField dk_gmail;
    public TMP_InputField dk_password;



    public TMP_InputField dn_gmail;
    public TMP_InputField dn_password;

    public TMP_InputField emailToReset;



    void Start()
    {
        siSuLoManager = GetComponent<SuSiLoFIREBASE>();
        Initialize();

        // dk_button.onClick.AddListener(()=>siSuLoManager.DKTK(dk_gmail.text,dk_password.text));

    }

   private void Initialize()
    {
        dk_UI.gameObject.SetActive(false);
        quenMK.gameObject.SetActive(false);
        dn_UI.gameObject.SetActive(true);
    }

    public void dk_button()
    {
        siSuLoManager.DKTK(dk_gmail.text, dk_password.text);
    }


    public void dn_button()
    {
        siSuLoManager.DNTK(dn_gmail.text, dn_password.text);
    }
    public void OnClickBackButton()
    {
        dk_UI.SetActive(false);
        dn_UI.SetActive(true);
        quenMK.SetActive(false);
    }
    public void DangKyNgay()
    {
        dn_UI.SetActive(false);
        dk_UI.SetActive(true);
    }
    public void QUENMK()
    {
        dn_UI.SetActive(false);
        dk_UI.SetActive(false);
        quenMK.SetActive(true);

    }


    public void GUIYEUCAURESET()
    {

        siSuLoManager.OnResetPasswordButtonClicked(emailToReset.text);
    }
}
