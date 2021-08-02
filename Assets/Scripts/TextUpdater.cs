using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextUpdater : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    private int countFps = 30;
    private float duration = 1f;
    private int _value;
    private Coroutine coroutine;

    public int value
    {
        get
        {
            return _value;
        }
        set
        {
            UpdateText(value);
            _value = value;
        }
    }

    private void UpdateText(int newValue)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);            
        }
        coroutine = StartCoroutine(CountText(newValue));
    }
    private IEnumerator CountText(int newValue)
    {
        WaitForSeconds wait = new WaitForSeconds(1f / countFps);
        int previousValue = _value;
        int stepAmount;
        if (newValue- previousValue < 0)
        {
            stepAmount = Mathf.FloorToInt((newValue - previousValue) / (countFps * duration));
        }
        else 
        {
            stepAmount = Mathf.CeilToInt((newValue - previousValue) / (countFps * duration));
        }
        if (previousValue< newValue)
        {
            while (previousValue < newValue)
            {
                previousValue += stepAmount;
                if (previousValue > newValue)
                {
                    previousValue = newValue;
                }
                tmp.SetText(previousValue.ToString("#,##0").Replace(',', '.'));
                yield return wait;
            }
        }
        else
        {
            while (previousValue > newValue)
            {
                previousValue += stepAmount;
                if (previousValue < newValue)
                {
                    previousValue = newValue;
                }
                tmp.SetText(previousValue.ToString("#,##0").Replace(',', '.'));
                yield return wait;
            }

        }
    }
}
