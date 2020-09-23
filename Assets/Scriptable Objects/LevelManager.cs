using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixeye.Unity;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Create Level")]
public class LevelManager : ScriptableObject
{
    public int LevelNum;
    public int CellsInLevel, ConnectionsNeededToFinishLevel;

    public GameObject BoardPrefab;

    public bool DoubleRing;

    public ColorData[] SpecificColors;
    public Symbols[] SpecificSymbols;

    public GameObject[] LeftShapesGameobjects;
    public GameObject[] RightShapesGameobjects;

    #region Normal Limiters
    [Foldout("Num of Normal Limiters", false)]
    public int NumOfGeneralColors, NumOfGeneralSymbol, NumOfSpecificColors, NumOfSpecificSymbols;

    [Foldout("General Limiter Prefabs", false)]
    public GameObject GeneralColorLimiter, GeneralSymbolLimiter;

    [Foldout("Specific Limiter Prefabs", false)]
    public GameObject[] SpecificColorLimitersPrefabs, SpecificSymbolLimitersPrefabs;
    #endregion

    #region Loot Slices
    [Foldout("Num of Loot Slices", false)]
    public int NumOfLootGeneralColors, NumOfLootGeneralSymbol, NumOfLootSpecificColors, NumOfLootSpecificSymbols;

    [Foldout("Loot Slice Prefabs", false)]
    public GameObject GeneralLootColorLimiter, GeneralLootSymbolLimiter;

    [Foldout("Specific Loot Slice Prefabs", false)]
    public GameObject[] SpecificLootColorLimitersPrefabs, SpecificLootSymbolLimitersPrefabs;
    #endregion

    #region Loot Lock Slices
    [Foldout("Num of Loot Lock Slices", false)]
    public int NumOfLootLockGeneralColors, NumOfLootLockGeneralSymbol, NumOfLootLockSpecificColors, NumOfLootLockSpecificSymbols;

    [Foldout("Loot Lock Slice Prefabs", false)]
    public GameObject GeneralLootLockColorLimiter, GeneralLootLockSymbolLimiter;

    [Foldout("Specific Loot Lock Slice Prefabs", false)]
    public GameObject[] SpecificLootLockColorLimitersPrefabs, SpecificLootLockSymbolLimitersPrefabs;
    #endregion

    #region Loot Limiters
    [Foldout("Num of Loot Limiters", false)]
    public int NumOfLootLimiterGeneralColors, NumOfLootLimiterGeneralSymbol, NumOfLootLimiterSpecificColors, NumOfLootLimiterSpecificSymbols;

    [Foldout("Loot Limiter Prefabs", false)]
    public GameObject GeneralLootLimiterColorLimiter, GeneralLootLimiterSymbolLimiter;

    [Foldout("Specific Loot Limiter Prefabs", false)]
    public GameObject[] SpecificLootLimiterColorLimitersPrefabs, SpecificLootLimiterSymbolLimitersPrefabs;
    #endregion

    #region Loot Zone
    public List<LootType> LootForLevel;
    #endregion

    //public int NumOfRedColorLimiter, NumOfBlueColorLimiter, NumOfYellowColorLimiter, NumOfPinkColorLimiter;

    //public int NumOfSquareShapeLimiter, NumOfTriangleShapeLimiter, NumOfPlusShapeLimiter, NumOfCircleShapeLimiter;

    //public GameObject RedColorLimiter, BlueColorLimiter, YellowColorLimiter, PinkColorLimiter;

    //public GameObject SquareShapeLimiter, TriangleShapeLimiter, PlusShapeLimiter, CircleShapeLimiter;

    public int CacluateNumOfLimiters()
    {
        int LimiterNum = NumOfGeneralColors + NumOfGeneralSymbol + NumOfSpecificColors + NumOfSpecificSymbols
            + NumOfLootGeneralColors + NumOfLootGeneralSymbol + NumOfLootSpecificColors + NumOfLootSpecificSymbols 
            + NumOfLootLockGeneralColors + NumOfLootLockGeneralSymbol + NumOfLootLockSpecificColors + NumOfLootLockSpecificSymbols +
             NumOfLootLimiterGeneralColors + NumOfLootLimiterGeneralSymbol + NumOfLootLimiterSpecificColors + NumOfLootLimiterSpecificSymbols
            /*+ NumOfRedColorLimiter + NumOfBlueColorLimiter + NumOfYellowColorLimiter
            + NumOfPinkColorLimiter + NumOfSquareShapeLimiter + NumOfTriangleShapeLimiter + NumOfPlusShapeLimiter + NumOfCircleShapeLimiter*/;

        return LimiterNum;
    }
}
