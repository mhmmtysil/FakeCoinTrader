using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    private bool isSoundMuted = false;

    [SerializeField] Image soundOffIcon;
    [SerializeField] Image soundOnIcon;


    public void Start()
    {
        if (!PlayerPrefs.HasKey("SoundMuted"))
        {
            PlayerPrefs.SetInt("SoundMuted", 0);
            SoundStatusLoad();
        }
        else
        {
            SoundStatusLoad();
        }
        UpdateSoundButtonIcon();
        AudioListener.pause = isSoundMuted;
    }


    #region Sound
    public void OnSoundButtonPressed()
    {
        if (isSoundMuted == false)
        {
            isSoundMuted = true;
            AudioListener.pause = true;
        }
        else
        {
            isSoundMuted = false;
            AudioListener.pause = false;
        }
        SoundStatusSave();
        UpdateSoundButtonIcon();
    }
    private void SoundStatusLoad()
    {
        isSoundMuted = PlayerPrefs.GetInt("SoundMuted") == 1;
    }

    private void SoundStatusSave()
    {
        PlayerPrefs.SetInt("SoundMuted", isSoundMuted ? 1 : 0);
    }
    private void UpdateSoundButtonIcon()
    {
        if (isSoundMuted == false)
        {
            soundOnIcon.enabled = true;
            soundOffIcon.enabled = false;
        }
        else
        {
            soundOnIcon.enabled = false;
            soundOffIcon.enabled = true;
        }
    }
    #endregion
}
