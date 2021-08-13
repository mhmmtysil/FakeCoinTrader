using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadingScreenManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] Slider slider;

    private void OnEnable()
    {
        slider.value = 0;
        StartCoroutine(Loading(4));
    }
    IEnumerator Loading(float second)
    {
        slider.gameObject.SetActive(true);
        loadingText.text = "Yükleniyor.";
        float animationTime = 0f;
        while (animationTime < second)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / second;
            slider.value = Mathf.Lerp(0f, 1f, lerpValue);

            gameObject.GetComponent<Button>().interactable = false;

            if (slider.value == slider.maxValue)
            {
                gameObject.GetComponent<Button>().interactable = true;
                loadingText.text = "Baþlamak için Dokunun";
                slider.gameObject.SetActive(false);
            }
            yield return null;
        }
    }
}
