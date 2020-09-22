using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootType
{
    SmallGold,
    BigGold,
    SmallRuby,
    BigRuby,
    MagicItem,
    None
}
public class LootManager : MonoBehaviour
{
    public static LootManager Instance;

    public int SmallGoldMin, SmallGoldMax;

    public int BigGoldMin, BigGoldMax;

    public int SmallRubyMin, SmallRubyMax;

    public int BigRubyMin, BigRubyMax;

    int AmoutOfGoldToRecive;
    int AmoutOfRubiesToRecive;

    private void Start()
    {
        Instance = this;
    }

    public void GainGold(LootType type)
    {
        switch (type)
        {
            case LootType.SmallGold:
                AmoutOfGoldToRecive = Random.Range(SmallGoldMin, SmallGoldMax);
                break;
            case LootType.BigGold:
                AmoutOfGoldToRecive = Random.Range(BigGoldMin, BigGoldMax);
                break;
        }

        UiManager.Instance.UpdateLootStats(AmoutOfGoldToRecive,0,0);
        //Debug.Log(AmoutOfGoldToRecive);
    }

    public void GainRubie(LootType type)
    {
        switch (type)
        {
            case LootType.SmallRuby:
                AmoutOfRubiesToRecive = Random.Range(SmallRubyMin, SmallRubyMax);
                break;
            case LootType.BigRuby:
                AmoutOfRubiesToRecive = Random.Range(BigRubyMin, BigRubyMax);
                break;
        }

        UiManager.Instance.UpdateLootStats(0, AmoutOfRubiesToRecive, 0);
        //Debug.Log(AmoutOfRubiesToRecive);
    }

    public void GainMagicalItems()
    {
        UiManager.Instance.UpdateLootStats(0, 0, 1);
        //Debug.Log("magic Item + 1");
    }
}
