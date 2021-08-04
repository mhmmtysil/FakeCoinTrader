using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private Sprite changedSprite;
    private Sprite defaultSprite;
    private bool statu;
    private void Awake()
    {
        defaultSprite = GetComponent<Image>().sprite;        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        statu = !statu;
        if (statu)
            GetComponent<Image>().sprite = defaultSprite;
        else
            GetComponent<Image>().sprite = changedSprite;

        PlayerPrefs.SetInt("Sound", statu ? 1 : 0);
        SoundManager.Instance.OnSoundStatuChanged(statu);
    }
}