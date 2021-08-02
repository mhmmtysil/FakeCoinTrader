using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public GameObject newUserPanel;
    public GameObject mainMenuPanel;
    public GameObject loadingScreen;

    public GameObject signInPanel;
    public GameObject signUpPanel;
    public GameObject setUsernamePanel;

    //public UnityEvent UserSignUp; //Sadece Kayýt Yaptýysa Bunu Invoke Et

    //public UnityEvent UserSignedUp; // Kayýttan Sonra Kullanýcý Adý da Seçtiyse Bunu Invoke Et

    //public UnityEvent UserSignIn; // Sadece Emaille Giriþ Yaptýysa Invoke Et
    //public UnityEvent UserSignedIn; // Emaille Giriþten Sonra Kullanýcý Adý da Varsa Bunu Invoke Et

    //public UnityEvent UserSignedOut; //Kullanýcý Çýkýþ Yaptýysa Bunu Invoke Et

    public void SignIn()
    {
        signInPanel.SetActive(false);
        signUpPanel.SetActive(false);
        setUsernamePanel.SetActive(true);
    }
    public void SignedIn()
    {
        mainMenuPanel.SetActive(true);
    }
    public void SignedOut()
    {
        mainMenuPanel.SetActive(false);
        newUserPanel.SetActive(true);
        signInPanel.SetActive(true);
        signUpPanel.SetActive(false);
        setUsernamePanel.SetActive(false);
    }
}
