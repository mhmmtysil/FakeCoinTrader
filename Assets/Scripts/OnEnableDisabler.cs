using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnableDisabler : MonoBehaviour
{
    public float time;
    void OnEnable()
    {
        StartCoroutine(StartToDisable());
    }

    IEnumerator StartToDisable()
    {
        yield return new WaitForSeconds(time);
        this.gameObject.SetActive(false);
    }
}
