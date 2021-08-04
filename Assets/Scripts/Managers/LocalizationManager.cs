using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public Language[] languages;
    [Header("TextMeshPRO's")]
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI usernameInputText;
    public TextMeshProUGUI saveUserNameText;
    public TextMeshProUGUI usernameWarningText;


    private void Start()
    {
        if (PlayerPrefs.HasKey("lang"))
        {
            int index = PlayerPrefs.GetInt("lang");
            CurrentLanguage(index);
        }
        else
        {
            PlayerPrefs.SetInt("lang", 0);
            CurrentLanguage(0);
        }
    }
    public void CurrentLanguage(int i)
    {
        usernameText.text = languages[i].username;
        usernameInputText.text = languages[i].usernameInput;
        saveUserNameText.text = languages[i].saveUserName;
        usernameWarningText.text = languages[i].usernameWarning;

        PlayerPrefs.SetInt("lang", i);
    }
}

