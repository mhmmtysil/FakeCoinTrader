using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopOpener : MonoBehaviour
{
    public GameObject shopPanel;
    public GameObject emeraldShop;
    public GameObject emeraldContent;
    public GameObject coinShop;
    public GameObject coinContent;
    public Button emeraldShopButton;
    public Button coinShopButton;
    public Sprite activeSprite;
    public Sprite deactiveSprite;


    public void OpenCoinShop()
    {
        shopPanel.SetActive(true);
        emeraldShop.SetActive(false);
        coinShop.SetActive(true);
        emeraldShopButton.image.sprite = activeSprite;
        coinShopButton.image.sprite = deactiveSprite;
    }
    public void OpenEmeraldShop()
    {
        shopPanel.SetActive(true);
        emeraldShop.SetActive(true);
        coinShop.SetActive(false);
        emeraldShopButton.image.sprite = deactiveSprite;
        coinShopButton.image.sprite = activeSprite;
    }
}
