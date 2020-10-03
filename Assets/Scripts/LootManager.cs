using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum LootType
{
    SmallGold,
    BigGold,
    SmallRuby,
    BigRuby,
    Wood,
    Stone,
    FireShard,
    PurpleFlower,
    Feather,
    None
}

[Serializable]
public class LootAmount
{
    public LootType TypeOfLoot;
    public CraftingMaterials Material;
    public int MinAmount;
    public int MaxAmount;
}

public class LootManager : MonoBehaviour
{
    public static LootManager Instance;

    //public int SmallGoldMin, SmallGoldMax;

    //public int BigGoldMin, BigGoldMax;

    //public int SmallRubyMin, SmallRubyMax;

    //public int BigRubyMin, BigRubyMax;

    public List<LootAmount> LootParameters;

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
                break;
            case LootType.BigGold:
                break;
        }

        //UiManager.Instance.UpdateLootStats(AmoutOfGoldToRecive,0,0);
        //Debug.Log(AmoutOfGoldToRecive);
    }

    public void GainRubie(LootType type)
    {
        switch (type)
        {
            case LootType.SmallRuby:
                break;
            case LootType.BigRuby:
                break;
        }

        //UiManager.Instance.UpdateLootStats(0, AmoutOfRubiesToRecive, 0);
        //Debug.Log(AmoutOfRubiesToRecive);
    }

    public void GainMagicalItems()
    {
        //UiManager.Instance.UpdateLootStats(0, 0, 1);
        //Debug.Log("magic Item + 1");
    }


    public void GetAmountOfLoot(LootType type)
    {
        AmoutOfGoldToRecive = 0;
        AmoutOfRubiesToRecive = 0;

        foreach (LootAmount item in LootParameters)
        {
            if(item.TypeOfLoot == type)
            {
                switch (item.TypeOfLoot)
                {
                    case LootType.SmallGold:
                        AmoutOfGoldToRecive = UnityEngine.Random.Range(item.MinAmount, item.MaxAmount);
                        break;
                    case LootType.BigGold:
                        AmoutOfGoldToRecive = UnityEngine.Random.Range(item.MinAmount, item.MaxAmount);
                        break;
                    case LootType.SmallRuby:
                        AmoutOfRubiesToRecive = UnityEngine.Random.Range(item.MinAmount, item.MaxAmount);
                        break;
                    case LootType.BigRuby:
                        AmoutOfRubiesToRecive = UnityEngine.Random.Range(item.MinAmount, item.MaxAmount);
                        break;
                    case LootType.Wood:
                        GameManager.Instance.ThePlayer.CraftingMatsInInventory.Add(new ItemAmount(item.Material, UnityEngine.Random.Range(item.MinAmount, item.MaxAmount)));
                        break;
                    case LootType.Stone:
                        GameManager.Instance.ThePlayer.CraftingMatsInInventory.Add(new ItemAmount(item.Material, UnityEngine.Random.Range(item.MinAmount, item.MaxAmount)));
                        break;
                    case LootType.FireShard:
                        GameManager.Instance.ThePlayer.CraftingMatsInInventory.Add(new ItemAmount(item.Material, UnityEngine.Random.Range(item.MinAmount, item.MaxAmount)));
                        break;
                    case LootType.PurpleFlower:
                        GameManager.Instance.ThePlayer.CraftingMatsInInventory.Add(new ItemAmount(item.Material, UnityEngine.Random.Range(item.MinAmount, item.MaxAmount)));
                        break;
                    case LootType.Feather:
                        GameManager.Instance.ThePlayer.CraftingMatsInInventory.Add(new ItemAmount(item.Material, UnityEngine.Random.Range(item.MinAmount, item.MaxAmount)));
                        break;
                    case LootType.None:
                        break;
                    default:
                        break;
                }
                break;
            }
        }

        GameManager.Instance.ThePlayer.Gold += AmoutOfGoldToRecive;
        GameManager.Instance.ThePlayer.Rubies += AmoutOfRubiesToRecive;
        //ThePlayer.MagicalItems += MagicalItem;
    }
}
