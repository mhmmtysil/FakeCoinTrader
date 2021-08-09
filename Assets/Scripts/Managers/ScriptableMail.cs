using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Mail", menuName = "Mail/New Mail")]

public class ScriptableMail : ScriptableObject
{
    [Header("Mail Fields")]
    public string mailSender;
    public string mailInfo;
    public MailStatus mailStatus;
    public int rewardPiece;
}
