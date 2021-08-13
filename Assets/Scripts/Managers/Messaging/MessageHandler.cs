using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageHandler : MonoBehaviour
{
    [SerializeField] private MessagePref _messageObjPrefab;
    [SerializeField] private MessagePref _messageMyObjPrefab;
    [SerializeField] private Scrollbar _scrollbar;
    public RectTransform _scrollContent;
    List<GameObject> messagesList = new List<GameObject>();


    private void Start()
    {
        if (GameManager.Instance.username != null)
        {
            MessageManager.Instance.NewMessageSended += OnNewMessageSended;
        }
    }
    private void OnDestroy()
    {
        MessageManager.Instance.NewMessageSended -= OnNewMessageSended;
    }

    private void Update()
    {
        _scrollContent.sizeDelta = new Vector2(_scrollContent.sizeDelta.x, 210 * messagesList.Count); 
        if (messagesList.Count>25)
        {
            Destroy(messagesList[0]);
            messagesList.RemoveAt(0);
        }
    }


    private void OnNewMessageSended(Message message)
    {
        if (message.UserMailID == GameManager.Instance.userMailID)
        {
            var go = Instantiate(_messageMyObjPrefab, transform);
            messagesList.Add(go.gameObject);
            MessagePref newMessage = go.GetComponent<MessagePref>();
            newMessage.SetMessage(message);
            Invoke(nameof(SetScrollBar), 0.3f);
        }
        else
        {
            var go = Instantiate(_messageObjPrefab, transform);
            messagesList.Add(go.gameObject);
            MessagePref newMessage = go.GetComponent<MessagePref>();
            newMessage.SetMessage(message);
            Invoke(nameof(SetScrollBar), 0.3f);
        }
       
    }
    private void SetScrollBar()
    {
        _scrollbar.value = 0;
    }
}
