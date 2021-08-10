using Firebase.Firestore;
[FirestoreData]
public class Message
{
    [FirestoreDocumentId]
    public string DocumentId { get; set; }
    [FirestoreProperty]
    public string UserNickname { get; set; }
    [FirestoreProperty]
    public string MessageText { get; set; }
    [FirestoreProperty]
    public string SendingDate { get; set; }
    [FirestoreProperty]
    public string UserMailID { get; set; }
}