using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LoadingScreen : MonoBehaviour
{
    public Slider loadingSlider;
    public TextMeshProUGUI tapToContinueText;


    private void OnEnable()
    {
        gameObject.GetComponent<Button>().interactable = false;
        tapToContinueText.gameObject.SetActive(false);
        StartCoroutine(LoadingScreenCR(5));
    }
    IEnumerator LoadingScreenCR(float seconds)
    {
        float animationTime = 0f;
        while (animationTime < seconds)
        {
            animationTime += Time.deltaTime;
            float lerpValue = animationTime / seconds;
            loadingSlider.value = Mathf.Lerp(0, 1f, lerpValue);
            if (loadingSlider.value >= 1)
            {
                loadingSlider.gameObject.SetActive(false);
                tapToContinueText.gameObject.SetActive(true);
                gameObject.GetComponent<Button>().interactable = true;
            }
            yield return null;
        }
    }
}
