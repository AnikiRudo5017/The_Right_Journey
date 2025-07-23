using UnityEngine;
using UnityEngine.SceneManagement;

namespace NamTT {
    public class BtnLoadScene : BaseButton
    {
        [SerializeField] private string sceneName;
        [SerializeField] private int sceneIndex;
        [SerializeField] private bool useSceneIndex = false;

        protected override void OnClick()
        {
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
            UIManager.instance.LoadSceneAsyncByName(sceneName);
        }
        private void LoadSceneByIndex(int sceneIndex)
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }
}