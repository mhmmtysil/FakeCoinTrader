using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OdeaCoin : MonoBehaviour
{
    public ScriptableCoin coin;
    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI coinBalanceTradePanelText;
    public TextMeshProUGUI coinPerMinuteText;
    public TextMeshProUGUI hiredText;

    public Button digButton;
    public Button hirePanelHireButton;
    public GameObject hirePanelHiredButton;
    public GameObject lockedImageHirePanel;
    public Button lockedButton;
    public Image lockedIcon;
    public Sprite grayLocked;
    public Sprite orangeLocked;
    public GameObject lockedHireImage;

    public Slider coinSlider;

    public Sprite canBeOpened;
    public Sprite cannotBeOpened;

    public Animator anim;
    int second;

    public GameObject nextCoin;


    private void FixedUpdate()
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
    }


    private void OnEnable()
    {
        StartCoroutine(GetInfos());
    }



    IEnumerator GetInfos()
    {
        second = (int)coin.diggingSpeed;
        yield return new WaitForSeconds(1);
        coin.coinBalance = GameManager.Instance.OdeaCoin;
        coin.isHired = GameManager.Instance.odeaCoinHired;
        coin.isOpened = GameManager.Instance.odeaCoinOpened;
        UpdateCoinBalanceTexts(coin.coinBalance);
        CheckHireStatus();
        CheckLockStatus();
        if (coin.isHired)
        {
            UpdateSliderValue();
        }
    }

    public void CoinsDiggingSpeedDoubler()
    {
        coin.diggingSpeed /= 2;
        second = (int)coin.diggingSpeed;
        TimeSpan result = TimeSpan.FromSeconds(second);
        string fromTimeString = result.ToString("mm':'ss");
        coinPerMinuteText.text = fromTimeString;
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
            hiredText.gameObject.SetActive(true);
            digButton.gameObject.SetActive(false);
        }
        else
        {
            hirePanelHireButton.gameObject.SetActive(true);
            hirePanelHiredButton.SetActive(false);
            hiredText.gameObject.SetActive(false);
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
        }
        else
        {
            hirePanelHireButton.GetComponent<Animator>().SetTrigger("notEnough");
        }
    }
    public void OpenCoin(int price)
    {
        if (price <= GameManager.Instance.Coin)
        {
            GameManager.Instance.BuyWithCoin(price);
            lockedButton.gameObject.SetActive(false);
            GameManager.Instance.SetCoinUnlock(coin.coinName);
        }
    }

    public void UpdateCoinBalanceTexts(int _coinBalance)
    {
        coinBalanceText.text = coin.coinBalance.ToString();
        coinBalanceTradePanelText.text = "Bakiye : " + _coinBalance;

        if (!coin.isHired)
        {
            second = (int)coin.diggingSpeed;
            TimeSpan result = TimeSpan.FromSeconds(second);
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
    public void UpdateSliderValue()
    {
        StartCoroutine(AnimateSliderOverTime());
    }
    IEnumerator AnimateSliderOverTime()
    {
        anim.enabled = true;
        anim.SetTrigger("producing");
        float animationTime = 0f;
        float countDownTo = coin.diggingSpeed;
        while (animationTime < coin.diggingSpeed)
        {
            animationTime += Time.deltaTime;
            countDownTo -= Time.deltaTime;
            int second = Mathf.RoundToInt(countDownTo);
            if (countDownTo > 0)
            {
                TimeSpan result = TimeSpan.FromSeconds(second);
                string fromTimeString = result.ToString("mm':'ss");
                coinPerMinuteText.text = fromTimeString;
            }
            float lerpValue = animationTime / coin.diggingSpeed;
            coinSlider.value = Mathf.Lerp(0, 1f, lerpValue);
            digButton.interactable = false;
            if (coinSlider.value >= 1)
            {
                anim.SetTrigger("nonproducing");
                coinSlider.value = 0;
                coin.coinBalance += coin.hirePerClicked;
                digButton.interactable = true;
                if (coin.isHired)
                {
                    coinSlider.value = 0;
                    animationTime = 0;
                    anim.SetTrigger("producing");
                }
                else
                {
                    anim.SetTrigger("nonproducing");
                }
                UpdateCoinBalanceTexts(coin.coinBalance);
                GameManager.Instance.GiveExp(coin.hirePerClicked);
                SoundManager.Instance.OnSoundActivated(SoundType.CoinProduceFinished);
            }
            yield return null;
        }
    }
}