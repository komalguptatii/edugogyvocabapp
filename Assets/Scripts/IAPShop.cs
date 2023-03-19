using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using TMPro;
using System;
using Random=UnityEngine.Random;
using UnityEngine.SceneManagement;


public class IAPShop : MonoBehaviour, IStoreListener
{
   // -----
    private string spaceExplorationID = "com.techies.edugogy.onemonth";
    private string spaceWalkID = "com.techies.edugogy.threemonth";
    private string spaceCaptureID = "com.techies.edugogy.sixmonth";
    //-----

    private string spaceExplorationIdBeginner = "com.edugogy.vocabapp.beginnerOneMonth";
    private string spaceWalkIdBeginner = "com.edugogy.vocabapp.beginnerThreeMonth";
    private string spaceCaptureIdBeginner = "com.edugogy.vocabapp.beginnerSixMonth";

    private string spaceExplorationIdLearner = "com.edugogy.vocabapp.learnerOneMonth";
    private string spaceWalkIdLearner = "com.edugogy.vocabapp.learnerThreeMonth";
    private string spaceCaptureIdLearner = "com.edugogy.vocabapp.learnerSixMonth";

     private string spaceExplorationIdSeeker = "com.edugogy.vocabapp.seekerOneMonth";
    private string spaceWalkIdSeeker = "com.edugogy.vocabapp.seekerThreeMonth";
    private string spaceCaptureIdSeeker = "com.edugogy.vocabapp.seekerSixMonth";

    private string spaceExplorationIdIntermediate = "com.edugogy.vocabapp.intermediateOneMonth";
    private string spaceWalkIdIntermediate = "com.edugogy.vocabapp.intermediateThreeMonth";
    private string spaceCaptureIdIntermediate = "com.edugogy.vocabapp.intermediateSixMonth";

    private string spaceExplorationIdGraduate = "com.edugogy.vocabapp.graduateOneMonth";
    private string spaceWalkIdGraduate = "com.edugogy.vocabapp.graduateThreeMonth";
    private string spaceCaptureIdGraduate = "com.edugogy.vocabapp.graduateSixMonth";

    private string spaceExplorationIdMaster = "com.edugogy.vocabapp.masterOneMonth";
    private string spaceWalkIdMaster = "com.edugogy.vocabapp.masterThreeMonth";
    private string spaceCaptureIdMaster = "com.edugogy.vocabapp.masterSixMonth";

    private string spaceExplorationIdWordsmith = "com.edugogy.vocabapp.wordsmithOneMonth";
    private string spaceWalkIdWordsmith = "com.edugogy.vocabapp.wordsmithThreeMonth";
    private string spaceCaptureIdWordsmith = "com.edugogy.vocabapp.wordsmithSixMonth";

    string auth_key;

    // [SerializeField] public TextMeshProUGUI transactionId;
    // [SerializeField] public TextMeshProUGUI payload;

    private IAppleExtensions m_AppleExtensions;
            IStoreController m_StoreController;
    private IExtensionProvider extensions;

    public static IAPShop instance;

    [Serializable]
    public class Error
    {
        public int code;
        public string source;
        public string title;
        public string detail;
    }

    [Serializable]
    public class ErrorList
    {
        public Error[] error;
    }

    [Serializable]
    public class KidsProfile
    {
        public int id;
        public string name;
        public string phone;
        public int age_group_id;
        public int country_code_id;
        public object social_id;
        public object social_media;
        public object email;
        public int total_level;
        public int total_passed_level;
        public int available_level;
        public bool is_trial_subscription;
        public string subscription_remaining_day;
        public int remaining_trial;
        public int remaining_level_for_day;

    }

    [Serializable]
    public class SubscriptionLoad
    {
        public string type;
        public string log;
    }

     string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";

     [SerializeField]
    public Button subscribeLater;
    [SerializeField]
    public Button restoreButton;
    public string infoLoad = "";

    public class SubscriptionForm
    {
        public string transaction_id;

        public string platform;

        public string platform_plan_id;

        public int is_renew;
    }

    public class TrialSubscriptionForm
    {
        public string transaction_id;

        public string platform;

        public string platform_plan_id;
    }

     public class PurchaseFailedForm
    {
        public string transaction_id;
        public string platform;
        public string platform_plan_id;
        public string reason;
    }
  
    string selectedId = "";
    string typeOfPlatform = "";
    public Sprite Image1;
    public Sprite subscribeLaterImage;

