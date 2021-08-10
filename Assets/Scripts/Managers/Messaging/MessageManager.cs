using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using TMPro;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;
    public delegate void OnNewMessageSended(Message message);
    public event OnNewMessageSended NewMessageSended;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }


    void Start()
    {
        StartListening();
    }
    void StartListening()
    {
        FirebaseFirestore.DefaultInstance.Collection("Messages").OrderBy("SendingDate").LimitToLast(10).Listen(task =>
        {
            foreach (DocumentChange change in task.GetChanges(MetadataChanges.Exclude))
            {
                if (change.ChangeType == DocumentChange.Type.Added)
                {
                    Message message = change.Document.ConvertTo<Message>();
                    try
                    {
                        NewMessageSended?.Invoke(message);
                    }
                    catch
                    {

                    }
                }
            }
        });
    }
    public void SendMessage(TMP_InputField textField)
    {
        string userID = GameManager.Instance.userMailID;
        Message msg = new Message() { UserNickname = GameManager.Instance.username, MessageText = textField.text, SendingDate = Timestamp.GetCurrentTimestamp().ToDateTime().ToString(), UserMailID = userID };
        FirebaseFirestore.DefaultInstance.Collection("Messages").AddAsync(msg);
    }


}
