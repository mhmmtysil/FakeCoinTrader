using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MailSystem : MonoBehaviour
{

    //Oyunumuzu yüklediðin için sana bir hoþgeldin hediyesi vermek istedim. Umarým geliþtirme süresince bizlerle birlikte kalmaya devam edersin. Seviliyorsun. :)
    public string mailSender;
    public string mailInfo;
    public MailStatus mailStatus;
    public int rewardPiece;
    public int mailNumber;

    public GameObject mailInfoBg;
    public GameObject mailPrefab;
    public GameObject mailHolder;
    public GameObject mailNotification;
    public GameObject mainNotification;
    public TextMeshProUGUI senderText;
    public TextMeshProUGUI infoText;
    public Image mailRewardType;
    public TextMeshProUGUI mailRewardPieceText;
    public Sprite emeraldSprite;
    public Sprite goldSprite;
    public Button collectButton;
    private GameObject instantiatedMail;

    int unreadedMailCount;
    public int UnreadedMailCount
    {
        get { return unreadedMailCount; }
        set { unreadedMailCount = value; }
    }
    int readedMailCount;
    public int ReadedMailCount
    {
        get { return readedMailCount; }
        set { readedMailCount = value; }
    }

    public void NotificationStatus()
    {
        if (unreadedMailCount>0)
        {
            var rect = mailHolder.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(240, 150 * unreadedMailCount);
            mainNotification.SetActive(true);
            mailNotification.SetActive(true);
        }
        else
        {
            mainNotification.SetActive(false);
            mailNotification.SetActive(false);
        }
    }

    public void SendGoldMail(int goldPiece)
    {
        unreadedMailCount++;
        GameObject mailPref = Instantiate(mailPrefab, mailHolder.transform);
        mailPref.transform.Find("mailFromSender").GetComponentInChildren<TextMeshProUGUI>().text = mailSender;
        mailPref.GetComponent<Button>().onClick.AddListener(delegate { DetailedMailOpen(mailSender, mailInfo, goldSprite, goldPiece, RewardTypes.gold); });
        instantiatedMail = mailPref;
        NotificationStatus();

    }
    public void SendEmeraldMail(int emeraldPiece)
    {
        unreadedMailCount++;
        GameObject mailPref = Instantiate(mailPrefab, mailHolder.transform);
        mailPref.transform.Find("mailFromSender").GetComponentInChildren<TextMeshProUGUI>().text = mailSender;
        mailPref.GetComponent<Button>().onClick.AddListener(delegate { DetailedMailOpen(mailSender, mailInfo,emeraldSprite,emeraldPiece,RewardTypes.emerald) ; });
        instantiatedMail = mailPref;
        NotificationStatus();

    }
    public void DetailedMailOpen(string sender, string info, Sprite mailRewardtype, int rewardPeice, RewardTypes r)
    {
        unreadedMailCount--;
        mailInfoBg.SetActive(true);
        senderText.text = sender;
        infoText.text = info;
        mailStatus = MailStatus.readed;
        rewardPiece = rewardPeice;
        mailRewardType.sprite = mailRewardtype;
        mailRewardPieceText.text = rewardPeice.ToString();
        collectButton.onClick.AddListener(delegate { GetRewardPiece(rewardPeice, r,mailNumber); });
        NotificationStatus();
    }
    public void GetRewardPiece(int piece, RewardTypes rewardType, int mailNumber)
    {
        if (rewardType == RewardTypes.gold)
        {
            GameManager.Instance.GiveCoin(piece);            
        }
        else if (rewardType == RewardTypes.emerald)
        {
            GameManager.Instance.GiveEmerald(piece);
        }
        mailInfoBg.SetActive(false);
        NotificationStatus();
        GameManager.Instance.DeleteMail(mailNumber);
    }
}
public enum RewardTypes
{
    gold,
    emerald
}

public enum MailStatus
{
    unReaded,
    readed,
    rewarded
}

