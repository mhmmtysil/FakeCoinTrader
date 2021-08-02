using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public TextMeshProUGUI usernameWarningText;
    public TMP_InputField usernameIF;
    FirebaseFirestore db;
    FirebaseAuth auth;
    FirebaseUser user;

    [HideInInspector]public string username;
    public TextMeshProUGUI userMailText;
    [Header("Panels")]
    public GameObject newUserPanel;
    public GameObject mainMenuPanel;
    public GameObject loadingScreen;

    public GameObject signInPanel;
    public GameObject signUpPanel;
    public GameObject setUsernamePanel;

    // newUserPanel mainMenuPanel loadingScreen signInPanel signUpPanel setUsernamePanel

    [Header("Exp Fied")]
    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelText;
    public Slider expSlider;
    public GameObject levelUp;
    [HideInInspector] public int exp;
    [HideInInspector] public int maxExp;
    [HideInInspector] public int level;

    [Header("Fake Coin")]

    [HideInInspector] public int fakeCoin;
    public int _fakeCoin { get { return fakeCoin; } set { fakeCoin = value; } }


    [Header("Horse Power Coin")]

    private int horsePower;
    public int _horsePower { get { return horsePower; } set { horsePower = value; } }

    [Header("Light Core Coin")]

    [HideInInspector] public int lightCore;
    public int _lightCore { get { return lightCore; } set { lightCore = value; } }

    [Header("Odea Coin")]
    [HideInInspector] public int odeaCoin;
    public int _odeaCoin { get { return odeaCoin; } set { odeaCoin = value; } }

    [Header("Inogami Coin")]

    [HideInInspector] public int inogamiCoin; 
    public int _inogamiCoin { get { return inogamiCoin; } set { inogamiCoin = value; } }

    [Header("Griffon Coin")]

    [HideInInspector] public int griffonCoin;
    public int _griffonCoin { get { return griffonCoin; } set { griffonCoin = value; } }


    [Header("User Input Fields")]
    public TMP_InputField userMailInputField;
    public TMP_InputField userPasswordInputField;

    public TMP_InputField signInEmailInputField;
    public TMP_InputField signInPasswordInputField;

    [Header("User Info Fields")]
    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI emeraldBalanceText;
    public TextMeshProUGUI usernameText;

    [Header("Emerald")]
    private int emerald; 
    public int _emerald{ get { return emerald; } set { emerald = value; }}

    [Header("Coin")]
    private int coin;
    public int _coin { get { return coin; } set { coin = value; } }


    [HideInInspector] public bool fakeCoinOpened;
    [HideInInspector] public bool horsePowerOpened;
    [HideInInspector] public bool lightCoreOpened;
    [HideInInspector] public bool odeaCoinOpened;
    [HideInInspector] public bool inogamiCoinOpened;
    [HideInInspector] public bool griffonCoinOpened;

    [HideInInspector] public bool fakeCoinHired;
    [HideInInspector] public bool horsePowerHired;
    [HideInInspector] public bool lightCoreHired;
    [HideInInspector] public bool odeaCoinHired;
    [HideInInspector] public bool inogamiCoinHired;
    [HideInInspector] public bool griffonCoinHired;

    [Header("Avatars")]
    public GameObject avatarPrefab;
    public GameObject avatar;
    public GameObject avatarProfile;
    private Sprite avatarChoosed;
    private int avatarNum;
    public Sprite[] avatarList;



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Start()
    {
        InitializeFirebase();
    }

    #region Firebase Field
    public void InitializeFirebase()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }
    public void SignUpWithMail()
    {
        auth.CreateUserWithEmailAndPasswordAsync(userMailInputField.text, userPasswordInputField.text).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }
            FirebaseUser newUser = task.Result;
            signInPanel.SetActive(false);
            signUpPanel.SetActive(false);
            setUsernamePanel.SetActive(true);
            Debug.Log("User Created Succesfully..");            

        });
    }

    public void SignInWithMail()
    {
        string email = signInEmailInputField.text;
        string password = signInPasswordInputField.text;

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
            }
            user = task.Result;            
            Debug.Log("User Signed in Succesfully..");
        });
    }

    public void SignOut()
    {
        auth.SignOut();
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {
                newUserPanel.SetActive(true);
                mainMenuPanel.SetActive(false);
                signInPanel.SetActive(true);
                signUpPanel.SetActive(false);
                setUsernamePanel.SetActive(false);
                Debug.Log("User Not Logged In..");
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                DocumentReference docRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
                docRef.GetSnapshotAsync().ContinueWithOnMainThread((task) =>
                {
                    var snapshot = task.Result;
                    if (snapshot.Exists && task.IsCompleted)
                    {                       
                        Debug.Log(String.Format("Document data for {0} document:", snapshot.Id));
                        UserDataClass user = snapshot.ConvertTo<UserDataClass>();

                        username = user.Username;
                        exp = user.Exp;
                        maxExp = user.MaxExp;
                        coin = user.Coin;
                        emerald = user.Emerald;
                        level = user.Level;
                        fakeCoin = user.FakeCoin;
                        horsePower = user.HorsePower;
                        lightCore = user.LightCore;
                        odeaCoin = user.OdeaCoin;
                        inogamiCoin = user.InogamiCoin;
                        griffonCoin = user.GriffonCoin;

                        fakeCoinOpened = user.FakeCoinOpened;
                        horsePowerOpened = user.HorsePowerOpened;
                        lightCoreOpened = user.LightCoreOpened;
                        odeaCoinOpened = user.OdeaCoinOpened;
                        inogamiCoinOpened = user.InogamiCoinOpened;
                        griffonCoinOpened = user.InogamiCoinOpened;

                        fakeCoinHired = user.FakeCoinHired;
                        horsePowerHired = user.HorsePowerHired;
                        lightCoreHired = user.LightCoreHired;
                        odeaCoinHired = user.OdeaCoinHired;
                        inogamiCoinHired = user.InogamiCoinHired;
                        griffonCoinHired = user.GriffonCoinHired;


                        newUserPanel.SetActive(false);
                        mainMenuPanel.SetActive(true);
                        signInPanel.SetActive(false);
                        signUpPanel.SetActive(false);
                        setUsernamePanel.SetActive(true);

                        mainMenuPanel.SetActive(true);
                        usernameText.text = username;
                        expText.text = exp + "/" + maxExp;
                        expSlider.value = Convert.ToSingle(exp / maxExp);
                        levelText.text = level.ToString();
                        coinBalanceText.text = coin.ToString();
                        emeraldBalanceText.text = emerald.ToString();

                    }
                    else
                    {
                        Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
                        newUserPanel.SetActive(true);
                        mainMenuPanel.SetActive(false);
                        signInPanel.SetActive(false);
                        signUpPanel.SetActive(false);
                        setUsernamePanel.SetActive(true);
                    }
                });
                userMailText.text = "Kullanıcı Mail Adresi : " + Environment.NewLine + user.Email;
                Debug.Log("User Logged In. Listener Works Correctly.");
            }
        }
    }


    #region UserName Warning Field

    IEnumerator UserNameTextLengthCount(string warningText)
    {
        usernameWarningText.text = warningText;
        usernameIF.text = "";
        yield return new WaitForSeconds(2);
        usernameWarningText.text = "";
    }

    #endregion


    public void SaveUserDatas()
    {
        username = usernameIF.text;
        Debug.Log("Username Length is : " + username.Length);
        if (username.Length<=4)
        {
            StartCoroutine(UserNameTextLengthCount("Kullanıcı adı 4 karakterden büyük olmalıdır."+ Environment.NewLine + "Tekrar Deneyin."));
        }
        if (username.Length >= 15)
        {
            StartCoroutine(UserNameTextLengthCount("Kullanıcı adı 15 karakterden küçük olmalıdır." + Environment.NewLine + "Tekrar Deneyin."));
        }
        if(username.Length > 4 && username.Length < 15)
        {
            StartCoroutine(UserNameTextLengthCount("Kayıt Başarılı." + Environment.NewLine + "Yönlendiriliyorsunuz."));
            DocumentReference docRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
            UserDataClass userDataClass = new UserDataClass
            {
                Username = username,
                Emerald = 100,
                Coin = 1000,
                Exp = 0,
                Level = 1,
                MaxExp = 50,
                FakeCoin = 0,
                HorsePower = 0,
                LightCore = 0,
                OdeaCoin = 0,
                InogamiCoin = 0,
                GriffonCoin = 0,

                FakeCoinOpened = true,
                HorsePowerOpened = false,
                LightCoreOpened = false,
                OdeaCoinOpened = false,
                InogamiCoinOpened = false,
                GriffonCoinOpened = false,

                FakeCoinHired = false,
                HorsePowerHired = false,
                LightCoreHired = false,
                OdeaCoinHired = false,
                InogamiCoinHired = false,
                GriffonCoinHired = false

            };
            docRef.SetAsync(userDataClass);

            db.Collection("Users").Document(auth.CurrentUser.UserId.ToString()).SetAsync(userDataClass).ContinueWith(task =>
            {
                if (task.IsCompleted)
                {
                    DocumentReference docRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
                    docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
                    {
                        DocumentSnapshot snapshot = task.Result;
                        if (snapshot.Exists)
                        {
                            Dictionary<string, object> _user = snapshot.ToDictionary();
                            foreach (KeyValuePair<string, object> pair in _user)
                            {
                                UserDataClass user = snapshot.ConvertTo<UserDataClass>();
                                username = user.Username;
                                exp = user.Exp;
                                maxExp = user.MaxExp;
                                coin = user.Coin;
                                emerald = user.Emerald;
                                level = user.Level;

                                fakeCoin = user.FakeCoin;
                                horsePower = user.HorsePower;
                                lightCore = user.LightCore;
                                odeaCoin = user.OdeaCoin;
                                inogamiCoin = user.InogamiCoin;
                                griffonCoin = user.GriffonCoin;

                                fakeCoinOpened = user.FakeCoinOpened;
                                horsePowerOpened = user.HorsePowerOpened;
                                lightCoreOpened = user.LightCoreOpened;
                                odeaCoinOpened = user.OdeaCoinOpened;
                                inogamiCoinOpened = user.InogamiCoinOpened;
                                griffonCoinOpened = user.InogamiCoinOpened;
                            }
                            newUserPanel.SetActive(false);
                            mainMenuPanel.SetActive(true);
                            signInPanel.SetActive(false);
                            signUpPanel.SetActive(false);
                            setUsernamePanel.SetActive(false);
                            Debug.Log("User Has a Doc..");
                        }
                        else
                        {
                            newUserPanel.SetActive(true);
                            mainMenuPanel.SetActive(false);
                            signInPanel.SetActive(false);
                            signUpPanel.SetActive(false);
                            setUsernamePanel.SetActive(true);
                            Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
                        }
                        usernameText.text = username;
                        expText.text = exp + "/" + maxExp;
                        expSlider.value = Convert.ToSingle(exp / maxExp);
                        levelText.text = level.ToString();
                        coinBalanceText.text = coin.ToString();
                        emeraldBalanceText.text = emerald.ToString();
                    });
                }
            });

        }
        
    }
    #endregion

    #region Update Firestore Fields

    public void UpdateEmerald()
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync("Emerald", emerald);
        emeraldBalanceText.text = emerald.ToString();
    }

    public void UpdateCoinsText(string coinname, int balance)
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync(coinname, balance);
    }

    public void SetCoinUnlock(string coinName, bool isopened)
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync(coinName+"Opened", true);
    }

    public void SetCoinHired(string coinName, bool isopened)
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync(coinName + "Hired", true);
    }

    #endregion

    #region Exp Manager
    public void SetLevel(int value)
    {
        level += value;
        levelText.text = level.ToString();
        maxExp += 200;
        UpdateMaxExp();
    }
    public void GiveExp(int value) 
    {
        exp += value;
        if (exp >= maxExp)
        {            
            exp -= maxExp;
            SetLevel(1);
            levelUp.SetActive(true);
        }
        UpdateExp();
        expText.text = exp + "/" + maxExp;
        expSlider.value = Convert.ToSingle(exp) / Convert.ToSingle(maxExp);
    }

    void UpdateExp()
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync("Exp", exp);
    }
    void UpdateMaxExp()
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync("MaxExp", maxExp);
        userRef.UpdateAsync("Level", level);
    }
    public void LevelUpPrice()
    {
        GiveEmerald(10);
        levelUp.SetActive(false);
    }

    #endregion

    #region AvatarManagement
    public void SelectAvatar(Sprite avatarChoosen)
    {
        avatarChoosed = avatarChoosen;
    }
    public void SelectAvatarNum(int num)
    {
        avatarNum = num;
    }
    public void SelectAvatarConfirm()
    {
        avatar.GetComponent<Image>().sprite = avatarChoosed;
        avatarProfile.GetComponent<Image>().sprite = avatarChoosed;
        PlayerPrefs.SetInt("AvatarSpriteNumber", avatarNum);
    }
    public void GetAvatarImage()
    {
        avatarNum = PlayerPrefs.GetInt("AvatarSpriteNumber");
        avatar.GetComponent<Image>().sprite = avatarList[avatarNum];
        avatarProfile.GetComponent<Image>().sprite = avatarList[avatarNum];
    }
    #endregion

    #region Emerald
    public void GiveEmerald(int emeraldToGive)
    {
        emerald += emeraldToGive;
        
        UpdateEmerald();
    }
    public void BuyWithEmerald(int price)
    {
        if (price > emerald)
        {
            //not enough emerald. Warn and offer some emerald
        }
        else
        {
            //emerald is enough. Buying proccess started
            emerald -= price;
            UpdateEmerald();
        }
    }
    #endregion
    
    #region Coin
    public void UpdateCoinTexts()
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync("Coin", coin);
        coinBalanceText.text = coin.ToString();
    }
    public void GiveCoin(int coinToGive)
    {
        coin += coinToGive;
        UpdateCoinTexts();
    }
    public void BuyWithCoin(int price)
    {
        if (price > coin)
        {
            //not enough coin. Warn and offer some emerald
        }
        else
        {
            //emerald is enough. Buying proccess started
            coin -= price;
            UpdateCoinTexts();
        }
    }
    #endregion


    public void CheckInterstitialStatue()
    {

    }
}

