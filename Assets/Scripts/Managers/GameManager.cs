using UnityEngine;
using Firebase.Auth;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using Firebase.Firestore;
using Firebase.Extensions;
using System.Collections;
using static EventManager;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private Panel[] panels;


    public TextMeshProUGUI usernameWarningText;
    public TMP_InputField usernameIF;
    FirebaseFirestore db;
    FirebaseAuth auth;
    FirebaseUser user;

    [HideInInspector] public string username;
    [HideInInspector] public string userMailID;
    [Header("Panels")]

    public GameObject signInPanel;
    public GameObject signUpPanel;
    public GameObject setUsernamePanel;

    // newUserPanel mainMenuPanel loadingScreen signInPanel signUpPanel setUsernamePanel

    [Header("Exp Fied")]
    public TextMeshProUGUI expText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI levelTextSettings;
    public Slider expSlider;
    public GameObject levelUp;
    [HideInInspector] public long exp;
    [HideInInspector] public long maxExp;
    [HideInInspector] public int level;
    public TextMeshProUGUI untakenLevelRewardText;
    int untakenLevelReward;


    #region Coin Fields
    private int fakeCoin;
    public int FakeCoin { get { return fakeCoin; } set { fakeCoin = value; } }

    private int horsePower;
    public int HorsePower { get { return horsePower; } set { horsePower = value; } }

    private int lightCore;
    public int LightCore { get { return lightCore; } set { lightCore = value; } }

    private int odeaCoin;
    public int OdeaCoin { get { return odeaCoin; } set { odeaCoin = value; } }

    private int inogamiCoin; 
    public int InogamiCoin { get { return inogamiCoin; } set { inogamiCoin = value; } }

    private int griffonCoin;
    public int GriffonCoin { get { return griffonCoin; } set { griffonCoin = value; } }

    #endregion

    #region Coin Open Status Fields

    private bool fakeCoinOpened;
    public bool FakeCoinOpened { get { return fakeCoinOpened; } set { fakeCoinOpened = value; } }

    private bool horsePowerOpened;
    public bool HorsePowerOpened { get { return horsePowerOpened; } set { horsePowerOpened = value; } }

    private bool lightCoreOpened;
    public bool LightCoreOpened { get { return lightCoreOpened; } set { lightCoreOpened = value; } }

    private bool odeaCoinOpened;
    public bool OdeaCoinOpened { get { return odeaCoinOpened; } set { odeaCoinOpened = value; } }

    private bool inogamiCoinOpened;
    public bool InogamiCoinOpened { get { return inogamiCoinOpened; } set { inogamiCoinOpened = value; } }

    private bool griffonCoinOpened;
    public bool GriffonCoinOpened { get { return griffonCoinOpened; } set { griffonCoinOpened = value; } }

    #endregion

    #region Coin Hire Fields
    private bool fakeCoinHired;
    public bool FakeCoinHired { get { return fakeCoinHired; } set { fakeCoinHired = value; } }

    private bool horsePowerHired;
    public bool HorsePowerHired { get { return horsePowerHired; } set { horsePowerHired = value; } }

    private bool lightCoreHired;
    public bool LightCoreHired { get { return lightCoreHired; } set { lightCoreHired = value; } }

    private bool odeaCoinHired;
    public bool OdeaCoinHired { get { return odeaCoinHired; } set { odeaCoinHired = value; } }

    private bool inogamiCoinHired;
    public bool InogamiCoinHired { get { return inogamiCoinHired; } set { inogamiCoinHired = value; } }

    private bool griffonCoinHired;
    public bool GriffonCoinHired { get { return griffonCoinHired; } set { griffonCoinHired = value; } }

    #endregion

    #region Speed Fields
    private bool fakeCoinSpeeded;
    public bool FakeCoinSpeeded { get { return fakeCoinSpeeded; } set { fakeCoinSpeeded = value; } }

    private bool horsePowerSpeeded;
    public bool HorsePowerSpeeded { get { return horsePowerSpeeded; } set { horsePowerSpeeded = value; } }

    private bool lightCoreSpeeded;
    public bool LightCoreSpeeded { get { return lightCoreSpeeded; } set { lightCoreSpeeded = value; } }

    private bool odeaCoinSpeeded;
    public bool OdeaCoinSpeeded { get { return odeaCoinSpeeded; } set { odeaCoinSpeeded = value; } }

    private bool inogamiCoinSpeeded;
    public bool InogamiCoinSpeeded { get { return inogamiCoinSpeeded; } set { inogamiCoinSpeeded = value; } }

    private bool griffonCoinSpeeded;
    public bool GriffonCoinSpeeded { get { return griffonCoinSpeeded; } set { griffonCoinSpeeded = value; } }
    #endregion

    [Header("Emerald")]
    private int emerald; 
    public int Emerald{ get { return emerald; } set { emerald = value; }}

    [Header("Coin")]
    private int coin;
    public int Coin { get { return coin; } set { coin = value; } }

    [Header("User Input Fields")]
    public TMP_InputField userMailInputField;
    public TMP_InputField userPasswordInputField;

    public TMP_InputField signInEmailInputField;
    public TMP_InputField signInPasswordInputField;

    [Header("User Info Fields")]
    public TextMeshProUGUI coinBalanceText;
    public TextMeshProUGUI emeraldBalanceText;
    public TextMeshProUGUI usernameText;


    [Header("Avatars")]
    public GameObject avatarPrefab;
    public GameObject avatar;
    public GameObject avatarProfile;
    private Sprite avatarChoosed;
    private int avatarNum;
    public Sprite[] avatarList;

    public CoinStatus coinStatus;

    #region In Game Notification Managers
    [SerializeField] GameObject mainMenuButtonNotification;
    [SerializeField] GameObject tradeAvailableNotification;


    #endregion





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
            
        });
    }

    public void SignOut()
    {
        auth.SignOut();
        coinStatus.Instance.SetDefaults();
    }

    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != user)
        {
            bool signedIn = user != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && user != null)
            {                
                PanelChange(PanelType.NewUser);
                signInPanel.SetActive(true);
                signUpPanel.SetActive(false);
                setUsernamePanel.SetActive(false);
            }
            user = auth.CurrentUser;
            if (signedIn)
            {
                userMailID = auth.CurrentUser.Email.ToString();
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
                        untakenLevelReward = user.UnTakenRewardForLevel;

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

                        fakeCoinSpeeded = user.FakeCoinSpeeded;
                        horsePowerSpeeded = user.HorsePowerSpeeded;
                        lightCoreSpeeded = user.LightCoreSpeeded;
                        odeaCoinSpeeded = user.OdeaCoinSpeeded;
                        inogamiCoinSpeeded = user.InogamiCoinSpeeded;
                        griffonCoinSpeeded = user.GriffonCoinSpeeded;

                        usernameText.text = username;
                        ConvertExpText();                        
                        levelText.text = level.ToString();
                        levelTextSettings.text = level.ToString();
                        coinBalanceText.text = coin.ToString();
                        emeraldBalanceText.text = emerald.ToString();

                        if (untakenLevelReward > 0)
                        {
                            PanelChange(PanelType.LevelUp);
                            untakenLevelRewardText.text = "x" + untakenLevelReward;
                        }
                        else
                        {
                            untakenLevelReward = 0;
                            untakenLevelRewardText.text = "x10";
                            PanelChange(PanelType.MainMenu);
                            coinStatus.Instance.GetStatus();
                            signInPanel.SetActive(false);
                            signUpPanel.SetActive(false);
                            setUsernamePanel.SetActive(true);
                        }
                        
                    }
                    else
                    {
                        Debug.Log(String.Format("Document {0} does not exist!", snapshot.Id));
                        PanelChange(PanelType.NewUser);
                        signInPanel.SetActive(false);
                        signUpPanel.SetActive(false);
                        setUsernamePanel.SetActive(true);
                    }
                });              
            }
            else
            {
            }
        }
    }

    public void SaveUserDatas()
    {
        username = usernameIF.text;
        Debug.Log("Username Length is : " + username.Length);
        if (username.Length <= 4)
        {
            StartCoroutine(UserNameTextLengthCount("Kullanıcı adı 4 karakterden büyük olmalıdır." + Environment.NewLine + "Tekrar Deneyin."));
        }
        if (username.Length >= 15)
        {
            StartCoroutine(UserNameTextLengthCount("Kullanıcı adı 15 karakterden küçük olmalıdır." + Environment.NewLine + "Tekrar Deneyin."));
        }
        if (username.Length > 4 && username.Length < 15)
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
                UnTakenRewardForLevel = 0,

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
                GriffonCoinHired = false,

                FakeCoinSpeeded = false,
                HorsePowerSpeeded = false,
                LightCoreSpeeded = false,
                OdeaCoinSpeeded = false,
                InogamiCoinSpeeded = false,
                GriffonCoinSpeeded = false
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
                                untakenLevelReward = user.UnTakenRewardForLevel;

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

                                fakeCoinSpeeded = user.FakeCoinSpeeded;
                                horsePowerSpeeded = user.HorsePowerSpeeded;
                                lightCoreSpeeded = user.LightCoreSpeeded;
                                odeaCoinSpeeded = user.OdeaCoinSpeeded;
                                inogamiCoinSpeeded = user.InogamiCoinSpeeded;
                                griffonCoinSpeeded = user.GriffonCoinSpeeded;
                            }
                            PanelChange(PanelType.MainMenu);
                            untakenLevelRewardText.text = "x10";
                            signInPanel.SetActive(false);
                            signUpPanel.SetActive(false);
                            setUsernamePanel.SetActive(false);

                            usernameText.text = username;
                            ConvertExpText();
                            expSlider.value = Convert.ToSingle(exp / maxExp);
                            levelText.text = level.ToString();
                            levelTextSettings.text = level.ToString();
                            coinBalanceText.text = coin.ToString();
                            emeraldBalanceText.text = emerald.ToString();
                        }
                        else
                        {
                            PanelChange(PanelType.NewUser);
                            coinStatus.Instance.GetStatus();
                            coinStatus.Instance.SetDefaults();
                            signInPanel.SetActive(false);
                            signUpPanel.SetActive(false);
                            setUsernamePanel.SetActive(true);
                        }
                    });
                }
            });

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
        if (auth.CurrentUser!=null)
        {
            DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
            userRef.UpdateAsync(coinname, balance);
        }
        
    }
    public void SetCoinUnlock(string coinName)
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync(coinName+"Opened", true);
    }

    public void SetCoinSpeeded(string coinName)
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync(coinName + "Speeded", true);
    }

    public void SetCoinHired(string coinName)
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync(coinName + "Hired", true);
    }
    public void SetCoinDiggingSpeed(string coinName, float value)
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync(coinName + "DiggingSpeed", value);
    }

    public void SetUntakenLevelReward(int r)
    {
        DocumentReference userRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
        userRef.UpdateAsync("UnTakenRewardForLevel", r);
    }
    public void GetUntakenLevelReward()
    {
        GiveEmerald(untakenLevelReward);
        SetUntakenLevelReward(0);
        untakenLevelReward = 0;
        untakenLevelRewardText.text = "x0";
        PanelChange(PanelType.MainMenu);
    }

    #endregion

    #region Exp Manager

    private void ConvertExpText()
    {
        expText.text = exp.ToString("#,##0").Replace(',', '.') + "/" + maxExp.ToString("#,##0").Replace(',', '.');
        expSlider.value = Convert.ToSingle(exp / maxExp);
    }

    public void SetLevel(int value)
    {
        level += value;
        levelText.text = level.ToString();
        levelTextSettings.text = level.ToString();
        maxExp = level * 100;
        UpdateMaxExp();
    }
    public void GiveExp(int value) 
    {
        exp += value;
        if (exp >= maxExp)
        {
            PanelChange(PanelType.LevelUp);
            exp -= maxExp;
            SetLevel(1);
            untakenLevelReward += 20;
            SetUntakenLevelReward(20);
            untakenLevelRewardText.text = "x" + untakenLevelReward;
            if (exp>maxExp)
            {
                GiveExp(0);
            }            
        }        
        UpdateExp();
        ConvertExpText();
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

    #region Panel Management

    public void PanelChange(PanelType panel)
    {
        switch (panel)
        {
            case PanelType.NewUser:
                panels[0].gameObject.SetActive(true);
                panels[1].gameObject.SetActive(false);
                panels[2].gameObject.SetActive(false);
                panels[3].gameObject.SetActive(false);
                panels[4].gameObject.SetActive(false);
                panels[5].gameObject.SetActive(false);
                panels[6].gameObject.SetActive(false);
                panels[7].gameObject.SetActive(false);
                panels[8].gameObject.SetActive(false);
                break;
            case PanelType.MainMenu:
                panels[0].gameObject.SetActive(false);
                panels[1].gameObject.SetActive(true);
                panels[2].gameObject.SetActive(false);
                panels[3].gameObject.SetActive(false);
                panels[4].gameObject.SetActive(false);
                panels[5].gameObject.SetActive(false);
                panels[6].gameObject.SetActive(false);
                panels[7].gameObject.SetActive(false);
                panels[8].gameObject.SetActive(false);
                break;
            case PanelType.Loading:
                panels[9].gameObject.SetActive(true);
                break;
        }
    }

    #endregion

    #region In Game Notification Manager

    public void TradeableNotification()
    {
        mainMenuButtonNotification.SetActive(true);
        tradeAvailableNotification.SetActive(true);
    }
    public void TradeableNotificationReceived()
    {
        mainMenuButtonNotification.SetActive(false);
        tradeAvailableNotification.SetActive(false);
    }

    #endregion

}

