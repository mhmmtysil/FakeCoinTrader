using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static SoundManager;

public class CoinDisplayer : MonoBehaviour
{
    public ScriptableCoin coin;
    [Header("Digging Panel")]
    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI coinPerMinuteText;
    public Button digButton;
    public Image lockedIcon;
    public Sprite grayLocked;
    public Sprite orangeLocked;
    public Button lockedButton;
    public Slider coinSlider;
    public Sprite canBeOpened;
    public Sprite cannotBeOpened;
    public Animator anim;

    [Header("Trade Panel")]
    public TextMeshProUGUI coinBalanceTradePanelText;
    public GameObject lockedToTrade;
    public GameObject notificationTrade;

    [Header("Hire Panel")]
    public GameObject hireButton;
    public GameObject hiredButton;
    public GameObject lockedHirePanel;

    [Header("Speed Panel")]
    public GameObject speedButton;
    public GameObject lockToSpeedUp;

    [Header("Next Coin")]
    public GameObject nextCoin;

    bool isproducing = false;

    private void FixedUpdate()
    {
        if (coin.isOpened)
        {
            if (nextCoin != null)
            {
                nextCoin.SetActive(true);
            }
            lockedButton.gameObject.SetActive(false);
            lockedHirePanel.SetActive(false);
            lockedToTrade.SetActive(false);
            lockToSpeedUp.SetActive(false);
        }
        else
        {
            lockedButton.gameObject.SetActive(true);
            lockedHirePanel.SetActive(true);
            lockedToTrade.SetActive(true);
            lockToSpeedUp.SetActive(true);

            if (coin.unlockPrice <= GameManager.Instance.Coin)
            {
                lockedButton.image.sprite = canBeOpened;
                lockedIcon.sprite = orangeLocked;
                lockedButton.interactable = true;
            }
            else
            {
                lockedButton.image.sprite = cannotBeOpened;
                lockedIcon.sprite = grayLocked;
                lockedButton.interactable = false;
            }
            if (nextCoin!=null)
            {
                nextCoin.SetActive(false);
            }
            
        }        
    }

    public void GetInfos()
    {
        coinSlider.value = 0;
        digButton.interactable = true;
        switch (coin.coinName)
        {
            case "FakeCoin":
                coin.coinBalance = GameManager.Instance.FakeCoin;
                coin.isHired = GameManager.Instance.FakeCoinHired;
                coin.isOpened = GameManager.Instance.FakeCoinOpened;
                coin.isSpeeded = GameManager.Instance.FakeCoinSpeeded;
                break;
            case "HorsePower":
                coin.coinBalance = GameManager.Instance.HorsePower;
                coin.isHired = GameManager.Instance.HorsePowerHired;
                coin.isOpened = GameManager.Instance.HorsePowerOpened;
                coin.isSpeeded = GameManager.Instance.HorsePowerSpeeded;
                break;
            case "LightCore":
                coin.coinBalance = GameManager.Instance.LightCore;
                coin.isHired = GameManager.Instance.LightCoreHired;
                coin.isOpened = GameManager.Instance.LightCoreOpened;
                coin.isSpeeded = GameManager.Instance.LightCoreSpeeded;
                break;
            case "OdeaCoin":
                coin.coinBalance = GameManager.Instance.OdeaCoin;
                coin.isHired = GameManager.Instance.OdeaCoinHired;
                coin.isOpened = GameManager.Instance.OdeaCoinOpened;
                coin.isSpeeded = GameManager.Instance.OdeaCoinSpeeded;
                break;
            case "InogamiCoin":
                coin.coinBalance = GameManager.Instance.InogamiCoin;
                coin.isHired = GameManager.Instance.InogamiCoinHired;
                coin.isOpened = GameManager.Instance.InogamiCoinOpened;
                coin.isSpeeded = GameManager.Instance.InogamiCoinSpeeded;
                break;
            case "GriffonCoin":
                coin.coinBalance = GameManager.Instance.GriffonCoin;
                coin.isHired = GameManager.Instance.GriffonCoinHired;
                coin.isOpened = GameManager.Instance.GriffonCoinOpened;
                coin.isSpeeded = GameManager.Instance.GriffonCoinSpeeded;
                break;
        }        
        UpdateCoinBalanceTexts(coin.coinBalance);
        CheckHireStatus();
        CheckSpeededStatus();
        CheckNotification();
        if (coin.isHired)
        {
            UpdateSliderValue();
        }
    }

    public void SetDefault()
    {
        coin.coinBalance = 0;
        coin.diggingSpeed = coin.diggingSpeedDefault;
        coin.hirePerClicked = coin.hirePerClickedDefault;
        coin.isHired = false;
    }
        
    void CheckNotification()
    {
        if (coin.coinBalance >= coin.tradeBalance)
        {
            notificationTrade.SetActive(true);
            GameManager.Instance.TradeableNotification();
        }
        else
        {
            notificationTrade.SetActive(false);
            GameManager.Instance.TradeableNotificationReceived();
        }
    }

    public void UnlockCoin()
    {
        GameManager.Instance.BuyWithCoin(coin.unlockPrice);
        GameManager.Instance.SetCoinUnlock(coin.coinName);
        coin.isOpened = true;  
        if (nextCoin!=null)
        {
            nextCoin.SetActive(true);
        }
        CheckLockStatus();
    }

    public void CoinsDiggingSpeedDoubler()
    {
        if (coin.speedUpPrice <= GameManager.Instance.Emerald)
        {
            coin.diggingSpeed /= 2;
            coin.isSpeeded = true;
            GameManager.Instance.SetCoinSpeeded(coin.coinName);
            CheckSpeededStatus();
        }
    }

