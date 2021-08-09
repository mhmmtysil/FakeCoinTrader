using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinStatus : MonoBehaviour
{
    public CoinStatus Instance;

    public FakeCoin fakeCoin;
    public HorseCoin horseCoin;
    public LightCore lightCore;
    public InogamiCoin inogamiCoin;
    public OdeaCoin odeaCoin;
    public GriffonCoin griffonCoin;


    private void Awake()
    {
        Instance = this;
    }

    public void GetStatus()
    {
        fakeCoin.GetInfos();
        horseCoin.GetInfos();
        lightCore.GetInfos();
        inogamiCoin.GetInfos();
        odeaCoin.GetInfos();
        griffonCoin.GetInfos();
    }

    public void SetDefaults()
    {
        fakeCoin.SetDefault();
        horseCoin.SetDefault();
        lightCore.SetDefault();
        inogamiCoin.SetDefault();
        odeaCoin.SetDefault();
        griffonCoin.SetDefault();
    }
}
