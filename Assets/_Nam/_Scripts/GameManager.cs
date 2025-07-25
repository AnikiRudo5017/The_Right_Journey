using UnityEngine;
public enum GameStats { Start, Playing, Pause, Win, Lose, Loading }
public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    [SerializeField] public GameStats gameStats;

    [Header("Variable")]
    [SerializeField] private UIManager uiManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameSettings gameSettings;

    // Convert from pritave Variable to public variable, Only Get
    public static GameManager Instance => instance;
    public UIManager UIManager => uiManager;
    public AudioManager AudioManager => audioManager;
    public GameSettings GameSettings => gameSettings;
    private void Awake()
    {
        LoadComponent();
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {

            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        gameStats = GameStats.Start;
    }

    private void Update()
    {
        if(gameStats == GameStats.Playing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
               
            }
        }
    }
    private void FixedUpdate()
    {
        if (gameStats != GameStats.Pause || gameStats != GameStats.Lose || gameStats != GameStats.Win)
        {
            Time.timeScale = 1f;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }
    void LoadComponent()
    {
        if (uiManager != null || audioManager != null || gameSettings != null) return;
        uiManager = GetComponentInChildren<UIManager>();
        audioManager = GetComponentInChildren<AudioManager>();
        gameSettings = GetComponentInChildren<GameSettings>();
        if(gameSettings == null)
        {
            Debug.LogError($"game setting null");
        }
    }

    public void LoadSceneSetStat(string name)
    {
        gameStats = GameStats.Loading;
        uiManager.LoadSceneAsyncByName(name);
    }
    public void OnClickPause()
    {
        audioManager.Play("ButtonClick");
        gameStats = GameStats.Pause;
        uiManager.OnOpenPausePanel();
    }


}
