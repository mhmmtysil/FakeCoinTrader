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

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private Panel[] panels;

    [Header("Mail Fields")]

    public string mailSender;
    public string mailInfo;
    public int rewardPiece;
    public int mailNumber;

    public GameObject mailPrefab;
    public GameObject mailHolder;
    public GameObject mailNotification;
    public GameObject mainNotification;
    public Sprite emeraldSprite;
    public Sprite goldSprite;
    int unreadedMailCount;
    int mailID;

    [Header("Mail Detailed Info")]
    public GameObject detailedMailBgPref;
    public RectTransform detailedMailBg;

    [Header("Loading Screen")]
    public GameObject loadingScreen;
    public GameObject loadingText;
    public GameObject tapToContinueText;

    public TextMeshProUGUI usernameWarningText;
    public TMP_InputField usernameIF;
    FirebaseFirestore db;
    FirebaseAuth auth;
    FirebaseUser user;

    [HideInInspector]public string username;
    public TextMeshProUGUI userMailText;
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

    [Header("Fake Coin")]

    private int fakeCoin;
    public int FakeCoin { get { return fakeCoin; } set { fakeCoin = value; } }


    [Header("Horse Power Coin")]

    private int horsePower;
    public int HorsePower { get { return horsePower; } set { horsePower = value; } }

    [Header("Light Core Coin")]

    private int lightCore;
    public int LightCore { get { return lightCore; } set { lightCore = value; } }

    [Header("Odea Coin")]
    private int odeaCoin;
    public int OdeaCoin { get { return odeaCoin; } set { odeaCoin = value; } }

    [Header("Inogami Coin")]

    private int inogamiCoin; 
    public int InogamiCoin { get { return inogamiCoin; } set { inogamiCoin = value; } }

    [Header("Griffon Coin")]

    private int griffonCoin;
    public int GriffonCoin { get { return griffonCoin; } set { griffonCoin = value; } }

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


    
    //OpenStatusField
    [HideInInspector] public bool fakeCoinOpened;
    [HideInInspector] public bool horsePowerOpened;
    [HideInInspector] public bool lightCoreOpened;
    [HideInInspector] public bool odeaCoinOpened;
    [HideInInspector] public bool inogamiCoinOpened;
    [HideInInspector] public bool griffonCoinOpened;

    //Hire Status Field
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

    public CoinStatus coinStatus;



    

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
                DocumentReference mailRef = db.Collection("Users").Document(auth.CurrentUser.UserId);
                mailRef.GetSnapshotAsync().ContinueWithOnMainThread((task) =>
                {
                    var snapshot = task.Result;
                    if (snapshot.Exists && task.IsCompleted)
                    {
                        MailCount mailcount = snapshot.ConvertTo<MailCount>();
                        mailNumber = mailcount.mailCount;
                    }
                });
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

                        usernameText.text = username;
                        ConvertExpText();
                        expSlider.value = Convert.ToSingle(exp / maxExp);
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
                            }
                            PanelChange(PanelType.MainMenu);
                            untakenLevelRewardText.text = "x10";
                            signInPanel.SetActive(false);
                            signUpPanel.SetActive(false);
                            setUsernamePanel.SetActive(false);
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
                        usernameText.text = username;
                        ConvertExpText();
                        expSlider.value = Convert.ToSingle(exp / maxExp);
                        levelText.text = level.ToString();
                        levelTextSettings.text = level.ToString();
                        coinBalanceText.text = coin.ToString();
                        emeraldBalanceText.text = emerald.ToString();
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
    }

    public void SetLevel(int value)
    {
        level += value;
        levelText.text = level.ToString();
        levelTextSettings.text = level.ToString();
        maxExp += level * 100;
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
            untakenLevelReward += 10;
            SetUntakenLevelReward(10);
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
    void SaveMailCount(int mailCount)
    {
        mailNumber = mailCount;
        DocumentReference userRef = db.Collection("Mails").Document("SendedMails");
        userRef.UpdateAsync("SendedMailCount", mailCount);
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

    #region Mail Field

    public void NotificationStatus()
    {
        if (unreadedMailCount > 0)
        {
            var rect = mailHolder.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(240, 150 * unreadedMailCount);
            mainNotification.SetActive(true);
            mailNotification.SetActive(true);
        }
        else
        {
            mainNotification.SetActive(false);
            mailNotification.SetActive(false);
        }
    }
    public void SendGold()
    {
        string info = "Gold Ödülü.";
        string sender = "Kurucu";
        int prizePiece = 100;
        SendGift(info, sender, RewardTypes.gold, prizePiece);
    }
    public void SendEmerald()
    {
        string info = "Emerald Ödülü.";
        string sender = "Kurucu";
        int prizePiece = 100;
        SendGift(info, sender, RewardTypes.emerald, prizePiece);

    }
    public void SendGift(string info, string sender,RewardTypes rewardType, int prizePiece)
    {
        int mailname = UnityEngine.Random.Range(0,1000000);
        SendMail(sender,info, rewardType, prizePiece, mailname);
    }

    public void SendMail(string sender, string info,RewardTypes rewardType,int howManyPieceWillGift,int mailId)
    {        
        unreadedMailCount++;
        DocumentReference docRef = db.Collection("Users").Document(auth.CurrentUser.UserId).Collection("Mails").Document(mailId.ToString());
        Mails mail = new Mails
        {
            Sender = sender,
            Info = info,
            RewardType = rewardType,
            PieceReward = howManyPieceWillGift,
            MailID = mailId
        };
        docRef.SetAsync(mail);
        mailSender = sender;
        mailInfo = info;
        rewardPiece = howManyPieceWillGift;
        mailID = mailId;
        GameObject mailPref = Instantiate(mailPrefab, mailHolder.transform);
        mailPref.name = mailId.ToString();
        mailPref.transform.Find("mailFromSender").GetComponentInChildren<TextMeshProUGUI>().text = mailSender;
        mailPref.GetComponent<Button>().onClick.AddListener(delegate { DetailedMailOpen(mailSender, mailInfo, goldSprite, rewardPiece, RewardTypes.gold, mailID); });
        NotificationStatus();

    }
    public void DetailedMailOpen(string sender, string info, Sprite mailRewardtype, int rewardPeice, RewardTypes r, int mailId)
    {
        unreadedMailCount--;
        if (GameObject.Find(mailId + "(1)") != null)
        {
            Destroy(GameObject.Find(mailId + "(1)"));
        }
        GameObject dMailPref = Instantiate(detailedMailBgPref, detailedMailBg.transform);
        dMailPref.name = mailId + "(1)";
        dMailPref.transform.Find("mailSenderInfo").GetComponentInChildren<TextMeshProUGUI>().text = sender;
        dMailPref.transform.Find("mailInfo").GetComponentInChildren<TextMeshProUGUI>().text = info;
        dMailPref.transform.Find("rewardType").GetComponentInChildren<Image>().sprite = mailRewardtype;
        dMailPref.transform.Find("rewardPiece").GetComponentInChildren<TextMeshProUGUI>().text = rewardPeice.ToString();
        var collectButton = dMailPref.transform.Find("collectButton").GetComponentInChildren<Button>();
        collectButton.onClick.AddListener(delegate { GetRewardPiece(rewardPeice, r, mailId); });
        NotificationStatus();
        Debug.Log("Opened Mails number is : " + mailId);

    }
    public void GetRewardPiece(int piece, RewardTypes rewardType, int mailid)
    {        
        if (rewardType == RewardTypes.gold)
        {
            GiveCoin(piece);
        }
        else if (rewardType == RewardTypes.emerald)
        {
            GiveEmerald(piece);
        }
        NotificationStatus();
        DeleteMail(mailid);
        Destroy(GameObject.Find(mailid + "(1)"));
        Destroy(GameObject.Find(mailid.ToString()));
        Debug.Log("Destroying  Mail with id : " + mailid);
    }
    public void SetInfoForMail(string info)
    {
        mailInfo = info;
    }
    public void DeleteMail(int number)
    {
        DocumentReference docRef = db.Collection("Users").Document(auth.CurrentUser.UserId).Collection("Mails").Document((number).ToString());
        docRef.DeleteAsync();
        Debug.Log("Deleted Mails number is : " + number);
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
            case PanelType.Trade:
                panels[0].gameObject.SetActive(false);
                panels[1].gameObject.SetActive(true);
                panels[2].gameObject.SetActive(true);
                panels[3].gameObject.SetActive(false);
                panels[4].gameObject.SetActive(false);
                panels[5].gameObject.SetActive(false);
                panels[6].gameObject.SetActive(false);
                panels[7].gameObject.SetActive(false);
                panels[8].gameObject.SetActive(false);
                break;
            case PanelType.Hire:
                panels[0].gameObject.SetActive(false);
                panels[1].gameObject.SetActive(true);
                panels[2].gameObject.SetActive(false);
                panels[3].gameObject.SetActive(true);
                panels[4].gameObject.SetActive(false);
                panels[5].gameObject.SetActive(false);
                panels[6].gameObject.SetActive(false);
                panels[7].gameObject.SetActive(false);
                panels[8].gameObject.SetActive(false);
                break;
            case PanelType.Shop:
                panels[0].gameObject.SetActive(false);
                panels[1].gameObject.SetActive(true);
                panels[2].gameObject.SetActive(false);
                panels[3].gameObject.SetActive(false);
                panels[4].gameObject.SetActive(true);
                panels[5].gameObject.SetActive(false);
                panels[6].gameObject.SetActive(false);
                panels[7].gameObject.SetActive(false);
                panels[8].gameObject.SetActive(false);
                break;
            case PanelType.Profile:
                panels[0].gameObject.SetActive(false);
                panels[1].gameObject.SetActive(true);
                panels[2].gameObject.SetActive(false);
                panels[3].gameObject.SetActive(false);
                panels[4].gameObject.SetActive(false);
                panels[5].gameObject.SetActive(true);
                panels[6].gameObject.SetActive(false);
                panels[7].gameObject.SetActive(false);
                panels[8].gameObject.SetActive(false);
                break;            
            case PanelType.Settings:
                panels[0].gameObject.SetActive(false);
                panels[1].gameObject.SetActive(true);
                panels[2].gameObject.SetActive(false);
                panels[3].gameObject.SetActive(false);
                panels[4].gameObject.SetActive(false);
                panels[5].gameObject.SetActive(false);
                panels[6].gameObject.SetActive(true);
                panels[7].gameObject.SetActive(false);
                panels[8].gameObject.SetActive(false);
                break;
            case PanelType.LevelUp:
                panels[0].gameObject.SetActive(false);
                panels[1].gameObject.SetActive(true);
                panels[2].gameObject.SetActive(false);
                panels[3].gameObject.SetActive(false);
                panels[4].gameObject.SetActive(false);
                panels[5].gameObject.SetActive(false);
                panels[6].gameObject.SetActive(false);
                panels[7].gameObject.SetActive(true);
                panels[8].gameObject.SetActive(false);
                break;
            case PanelType.Loading:
                panels[8].gameObject.SetActive(true);
                break;
        }
    }

    #endregion

    
}


public enum PanelType
{
    NewUser,
    MainMenu,
    Trade,
    Hire,
    Shop,
    Profile,
    Settings,
    LevelUp,
    Loading
}

public class MailCount
{
    public int mailCount;
        public MailCount(int mailCount)
        {
        this.mailCount = mailCount;
        }
}
//-*-*-*-*-*-*-*-*-*-*-*- Mail Properties -*-*-*-*-*-*-*-*-*-*-*-
[FirestoreData]
public class Mails
{
    [FirestoreProperty]
    public string Sender { get; set; }

    [FirestoreProperty]
    public RewardTypes RewardType { get; set; }

    [FirestoreProperty]
    public string Info { get; set; }

    [FirestoreProperty]
    public int PieceReward { get; set; }

    [FirestoreProperty]
    public int MailID { get; set; }

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
