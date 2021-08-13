using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinStatus : MonoBehaviour
{
    public CoinStatus Instance;
    public CoinDisplayer[] coinDisplayers;


    private void Awake()
    {
        Instance = this;
    }

    public void GetStatus()
    {
        for (int i = 0; i < coinDisplayers.Length; i++)
        {
            coinDisplayers[i].GetInfos();
        }
    }

    public void SetDefaults()
    {
        for (int i = 0; i < coinDisplayers.Length; i++)
        {
            coinDisplayers[i].SetDefault();
        }
    }
}
