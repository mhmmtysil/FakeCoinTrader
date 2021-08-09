using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailDestroyer : MonoBehaviour
{
    private readonly MailStatus ms;
    private void Update()
    {
        if (ms == MailStatus.rewarded)
        {
            Destroy(gameObject);
        }
    }
}
