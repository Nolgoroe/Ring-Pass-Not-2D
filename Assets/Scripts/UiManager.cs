﻿using System.Collections;
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

    public Text FullSlotCountText;
    public Text SuccessfullConnectionsCountText;

    public Button GoToNextLevelButton;
    public GameObject MainMenuScreen;
    public Button Commit;
    public Button BackToLevelHubButton;

    public GameObject InGameUI;
    public GameObject LevelHub;
    public GameObject OptionsWindow;

    public AudioSource AudioManager;

    public Slider AudioVolume;

    public static UiManager Instance;

    PlayerData ThePlayer;

    public List<Button> LevelButtons;

    public bool OptionsOpen;
    void Start()
    {
        Instance = this;
        ThePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }

    private void Update()
    {
        FullSlotCountText.text = "Full cells: " +  GameManager.Instance.FullCellCounter + "/" + GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].CellsInLevel;

        SuccessfullConnectionsCountText.text = "Connections: " + GameManager.Instance.SuccesfullConnectionsMade + "/" + GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].ConnectionsNeededToFinishLevel;

    }
    public void YouLoseMessage()
    {
        YouLose.gameObject.SetActive(true);
    }

    public void YouWinMessage()
    {
        YouWin.gameObject.SetActive(true);
        BackToLevelHubButton.gameObject.SetActive(true);
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

    public void ToggleLevelHub(bool OnOff)
    {
        if (OnOff)
        {
            LevelHub.gameObject.SetActive(true);

            if (OptionsWindow.activeInHierarchy)
            {
                ToggleOptions(false);
            }

            GameManager.Instance.OrganizeForNextLevel();

            GameManager.Instance.PowerUpManager.LoseGame = false;

            BackToLevelHubButton.gameObject.SetActive(false);

            EnableLevels();
        }
        else
        {
            LevelHub.gameObject.SetActive(false);
        }
    }

    public void ToggleOptions(bool Toggle)
    {
        if (Toggle)
        {
            OptionsWindow.SetActive(true);
            OptionsOpen = true;
        }
        else
        {
            OptionsWindow.SetActive(false);
            OptionsOpen = false;
        }
    }

    public void ChangeVolume()
    {
        AudioManager.volume = AudioVolume.value;
    }

    public void EnableLevels()
    {
        if(GameManager.Instance.ThePlayer.MaxLevelReached == 0)
        {
            LevelButtons[0].interactable = true;
        }
        else
        {
            for (int i = 0; i < GameManager.Instance.ThePlayer.MaxLevelReached; i++)
            {
                LevelButtons[i].interactable = true;
            }
        }
    }
}
