using UnityEngine;
using UnityEngine.UI;

public class AudioSetting : MonoBehaviour
{
    [SerializeField] private Toggle toggleOn;
    [SerializeField] private Toggle toggleOff;

    private void Start()
    {
        bool audioState = GameManager.Instance.GameSettings.GetMasterAudioState();
        toggleOn.isOn = audioState;
        toggleOff.isOn = !audioState;

        toggleOn.onValueChanged.AddListener(OnToggleChanged);
        toggleOff.onValueChanged.AddListener(OnToggleChanged);

        OnToggleChanged(toggleOn.isOn);
    }

    private void OnToggleChanged(bool isOn)
    {
        GameManager.Instance.GameSettings.SetMasterAudio(toggleOn.isOn);
    }
}
