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
    public Button CraftingMaterialsButton, ForgeButton;

    public GameObject InGameUI;
    public GameObject LevelHub;
    public GameObject OptionsWindow;
    public GameObject RingersHut;
    public GameObject RingersHutUI;

    public GameObject Wardrobe;

    public GameObject MaterialsBagTab;
    public GameObject ForgeTab;
    public GameObject MaterialsForgeScreen;

    public Image MaterialsForgeBG;
    public Sprite ItemsBG, ForgeBG;
    public AudioSource AudioManager;

    public Slider AudioVolume;

    public static UiManager Instance;

    PlayerData ThePlayer;

    public List<Button> LevelButtons;

    public bool OptionsOpen;

    public List <Button> PowerUpButtons;

    bool IsAnySlotFull;

    void Start()
    {
        Instance = this;
        ThePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();
    }

    private void Update()
    {
        if (GameManager.Instance.SceneBoard != null)
        {
            FullSlotCountText.text = "Full cells: " + GameManager.Instance.FullCellCounter + "/" + GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].CellsInLevel;

            SuccessfullConnectionsCountText.text = "Connections: " + GameManager.Instance.SuccesfullConnectionsMade + "/" + GameManager.Instance.GameLevels[GameManager.Instance.CurrentLevelNum].ConnectionsNeededToFinishLevel;
        }

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

            GameManager.Instance.PowerUpManager.PowerUpInUse = PowerUpChooseItemTypes.None;

            BackToLevelHubButton.gameObject.SetActive(false);

            for (int i = 0; i < PowerUpButtons.Count; i++)
            {
                PowerUpButtons[i].interactable = false;
                PowerUpButtons[i].onClick.RemoveAllListeners();
            }

            for (int i = 0; i < GameManager.Instance.ThePlayer.SlotsForEquipment.Length; i++)
            {
               //GameManager.Instance.ThePlayer.SlotsForEquipment[i].DestructionButtons.Clear();

                if (GameManager.Instance.ThePlayer.SlotsForEquipment[i].Full)
                {
                    if (!GameManager.Instance.ThePlayer.SlotsForEquipment[i].TheItem.HasTimeCooldown)
                    {
                        GameManager.Instance.ThePlayer.SlotsForEquipment[i].TimesLeftToUseInMatch = GameManager.Instance.ThePlayer.SlotsForEquipment[i].TheItem.UsesInMatch;
                        //GameManager.Instance.ThePlayer.SlotsForEquipment[i].Usable = true;
                    }
                }
            }

            EnableLevels();


            RingersHut.SetActive(false);
            RingersHutUI.SetActive(false);
            InGameUI.SetActive(false);
            MainMenuScreen.SetActive(false);
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

    public void UpdatePowerUpOptions()
    {
        ///// WTF IS GOING ON HERE?! TALK TO ALON.
        IsAnySlotFull = false;

        for (int i = 0; i < GameManager.Instance.ThePlayer.SlotsForEquipment.Length; i++)
        {
            if (GameManager.Instance.ThePlayer.SlotsForEquipment[i].Full)
            {
                IsAnySlotFull = true;

                for (int r = 0; r < GameManager.Instance.ThePlayer.SlotsForEquipment[i].TheItem.PowerUpToGive.Count; r++)
                {
                    for (int k = 0; k < PowerUpButtons.Count; k++)
                    {
                        if (!PowerUpButtons[k].interactable)
                        {
                            ///PowerUpButtons[k].GetComponent<Image>().sprite = GameManager.Instance.ThePlayer.SlotsForEquipment[i].TheItem.SpriteOfEquipment; Open When We Have Sprites!!!!!!!!!!!!
                            switch (GameManager.Instance.ThePlayer.SlotsForEquipment[i].TheItem.PowerUpToGive[r])
                            {
                                case PowerUpChooseItemTypes.Joker:
                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.UsingPowerUpToggle((int)PowerUpChooseItemTypes.Joker); });
                                    PowerUpButtons[k].transform.GetChild(0).GetComponent<Text>().text = PowerUpChooseItemTypes.Joker.ToString();
                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.FillPowerUpButton(PowerUpButtons[k]); });
                                    PowerUpButtons[k].interactable = true;
                                    //GameManager.Instance.ThePlayer.SlotsForEquipment[i].DestructionButtons.Add(PowerUpButtons[k]);
                                    break;
                                case PowerUpChooseItemTypes.Switch:
                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.UsingPowerUpToggle((int)PowerUpChooseItemTypes.Switch); });
                                    PowerUpButtons[k].transform.GetChild(0).GetComponent<Text>().text = PowerUpChooseItemTypes.Switch.ToString();
                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.FillPowerUpButton(PowerUpButtons[k]); });
                                    PowerUpButtons[k].interactable = true;
                                    //GameManager.Instance.ThePlayer.SlotsForEquipment[i].DestructionButtons.Add(PowerUpButtons[k]);
                                    break;
                                case PowerUpChooseItemTypes.TileBomb:
                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.UsingPowerUpToggle((int)PowerUpChooseItemTypes.TileBomb); });
                                    PowerUpButtons[k].transform.GetChild(0).GetComponent<Text>().text = PowerUpChooseItemTypes.TileBomb.ToString();
                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.FillPowerUpButton(PowerUpButtons[k]); });
                                    PowerUpButtons[k].interactable = true;
                                    //GameManager.Instance.ThePlayer.SlotsForEquipment[i].DestructionButtons.Add(PowerUpButtons[k]);
                                    break;
                                case PowerUpChooseItemTypes.SliceBomb:
                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.UsingPowerUpToggle((int)PowerUpChooseItemTypes.SliceBomb); });
                                    PowerUpButtons[k].transform.GetChild(0).GetComponent<Text>().text = PowerUpChooseItemTypes.SliceBomb.ToString();
                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.FillPowerUpButton(PowerUpButtons[k]); });
                                    PowerUpButtons[k].interactable = true;
                                    //GameManager.Instance.ThePlayer.SlotsForEquipment[i].DestructionButtons.Add(PowerUpButtons[k]);
                                    break;
                                case PowerUpChooseItemTypes.ColorTransform:
                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.UsingPowerUpToggle((int)PowerUpChooseItemTypes.ColorTransform); });

                                    int Col = FindColorOrSymbolNumForPowerUp(Symbols.None, GameManager.Instance.ThePlayer.SlotsForEquipment[i].TheItem.ColorForPowerUp);

                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.ChooseColorForColorTransformPowerUp(Col); });

                                    PowerUpButtons[k].transform.GetChild(0).GetComponent<Text>().text = PowerUpChooseItemTypes.ColorTransform.ToString() + " " + GameManager.Instance.ThePlayer.SlotsForEquipment[i].TheItem.ColorForPowerUp.ToString();

                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.FillPowerUpButton(PowerUpButtons[k]); });
                                    PowerUpButtons[k].interactable = true;
                                    //GameManager.Instance.ThePlayer.SlotsForEquipment[i].DestructionButtons.Add(PowerUpButtons[k]);
                                    break;
                                case PowerUpChooseItemTypes.ShapeTransform:
                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.UsingPowerUpToggle((int)PowerUpChooseItemTypes.ShapeTransform); });

                                    int Sym = FindColorOrSymbolNumForPowerUp(GameManager.Instance.ThePlayer.SlotsForEquipment[i].TheItem.SymbolForPowerUp, ColorData.None);

                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.ChooseSymbolForShapeTransformPowerUp(Sym); });

                                    PowerUpButtons[k].transform.GetChild(0).GetComponent<Text>().text = PowerUpChooseItemTypes.ShapeTransform.ToString() + " " + GameManager.Instance.ThePlayer.SlotsForEquipment[i].TheItem.SymbolForPowerUp.ToString();

                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.FillPowerUpButton(PowerUpButtons[k]); });
                                    PowerUpButtons[k].interactable = true;
                                    //GameManager.Instance.ThePlayer.SlotsForEquipment[i].DestructionButtons.Add(PowerUpButtons[k]);
                                    break;
                                case PowerUpChooseItemTypes.Reshuffle:
                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.FillPowerUpButton(PowerUpButtons[k]); });

                                    PowerUpButtons[k].onClick.AddListener(delegate { GameManager.Instance.PowerUpManager.UsingPowerUpToggle((int)PowerUpChooseItemTypes.Reshuffle); });

                                    PowerUpButtons[k].onClick.AddListener(GameManager.Instance.PowerUpManager.RefillClip);

                                    PowerUpButtons[k].transform.GetChild(0).GetComponent<Text>().text = PowerUpChooseItemTypes.Reshuffle.ToString();
                                    PowerUpButtons[k].interactable = true;
                                    //GameManager.Instance.ThePlayer.SlotsForEquipment[i].DestructionButtons.Add(PowerUpButtons[k]);

                                    break;
                                default:
                                    Debug.LogError("WHAT THE FUCK HAPPEND HERE?!");
                                    break;
                            }

                            CheckIfItemInCooldown(PowerUpButtons[k], GameManager.Instance.ThePlayer.SlotsForEquipment[i]);
                            break; //// Exit the for loop for the Buttons or the names of all buttons will be the same
                        }
                    }
                }
            }
        }

        if (IsAnySlotFull == false)
        {
            for (int k = 0; k < PowerUpButtons.Count; k++)
            {
                PowerUpButtons[k].interactable = false;
                PowerUpButtons[k].transform.GetChild(0).GetComponent<Text>().text = "Power Up";
                PowerUpButtons[k].onClick.RemoveAllListeners();
            }
        }
    }

    public void CheckIfItemInCooldown(Button ToDeactivate, EquipmentSlot TheSlot)
    {
        if (PlayerPrefs.GetInt("ItemsWithCooldownCount") > 0)
        {
            int count = PlayerPrefs.GetInt("ItemsWithCooldownCount");

            for (int i = 0; i < count; i++)
            {
                if (TheSlot.TheItem.ID == GameManager.Instance.ThePlayer.EquipmentWithTimeCooldown[i].ID)
                {
                    ToDeactivate.interactable = false;
                    ToDeactivate.transform.GetChild(0).GetComponent<Text>().text = "Cooldown";
                    ToDeactivate.onClick.RemoveAllListeners();
                    PowerUpButtons.Remove(ToDeactivate);
                }
            }
        }
    }

    public int FindColorOrSymbolNumForPowerUp(Symbols TheSymbol, ColorData TheColor)
    {
        if (TheSymbol != Symbols.None)
        {
            switch (TheSymbol)
            {
                case Symbols.Circle:
                    return 0;
                case Symbols.Plus:
                    return 1;
                case Symbols.Triangle:
                    return 2;
                case Symbols.Square:
                    return 3;
                case Symbols.Joker:
                    return 4;
                default:
                    return -1;
            }
        }

        if(TheColor != ColorData.None)
        {
            switch (TheColor)
            {
                case ColorData.Red:
                    return 0;
                case ColorData.Pink:
                    return 1;
                case ColorData.Blue:
                    return 2;
                case ColorData.Yellow:
                    return 3;
                case ColorData.Joker:
                    return 4;
                default:
                    return -1;
            }
        }

        return -2;
    }

    public void BackToMainMenu()
    {
        MainMenuScreen.SetActive(true);


        RingersHut.SetActive(false);
        RingersHutUI.SetActive(false);
        InGameUI.SetActive(false);
        LevelHub.SetActive(false);
        ToggleOptions(false);
    }

    public void ToRingersHut()
    {
        RingersHut.SetActive(true);
        RingersHutUI.SetActive(true);


        MainMenuScreen.SetActive(false);
        InGameUI.SetActive(false);
        LevelHub.SetActive(false);
        ToggleOptions(false);
    }

    public void ToggleWardrobe(bool On)
    {
        if (On)
        {
            Wardrobe.SetActive(true);
            WardrobeManager.Instance.OpenWardrobe();
        }
        else
        {
            Wardrobe.SetActive(false);
        }
    }

    public void MaterialsForgeTabSwitch(bool Materials)
    {
        if (Materials)
        {
            MaterialsBagTab.SetActive(true);
            ForgeTab.SetActive(false);
            MaterialsForgeBG.sprite = ItemsBG;
            //ForgeButton.GetComponent<Image>().color = Color.grey;
            //CraftingMaterialsButton.GetComponent<Image>().color = Color.white;
            GameManager.Instance.MaterialBagManagerScript.RefreshMaterialBag();
        }
        else
        {
            MaterialsBagTab.SetActive(false);
            ForgeTab.SetActive(true);
            MaterialsForgeBG.sprite = ForgeBG;
            //ForgeButton.GetComponent<Image>().color = Color.white;
            //CraftingMaterialsButton.GetComponent<Image>().color = Color.grey;
        }
    }



    public void OpenCraftingMatForgeBag(bool open)
    {
        if (open)
        {
            MaterialsForgeScreen.SetActive(true);
            MaterialsForgeTabSwitch(true);
            GameManager.Instance.ForgeManagerScript.CheckIfCanCraftItem();
            GameManager.Instance.MaterialBagManagerScript.RefreshMaterialBag();

        }
        else
        {
            MaterialsForgeScreen.SetActive(false);
        }
    }
}
