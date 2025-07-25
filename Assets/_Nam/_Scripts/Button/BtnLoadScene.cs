using UnityEngine;
using UnityEngine.SceneManagement;


public class BtnLoadScene : BaseButton
{
    [SerializeField] private string sceneName;
    [SerializeField] private int sceneIndex;
    [SerializeField] private bool useSceneIndex = false;
    public GameSaveManager saveManager;


    protected override void Start()
    {
        saveManager= FindFirstObjectByType<GameSaveManager>();
    }
    protected override void OnClick()
    {
        GameManager.Instance.AudioManager.Play("ButtonClick");
        GameManager.Instance.AudioManager.Stop("Theme");
        NeworLoad.newGAME = true;
        saveManager.NEWGAME("TRADV");

        if (useSceneIndex)
        {
            if (sceneIndex < 0)
            {
                Debug.LogError("Invalid scene index: " + sceneIndex);
                return;
            }
            LoadSceneByIndex(sceneIndex);
        }
        else
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Scene name is null or empty.");
                return;
            }
            LoadSceneName(sceneName);
        }
    }
    private void LoadSceneName(string sceneName)
    {
        GameManager.Instance.LoadSceneSetStat(sceneName);
    }
    private void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
