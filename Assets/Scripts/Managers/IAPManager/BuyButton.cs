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
    [SerializeField] TextMeshProUGUI priceText;
    private string defaultText;

    private void Start()
    {
        defaultText = priceText.text;
        StartCoroutine(LoadPrices());
    }

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

    IEnumerator LoadPrices()
    {
        while (IAPManager.Instance.IsInitialized())
        {
            yield return null;
        }
        string loadedPrice = "";
        switch (type)
        {
            case BuyingType.gold1000:
                loadedPrice = IAPManager.Instance.GetProductPrices(IAPManager.Instance.gold1000);
                break;
            case BuyingType.gold5000:
                loadedPrice = IAPManager.Instance.GetProductPrices(IAPManager.Instance.gold5000);
                break;
            case BuyingType.gold10000:
                loadedPrice = IAPManager.Instance.GetProductPrices(IAPManager.Instance.gold10000);
                break;
            case BuyingType.gold100000:
                loadedPrice = IAPManager.Instance.GetProductPrices(IAPManager.Instance.gold100000);
                break;
            case BuyingType.emerald100:
                loadedPrice = IAPManager.Instance.GetProductPrices(IAPManager.Instance.emerald100);
                break;
            case BuyingType.emerald500:
                loadedPrice = IAPManager.Instance.GetProductPrices(IAPManager.Instance.emerald500);
                break;
            case BuyingType.emerald1000:
                loadedPrice = IAPManager.Instance.GetProductPrices(IAPManager.Instance.emerald1000);
                break;
            case BuyingType.emerald5000:
                loadedPrice = IAPManager.Instance.GetProductPrices(IAPManager.Instance.emerald5000);
                break;
            case BuyingType.pack1:
                loadedPrice = IAPManager.Instance.GetProductPrices(IAPManager.Instance.pack1);
                break;
        }
        priceText.text = defaultText + "" + loadedPrice;
        Debug.Log("Loaded Price is " + loadedPrice);
    }

}
