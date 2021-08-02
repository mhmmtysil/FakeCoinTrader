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

    //public UnityEvent UserSignUp; //Sadece Kay�t Yapt�ysa Bunu Invoke Et

    //public UnityEvent UserSignedUp; // Kay�ttan Sonra Kullan�c� Ad� da Se�tiyse Bunu Invoke Et

    //public UnityEvent UserSignIn; // Sadece Emaille Giri� Yapt�ysa Invoke Et
    //public UnityEvent UserSignedIn; // Emaille Giri�ten Sonra Kullan�c� Ad� da Varsa Bunu Invoke Et

    //public UnityEvent UserSignedOut; //Kullan�c� ��k�� Yapt�ysa Bunu Invoke Et

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
