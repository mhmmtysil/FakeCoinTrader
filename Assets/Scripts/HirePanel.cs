using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HirePanel : MonoBehaviour
{
    public FakeCoin fakeCoin;
    public HorseCoin horseCoin;
    public LightCore lightCore;
    public OdeaCoin odeaCoin;
    public InogamiCoin inogamiCoin;
    public GriffonCoin griffonCoin;

    public void OpenHirePanel()
    {
        fakeCoin.CheckHireStatus();
        horseCoin.CheckHireStatus();
        lightCore.CheckHireStatus();
        odeaCoin.CheckHireStatus();
        inogamiCoin.CheckHireStatus();
        griffonCoin.CheckHireStatus();

        gameObject.SetActive(true);
    }
}
