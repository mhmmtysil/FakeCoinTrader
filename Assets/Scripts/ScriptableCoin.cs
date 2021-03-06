using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Name", menuName = "Item/Yeni Item")]
public class ScriptableCoin : ScriptableObject
{
    [Header("Coin Name")]
    public string coinName;
    public int coinBalance;
    public int hirePerClicked;
    public float diggingSpeed;
    public bool isOpened;
    public bool isHired;
    public bool isSpeeded;
    public int unlockPrice;
    public bool canBeOpened;
    public int tradeBalance;
    public int speedUpPrice;

    [Header("Coin Defaults")]
    public int hirePerClickedDefault;
    public float diggingSpeedDefault;

}