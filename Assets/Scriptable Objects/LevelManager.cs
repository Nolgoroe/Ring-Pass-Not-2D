using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "ScriptableObjects/Create Level")]
public class LevelManager : ScriptableObject
{
    public int LevelNum;

    public int NumOfGeneralColors, NumOfGeneralSymbol;

    public int NumOfSpecificColors, NumOfSpecificSymbols;

    public int NumOfRedColorLimiter, NumOfBlueColorLimiter, NumOfYellowColorLimiter, NumOfPinkColorLimiter;

    public int NumOfSquareShapeLimiter, NumOfTriangleShapeLimiter, NumOfPlusShapeLimiter, NumOfCircleShapeLimiter;

    public GameObject GeneralColorLimiter, GeneralSymbolLimiter;

    public GameObject RedColorLimiter, BlueColorLimiter, YellowColorLimiter, PinkColorLimiter;

    public GameObject SquareShapeLimiter, TriangleShapeLimiter, PlusShapeLimiter, CircleShapeLimiter;

    public GameObject[] SpecificColorLimiters, SpecificSymbolLimiters;

    public int CacluateNumOfLimiters()
    {
        int LimiterNum = NumOfGeneralColors + NumOfGeneralSymbol + NumOfRedColorLimiter + NumOfBlueColorLimiter + NumOfYellowColorLimiter
            + NumOfPinkColorLimiter + NumOfSquareShapeLimiter + NumOfTriangleShapeLimiter + NumOfPlusShapeLimiter + NumOfCircleShapeLimiter;

        return LimiterNum;
    }
}
