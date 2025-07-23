using UnityEngine;
using UnityEngine.UI;

namespace NamTT {
    public class FpsCotroller : MonoBehaviour
    {
        private ToggleGroup ToggleGroup;
        [SerializeField] Toggle toggle45fps;
        [SerializeField] Toggle toggle60fps;

        private void Awake()
        {
            ToggleGroup = GetComponent<ToggleGroup>();
        }
        private void Start()
        {
            SetFps(toggle45fps.isOn ? 45 : 60);

            toggle45fps.onValueChanged.AddListener((isOn) => OnToggleChanged(toggle45fps, isOn, 45));
            toggle60fps.onValueChanged.AddListener((isOn) => OnToggleChanged(toggle45fps, isOn, 60));
        }
        private void Update()
        {

        }
        void SetFps(int fps)
        {
            Application.targetFrameRate = fps;
        }

        void OnToggleChanged(bool toggle, bool isOn, int fps)
        {
            if (isOn)
            {
                SetFps((int)fps);
            }
        }

    }
}
