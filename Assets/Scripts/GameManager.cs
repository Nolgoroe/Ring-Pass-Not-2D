using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int CurrentLevelNum = 1;

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

    public GameObject DeleteOnLevelTransfer;
    int LevelNumOfLimiters;

    void Start()
    {
        Instance = this;
    }

    public void StartGame()
    {
        StartLevel();
    }

    public void StartLevel()
    {
        GameObject C = Instantiate(ClipPrefab, DeleteOnLevelTransfer.transform);
        C.tag = "Clip";

        GameObject B = Instantiate(BoardPrefab, DeleteOnLevelTransfer.transform);
        B.tag = "Board";

        PieceTargetLookAt = GameObject.FindGameObjectWithTag("Center").transform;

        FillClip();
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

        UiManager.Instance.InGameUI.SetActive(true);
        UiManager.Instance.Commit.interactable = false;
        UiManager.Instance.CommitCanvas.SetActive(true);
        UiManager.Instance.StartGameButton.gameObject.SetActive(false);
        UiManager.Instance.LevelNum.text = "Level: " + GameLevels[CurrentLevelNum].LevelNum.ToString();
        UiManager.Instance.LevelNum.gameObject.SetActive(true);
        //UiManager.Instance.Commit.interactable = false;
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
            PieceNum = Random.Range(0, LeftShapesGameobjects.Length);
            GameObject go = Instantiate(LeftShapesGameobjects[PieceNum], LeftSideClipsPieces[i].transform);
            go.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
        }

        for (int i = 0; i < RightSideClipsPieces.Count; i++)
        {
            PieceNum = Random.Range(0, RightShapesGameobjects.Length);
            GameObject go = Instantiate(RightShapesGameobjects[PieceNum], RightSideClipsPieces[i].transform);
            go.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
        }
    }

    public void FillLimiters()
    {
        LevelNumOfLimiters = GameLevels[CurrentLevelNum].CacluateNumOfLimiters();
        SceneBoard = GameObject.FindGameObjectWithTag("Board").transform;

        foreach (GameObject Limiter in GameObject.FindGameObjectsWithTag("Limiter"))
        {
            LimiterCells.Add(Limiter.GetComponent<LimiterCellManager>());
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfGeneralColors;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].GeneralColorLimiter, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].GeneralColorLimiter.GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfGeneralSymbol;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].GeneralSymbolLimiter, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].GeneralSymbolLimiter.GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfRedColorLimiter;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].RedColorLimiter, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].RedColorLimiter.GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfBlueColorLimiter;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].BlueColorLimiter, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].BlueColorLimiter.GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfYellowColorLimiter;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].YellowColorLimiter, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].YellowColorLimiter.GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfPinkColorLimiter;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].PinkColorLimiter, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].PinkColorLimiter.GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfSquareShapeLimiter;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].SquareShapeLimiter, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].SquareShapeLimiter.GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfTriangleShapeLimiter;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].TriangleShapeLimiter, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].TriangleShapeLimiter.GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfPlusShapeLimiter;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].PlusShapeLimiter, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].PlusShapeLimiter.GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfCircleShapeLimiter;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].CircleShapeLimiter, LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].CircleShapeLimiter.GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfSpecificColors;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);
            int RandomSpecific = Random.Range(0, GameLevels[CurrentLevelNum].SpecificColorLimiters.Length);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].SpecificColorLimiters[RandomSpecific], LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].SpecificColorLimiters[RandomSpecific].GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }

        for (int i = 0; i < GameLevels[CurrentLevelNum].NumOfSpecificSymbols;)
        {
            if (LevelNumOfLimiters > LimiterCells.Count)
            {
                Debug.LogError("Too many limiters! what the hell man!");
                break;
            }

            int RandomCell = Random.Range(0, LimiterCells.Count);
            int RandomSpecific = Random.Range(0, GameLevels[CurrentLevelNum].SpecificSymbolLimiters.Length);

            if (!LimiterCells[RandomCell].IsFull)
            {
                Instantiate(GameLevels[CurrentLevelNum].SpecificSymbolLimiters[RandomSpecific], LimiterCells[RandomCell].transform.position, LimiterCells[RandomCell].transform.rotation, LimiterCells[RandomCell].transform);
                LimiterCells[RandomCell].IsFull = true;
                LimiterCells[RandomCell].TypeOfLimiter = GameLevels[CurrentLevelNum].SpecificSymbolLimiters[RandomSpecific].GetComponent<LimiterPiece>().TypeOfLimiter;
                i++;
            }
        }
    }

    public void FillClipPiece(GameObject ToFill)
    {
        GameObject go = Instantiate(MovableDaddyPrefab, ToFill.transform);

        foreach (Transform child in go.transform)
        {
            if (child.CompareTag("RightPiece"))
            {
                PieceNum = Random.Range(0, RightShapesGameobjects.Length);
                GameObject P = Instantiate(RightShapesGameobjects[PieceNum], child.position, child.rotation, child.transform);
                P.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
            }

            if (child.CompareTag("LeftPiece"))
            {
                PieceNum = Random.Range(0, LeftShapesGameobjects.Length);
                GameObject P = Instantiate(LeftShapesGameobjects[PieceNum], child.position, child.rotation, child.transform);
                P.GetComponent<ColorSymbolData>().ChooseColorAndSprtie(PieceNum);
            }

        }
    }

    public void CheckEndGame()
    {
        if(FullCellCounter == 8 && SuccesfullConnectionsMade == 8)
        {
            UiManager.Instance.YouWinMessage();
        }
        else
        {
            UiManager.Instance.YouLoseMessage();
        }
    }

    public void StartNextLevel()
    {
        for (int i = 0; i < DeleteOnLevelTransfer.transform.childCount; i++)
        {
            Destroy(DeleteOnLevelTransfer.transform.GetChild(i).gameObject);
        }

        UiManager.Instance.GoToNextLevelButton.gameObject.SetActive(false);
        LeftSideClipsPieces.Clear();
        RightSideClipsPieces.Clear();
        LimiterCells.Clear();
        Connectors.Clear();
        PieceCells.Clear();
        FullCellCounter = 0;
        SuccesfullConnectionsMade = 0;
        CurrentLevelNum++;

        StartCoroutine(CallNextLevelFuntion());
    }

    public IEnumerator CallNextLevelFuntion()
    {
        yield return new WaitForEndOfFrame();
        StartGame();
    }
}
