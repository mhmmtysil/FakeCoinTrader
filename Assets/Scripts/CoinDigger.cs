using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CoinDigger : MonoBehaviour
{
    public string coinName;
    public string nextCoinName;
    public Slider coinSlider;
    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI coinBalanceTradePanelText;
    public TextMeshProUGUI coinPerMinuteText;
    public int hirePerClicked;
    public Button digButton;
    public Button hireButton;
    public Button hirePanelHireButton;
    public GameObject hirePanelHiredButton;
    public TextMeshProUGUI hiredText;
    public Button nextLocked;
    public int nextLockOpenPrice;
    public Sprite canBeOpened;
    public Sprite cannotBeOpened;
    bool hired = false;
    int coinBalance = 0;
    public float diggingSpeed;

    private void OnEnable()
    {
        GetInfos();
        UpdateCoinBalanceTexts(coinBalance);
        CheckHireStatus();
        CheckLockStatus();
        UpdatePerMinuteText();
    }
    private void GetInfos()
    {

    }

    public void CoinsDiggingSpeedDoubler()
    {
        diggingSpeed /= 2;
        PlayerPrefs.SetFloat(coinName + "DiggingSpeed",diggingSpeed);
        UpdatePerMinuteText();
        coinSlider.value = 1;
    }
    private void CheckHireStatus()
    {
        if (PlayerPrefs.GetInt(coinName + "Hired") == 1)
        {
            hired = true;
            coinSlider.value = 0;
            UpdateSliderValue();
            hirePanelHireButton.gameObject.SetActive(false);
            hirePanelHiredButton.SetActive(true);
            hiredText.gameObject.SetActive(true);
            digButton.gameObject.SetActive(false);
            hireButton.gameObject.SetActive(false);
        }
        else
        {
            hired = false;
            hirePanelHireButton.gameObject.SetActive(true);
            hirePanelHiredButton.SetActive(false);
            hiredText.gameObject.SetActive(false);
            digButton.gameObject.SetActive(true);
            hireButton.gameObject.SetActive(true);
        }
    }
    public void Hire(int price)
    {
        int emerald = GameManager.Instance.Emerald;
        Debug.Log(emerald);
        if (price<= emerald)
        {
            emerald -= price;
            PlayerPrefs.SetInt(coinName + "Hired", 1);
            GameManager.Instance.UpdateEmerald();
        }
        else
        {
            hirePanelHireButton.GetComponent<Animator>().SetTrigger("notEnough");
        }
        CheckHireStatus();
    }
    public void OpenNextCoin(int price)
    {        
        if (price<= coinBalance)
        {
            coinBalance -= price;
            UpdateCoinBalanceTexts(coinBalance);
            PlayerPrefs.SetInt(nextCoinName + "Opened", 1);
            CheckLockStatus();
        }
    }

    public void CheckLockStatus() 
    {
        if (PlayerPrefs.HasKey(nextCoinName + "Opened"))
        {
            nextLocked.gameObject.SetActive(false);
        }
        else
        {
            if (nextLocked!=null)
            {
                nextLocked.gameObject.SetActive(true);
            }
            
        }
        if (nextLockOpenPrice<=coinBalance && nextLocked!= null)
        {
            nextLocked.image.sprite = canBeOpened;
            nextLocked.interactable = true;
        }
        else
        {
            if (nextLocked!=null)
            {
                nextLocked.image.sprite = cannotBeOpened;
                nextLocked.interactable = false;
            }
            
        }
    }
    public void UpdateSliderValue()
    {
        StartCoroutine(AnimateSliderOverTime(diggingSpeed));
    }
    public void UpdateCoinBalanceTexts(int _coinBalance)
    {
        coinBalanceTradePanelText.text = "Bakiye : " + _coinBalance;
        PlayerPrefs.SetInt(coinName, coinBalance);
    }
    public void UpdatePerMinuteText()
    {
        coinPerMinuteText.text = (60 / diggingSpeed) * hirePerClicked + "/dakika.";
    }

    public void BuyCoinWithGold(int price)
    {
        if (GameManager.Instance.Coin >= price)
        {
            GameManager.Instance.Coin -= price;
            coinBalance += price;
            UpdateCoinBalanceTexts(coinBalance);
            GameManager.Instance.UpdateCoinTexts();
            CheckLockStatus();
        }
        
    }

    public void TradeWithGold(int amount)
    {
        if (coinBalance>=amount)
        {
            coinBalance -= amount;
            UpdateCoinBalanceTexts(coinBalance);
            GameManager.Instance.GiveCoin(100);
            GameManager.Instance.GiveExp(100);
        }
    }
    public void TradeWithEmerald(int amount)
    {
        if (coinBalance >= amount)
        {
            coinBalance -= amount;
            UpdateCoinBalanceTexts(coinBalance);
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
                coinBalance += hirePerClicked;
                digButton.interactable = true;               
                CheckLockStatus();
                if (hired)
                {
                    coinSlider.value = 0;
                    animationTime = 0;
                }
                UpdateCoinBalanceTexts(coinBalance);
                yield return new WaitForSeconds(1);
                GameManager.Instance.GiveExp(hirePerClicked);
            }
            yield return null;
        }
    }
}
