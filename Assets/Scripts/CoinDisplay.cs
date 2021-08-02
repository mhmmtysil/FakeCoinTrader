using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CoinDisplay : MonoBehaviour
{
    public ScriptableCoin coin;
    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI coinBalanceTradePanelText;
    public TextMeshProUGUI coinPerMinuteText;
    public TextMeshProUGUI hiredText;

    public Button digButton;
    public Button hireButton;
    public Button hirePanelHireButton;
    public GameObject hirePanelHiredButton;
    public Button lockedButton; 

    public Slider coinSlider;

    public Sprite canBeOpened;
    public Sprite cannotBeOpened;


    public int coinbalance;
    public int hirePerClicked;
    public float diggingSpeed;

    void OnEnable()
    {
        StartCoroutine(GetInfos());
    }

    public void UpdateSliderValue()
    {
        StartCoroutine(AnimateSliderOverTime(diggingSpeed));
    }
    IEnumerator GetInfos()
    {
        yield return new WaitForSeconds(2);        
        switch (coin.coinName)
        {
            case "FakeCoin":
                coin.coinBalance = GameManager.Instance._fakeCoin;
                coin.isHired = GameManager.Instance.fakeCoinHired; 
                break;
            case "HorsePower":
                coin.coinBalance = GameManager.Instance._horsePower;
                coin.isOpened = GameManager.Instance.horsePowerOpened;
                coin.isHired = GameManager.Instance.horsePowerHired;
                break;
            case "LightCore":
                coin.coinBalance = GameManager.Instance._lightCore;
                coin.isOpened = GameManager.Instance.lightCoreOpened;
                coin.isHired = GameManager.Instance.lightCoreHired;
                break;
            case "OdeaCoin":
                coin.coinBalance = GameManager.Instance._odeaCoin;
                coin.isOpened = GameManager.Instance.odeaCoinOpened;
                coin.isHired = GameManager.Instance.odeaCoinHired;
                break;
            case "InogamiCoin":
                coin.coinBalance = GameManager.Instance._inogamiCoin;
                coin.isOpened = GameManager.Instance.inogamiCoinOpened;
                coin.isHired = GameManager.Instance.inogamiCoinHired;
                break;
            case "GriffonCoin":
                coin.coinBalance = GameManager.Instance._griffonCoin;
                coin.isOpened = GameManager.Instance.griffonCoinOpened;
                coin.isHired = GameManager.Instance.griffonCoinHired;
                break;
        }
        Debug.Log("Hired Statu for : " + coin.coinName + " is : " + coin.isHired);
        UpdateCoinBalanceTexts(coin.coinBalance);
        CheckHireStatus();
        CheckLockStatus();
        CheckLockImageSprite();
    }

    public void CoinsDiggingSpeedDoubler()
    {
        diggingSpeed /= 2;
        coinSlider.value = 1;
    }

    public void CheckLockStatus()
    {
        if (coin.isOpened)
        {
            lockedButton.gameObject.SetActive(false);
        }
        else
        {
            lockedButton.gameObject.SetActive(true);

        }        
    }
    private void CheckLockImageSprite()
    {
        if (coin.unlockPrice <= GameManager.Instance._coin)
        {
            lockedButton.image.sprite = canBeOpened;
            lockedButton.interactable = true;
        }
        else
        {
            lockedButton.image.sprite = cannotBeOpened;
            lockedButton.interactable = false;
        }
    }

    private void CheckHireStatus()
    {
        if (coin.isHired)
        {
            UpdateSliderValue();
            hirePanelHireButton.gameObject.SetActive(false);
            hirePanelHiredButton.SetActive(true);
            hiredText.gameObject.SetActive(true);
            digButton.gameObject.SetActive(false);
            hireButton.gameObject.SetActive(false);
        }
        else
        {
            hirePanelHireButton.gameObject.SetActive(true);
            hirePanelHiredButton.SetActive(false);
            hiredText.gameObject.SetActive(false);
            digButton.gameObject.SetActive(true);
            hireButton.gameObject.SetActive(true);
        }
    }
    public void Hire(int price)
    {
        if (price <= GameManager.Instance._emerald)
        {
            GameManager.Instance._emerald -= price;
            GameManager.Instance.UpdateEmerald();
            GameManager.Instance.SetCoinHired(coin.coinName, true);
        }
        else
        {
            hirePanelHireButton.GetComponent<Animator>().SetTrigger("notEnough");
        }
        CheckHireStatus();
    }


    public void OpenCoin(int price)
    {
        if (price <= GameManager.Instance._coin)
        {
            GameManager.Instance.BuyWithCoin(price);
            lockedButton.gameObject.SetActive(false);
            GameManager.Instance.SetCoinUnlock(coin.coinName, true);
        }
    }
    public void OpenCoinWithEmerald(int price)
    {
        if (price <= GameManager.Instance._emerald)
        {
            GameManager.Instance.BuyWithEmerald(price);
            lockedButton.gameObject.SetActive(false);
            GameManager.Instance.SetCoinUnlock(coin.coinName, true);
        }
    }
    
    public void UpdateCoinBalanceTexts(int _coinBalance)
    {
        coinBalanceText.text = coin.coinName + " (" + coin.coinBalance + " adet)";
        coinBalanceTradePanelText.text = "Bakiye : " + _coinBalance;
        coinPerMinuteText.text = 60 / diggingSpeed * hirePerClicked + "/dakika.";
        GameManager.Instance.UpdateCoinsText(coin.coinName, coin.coinBalance);
    }
    public void GiveCoin()
    {
        coin.coinBalance++;
        GameManager.Instance.UpdateCoinsText(coin.coinName, coin.coinBalance);

    }

    public void BuyCoinWithGold(int price)
    {
        if (GameManager.Instance._coin >= price)
        {
            GameManager.Instance._coin -= price;
            coin.coinBalance += price;
            UpdateCoinBalanceTexts(coin.coinBalance);
            GameManager.Instance.UpdateCoinTexts();
            CheckLockStatus();
        }

    }

    public void TradeWithGold(int amount)
    {
        if (coin.coinBalance >= amount)
        {
            coin.coinBalance -= amount;
            UpdateCoinBalanceTexts(coin.coinBalance);
            GameManager.Instance.GiveCoin(100);
            GameManager.Instance.GiveExp(100);
        }
        else
        {
            coinBalanceTradePanelText.GetComponent<Animator>().SetTrigger("notEnough");
        }
    }
    public void TradeWithEmerald(int amount)
    {
        if (coin.coinBalance >= amount)
        {
            coin.coinBalance -= amount;
            UpdateCoinBalanceTexts(coin.coinBalance);
            GameManager.Instance.GiveEmerald(10);
        }
    }

    IEnumerator AnimateSliderOverTime(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            coinSlider.value = Mathf.Lerp(0, 1f, lerpValue);
            digButton.interactable = false;
            if (coinSlider.value >= 1)
            {
                coinSlider.value = 0;
                coin.coinBalance += hirePerClicked;
                digButton.interactable = true;                
                if (coin.isHired)
                {
                    coinSlider.value = 0;
                    animationTime = 0;
                }
                UpdateCoinBalanceTexts(coin.coinBalance);
                CheckLockImageSprite();
                GameManager.Instance.GiveExp(hirePerClicked);
            }
            yield return null;
        }
    }
}
