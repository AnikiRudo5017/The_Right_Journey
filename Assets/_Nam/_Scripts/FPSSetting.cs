using UnityEngine;
using UnityEngine.UI;

namespace NamTT {
    public class FPSSetting : MonoBehaviour
    {
        [SerializeField] Toggle toggle45fps;
        [SerializeField] Toggle toggle60fps;

        private void Awake()
        {
            Application.targetFrameRate = -1;
        }
        private void Start()
        {
            int saveFPS = PlayerPrefs.GetInt("TarGetFPS", 60);

            toggle45fps.isOn = saveFPS == 45;
            toggle60fps.isOn = saveFPS == 60;

            GameManager.Instance.GameSettings.SetTargetFPS(saveFPS);

            toggle45fps.onValueChanged.AddListener((isOn) => OnToggleChanged(toggle45fps, isOn, 45));
            toggle60fps.onValueChanged.AddListener((isOn) => OnToggleChanged(toggle45fps, isOn, 60));
        }

        void OnToggleChanged(bool toggle, bool isOn, int fps)
        {
            if (isOn)
            {
               GameManager.Instance.GameSettings.SetTargetFPS(fps);
            }
        }


       
    }
}
