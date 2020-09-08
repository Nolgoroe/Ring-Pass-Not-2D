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

    public Button GoToNextLevelButton;
    public Button StartGameButton;
    public Button Commit;

    public GameObject InGameUI;
    public GameObject CommitCanvas;

    public static UiManager Instance;
    void Start()
    {
        Instance = this;
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
}
