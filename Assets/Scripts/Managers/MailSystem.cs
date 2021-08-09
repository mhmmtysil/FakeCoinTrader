using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MailSystem : MonoBehaviour
{

    //Oyunumuzu yüklediðin için sana bir hoþgeldin hediyesi vermek istedim. Umarým geliþtirme süresince bizlerle birlikte kalmaya devam edersin. Seviliyorsun. :)
    public ScriptableMail mail;
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
    public int mailNumber;

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
    public void SendEmeraldMail(int emeraldPiece)
    {
        unreadedMailCount++;
        GameObject mailPref = Instantiate(mailPrefab, mailHolder.transform);
        mailPref.transform.Find("mailFromSender").GetComponentInChildren<TextMeshProUGUI>().text = mail.mailSender;
        mailPref.GetComponent<Button>().onClick.AddListener(delegate { DetailedMailOpen(mail.mailSender, mail.mailInfo,emeraldSprite,emeraldPiece,RewardTypes.emerald) ; });
        instantiatedMail = mailPref;
        NotificationStatus();

    }
    public void DetailedMailOpen(string sender, string info, Sprite mailRewardtype, int rewardPeice, RewardTypes r)
    {
        unreadedMailCount--;
        mailInfoBg.SetActive(true);
        senderText.text = sender;
        infoText.text = info;
        mail.mailStatus = MailStatus.readed;
        mail.rewardPiece = rewardPeice;
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

        instantiatedMail.GetComponent<Mail>().mailStatus = MailStatus.rewarded;
        mailInfoBg.SetActive(false);
        NotificationStatus();
        DeleteMail(mailNumber);
    }

    public void DeleteMail(int mailNumber)
    {
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

