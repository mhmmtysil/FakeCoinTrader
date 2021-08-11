using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static SoundManager;


public class LightCore : MonoBehaviour
{
    public ScriptableCoin coin;

    [Header("Digging Panel")]
    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI coinPerMinuteText;
    public GameObject hiredText;
    public Button digButton;
    public Image lockedIcon;
    public Sprite grayLocked;
    public Sprite orangeLocked;
    public Button lockedButton;
    public Slider coinSlider;
    public Sprite canBeOpened;
    public Sprite cannotBeOpened;
    public Animator anim;

    [Header("Hire Panel")]
    public TextMeshProUGUI coinBalanceTradePanelText;
    public Button hirePanelHireButton;
    public GameObject hirePanelHiredButton;
    public GameObject lockedImageHirePanel;
    public GameObject lockedHireImage;

    [Header("Speed Panel")]
    public GameObject speedButton;

    public GameObject nextCoin;


    private void FixedUpdate()
    {        
        if (coin.isOpened)
        {
            nextCoin.SetActive(true);
            lockedImageHirePanel.SetActive(false);
        }
        else
        {
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
            nextCoin.SetActive(false);
            lockedImageHirePanel.SetActive(true);
        }
    }


    public void UnlockCoin(int price)
    {
        coin.isOpened = true;
        GameManager.Instance.BuyWithCoin(price);
        GameManager.Instance.SetCoinUnlock(coin.coinName);
        nextCoin.SetActive(true);
        lockedImageHirePanel.SetActive(false);
        lockedButton.gameObject.SetActive(false);
    }
    public void SetDefault()
    {
        coin.coinBalance = 0;
        coin.diggingSpeed = coin.diggingSpeedDefault;
        coin.hirePerClicked = coin.hirePerClickedDefault;
        coin.isHired = false;
        coin.isOpened = false;
    }

    public void GetInfos()
    {
        coin.coinBalance = GameManager.Instance.LightCore;
        coin.isHired = GameManager.Instance.LightCoreHired;
        coin.isOpened = GameManager.Instance.LightCoreOpened;
        coin.isSpeeded = GameManager.Instance.LightCoreSpeeded;
        UpdateCoinBalanceTexts(coin.coinBalance);
        CheckHireStatus();
        CheckLockStatus();
        CheckSpeededStatus();
        if (coin.isHired)
        {
            UpdateSliderValue();
        }
    }

    public void CoinsDiggingSpeedDoubler(int price)
    {
        if (price <= GameManager.Instance.Emerald)
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

    public void CheckHireStatus()
    {
        if (coin.isHired)
        {
            hirePanelHireButton.gameObject.SetActive(false);
            hirePanelHiredButton.SetActive(true);
            hiredText.SetActive(true);
            digButton.gameObject.SetActive(false);
        }
        else
        {
            hirePanelHireButton.gameObject.SetActive(true);
            hirePanelHiredButton.SetActive(false);
            hiredText.SetActive(false);
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
            hirePanelHireButton.GetComponent<Animator>().SetTrigger("notEnough");
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
            UpdateSliderValue();
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

    public void TradeWithGold(int amount)
    {
        if (coin.coinBalance >= amount)
        {
            coin.coinBalance -= amount;
            UpdateCoinBalanceTexts(coin.coinBalance);
            GameManager.Instance.GiveCoin(300);
            GameManager.Instance.GiveExp(300);
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
    void HiredUpdate()
    {
        if (coin.isHired)
        {
            UpdateSliderValue();
        }
    }
    public void UpdateSliderValue()
    {
        StartCoroutine(StartDigging(coin.diggingSpeed));
    }

    IEnumerator StartDigging(float diggingspeed)
    {
        float animationTime = 0f;
        float second = coin.diggingSpeed;
        anim.SetTrigger("producing");
        while (animationTime < diggingspeed)
        {
            animationTime += Time.deltaTime;
            second -= Time.deltaTime;
            float lerpValue = animationTime / diggingspeed;
            coinSlider.value = Mathf.Lerp(0f, 1f, lerpValue);

            TimeSpan result = TimeSpan.FromSeconds(second);
            string fromTimeString = result.ToString("mm':'ss");
            coinPerMinuteText.text = fromTimeString;

            digButton.interactable = false;

            if (coinSlider.value == 1)
            {                
                coinSlider.value = 0;
                anim.SetTrigger("nonproducing");
                coin.coinBalance += coin.hirePerClicked;
                digButton.interactable = true;
                UpdateCoinBalanceTexts(coin.coinBalance);
                GameManager.Instance.GiveExp(coin.hirePerClicked);
                Instance.OnSoundActivated(SoundType.CoinProduceFinished);
                HiredUpdate();
                yield break;
            }            
        }
    }
}