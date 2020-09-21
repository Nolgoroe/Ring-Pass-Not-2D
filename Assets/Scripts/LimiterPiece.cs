using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BoolSliceType
{
    NormalLimiter,
    LootLimiter,
    LootSlice,
    LootLockSlice,
    None
}
public class LimiterPiece : MonoBehaviour
{
    public LimiterType TypeOfLimiter;

    public LootLimiterType TypeOfLootLimiter;

    public LootSliceType TypeOfLootSlice;

    public LootLockSliceType TypeOfLootLockSlice;

    public BoolSliceType boolTypeOfSlice;

    public LimiterCellManager ParentLimiterCell;

    private void Start()
    {
        ParentLimiterCell = transform.parent.GetComponent<LimiterCellManager>();
    }
}
