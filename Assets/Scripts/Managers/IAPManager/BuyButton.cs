using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyButton : MonoBehaviour
{
    public enum BuyingType
    {
        gold1000,
        gold5000,
        gold10000,
        gold100000,
        emerald100,
        emerald500,
        emerald1000,
        emerald5000,
        pack1
    }
    public BuyingType type;

    public void Buy()
    {
        switch (type)
        {
            case BuyingType.gold1000:
                IAPManager.Instance.BuyGold1000();
                break;
            case BuyingType.gold5000:
                IAPManager.Instance.BuyGold5000();
                break;
            case BuyingType.gold10000:
                IAPManager.Instance.BuyGold10000();
                break;
            case BuyingType.gold100000:
                IAPManager.Instance.BuyGold100000();
                break;
            case BuyingType.emerald100:
                IAPManager.Instance.BuyEmerald100();
                break;
            case BuyingType.emerald500:
                IAPManager.Instance.BuyEmerald500();
                break;
            case BuyingType.emerald1000:
                IAPManager.Instance.BuyEmerald1000();
                break;
            case BuyingType.emerald5000:
                IAPManager.Instance.BuyEmerald5000();
                break;
            case BuyingType.pack1:
                IAPManager.Instance.BuyPack1();
                break;
        }
    }

    
}
