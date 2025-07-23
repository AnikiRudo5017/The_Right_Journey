using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public bool GetMasterAudioState()
    {
        return PlayerPrefs.GetInt("MasterAudio", 1) == 1;
    }

    public void SetMasterAudio(bool isOn)
    {
        AudioListener.volume = isOn ? 1f : 0f;
        PlayerPrefs.SetInt("MasterAudio", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    public int GetTargetFPS()
    {
        return PlayerPrefs.GetInt("TargetFPS", 60);
    }

    public void SetTargetFPS(int fps)
    {
        Application.targetFrameRate = fps;
        PlayerPrefs.SetInt("TargetFPS", fps);
        PlayerPrefs.Save();
    }
}
