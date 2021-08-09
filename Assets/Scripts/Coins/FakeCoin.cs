using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using static SoundManager;

public class FakeCoin : MonoBehaviour
{
    public ScriptableCoin coin;
    
    [Header("Digging Panel")]
    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI coinPerMinuteText;
    public GameObject hiredText;
    public Button digButton;
    public Button lockedButton;
    public Slider coinSlider;
    public Animator anim;

    [Header("Hire Panel")]
    public TextMeshProUGUI coinBalanceTradePanelText;
    public GameObject hireButton;
    public GameObject hiredButton;


    public void GetInfos()
    {
        coin.coinBalance = GameManager.Instance.FakeCoin;
        coin.isHired = GameManager.Instance.fakeCoinHired;
        coin.isOpened = GameManager.Instance.fakeCoinOpened;
        UpdateCoinBalanceTexts(coin.coinBalance);
        CheckHireStatus();
        if (coin.isHired)
        {
            StartCoroutine(StartDigging(coin.diggingSpeed));
        }
    }

    public void SetDefault()
    {
        coin.coinBalance = 0;
        coin.diggingSpeed = coin.diggingSpeedDefault;
        coin.hirePerClicked = coin.hirePerClickedDefault;
        coin.isHired = false;
    }

    public void CoinsDiggingSpeedDoubler(int price)
    {
        if (price <= GameManager.Instance.Emerald)
        {
            if (coin.diggingSpeed / 2 > 0)
            {
                coin.diggingSpeed /= 2;
            }
        }
    }

    public void CheckHireStatus()
    {
        if (coin.isHired)
        {
            hireButton.SetActive(false);
            hiredButton.SetActive(true);
            hiredText.SetActive(true);
            digButton.gameObject.SetActive(false);
        }
        else
        {
            hireButton.SetActive(true);
            hiredButton.SetActive(false);
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
            hireButton.GetComponent<Animator>().SetTrigger("notEnough");
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
            }

            yield return null;
        }
    }
}