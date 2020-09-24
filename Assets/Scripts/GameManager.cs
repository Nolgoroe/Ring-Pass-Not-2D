using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int CurrentLevelNum = 1;
    public int ConnectionsNeededToFinishLevel;

    public Transform PieceTargetLookAt;
    public ParticleSystem ConnectionVFX;
    public GameObject MovableDaddyPrefab;
    public GameObject ClipPrefab;
    public GameObject BoardPrefab;

    public PowerUps PowerUpManager;
    public Transform SceneClip;
    public Transform SceneBoard;

    public Sprite[] LeftShapesSprites;
    public Sprite[] RightShapesSprites;

    public GameObject[] LeftShapesGameobjects;
    public GameObject[] RightShapesGameobjects;


    public List<GameObject> LeftSideClipsPieces;
    public List<GameObject> RightSideClipsPieces;

    //public List<GameObject> LimiterPrefabs;
    public List<LimiterCellManager> LimiterCells;

    public List<ConnectorManager> Connectors;
    public List<CellInfo> PieceCells;

    public List<LevelManager> GameLevels;

    [HideInInspector]
    public int PieceNum;

    public int FullCellCounter = 0;
    public int SuccesfullConnectionsMade = 0;
    public int CellsNeeedToFinish = 0;

    public GameObject DeleteOnLevelTransfer;

    public List<LootType> LevelSpecificLoot;

    
    public PlayerData ThePlayer;
    GameObject go;
    int CheckAllOptionalLimiterZonesFull = 0;

    int LevelNumOfLimiters;

    public Equipment[] GameItems;
    void Start()
    {
        Instance = this;
        ThePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerData>();

        if (PlayerPrefs.HasKey("MaxLevelReached"))
        {
            CurrentLevelNum = PlayerPrefs.GetInt("MaxLevelReached") - 1; ///// -1 Because counting levels start from 1 while loading levels starts from 0 from an array
        }

        UiManager.Instance.ToggleLevelHub(false);
    }

    //public void StartGame()
    //{
    //    StartLevel();
    //}

    public void StartLevel(int LevelNum)
    {
        CurrentLevelNum = LevelNum;

        if (GameLevels.Count == 0)
        {
            Debug.LogError("NO LEVELS LOADED! Please load levels.");
            return;
        }

        GameObject C = Instantiate(ClipPrefab, DeleteOnLevelTransfer.transform);
        C.tag = "Clip";

        BoardPrefab = Instantiate(GameLevels[CurrentLevelNum].BoardPrefab, DeleteOnLevelTransfer.transform);
        BoardPrefab.tag = "Board";

        PieceTargetLookAt = GameObject.FindGameObjectWithTag("Center").transform;

        FillClip();

        LevelNumOfLimiters = GameLevels[CurrentLevelNum].CacluateNumOfLimiters();

        if(LevelNumOfLimiters <= 0)
        {
            Debug.LogError("No Limiters!!!");
            return;
        }

        FillLimiters();

        PowerUpManager.Init();
        
        foreach (GameObject Connector in GameObject.FindGameObjectsWithTag("Connector"))
        {
            Connectors.Add(Connector.GetComponent<ConnectorManager>());
        }
        foreach (GameObject Cell in GameObject.FindGameObjectsWithTag("PieceCell"))
        {
            PieceCells.Add(Cell.GetComponent<CellInfo>());
        }

        ConnectionsNeededToFinishLevel = GameLevels[CurrentLevelNum].ConnectionsNeededToFinishLevel;
        CellsNeeedToFinish = GameLevels[CurrentLevelNum].CellsInLevel;
        UiManager.Instance.Commit = GameObject.FindGameObjectWithTag("Commit").GetComponent<Button>();
        UiManager.Instance.Commit.onClick.AddListener(CheckEndGame);
        UiManager.Instance.Commit.interactable = false;
        UiManager.Instance.InGameUI.SetActive(true);
        UiManager.Instance.MainMenuScreen.SetActive(false);
        UiManager.Instance.LevelNum.text = "Level: " + GameLevels[CurrentLevelNum].LevelNum.ToString();
        UiManager.Instance.LevelNum.gameObject.SetActive(true);

        UiManager.Instance.GoldText.text = "Gold: " + ThePlayer.Gold;
        UiManager.Instance.RubiesText.text = "Rubies: " + ThePlayer.Rubies;
        UiManager.Instance.MagicalItemText.text = "Magical Items: " + ThePlayer.MagicalItems;
        LevelSpecificLoot.Clear();
        LevelSpecificLoot.AddRange(GameLevels[CurrentLevelNum].LootForLevel);
        UiManager.Instance.ToggleLevelHub(false);

        UiManager.Instance.PowerUpButtons.Clear();

        foreach (GameObject powerUpButton in GameObject.FindGameObjectsWithTag("PowerUpButtons"))
        {
            UiManager.Instance.PowerUpButtons.Add(powerUpButton.GetComponent<Button>());
        }

        UiManager.Instance.UpdatePowerUpOptions();
    }

    public void FillClip()
    {
        SceneClip = GameObject.FindGameObjectWithTag("Clip").transform;
        LeftSideClipsPieces = new List<GameObject>();
        RightSideClipsPieces = new List<GameObject>();

        foreach (GameObject Piece in GameObject.FindGameObjectsWithTag("RightPiece"))
        {
            RightSideClipsPieces.Add(Piece);
        }

        foreach (GameObject Piece in GameObject.FindGameObjectsWithTag("LeftPiece"))
        {
            LeftSideClipsPieces.Add(Piece);
        }

        for (int i = 0; i < LeftSideClipsPieces.Count; i++)
        {
            if (GameLevels[CurrentLevelNum].DoubleRing)
            {
                PieceNum = Random.Range(0, GameLevels[CurrentLevelNum].LeftShapesGameobjects.Length);
                GameObject go = Instantiate(GameLevels[CurrentLevelNum].LeftShapesGameobjects[PieceNum], LeftSideClipsPieces[i].transform);
                go.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
            }
            else
            {
                PieceNum = Random.Range(0, LeftShapesGameobjects.Length);
                GameObject go = Instantiate(LeftShapesGameobjects[PieceNum], LeftSideClipsPieces[i].transform);
                go.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
            }

        }

        for (int i = 0; i < RightSideClipsPieces.Count; i++)
        {
            if (GameLevels[CurrentLevelNum].DoubleRing)
            {
                PieceNum = Random.Range(0, GameLevels[CurrentLevelNum].RightShapesGameobjects.Length);
                GameObject go = Instantiate(GameLevels[CurrentLevelNum].RightShapesGameobjects[PieceNum], RightSideClipsPieces[i].transform);
                go.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
            }
            else
            {
                PieceNum = Random.Range(0, RightShapesGameobjects.Length);
                GameObject go = Instantiate(RightShapesGameobjects[PieceNum], RightSideClipsPieces[i].transform);
                go.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
            }
        }
    }

    public void FillLimiters()
    {
        SceneBoard = GameObject.FindGameObjectWithTag("Board").transform;

        foreach (GameObject Limiter in GameObject.FindGameObjectsWithTag("Limiter"))
        {
            LimiterCells.Add(Limiter.GetComponent<LimiterCellManager>());
        }

        int RandomCell = Random.Range(0, LimiterCells.Count);

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfGeneralColors;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            SummonLimiterConditions(GameLevels[CurrentLevelNum].GeneralColorLimiter, RandomCell, 1, LimiterCells[RandomCell].IsFull);
            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfGeneralSymbol;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            SummonLimiterConditions(GameLevels[CurrentLevelNum].GeneralSymbolLimiter, RandomCell, 1, LimiterCells[RandomCell].IsFull);
            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfSpecificColors;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomSpecific = Random.Range(0, GameLevels[CurrentLevelNum].SpecificColorLimitersPrefabs.Length);

            SummonLimiterConditions(GameLevels[CurrentLevelNum].SpecificColorLimitersPrefabs[RandomSpecific], RandomCell, 1, LimiterCells[RandomCell].IsFull);
            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfSpecificSymbols;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomSpecific = Random.Range(0, GameLevels[CurrentLevelNum].SpecificSymbolLimitersPrefabs.Length);

            SummonLimiterConditions(GameLevels[CurrentLevelNum].SpecificSymbolLimitersPrefabs[RandomSpecific], RandomCell, 1, LimiterCells[RandomCell].IsFull);
            i++;
        }





        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootGeneralColors;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            SummonLimiterConditions(GameLevels[CurrentLevelNum].GeneralLootColorLimiter, RandomCell, 2, LimiterCells[RandomCell].IsFull);

            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootGeneralSymbol;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }


            SummonLimiterConditions(GameLevels[CurrentLevelNum].GeneralLootSymbolLimiter, RandomCell, 2, LimiterCells[RandomCell].IsFull);
            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootSpecificColors;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomSpecific = Random.Range(0, GameLevels[CurrentLevelNum].SpecificLootColorLimitersPrefabs.Length);

            SummonLimiterConditions(GameLevels[CurrentLevelNum].SpecificLootColorLimitersPrefabs[RandomSpecific], RandomCell, 2, LimiterCells[RandomCell].IsFull);
            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootSpecificSymbols;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomSpecific = Random.Range(0, GameLevels[CurrentLevelNum].SpecificLootSymbolLimitersPrefabs.Length);

            SummonLimiterConditions(GameLevels[CurrentLevelNum].SpecificLootSymbolLimitersPrefabs[RandomSpecific], RandomCell, 2, LimiterCells[RandomCell].IsFull);
            i++;
        }





        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootLockGeneralColors;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }


            SummonLimiterConditions(GameLevels[CurrentLevelNum].GeneralLootLockColorLimiter, RandomCell, 3, LimiterCells[RandomCell].IsFull);
            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootLockGeneralSymbol;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }


            SummonLimiterConditions(GameLevels[CurrentLevelNum].GeneralLootLockSymbolLimiter, RandomCell, 3, LimiterCells[RandomCell].IsFull);

            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootLockSpecificColors;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomSpecific = Random.Range(0, GameLevels[CurrentLevelNum].SpecificLootLockColorLimitersPrefabs.Length);

            SummonLimiterConditions(GameLevels[CurrentLevelNum].SpecificLootLockColorLimitersPrefabs[RandomSpecific], RandomCell, 3, LimiterCells[RandomCell].IsFull);

            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootLockSpecificSymbols;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomSpecific = Random.Range(0, GameLevels[CurrentLevelNum].SpecificLootLockSymbolLimitersPrefabs.Length);

            SummonLimiterConditions(GameLevels[CurrentLevelNum].SpecificLootLockSymbolLimitersPrefabs[RandomSpecific], RandomCell, 3, LimiterCells[RandomCell].IsFull);
            i++;
        }




        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootLimiterGeneralColors;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }


            SummonLimiterConditions(GameLevels[CurrentLevelNum].GeneralLootLimiterColorLimiter, RandomCell, 4, LimiterCells[RandomCell].IsFull);
            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootLimiterGeneralSymbol;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }


            SummonLimiterConditions(GameLevels[CurrentLevelNum].GeneralLootLimiterSymbolLimiter, RandomCell, 4, LimiterCells[RandomCell].IsFull);

            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootLimiterSpecificColors;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomSpecific = Random.Range(0, GameLevels[CurrentLevelNum].SpecificLootLimiterColorLimitersPrefabs.Length);

            SummonLimiterConditions(GameLevels[CurrentLevelNum].SpecificLootLimiterColorLimitersPrefabs[RandomSpecific], RandomCell, 4, LimiterCells[RandomCell].IsFull);

            i++;
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfLootLimiterSpecificSymbols;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomSpecific = Random.Range(0, GameLevels[CurrentLevelNum].SpecificLootLimiterSymbolLimitersPrefabs.Length);

            SummonLimiterConditions(GameLevels[CurrentLevelNum].SpecificLootLimiterSymbolLimitersPrefabs[RandomSpecific], RandomCell, 4, LimiterCells[RandomCell].IsFull);

            i++;
        }

    }


    public void FillClipPiece(GameObject ToFill)
    {
        GameObject go = Instantiate(MovableDaddyPrefab, ToFill.transform);

        foreach (Transform child in go.transform)
        {
            if (child.CompareTag("RightPiece"))
            {
                if (GameLevels[CurrentLevelNum].DoubleRing)
                {
                    PieceNum = Random.Range(0, GameLevels[CurrentLevelNum].RightShapesGameobjects.Length);
                    GameObject P = Instantiate(GameLevels[CurrentLevelNum].RightShapesGameobjects[PieceNum], child.position, child.rotation, child.transform);
                    P.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
                }
                else
                {
                    PieceNum = Random.Range(0, RightShapesGameobjects.Length);
                    GameObject P = Instantiate(RightShapesGameobjects[PieceNum], child.position, child.rotation, child.transform);
                    P.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
                }
            }

            if (child.CompareTag("LeftPiece"))
            {
                if (GameLevels[CurrentLevelNum].DoubleRing)
                {
                    PieceNum = Random.Range(0, GameLevels[CurrentLevelNum].LeftShapesGameobjects.Length);
                    GameObject P = Instantiate(GameLevels[CurrentLevelNum].LeftShapesGameobjects[PieceNum], child.position, child.rotation, child.transform);
                    P.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
                }
                else
                {
                    PieceNum = Random.Range(0, LeftShapesGameobjects.Length);
                    GameObject P = Instantiate(LeftShapesGameobjects[PieceNum], child.position, child.rotation, child.transform);
                    P.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
                }
            }

        }
    }

    public void CheckEndGame()
    {
        //Debug.Log("Checking");
        if(FullCellCounter == CellsNeeedToFinish && SuccesfullConnectionsMade == ConnectionsNeededToFinishLevel)
        {
            //Debug.Log("Win");
            UiManager.Instance.YouWinMessage();

            LeftSideClipsPieces.Clear();
            RightSideClipsPieces.Clear();
            LimiterCells.Clear();
            Connectors.Clear();
            PieceCells.Clear();
            FullCellCounter = 0;
            SuccesfullConnectionsMade = 0;
            CurrentLevelNum++;

            if(GameLevels[CurrentLevelNum].LevelNum > ThePlayer.MaxLevelReached)
            {
                ThePlayer.MaxLevelReached = GameLevels[CurrentLevelNum].LevelNum;
            }

            ThePlayer.SaveDate();
        }
        else
        {
            UiManager.Instance.YouLoseMessage();

            LeftSideClipsPieces.Clear();
            RightSideClipsPieces.Clear();
            LimiterCells.Clear();
            Connectors.Clear();
            PieceCells.Clear();
            FullCellCounter = 0;
            //Debug.Log("3");
            SuccesfullConnectionsMade = 0;
        }
    }

    //public void StartNextLevel()
    //{
    //    //for (int i = 0; i < DeleteOnLevelTransfer.transform.childCount; i++)
    //    //{
    //    //    Destroy(DeleteOnLevelTransfer.transform.GetChild(i).gameObject);
    //    //}

    //    //UiManager.Instance.GoToNextLevelButton.gameObject.SetActive(false);
    //    LeftSideClipsPieces.Clear();
    //    RightSideClipsPieces.Clear();
    //    LimiterCells.Clear();
    //    Connectors.Clear();
    //    PieceCells.Clear();
    //    FullCellCounter = 0;
    //    SuccesfullConnectionsMade = 0;
    //    CurrentLevelNum++;

    //    ThePlayer.MaxLevelReached = GameLevels[CurrentLevelNum].LevelNum;
    //    ThePlayer.SaveDate();

    //    //StartCoroutine(CallNextLevelFuntion());
    //}

    //public IEnumerator CallNextLevelFuntion()
    //{
    //    yield return new WaitForEndOfFrame();
    //    StartGame();
    //}

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ToggleLevelHub(bool Toggle)
    {
        UiManager.Instance.ToggleLevelHub(Toggle);
    }


    public void OrganizeForNextLevel()
    {
        for (int i = 0; i < DeleteOnLevelTransfer.transform.childCount; i++)
        {
            Destroy(DeleteOnLevelTransfer.transform.GetChild(i).gameObject);
        }

        LeftSideClipsPieces.Clear();
        RightSideClipsPieces.Clear();
        LimiterCells.Clear();
        Connectors.Clear();
        PieceCells.Clear();
        FullCellCounter = 0;
        SuccesfullConnectionsMade = 0;
    }


    public void SummonLimiterConditions(GameObject ToSummon, int RandomCell, int SliceIndex, bool OriginSlotFull)
    {
        int randomOptionalLimiterZone = Random.Range(0, LimiterCells[RandomCell].OptionalLimiterZones.Length);
        CheckAllOptionalLimiterZonesFull = 0;
        switch (SliceIndex)
        {
            case 1:
                if (!OriginSlotFull)
                {
                    go = Instantiate(ToSummon, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                    LimiterCells[RandomCell].IsFull = true;
                    LimiterCells[RandomCell].TypeOfLimiter = go.GetComponent<LimiterPiece>().TypeOfLimiter;
                    LimiterCells[RandomCell].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;
                }
                else
                {
                    for (int i = 0; i < LimiterCells[RandomCell].OptionalLimiterZones.Length; i++)
                    {
                        if (LimiterCells[RandomCell].OptionalLimiterZones[i].IsFull)
                        {
                            CheckAllOptionalLimiterZonesFull++;
                        }
                    }

                    if (CheckAllOptionalLimiterZonesFull == LimiterCells[RandomCell].OptionalLimiterZones.Length)
                    {
                        RandomCell = Random.Range(0, LimiterCells.Count);

                        while (LimiterCells[RandomCell].IsFull)
                        {
                            RandomCell = Random.Range(0, LimiterCells.Count);
                        }

                        go = Instantiate(ToSummon, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                        LimiterCells[RandomCell].IsFull = true;
                        LimiterCells[RandomCell].TypeOfLimiter = go.GetComponent<LimiterPiece>().TypeOfLimiter;
                        LimiterCells[RandomCell].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;

                        return;
                    }

                    while (LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].IsFull)
                    {
                        randomOptionalLimiterZone = Random.Range(0, LimiterCells[RandomCell].OptionalLimiterZones.Length);
                    }

                    go = Instantiate(ToSummon, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform.position, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform.rotation, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform);
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].IsFull = true;
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].TypeOfLimiter = go.GetComponent<LimiterPiece>().TypeOfLimiter;
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;
                }
                break;
            case 2:
                if (!OriginSlotFull)
                {
                    go = Instantiate(ToSummon, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                    LimiterCells[RandomCell].IsFull = true;
                    LimiterCells[RandomCell].TypeOfLootSlice = go.GetComponent<LimiterPiece>().TypeOfLootSlice;
                    LimiterCells[RandomCell].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;
                }
                else
                {
                    for (int i = 0; i < LimiterCells[RandomCell].OptionalLimiterZones.Length; i++)
                    {
                        if (LimiterCells[RandomCell].OptionalLimiterZones[i].IsFull)
                        {
                            CheckAllOptionalLimiterZonesFull++;
                        }
                    }

                    if (CheckAllOptionalLimiterZonesFull == LimiterCells[RandomCell].OptionalLimiterZones.Length)
                    {
                        RandomCell = Random.Range(0, LimiterCells.Count);

                        while (LimiterCells[RandomCell].IsFull)
                        {
                            RandomCell = Random.Range(0, LimiterCells.Count);
                        }

                        go = Instantiate(ToSummon, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                        LimiterCells[RandomCell].IsFull = true;
                        LimiterCells[RandomCell].TypeOfLootSlice = go.GetComponent<LimiterPiece>().TypeOfLootSlice;
                        LimiterCells[RandomCell].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;

                        return;
                    }


                    while (LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].IsFull)
                    {
                        randomOptionalLimiterZone = Random.Range(0, LimiterCells[RandomCell].OptionalLimiterZones.Length);
                    }

                    go = Instantiate(ToSummon, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform.position, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform.rotation, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform);
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].IsFull = true;
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].TypeOfLootSlice = go.GetComponent<LimiterPiece>().TypeOfLootSlice;
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;
                }

                break;
            case 3:
                if (!OriginSlotFull)
                {
                    go = Instantiate(ToSummon, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                    LimiterCells[RandomCell].IsFull = true;
                    LimiterCells[RandomCell].TypeOfLootLockSlice = go.GetComponent<LimiterPiece>().TypeOfLootLockSlice;
                    LimiterCells[RandomCell].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;
                }
                else
                {
                    for (int i = 0; i < LimiterCells[RandomCell].OptionalLimiterZones.Length; i++)
                    {
                        if (LimiterCells[RandomCell].OptionalLimiterZones[i].IsFull)
                        {
                            CheckAllOptionalLimiterZonesFull++;
                        }
                    }

                    if (CheckAllOptionalLimiterZonesFull == LimiterCells[RandomCell].OptionalLimiterZones.Length)
                    {
                        RandomCell = Random.Range(0, LimiterCells.Count);

                        while (LimiterCells[RandomCell].IsFull)
                        {
                            RandomCell = Random.Range(0, LimiterCells.Count);
                        }

                        go = Instantiate(ToSummon, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                        LimiterCells[RandomCell].IsFull = true;
                        LimiterCells[RandomCell].TypeOfLootLockSlice = go.GetComponent<LimiterPiece>().TypeOfLootLockSlice;
                        LimiterCells[RandomCell].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;

                        return;
                    }


                    while (LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].IsFull)
                    {
                        randomOptionalLimiterZone = Random.Range(0, LimiterCells[RandomCell].OptionalLimiterZones.Length);
                    }

                    go = Instantiate(ToSummon, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform.position, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform.rotation, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform);
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].IsFull = true;
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].TypeOfLootLockSlice = go.GetComponent<LimiterPiece>().TypeOfLootLockSlice;
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;
                }

                break;
            case 4:
                if (!OriginSlotFull)
                {
                    go = Instantiate(ToSummon, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                    LimiterCells[RandomCell].IsFull = true;
                    LimiterCells[RandomCell].TypeOfLootLimiter = go.GetComponent<LimiterPiece>().TypeOfLootLimiter;
                    LimiterCells[RandomCell].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;
                }
                else
                {
                    for (int i = 0; i < LimiterCells[RandomCell].OptionalLimiterZones.Length; i++)
                    {
                        if (LimiterCells[RandomCell].OptionalLimiterZones[i].IsFull)
                        {
                            CheckAllOptionalLimiterZonesFull++;
                        }
                    }

                    if (CheckAllOptionalLimiterZonesFull == LimiterCells[RandomCell].OptionalLimiterZones.Length)
                    {
                        RandomCell = Random.Range(0, LimiterCells.Count);

                        while (LimiterCells[RandomCell].IsFull)
                        {
                            RandomCell = Random.Range(0, LimiterCells.Count);
                        }

                        go = Instantiate(ToSummon, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                        LimiterCells[RandomCell].IsFull = true;
                        LimiterCells[RandomCell].TypeOfLootLimiter = go.GetComponent<LimiterPiece>().TypeOfLootLimiter;
                        LimiterCells[RandomCell].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;

                        return;
                    }


                    while (LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].IsFull)
                    {
                        randomOptionalLimiterZone = Random.Range(0, LimiterCells[RandomCell].OptionalLimiterZones.Length);
                    }

                    go = Instantiate(ToSummon, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform.position, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform.rotation, LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].transform);
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].IsFull = true;
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].TypeOfLootLimiter = go.GetComponent<LimiterPiece>().TypeOfLootLimiter;
                    LimiterCells[RandomCell].OptionalLimiterZones[randomOptionalLimiterZone].boolTypeOfSlice = go.GetComponent<LimiterPiece>().boolTypeOfSlice;
                }

                break;

            default:
                break;
        }

    }
}
