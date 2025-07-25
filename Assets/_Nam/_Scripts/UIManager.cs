using UnityEngine;
using DG.Tweening;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using System.Security.Cryptography;


public class UIManager : MonoBehaviour
{
    public enum UIStat { Ready, Opening }

    [SerializeField] private GameObject _settingPanel;
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private GameObject _pausePanel;
   

    [SerializeField] private TextMeshProUGUI _textLoading;
    private UIStat _stat;
    private void Start()
    {
        Initialization();
    }

    private void Initialization()
    {
        _settingPanel.SetActive(false);
        _loadingPanel.SetActive(false);
        _pausePanel.SetActive(false);
      
        if (_stat == UIStat.Ready)
        {
            _stat = UIStat.Opening;
            _mainMenuPanel.SetActive(true);
            _mainMenuPanel.transform.localScale = Vector3.zero;
            _mainMenuPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f)
          
          .OnComplete(() =>
          {
              _stat = UIStat.Ready;
          });
        }
    }
    public void OnOpenSettingPanel()
    {
        if (_stat == UIStat.Ready)
        {
            _stat = UIStat.Opening;
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
    public void OnOpenPausePanel()
    {
        if (_stat == UIStat.Ready)
        {
            _stat = UIStat.Opening;
         
            _pausePanel.SetActive(!_pausePanel.activeSelf);
            _pausePanel.transform.localScale = Vector3.zero;
            _pausePanel.transform.DOScale(new Vector3(0.8f, 0.8f, 0.8f), 0.5f)
          .SetEase(Ease.InBack)
          .OnComplete(() =>
          {
              _stat = UIStat.Ready;
          });
        }
    }
    public void OnClosePausePanel()
    {
        if (_stat == UIStat.Ready)
        {
            _stat = UIStat.Opening;
            _pausePanel.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f)
          .SetEase(Ease.InBack)
          .OnComplete(() =>
          {
              _stat = UIStat.Ready;
              _pausePanel.SetActive(!_pausePanel.activeSelf);
        
              GameManager.Instance.gameStats = GameStats.Playing;
          });
        }
    }
    public void BackHome()
    {
        if(_stat == UIStat.Ready)
        {
            _stat = UIStat.Opening;
            _pausePanel.SetActive(!_pausePanel.activeSelf);
            StartCoroutine(LoadSceneEventAsync("MainMenu"));
            _mainMenuPanel.SetActive(!_mainMenuPanel.activeSelf);
            _mainMenuPanel.transform.DOScale(new Vector3(1f, 1f, 1f), 0.5f)
             .SetEase(Ease.InBack)
             .OnComplete(() =>
             {
                 _stat = UIStat.Ready;
                 GameManager.Instance.gameStats = GameStats.Start;
                 GameManager.Instance.AudioManager.Play("Theme");
             });
        }
    }

    #region HidePanel==========================================================
    public void OnCloseSettingpanel()
    {
        if (_stat == UIStat.Ready)
        {
            _stat = _stat = UIStat.Opening;
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
    #endregion

    public void LoadSceneAsyncByName(string name)
    {
        if (_stat == UIStat.Ready)
        {
            _stat = UIStat.Opening;
            _mainMenuPanel.transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.5f)
          .SetEase(Ease.InBack)
          .OnComplete(() =>
          {
              _mainMenuPanel.SetActive(!_mainMenuPanel.activeSelf);
              _stat = UIStat.Ready;
              StartCoroutine(LoadSceneEventAsync(name));
              GameManager.Instance.AudioManager.Play("InGame");
              GameManager.Instance.gameStats = GameStats.Playing;
            
          });
        }
    }
    private IEnumerator LoadSceneEventAsync(string name)
    {
        _loadingPanel.SetActive(!_loadingPanel.activeSelf);
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(name);
        while (!asyncOperation.isDone)
        {
            float progessValue = Mathf.Clamp01(asyncOperation.progress / 0.9f);
            _textLoading.text = $"{progessValue * 100} % ";

            yield return null;
        }
        _loadingPanel.SetActive(!_loadingPanel.activeSelf);
        
    }

}