    public Sprite selectedSpaceCapture;
    public Sprite selectedSpaceWalk;
    public Sprite selectedSpaceExploration;

    public Sprite inActiveSpaceCapture;
    public Sprite inActiveSpaceWalk;
    public Sprite inActiveSpaceExploration;

    [SerializeField]
    public Button ExplorationButton;
    [SerializeField]
    public Button WalkButton;
    [SerializeField]
    public Button CaptureButton;

    [SerializeField]
    public Button subscribeNowButton;

    string receiptReceived = "";
    string transactionIdReceived = "";
    bool isAddingSubscription = true;

    
    KidsProfile profile = new KidsProfile();
    string processedTransactionId = "";

    private Animator loadingIndicator;
    public GameObject Indicator;
    public string stateOfApp = "start";
    string failureReason = "";

    [SerializeField] public TextMeshProUGUI firstPlanTitle;

    [SerializeField] public TextMeshProUGUI secondPlanTitle;

    [SerializeField] public TextMeshProUGUI thirdPlanTitle;

    public int levelId = 0;
    public string productId = "";

    Dictionary<string, string> dict = new Dictionary<string, string>();


    private void Awake()
    {
         loadingIndicator = Indicator.GetComponent<Animator>(); 
         loadingIndicator.enabled = false;
        Indicator.SetActive(false);

        subscribeNowButton.enabled = false;
        
        if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
        }

             
        GetProfile();
        
    }

    private void Start()
    {
        Debug.Log("level id is " + levelId);
       if (Application.platform == RuntimePlatform.Android)
        {
            typeOfPlatform = "google";
        }
        else
        {
            typeOfPlatform = "apple";
        }

    }

    void SetTitle()
    {
        if (levelId == 1)
        {
            firstPlanTitle.GetComponent<TextMeshProUGUI>().text = "Beginner - Space Exploration";
            secondPlanTitle.GetComponent<TextMeshProUGUI>().text = "Beginner - Space Walk";
            thirdPlanTitle.GetComponent<TextMeshProUGUI>().text = "Beginner - Space Capture";
        } 
        else if (levelId == 2)
        {
             firstPlanTitle.GetComponent<TextMeshProUGUI>().text = "Learner - Space Exploration";
            secondPlanTitle.GetComponent<TextMeshProUGUI>().text = "Learner - Space Walk";
            thirdPlanTitle.GetComponent<TextMeshProUGUI>().text = "Learner - Space Capture";
        }
        else if (levelId == 3)
        {
             firstPlanTitle.GetComponent<TextMeshProUGUI>().text = "Seeker - Space Exploration";
            secondPlanTitle.GetComponent<TextMeshProUGUI>().text = "Seeker - Space Walk";
            thirdPlanTitle.GetComponent<TextMeshProUGUI>().text = "Seeker - Space Capture";
        }
        else if (levelId == 4)
        {
            firstPlanTitle.GetComponent<TextMeshProUGUI>().text = "Intermediate - Space Exploration";
            secondPlanTitle.GetComponent<TextMeshProUGUI>().text = "Intermediate - Space Walk";
            thirdPlanTitle.GetComponent<TextMeshProUGUI>().text = "Intermediate - Space Capture";
        }
        else if (levelId == 5)
        {
            firstPlanTitle.GetComponent<TextMeshProUGUI>().text = "Graduate - Space Exploration";
            secondPlanTitle.GetComponent<TextMeshProUGUI>().text = "Graduate - Space Walk";
            thirdPlanTitle.GetComponent<TextMeshProUGUI>().text = "Graduate - Space Capture";
        }
        else if (levelId == 6)
        {
            firstPlanTitle.GetComponent<TextMeshProUGUI>().text = "Master - Space Exploration";
            secondPlanTitle.GetComponent<TextMeshProUGUI>().text = "Master - Space Walk";
            thirdPlanTitle.GetComponent<TextMeshProUGUI>().text = "Master - Space Capture";
        }
        else if (levelId == 7)
        {
             firstPlanTitle.GetComponent<TextMeshProUGUI>().text = "Wordsmith - Space Exploration";
            secondPlanTitle.GetComponent<TextMeshProUGUI>().text = "Wordsmith - Space Walk";
            thirdPlanTitle.GetComponent<TextMeshProUGUI>().text = "Wordsmith - Space Capture";
        }
    }

     void InitializePurchasing()
    {
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

        
        if (levelId == 1)
        {
            
             builder.AddProduct(spaceExplorationIdBeginner, ProductType.Subscription);
             builder.AddProduct(spaceWalkIdBeginner, ProductType.Subscription);
            builder.AddProduct(spaceCaptureIdBeginner, ProductType.Subscription);
        }
        else if (levelId == 2)
        {
           
             builder.AddProduct(spaceExplorationIdLearner, ProductType.Subscription);
             builder.AddProduct(spaceWalkIdLearner, ProductType.Subscription);
            builder.AddProduct(spaceCaptureIdLearner, ProductType.Subscription);
        }
        else if (levelId == 3)
        {
           
             builder.AddProduct(spaceExplorationIdSeeker, ProductType.Subscription);
             builder.AddProduct(spaceWalkIdSeeker, ProductType.Subscription);
            builder.AddProduct(spaceCaptureIdSeeker, ProductType.Subscription);
        }
        else if (levelId == 4)
        {
            
             builder.AddProduct(spaceExplorationIdIntermediate, ProductType.Subscription);
             builder.AddProduct(spaceWalkIdIntermediate, ProductType.Subscription);
            builder.AddProduct(spaceCaptureIdIntermediate, ProductType.Subscription);
        }
        else if (levelId == 5)
        {
            
             builder.AddProduct(spaceExplorationIdGraduate, ProductType.Subscription);
             builder.AddProduct(spaceWalkIdGraduate, ProductType.Subscription);
            builder.AddProduct(spaceCaptureIdGraduate, ProductType.Subscription);
        }
        else if (levelId == 6)
        {
            
             builder.AddProduct(spaceExplorationIdMaster, ProductType.Subscription);
             builder.AddProduct(spaceWalkIdMaster, ProductType.Subscription);
            builder.AddProduct(spaceCaptureIdMaster, ProductType.Subscription);
        }
        else if (levelId == 7)
        {
           
             builder.AddProduct(spaceExplorationIdWordsmith, ProductType.Subscription);
             builder.AddProduct(spaceWalkIdWordsmith, ProductType.Subscription);
            builder.AddProduct(spaceCaptureIdWordsmith, ProductType.Subscription);
        }
        

        UnityPurchasing.Initialize(this, builder);
    }

    public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
    {
         m_StoreController = controller;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>(); 
          dict = m_AppleExtensions.GetIntroductoryPriceDictionary();

    //    if (PlayerPrefs.HasKey("GetRenewalId"))
    //     {
    //         string valueOfIfCheckForRenewal = PlayerPrefs.GetString("GetRenewalId");
    //         if(valueOfIfCheckForRenewal == "true")
    //         {
    //             ListProducts();
    //             PlayerPrefs.SetString("GetRenewalId", "false");
    //         }
    //     }
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        // transactionId.GetComponent<TextMeshProUGUI>().text = "Initialization failed";
    }

    public void ListProducts()
    {
        loadingIndicator.enabled = true;
         Indicator.SetActive(true);
        // transactionId.GetComponent<TextMeshProUGUI>().text = "checking for all products";
        foreach (UnityEngine.Purchasing.Product item in m_StoreController.products.all)
        {
            // transactionId.GetComponent<TextMeshProUGUI>().text = "coming into loop";
            if (item.receipt != null)
            {
                Debug.Log("Receipt found for Product = " + item.definition.id.ToString());
                // transactionId.GetComponent<TextMeshProUGUI>().text = "Receipt found for Product = " + item.definition.id.ToString() + "transaction id is " + item.transactionID.ToString();

                transactionIdReceived = item.transactionID.ToString();
                selectedId = item.definition.id.ToString();

                if (isAddingSubscription)
                {
                    AddSubscriptionData();
                    isAddingSubscription = false;
                }
            }
        }
    }

    public void GetProfile() => StartCoroutine(GetKidProfile_Coroutine());


    IEnumerator GetKidProfile_Coroutine()
    {
        // outputArea.text = "Loading...";
       
        string uri = baseURL + "students/view";

        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            Debug.Log("auth key is " + auth_key);
             request.SetRequestHeader("Authorization", auth_key);

            yield return request.SendWebRequest();
           
            string jsonString = request.downloadHandler.text;
            Debug.Log(jsonString);
            profile = JsonUtility.FromJson<KidsProfile>(jsonString);
            Debug.Log(profile.is_trial_subscription);
            Debug.Log(profile.available_level);

            levelId = profile.age_group_id;

            if (profile.available_level == 0)
            {
                restoreButton.gameObject.SetActive(false);

                Vector3 subscribeLaterPos = subscribeLater.transform.position;
                subscribeLaterPos.x += 280f;
                subscribeLater.transform.position = subscribeLaterPos;
                // subscribeLater
            }
            else if (profile.available_level <= 15 )
            {
                Vector3 subscribeLaterPos = subscribeLater.transform.position;
                subscribeLaterPos.x += 280f;
                subscribeLater.transform.position = subscribeLaterPos;
                 restoreButton.gameObject.SetActive(false);
                 subscribeLater.enabled = true;
                subscribeLater.GetComponent<Image>().sprite = subscribeLaterImage; 
               

            }
            else
            {
                restoreButton.gameObject.SetActive(true);
            }

            if ((profile.is_trial_subscription == false && profile.available_level > 0) && profile.remaining_trial == 0)
            {
                subscribeLater.enabled = false;
                subscribeLater.GetComponent<Image>().sprite = Image1;

            }
            
            if (profile.remaining_trial > 0 )
            {
                subscribeLater.enabled = true;
                subscribeLater.GetComponent<Image>().sprite = subscribeLaterImage; 
                
            }
            else
            {
                subscribeLater.enabled = false;
                subscribeLater.GetComponent<Image>().sprite = Image1;
            }
            SetTitle();
             InitializePurchasing();
            request.Dispose();
        }
        
    }

    public void AddTrial()
    {
        string confirmationMessage = "You have " + profile.remaining_trial + " chances to hop between 3 levels within the trial period of 5 days to finalise one.";
        Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    confirmationMessage,
                    "Cancel",
                    "Continue",
                    AddSubscriptionTrial
                    );
    }

    public void AddSubscriptionData() => StartCoroutine(AddSubscription_Coroutine());
    public void AddSubscriptionTrial() => StartCoroutine(AddTrialSubscription_Coroutine());
    public void SaveLog() => StartCoroutine(SaveLog_Coroutine());

    
    IEnumerator SaveLog_Coroutine()
    {
       
        SubscriptionLoad loadData = new SubscriptionLoad {type="Payment API", log = infoLoad};
        string json = JsonUtility.ToJson(loadData);

        Debug.Log(json);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        string uri = baseURL + "/logs/save";

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", auth_key);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

        }
    }

    IEnumerator AddSubscription_Coroutine()
    {
        int randomNumber = Random.Range(2000, 3000); // randomNumber.ToString()
    // transactionIdReceived
        int isRenew = 0;
        if (stateOfApp == "Renew")
        {
            isRenew = 1;
        }
        SubscriptionForm subscriptionFormData = new SubscriptionForm { transaction_id = transactionIdReceived, platform = typeOfPlatform, platform_plan_id = selectedId, is_renew = isRenew };
        string json = JsonUtility.ToJson(subscriptionFormData);

        Debug.Log(json);
        // payload.GetComponent<TextMeshProUGUI>().text = json;

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        string uri = baseURL + "student-subscriptions";

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", auth_key);

        yield return request.SendWebRequest();

        // payload.GetComponent<TextMeshProUGUI>().text = json + " \n " + request.downloadHandler.text;

        if (request.result != UnityWebRequest.Result.Success)
        {
             loadingIndicator.enabled = false;
            Indicator.SetActive(false);
            Debug.Log("Error: " + request.error);
            isAddingSubscription = false;
            // SceneManager.LoadScene("Dashboard");
            // transactionId.GetComponent<TextMeshProUGUI>().text = (request.responseCode).ToString()  + "Error: " + request.error;
        }
        else
        {
            // transactionId.GetComponent<TextMeshProUGUI>().text = (request.responseCode).ToString();
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);
            
            // SceneManager.LoadScene("KidsName");
            PlayerPrefs.SetString("isSubscribed", "true");
             loadingIndicator.enabled = false;
            Indicator.SetActive(false);
            SceneManager.LoadScene("Dashboard");

            // {"transaction_id":"14277687","platform":"apple","platform_plan_id":"com.techies.edugogy.onemonth","student_id":3,"age_group_id":2,"plan_id":2,"plan_title":"1 Month","plan_term":"30 day","number_of_level":30,"start_at":1655356249,"expire_at":1657948249,"created_at":1652797078,"updated_at":1652797078,"id":4}
        }
    }

    IEnumerator AddTrialSubscription_Coroutine()
    {
        ErrorList list = new ErrorList();
        Error error = new Error();

        int randomNumber = Random.Range(10000, 90000);

        TrialSubscriptionForm subscriptionFormData = new TrialSubscriptionForm { transaction_id = randomNumber.ToString(), platform = typeOfPlatform, platform_plan_id = "trial" };
        string json = JsonUtility.ToJson(subscriptionFormData);

        Debug.Log(json);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        string uri = baseURL + "student-subscriptions";

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", auth_key);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);

            string jsonString = request.downloadHandler.text;

            list = JsonUtility.FromJson<ErrorList>(jsonString);

            Debug.Log(list);
            
            string message = list.error[0].detail;

            Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    message,
                    "Cancel",
                    "Subscribe Now",
                    GoSubscribe
                    );

        }
        else
        {
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);
            // SceneManager.LoadScene("KidsName");
            PlayerPrefs.SetString("isSubscribed", "true");
            SceneManager.LoadScene("Dashboard");
        }
    }

    public void GoSubscribe()
    {
        Debug.Log("Subscribe now");
    }

    public void OnButtonClick(Button thisButton)
    {
        if (thisButton.tag == "Exploration")
        {
            thisButton.GetComponent<Image>().sprite = selectedSpaceExploration;
             WalkButton.GetComponent<Image>().sprite = inActiveSpaceWalk;
              CaptureButton.GetComponent<Image>().sprite = inActiveSpaceCapture;
        }
        else if (thisButton.tag == "Walk")
        {
            thisButton.GetComponent<Image>().sprite = selectedSpaceWalk;
             ExplorationButton.GetComponent<Image>().sprite = inActiveSpaceExploration;
              CaptureButton.GetComponent<Image>().sprite = inActiveSpaceCapture;
        }
        else if (thisButton.tag == "Capture")
        {
            thisButton.GetComponent<Image>().sprite = selectedSpaceCapture;
             WalkButton.GetComponent<Image>().sprite = inActiveSpaceWalk;
             ExplorationButton.GetComponent<Image>().sprite = inActiveSpaceExploration;
        }
    }

    public void OnPurchaseComplete(Product product)
    {
        // transactionId.GetComponent<TextMeshProUGUI>().text = "Completing Purchase";
        selectedId = product.definition.id;
        if (product.definition.id == productId)
        {
            Debug.Log("Subscription successful");

        }
       

        // var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        // // Get a reference to IAppleConfiguration during IAP initialization.
        // var appleConfig = builder.Configure<IAppleConfiguration>();
        // var receiptData = System.Convert.FromBase64String(appleConfig.appReceipt);
        // AppleReceipt receipt = new AppleValidator(AppleTangle.Data()).Validate(receiptData);

        // Debug.Log(receipt.bundleID);
        // Debug.Log(receipt.receiptCreationDate);

        // // transactionId.GetComponent<TextMeshProUGUI>().text = Application.identifier + "" + receipt.receiptCreationDate;

        // foreach (AppleInAppPurchaseReceipt productReceipt in receipt.inAppPurchaseReceipts) {
        //     Debug.Log(productReceipt.transactionID);
        //     Debug.Log(productReceipt.productID);
        //     transactionIdReceived = productReceipt.transactionID;
        //     // transactionId.GetComponent<TextMeshProUGUI>().text = productReceipt.transactionID;
        //     Debug.Log("Reading Purchase receipt");
        //     transactionId.GetComponent<TextMeshProUGUI>().text = "Reading Purchase receipt";
        //     if (isAddingSubscription && transactionIdReceived != "")
        //     {
        //         transactionId.GetComponent<TextMeshProUGUI>().text = "subs-id " + productReceipt.transactionID;

        //         AddSubscriptionData();
        //         isAddingSubscription = false;
        //     }
        // }
        
        
    }

    public void RegisterFailure() => StartCoroutine(AddPurchaseFailure_Coroutine());

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("Purchase of " + product.definition.id + " failed due to" + reason);


        Debug.Log($"Processing Purchase: {product.transactionID}");

        selectedId = product.definition.id;
        transactionIdReceived = product.transactionID.ToString();

        failureReason = reason.ToString();
    
        RegisterFailure();
        
    }



    IEnumerator AddPurchaseFailure_Coroutine()
    {
      
        PurchaseFailedForm purchaseFailedFormData = new PurchaseFailedForm { transaction_id = transactionIdReceived, platform = typeOfPlatform, platform_plan_id = selectedId, reason = failureReason };
        string json = JsonUtility.ToJson(purchaseFailedFormData);

        Debug.Log(json);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        string uri = baseURL + "student-subscriptions";

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", auth_key);

        yield return request.SendWebRequest();

        // payload.GetComponent<TextMeshProUGUI>().text = json + " \n " + request.downloadHandler.text;

        if (request.result != UnityWebRequest.Result.Success)
        {
             loadingIndicator.enabled = false;
            Indicator.SetActive(false);
            Debug.Log("Error: " + request.error);
            
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);
        
            loadingIndicator.enabled = false;
            Indicator.SetActive(false);


             string confirmationMessage = "Your purchase has failed";
        Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    confirmationMessage,
                    "Try again",
                    "Cancel",
                    GoToDashboard
                    );
            

        }
    }

    public void GoToDashboard()
    {
        SceneManager.LoadScene("Dashboard");
    }

    public void BuyProduct(string productName)
    {
        // m_AppleExtensions.PresentCodeRedemptionSheet(); 
        //check combination of product type and level based on that assign product name

        if (productName == "onemonth")
        {
            if (levelId == 1)
            {
                productId = spaceExplorationIdBeginner;
            }
            else if(levelId == 2)
            {
                productId = spaceExplorationIdLearner;
            }
            else if(levelId == 3)
            {
                productId = spaceExplorationIdSeeker;
            }
            else if(levelId == 4)
            {
                productId = spaceExplorationIdIntermediate;
            }
            else if(levelId == 5)
            {
                productId = spaceExplorationIdGraduate;
            }
            else if(levelId == 6)
            {
                productId = spaceExplorationIdMaster;
            }
            else if(levelId == 7)
            {
                productId = spaceExplorationIdWordsmith;
            }
           
        }
        else if (productName == "threemonth")
        {
            if (levelId == 1)
            {
                productId = spaceWalkIdBeginner;
            }
            else if(levelId == 2)
            {
                productId = spaceWalkIdLearner;
            }
            else if(levelId == 3)
            {
                productId = spaceWalkIdSeeker;
            }
            else if(levelId == 4)
            {
                productId = spaceWalkIdIntermediate;
            }
            else if(levelId == 5)
            {
                productId = spaceWalkIdGraduate;
            }
            else if(levelId == 6)
            {
                productId = spaceWalkIdMaster;
            }
            else if(levelId == 7)
            {
                productId = spaceWalkIdWordsmith;
            }
        }
         else if (productName == "sixmonth")
        {
            if (levelId == 1)
            {
                productId = spaceCaptureIdBeginner;
            }
            else if(levelId == 2)
            {
                productId = spaceCaptureIdLearner;
            }
            else if(levelId == 3)
            {
                productId = spaceCaptureIdSeeker;
            }
            else if(levelId == 4)
            {
                productId = spaceCaptureIdIntermediate;
            }
            else if(levelId == 5)
            {
                productId = spaceCaptureIdGraduate;
            }
            else if(levelId == 6)
            {
                productId = spaceCaptureIdMaster;
            }
            else if(levelId == 7)
            {
                productId = spaceCaptureIdWordsmith;
            }
        }

        
        m_StoreController.InitiatePurchase(productId);
         
        loadingIndicator.enabled = true;
         Indicator.SetActive(true);
         stateOfApp = "Purchase";
          

        
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        //  loadingIndicator.enabled = true;
        //  Indicator.SetActive(true);

        if (stateOfApp != "Purchase")
        {
            stateOfApp = "Renew";
        }
        

         var product = e.purchasedProduct;

        Debug.Log($"Processing Purchase: {product.definition.id}");

            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.tvOS)
            {
                var thisreceipt = m_AppleExtensions.GetTransactionReceiptForProduct(product);
                Debug.Log($"Product receipt for deferred purchase: {thisreceipt}");
                // transactionId.GetComponent<TextMeshProUGUI>().text = thisreceipt.purchaseDate + thisreceipt.subscriptionExpirationDate;
                // Send transaction receipt to server for validation

                if (product.definition.type == ProductType.Subscription) {
                string intro_json = (dict == null || !dict.ContainsKey(product.definition.storeSpecificId)) ? null :  dict[product.definition.storeSpecificId];
                SubscriptionManager p = new SubscriptionManager(product, intro_json);
                SubscriptionInfo info = p.getSubscriptionInfo();
                Debug.Log(info.getProductId());
                Debug.Log(info.getPurchaseDate());
                Debug.Log(info.getExpireDate());
                Debug.Log(info.isSubscribed());
                Debug.Log(info.isExpired());
                Debug.Log(info.isCancelled());
                Debug.Log(info.isFreeTrial());
                Debug.Log(info.isAutoRenewing());
                Debug.Log(info.getRemainingTime());
                Debug.Log(info.isIntroductoryPricePeriod());
                Debug.Log(info.getIntroductoryPrice());
                Debug.Log(info.getIntroductoryPricePeriod());
                Debug.Log(info.getIntroductoryPricePeriodCycles());
                DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(info.getPurchaseDate());
                infoLoad = "product id is " + info.getProductId() + " purchase date is " + info.getPurchaseDate() + " expired date " + info.getExpireDate() + " isSubscribed " + info.isSubscribed() + " isExpired " + info.isExpired() + " isCancelled " + info.isCancelled() + " isFreeTrial " + info.isFreeTrial() + " isAutoRenewing " + info.isAutoRenewing() + " getRemainingTime " + info.getRemainingTime() + " transaction id is " + e.purchasedProduct.transactionID;
                SaveLog();
                // transactionId.GetComponent<TextMeshProUGUI>().text = "ED is " + info.getExpireDate() + " PD is " + info.getPurchaseDate() + " RT is " + info.getRemainingTime() + " for product id " + info.getProductId() + " local PD is " + dt;

            } else {
                Debug.Log("the product is not a subscription product");
            }
                
            }

        processedTransactionId = e.purchasedProduct.transactionID;
        Debug.Log("getting Purchase transaction id" + e.purchasedProduct.transactionID);// + e.purchasedProduct.SubscriptionInfo());// + e.purchasedProduct.isSubscriptionActive + );
        transactionIdReceived = e.purchasedProduct.transactionID;

        // transactionId.GetComponent<TextMeshProUGUI>().text = "tid is " + transactionIdReceived;
         selectedId = product.definition.id;

        if (processedTransactionId == "")
        {
            loadingIndicator.enabled = false;
             Indicator.SetActive(false);
        }

         if (isAddingSubscription)
         {       
            AddSubscriptionData();
            isAddingSubscription = false;
         }

        
        // transactionId.GetComponent<TextMeshProUGUI>().text = "purchase product id is " + e.purchasedProduct.transactionID;

        // if (String.Equals(e.purchasedProduct.definition.id, spaceExplorationID, StringComparison.Ordinal))
        // {
            
        //     Debug.Log("Purchase completion going on");
        //     AddSubscriptionData();
        // }
        // else
        // {
        //     Debug.Log("Purchase Failed");
        // }

        // if (purchaseReceipt.productID == productId
        // && ((purchaseReceipt as GooglePlayReceipt)?.purchaseToken == transactionId
        //     || (purchaseReceipt as AppleReceipt)?.transactionID == transactionID))
        // {
            
        //     break;
        // }

        Debug.Log("Reading receipt");
        // transactionId.GetComponent<TextMeshProUGUI>().text = "Reading receipt";
        bool validPurchase = true;
    
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                AppleStoreKitTestTangle.Data(), Application.identifier);
       

        try
        {
            // On Google Play, result has a single product ID.
            // On Apple stores, receipts contain multiple products.
            var result = validator.Validate(e.purchasedProduct.receipt);
            // For informational purposes, we list the receipt(s)
            Debug.Log("Receipt is valid. Contents:");
            foreach (IPurchaseReceipt productReceipt in result)
            {
                Debug.Log(productReceipt.productID);
                Debug.Log(productReceipt.purchaseDate);
                Debug.Log(productReceipt.transactionID);


                // AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
                // if (null != apple)
                // {
                //     Debug.Log(apple.transactionID);
                //     Debug.Log(apple.purchaseDate);
                //     Debug.Log(apple.originalTransactionIdentifier);
                //     Debug.Log(apple.subscriptionExpirationDate);
                //     Debug.Log(apple.cancellationDate);
                //     Debug.Log(apple.quantity);
                //     transactionIdReceived = apple.transactionID;
                //     transactionId.GetComponent<TextMeshProUGUI>().text = "pd is" + apple.purchaseDate + " sed is " + apple.subscriptionExpirationDate + " cd is " + apple.cancellationDate;
                // }
            }

        }
        catch (IAPSecurityException)
        {
            Debug.Log("Invalid receipt, not unlocking content");
            validPurchase = false;
        }

        return PurchaseProcessingResult.Complete;

    }

    public void RestoreTransactions() {

        stateOfApp = "Renew";
        restoreButton.enabled = false;
        ListProducts();
       
    //     var thisreceipt = m_AppleExtensions.GetTransactionReceiptForProduct(spaceExplorationID);
    //     Debug.Log($"Product receipt for deferred purchase: {thisreceipt}");
    //    transactionId.GetComponent<TextMeshProUGUI>().text = thisreceipt;
        Debug.Log("Checking for restoration");
         bool isActive = false;
       // -----
    
    //    var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
    //     var appleConfig = builder.Configure<IAppleConfiguration>();
    //     var receiptData = System.Convert.FromBase64String(appleConfig.appReceipt);
    //     AppleReceipt receipt = new AppleValidator(AppleTangle.Data()).Validate(receiptData);

   


    // ------
        // Debug.Log("Receipt bundle is " + receipt.bundleID);
        // Debug.Log("Receipt date is " + receipt.receiptCreationDate);

        // transactionId.GetComponent<TextMeshProUGUI>().text = Application.identifier + "" + receipt.receiptCreationDate;

        // foreach (AppleInAppPurchaseReceipt productReceipt in receipt.inAppPurchaseReceipts) {
        //     Debug.Log(productReceipt.transactionID);
        //     Debug.Log(productReceipt.productID);
        //     // Debug.Log(productReceipt.subscriptionExpirationDate);
        //     DateTime expirationDate = productReceipt.subscriptionExpirationDate;
        //     Debug.Log("Checking for expirationDate " + expirationDate + "& product id is " + productReceipt.productID);

        //     DateTime now = DateTime.Now;
        //     //DateTime cancellationDate = apple.cancellationDate;

           
        //     transactionIdReceived = productReceipt.transactionID;
        //     selectedId = productReceipt.productID;
        //     transactionId.GetComponent<TextMeshProUGUI>().text =  productReceipt.transactionID + " Checking for expirationDate " + expirationDate + "& product id is " + productReceipt.productID;
            
            // if(DateTime.Compare(now, expirationDate) < 0)
            // {
            //       isActive = true;
            //       Debug.Log("Checking for true expirationDate " + expirationDate + "& product id is " + productReceipt.productID);

            //     transactionId.GetComponent<TextMeshProUGUI>().text = "transaction id is " + productReceipt.transactionID + " product id is " + productReceipt.productID;

            // }
            //  else
            // {
            //     transactionId.GetComponent<TextMeshProUGUI>().text = "false";
            // }
          
            // if (processedTransactionId == transactionIdReceived)
            // {
            //     transactionId.GetComponent<TextMeshProUGUI>().text = "transaction id is " + productReceipt.transactionID + " product id is " + productReceipt.productID;
                
            // }
            // else
            // {
            //      transactionId.GetComponent<TextMeshProUGUI>().text = "Process purchase not working ";
            // }

        // }
    
    }

    public bool isSubscriptionActive(AppleInAppPurchaseReceipt appleReceipt)
    {

        bool isActive = false;
        AppleInAppPurchaseReceipt apple = appleReceipt;
        if (null != apple)
        {
            DateTime expirationDate = apple.subscriptionExpirationDate;
            DateTime now = DateTime.Now;
            //DateTime cancellationDate = apple.cancellationDate;
            if(DateTime.Compare(now, expirationDate) < 0)
            {
                isActive = true;
                // transactionId.GetComponent<TextMeshProUGUI>().text = "transaction id is " + apple.transactionID + " product id is " + apple.productID;
            }
            else
            {
                // transactionId.GetComponent<TextMeshProUGUI>().text = "false";
            }
        }
 
        return isActive;
    }
    
}



 public static class Extensions
 {
      public static void SetTransparency(this UnityEngine.UI.Image p_image, float p_transparency)
      {
          if (p_image != null)
          {
              UnityEngine.Color __alpha = p_image.color;
              __alpha.a = p_transparency;
              p_image.color = __alpha;
          }
      }
 }