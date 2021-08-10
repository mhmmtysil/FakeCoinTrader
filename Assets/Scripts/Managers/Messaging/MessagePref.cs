using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MessagePref : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtSenderName;
    [SerializeField] private TextMeshProUGUI _txtMessage;
    [SerializeField] private TextMeshProUGUI _txtTime;

    public void SetMessage(Message message)
    {
        _txtSenderName.text = message.UserNickname;
        _txtMessage.text = message.MessageText;
        _txtTime.text = message.SendingDate;
    }
}
