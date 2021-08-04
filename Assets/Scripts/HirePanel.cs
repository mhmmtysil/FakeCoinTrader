using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HirePanel : MonoBehaviour
{
    public GriffonCoin griffonCoin;
    public HorseCoin horseCoin;
    public InogamiCoin inogamiCoin;
    public LightCore lightCore;
    public OdeaCoin odeaCoin;

    public void OpenHirePanel()
    {
        griffonCoin.CheckLockStatus();
        horseCoin.CheckLockStatus();
        inogamiCoin.CheckLockStatus();
        lightCore.CheckLockStatus();
        odeaCoin.CheckLockStatus();

        griffonCoin.CheckHireStatus();
        horseCoin.CheckHireStatus();
        inogamiCoin.CheckHireStatus();
        lightCore.CheckHireStatus();
        odeaCoin.CheckHireStatus();

        gameObject.SetActive(true);
    }
}