[FirestoreData]
public class UserDataClass
{
    [FirestoreProperty]
    public string Username { get; set; }
    [FirestoreProperty]
    public int Emerald { get; set; }
    [FirestoreProperty]
    public int Coin { get; set; }
    [FirestoreProperty]
    public int Exp { get; set; }

    [FirestoreProperty]
    public int MaxExp { get; set; }
    [FirestoreProperty]
    public int Level { get; set; }
    [FirestoreProperty]

    // --------------------- Coin Balances Properties ---------------------
    public int FakeCoin { get; set; }
    [FirestoreProperty]
    public int HorsePower { get; set; }
    [FirestoreProperty]
    public int LightCore { get; set; }
    [FirestoreProperty]
    public int OdeaCoin { get; set; }
    [FirestoreProperty]
    public int InogamiCoin { get; set; }
    [FirestoreProperty]
    public int GriffonCoin { get; set; }

    // --------------------- Opened Status Properties ---------------------
    [FirestoreProperty]
    public bool FakeCoinOpened { get; set; }
    [FirestoreProperty]
    public bool HorsePowerOpened { get; set; }
    [FirestoreProperty]
    public bool LightCoreOpened { get; set; }
    [FirestoreProperty]
    public bool OdeaCoinOpened { get; set; }
    [FirestoreProperty]
    public bool InogamiCoinOpened { get; set; }
    [FirestoreProperty]
    public bool GriffonCoinOpened { get; set; }


    // --------------------- Hired Status Properties ---------------------
    [FirestoreProperty]
    public bool FakeCoinHired { get; set; }
    [FirestoreProperty]
    public bool HorsePowerHired { get; set; }
    [FirestoreProperty]
    public bool LightCoreHired { get; set; }
    [FirestoreProperty]
    public bool OdeaCoinHired { get; set; }
    [FirestoreProperty]
    public bool InogamiCoinHired { get; set; }
    [FirestoreProperty]
    public bool GriffonCoinHired { get; set; }
}