    public void CheckSpeededStatus()
    {
        if (coin.isSpeeded)
        {
            speedButton.SetActive(false);
        }
        else
        {
            speedButton.SetActive(true);
        }
    }

    public void CheckHireStatus()
    {
        if (coin.isHired)
        {
            hiredButton.SetActive(true);
            hireButton.SetActive(false);
            digButton.gameObject.SetActive(false);
        }
        else
        {
            hiredButton.SetActive(false);
            hireButton.SetActive(true);
            digButton.gameObject.SetActive(true);
        }
    }
    public void Hire(int price)
    {
        if (price <= GameManager.Instance.Emerald)
        {
            GameManager.Instance.Emerald -= price;
            GameManager.Instance.UpdateEmerald();
            GameManager.Instance.SetCoinHired(coin.coinName);
            coin.isHired = true;
            CheckHireStatus();
            HiredUpdate();
        }
        else
        {
            hireButton.GetComponent<Animator>().SetTrigger("notEnough");
        }
    }

    public void CheckLockStatus()
    {
        if (coin.isOpened)
        {
            if (nextCoin!=null)
            {
                nextCoin.SetActive(true);
            }
            lockedHirePanel.SetActive(false);
            lockedButton.gameObject.SetActive(false);
            lockedToTrade.SetActive(false);
        }
        else
        {
            if (nextCoin != null)
            {
                nextCoin.SetActive(false);
            }
            lockedHirePanel.SetActive(true);
            lockedButton.gameObject.SetActive(true);
            lockedToTrade.SetActive(true);
        }
    }

    public void UpdateCoinBalanceTexts(int _coinBalance)
    {
        coinBalanceText.text = coin.coinBalance.ToString();
        coinBalanceTradePanelText.text = "Bakiye : " + _coinBalance;

        if (!coin.isHired)
        {
            TimeSpan result = TimeSpan.FromSeconds(coin.diggingSpeed);
            string fromTimeString = result.ToString("mm':'ss");
            coinPerMinuteText.text = fromTimeString;
        }
        else
        {
            coinPerMinuteText.text = 60 / coin.diggingSpeed * coin.hirePerClicked + "/dakika.";
        }

        GameManager.Instance.UpdateCoinsText(coin.coinName, coin.coinBalance);
    }
    public void GiveCoin()
    {
        coin.coinBalance++;
        GameManager.Instance.UpdateCoinsText(coin.coinName, coin.coinBalance);

    }

    public void BuyCoinWithGold(int price)
    {
        if (GameManager.Instance.Coin >= price)
        {
            GameManager.Instance.Coin -= price;
            coin.coinBalance += 250;
            UpdateCoinBalanceTexts(coin.coinBalance);
            GameManager.Instance.UpdateCoinTexts();
        }

    }

    public void TradeWithGold(int giveCoin)
    {
        if (coin.coinBalance >= coin.tradeBalance)
        {
            coin.coinBalance -= coin.tradeBalance;
            UpdateCoinBalanceTexts(coin.coinBalance);
            GameManager.Instance.GiveCoin(giveCoin);
            GameManager.Instance.GiveExp(giveCoin);
            notificationTrade.SetActive(false);
        }
        else
        {
            coinBalanceTradePanelText.GetComponent<Animator>().SetTrigger("notEnough");
        }
    }

    public void TradeWithEmerald(int giveEmerald)
    {
        if (coin.coinBalance >= coin.tradeBalance)
        {
            coin.coinBalance -= coin.tradeBalance;
            UpdateCoinBalanceTexts(coin.coinBalance);
            GameManager.Instance.GiveEmerald(giveEmerald);
            GameManager.Instance.GiveExp(giveEmerald);
            notificationTrade.SetActive(false);
        }
        else
        {
            coinBalanceTradePanelText.GetComponent<Animator>().SetTrigger("notEnough");
        }
    }

    void HiredUpdate()
    {
        if (coin.isHired)
        {
            UpdateSliderValue();
        }
    }
    public void UpdateSliderValue()
    {
        StartCoroutine(StartDigging());
    }

    IEnumerator StopDigging()
    {
        isproducing = false;
        StopCoroutine(StartDigging());
        coinSlider.value = 0;
        anim.SetTrigger("nonproducing");
        yield return new WaitForSeconds(0.1f);
        coin.coinBalance += coin.hirePerClicked;
        digButton.interactable = true;
        UpdateCoinBalanceTexts(coin.coinBalance);
        GameManager.Instance.GiveExp(coin.hirePerClicked);
        Instance.OnSoundActivated(SoundType.CoinProduceFinished);
        HiredUpdate();    
        CheckNotification();
    }

    IEnumerator StartDigging()
    {
        isproducing = true;
        float diggingSpeed = coin.diggingSpeed;
        float animationTime = 0f;
        float second = coin.diggingSpeed;
        anim.SetTrigger("producing");
        while (animationTime < diggingSpeed && isproducing)
        {

            animationTime += Time.deltaTime;
            second -= Time.deltaTime;
            float lerpValue = animationTime / diggingSpeed;
            coinSlider.value = Mathf.Lerp(0f, 1f, lerpValue);

            TimeSpan result = TimeSpan.FromSeconds(second);
            string fromTimeString = result.ToString("mm':'ss");
            coinPerMinuteText.text = fromTimeString;

            digButton.interactable = false;

            if (coinSlider.value == coinSlider.maxValue)
            {
                StartCoroutine(StopDigging());
            }
            yield return null;
        }
    }
}
