using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

namespace NamTT {
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance { get; private set; }
        public enum UIStat { Ready, Open }

        [SerializeField] private GameObject _settingPanel;
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _loadingPanel;

        [SerializeField] private TextMeshProUGUI _textLoading;
        private UIStat _stat;

        private void Awake()
        {
            instance = this;
        }
        private void Start()
        {
            Initialization();
        }

        private void Initialization()
        {
            _settingPanel.SetActive(false);
            _mainMenuPanel.SetActive(true);
            _loadingPanel.SetActive(false);
        }
        public void OnOpenSettingPanel()
        {
            if (_stat == UIStat.Ready)
            {
                _stat = UIStat.Open;
                _mainMenuPanel.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f)
              .SetEase(Ease.InBack)
              .OnComplete(() =>
              {
                  _mainMenuPanel.SetActive(!_mainMenuPanel.activeSelf);
                  _settingPanel.SetActive(!_settingPanel.activeSelf);
                  _settingPanel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                  _settingPanel.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f);
                  _stat = UIStat.Ready;
              });
            }

            //.SetEase(Ease.InBounce);  
        }
        public void OnCloseSettingpanel()
        {
            if (_stat == UIStat.Ready)
            {
                _stat = _stat = UIStat.Open;
                _settingPanel.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    _settingPanel.SetActive(!_settingPanel.activeSelf);
                    _mainMenuPanel.SetActive(!_mainMenuPanel.activeSelf);
                    _mainMenuPanel.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                    _mainMenuPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
                    _stat = _stat = UIStat.Ready;
                })
                ;
            }
        }

        public void LoadSceneAsyncByName(string name)
        {
            if (_stat == UIStat.Ready)
            {
                _stat = UIStat.Open;
                _mainMenuPanel.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.2f)
              .SetEase(Ease.InBack)
              .OnComplete(() =>
              {

                  _mainMenuPanel.SetActive(!_mainMenuPanel.activeSelf);
                  _stat = UIStat.Ready;
                  StartCoroutine(LoadSceneEventAsync(name));
              });
            }
        }
        private IEnumerator LoadSceneEventAsync(string name)
        {
            _loadingPanel.SetActive(true);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
            while (!asyncOperation.isDone)
            {
                float progessValue = Mathf.Clamp01(asyncOperation.progress / 0.9f);
                _textLoading.text = $"{progessValue * 100} % ";
                yield return null;
            }
            // _loadingPanel.SetActive(false);
        }
    }

}


