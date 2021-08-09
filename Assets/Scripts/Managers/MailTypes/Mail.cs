using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mail : MonoBehaviour
{
    public MailStatus mailStatus;

    private void Update()
    {
        if (mailStatus == MailStatus.rewarded)
        {
            Destroy(gameObject);
        }
    }
}
