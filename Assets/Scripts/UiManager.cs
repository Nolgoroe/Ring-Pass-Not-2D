using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    public Text SlotsFull;
    public Text YouLose;
    public Text YouWin;
    public Text LevelNum;

    public Text GoldText;
    public Text RubiesText;
    public Text MagicalItemText;

    public Button GoToNextLevelButton;
    public Button StartGameButton;
    public Button Commit;

    public GameObject InGameUI;
    public GameObject CommitCanvas;

    public static UiManager Instance;

    PlayerData ThePlayer;

    void Start()
    {
        Instance = this;
        ThePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }
    
    public void YouLoseMessage()
    {
        YouLose.gameObject.SetActive(true);
    }

    public void YouWinMessage()
    {
        YouWin.gameObject.SetActive(true);
        GoToNextLevelButton.gameObject.SetActive(true);
    }

    public void SlotsFullMessage()
    {
        SlotsFull.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void UpdateLootStats(int Gold, int Ruby, int MagicalItem)
    {
        GoldText.text = "Gold: " + (ThePlayer.Gold + Gold);

        RubiesText.text = "Rubies: " + (ThePlayer.Rubies + Ruby);

        MagicalItemText.text = "Magical Items: " + (ThePlayer.MagicalItems + MagicalItem);

        ThePlayer.Gold += Gold;
        ThePlayer.Rubies += Ruby;
        ThePlayer.MagicalItems += MagicalItem;
    }
}
