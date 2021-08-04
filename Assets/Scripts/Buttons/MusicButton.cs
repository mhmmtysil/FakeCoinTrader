using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Sprite changedSprite;
    private Sprite defaultSprite;
    private bool statu;
    private void Awake()
    {
        defaultSprite = GetComponent<Image>().sprite;
        if (PlayerPrefs.HasKey("Music"))
        {
            statu = PlayerPrefs.GetInt("Music") == 1 ? true : false;
            if (statu)
            {
                GetComponent<Image>().sprite = defaultSprite;
                Debug.Log("Müzik Açýk");
            }
            else
            {
                GetComponent<Image>().sprite = changedSprite;
                Debug.Log("Müzik Kapalý");
            }
        }
        else
        {
            PlayerPrefs.SetInt("Music", 1);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        statu = !statu;
        if (statu)
            GetComponent<Image>().sprite = defaultSprite;
        else
            GetComponent<Image>().sprite = changedSprite;

        PlayerPrefs.SetInt("Music", statu ? 1 : 0);
    }
}
