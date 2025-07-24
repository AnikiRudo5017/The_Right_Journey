using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Menucontroller : MonoBehaviour
{
    GameSaveManager saveManager;

    public TMP_InputField NamePlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Button choitiep; 
    public Button choiMoi; 
    void Start()
    {
        saveManager= FindFirstObjectByType<GameSaveManager>();
       

    }

    public void NewGameButton()
    {
        NeworLoad.newGAME=true;
        saveManager.NEWGAME(NamePlayer.text);

    }
    
    public void LoadGameButton()
    {
        NeworLoad.newGAME = false;
        // sau đấy sẽ gọi hàm GetPlayerData của gamesaveManager để truyền lại dữ liệu vào game 
    }


    
}
