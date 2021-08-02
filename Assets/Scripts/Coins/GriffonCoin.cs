using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GriffonCoin : MonoBehaviour
{
    public ScriptableCoin coin;
    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI coinBalanceTradePanelText;
    public TextMeshProUGUI coinPerMinuteText;
    public TextMeshProUGUI hiredText;

    public Button digButton;
    public Button hirePanelHireButton;
    public GameObject hirePanelHiredButton;
    public Button lockedButton;
    public GameObject lockedHireImage;

    public Slider coinSlider;

    public Sprite canBeOpened;
    public Sprite cannotBeOpened;



    private void OnEnable()
    {
        StartCoroutine(GetInfos());
    }
    IEnumerator GetInfos()
    {
        yield return new WaitForSeconds(2);
        coin.coinBalance = GameManager.Instance._griffonCoin;
        coin.isHired = GameManager.Instance.griffonCoinHired;
        coin.isOpened = GameManager.Instance.griffonCoinOpened;
        UpdateCoinBalanceTexts(coin.coinBalance);
        CheckHireStatus();
        CheckLockStatus();

    }
    public void CoinsDiggingSpeedDoubler()
    {
        coin.diggingSpeed /= 2;
        coinSlider.value = 1;
    }

    public void CheckLockStatus()
    {
        if (coin.isOpened)
        {
            lockedButton.gameObject.SetActive(false);
            lockedHireImage.SetActive(false);
        }
        else
        {
            lockedButton.gameObject.SetActive(true);
            lockedHireImage.SetActive(true);
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
        if (price <= GameManager.Instance._emerald)
        {
            GameManager.Instance._emerald -= price;
            GameManager.Instance.UpdateEmerald();
            GameManager.Instance.SetCoinHired(coin.coinName, true);
            coin.isHired = true;
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
        coinPerMinuteText.text = 60 / coin.diggingSpeed * coin.hirePerClicked + "/dakika.";
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
    public void UpdateSliderValue()
    {
        StartCoroutine(AnimateSliderOverTime(coin.diggingSpeed));
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
                coin.coinBalance += coin.hirePerClicked;
                digButton.interactable = true;
                if (coin.isHired)
                {
                    coinSlider.value = 0;
                    animationTime = 0;
                }
                UpdateCoinBalanceTexts(coin.coinBalance);
                GameManager.Instance.GiveExp(coin.hirePerClicked);
            }
            yield return null;
        }
    }
}