public enum NotificationType
{
    tradeAvailable,
    achievementReached
}

public enum PanelType
{
    NewUser,
    MainMenu,
    Trade,
    SpeedUp,
    Hire,
    Shop,
    Profile,
    Settings,
    HowToEarn,
    LevelUp,
    Loading
}

[FirestoreData]
public class UserDataClass
{
    [FirestoreProperty]
    public float Patch { get; private set; }

    [FirestoreProperty]
    public string Username { get; set; }
    [FirestoreProperty]
    public int Emerald { get; set; }
    [FirestoreProperty]
    public int Coin { get; set; }
    [FirestoreProperty]
    public long Exp { get; set; }

    [FirestoreProperty]
    public long MaxExp { get; set; }
    [FirestoreProperty]
    public int Level { get; set; }

    [FirestoreProperty]
    public int UnTakenRewardForLevel { get; set; }


    // --------------------- Coin Balances Properties ---------------------
    [FirestoreProperty]
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

    // --------------------- Coin Speed Properties ---------------------
    [FirestoreProperty]
    public bool FakeCoinSpeeded { get; set; }
    [FirestoreProperty]
    public bool HorsePowerSpeeded { get; set; }
    [FirestoreProperty]
    public bool LightCoreSpeeded { get; set; }
    [FirestoreProperty]
    public bool OdeaCoinSpeeded { get; set; }
    [FirestoreProperty]
    public bool InogamiCoinSpeeded { get; set; }
    [FirestoreProperty]
    public bool GriffonCoinSpeeded { get; set; }

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
