using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LimiterType
{
    GeneralColor,
    GeneralSymbol,
    Red,
    Blue,
    Yellow,
    Pink,
    Square,
    Triangle,
    Plus,
    Circle,
    None
}

public enum LootLimiterType
{
    LootLimiterGeneralColor,
    LootLimiterGeneralSymbol,
    LootLimiterSpecificRed,
    LootLimiterSpecificBlue,
    LootLimiterSpecificYellow,
    LootLimiterSpecificPink,
    LootLimiterSpecificSquare,
    LootLimiterSpecificTriangle,
    LootLimiterSpecificPlus,
    LootLimiterSpecificCircle,
    None
}

public enum LootSliceType
{
    LootGeneralColor,
    LootGeneralSymbol,
    LootSpecificRed,
    LootSpecificBlue,
    LootSpecificYellow,
    LootSpecificPink,
    LootSpecificSquare,
    LootSpecificTriangle,
    LootSpecificPlus,
    LootSpecificCircle,
    None
}

public enum LootLockSliceType
{
    LootLockGeneralColor,
    LootLockGeneralSymbol,
    LootLockSpecificRed,
    LootLockSpecificBlue,
    LootLockSpecificYellow,
    LootLockSpecificPink,
    LootLockSpecificSquare,
    LootLockSpecificTriangle,
    LootLockSpecificPlus,
    LootLockSpecificCircle,
    None
}


public class LimiterCellManager : MonoBehaviour
{
    public bool IsFull;

    public LimiterType TypeOfLimiter;

    public LootLimiterType TypeOfLootLimiter;

    public LootSliceType TypeOfLootSlice;

    public LootLockSliceType TypeOfLootLockSlice;

    public BoolSliceType boolTypeOfSlice;
}
