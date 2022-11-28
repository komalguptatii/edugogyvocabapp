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


public class IAPShop : MonoBehaviour
{
    private string spaceExplorationID = "com.techies.edugogy.onemonth";
    private string spaceWalkID = "com.techies.edugogy.sixmonth";
    private string spaceCaptureID = "com.techies.edugogy.threemonth";
    string auth_key;
    // [SerializeField] public TextMeshProUGUI transactionId;
    // [SerializeField] public TextMeshProUGUI payload;

    private IAppleExtensions m_AppleExtensions;
    private IStoreController m_Controller;


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

     string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";

     [SerializeField]
    public Button subscribeLater;

    public class SubscriptionForm
    {
        public string transaction_id;

        public string platform;

        public string platform_plan_id;
    }

  
    string selectedId = "";
    string typeOfPlatform = "";
    public Sprite Image1;

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
    private void Awake()
    {
        subscribeNowButton.enabled = false;
          if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
            
        }
        // auth_key = "Bearer r5Gl713cqD2iCuL4ufEUDhkgpoLbe7-4";  
    }

    private void Start()
    {
        //     StandardPurchasingModule.Instance().useFakeStoreAlways = true;
      
       if (Application.platform == RuntimePlatform.Android)
        {
            typeOfPlatform = "android";
        }
        else
        {
            typeOfPlatform = "apple";
        }
         
        GetProfile();
        // AddSubscriptionData(); // otherwise call on receipt validation and information received

    }

    // void Update()
    // {
    //     if (validPurchase)
    //     {
    //         transactionId.GetComponent<TextMeshProUGUI>().text = receiptReceived;
    //         validPurchase = false;
    //     }
    // }

    // public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
    // {
    //     m_Controller = controller;
    //     m_AppleExtensions = extensions.GetExtension<IAppleExtensions> ();

    //     // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
    //     // On non-Apple platforms this will have no effect; OnDeferred will never be called.
    //     m_AppleExtensions.RegisterPurchaseDeferredListener (OnDeferred);
    // }

    // private void OnDeferred (Product item)
    // {
    //     Debug.Log ("Purchase deferred: " + item.definition.id);
    // }

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

            if ((profile.is_trial_subscription == false && profile.available_level > 0) || profile.remaining_trial == 0)
            {
                subscribeLater.enabled = false;
                 subscribeLater.GetComponent<Image>().sprite = Image1;

                //opacity
                // subscribeLater.GetComponent<Image>().SetTransparency(50.0f);

            }
        
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


    IEnumerator AddSubscription_Coroutine()
    {
        int randomNumber = Random.Range(2000, 3000); // randomNumber.ToString()
    // transactionIdReceived
        SubscriptionForm subscriptionFormData = new SubscriptionForm { transaction_id = transactionIdReceived, platform = typeOfPlatform, platform_plan_id = selectedId };
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
            Debug.Log("Error: " + request.error);
            isAddingSubscription = false;
            // transactionId.GetComponent<TextMeshProUGUI>().text = (request.responseCode).ToString()  + "Error: " + request.error;
        }
        else
        {
            // transactionId.GetComponent<TextMeshProUGUI>().text = (request.responseCode).ToString();
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);
            // SceneManager.LoadScene("KidsName");
            PlayerPrefs.SetString("isSubscribed", "true");
            SceneManager.LoadScene("Dashboard");

            // {"transaction_id":"14277687","platform":"apple","platform_plan_id":"com.techies.edugogy.onemonth","student_id":3,"age_group_id":2,"plan_id":2,"plan_title":"1 Month","plan_term":"30 day","number_of_level":30,"start_at":1655356249,"expire_at":1657948249,"created_at":1652797078,"updated_at":1652797078,"id":4}
        }
    }

    IEnumerator AddTrialSubscription_Coroutine()
    {
        ErrorList list = new ErrorList();
        Error error = new Error();

        int randomNumber = Random.Range(1000, 2000);

        SubscriptionForm subscriptionFormData = new SubscriptionForm { transaction_id = randomNumber.ToString(), platform = typeOfPlatform, platform_plan_id = "trial" };
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
        selectedId = product.definition.id;
        if (product.definition.id == spaceExplorationID)
        {
            Debug.Log("Subscription successful for one month");

        }
        else if (product.definition.id == spaceWalkID)
        {
            Debug.Log("Subscription successful for three month");

        }
        else if (product.definition.id == spaceCaptureID)
        {
            Debug.Log("Subscription successful for capture month");
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        // Get a reference to IAppleConfiguration during IAP initialization.
        var appleConfig = builder.Configure<IAppleConfiguration>();
        var receiptData = System.Convert.FromBase64String(appleConfig.appReceipt);
        AppleReceipt receipt = new AppleValidator(AppleTangle.Data()).Validate(receiptData);

        Debug.Log(receipt.bundleID);
        Debug.Log(receipt.receiptCreationDate);

        // transactionId.GetComponent<TextMeshProUGUI>().text = Application.identifier + "" + receipt.receiptCreationDate;

        foreach (AppleInAppPurchaseReceipt productReceipt in receipt.inAppPurchaseReceipts) {
            Debug.Log(productReceipt.transactionID);
            Debug.Log(productReceipt.productID);
            transactionIdReceived = productReceipt.transactionID;
            //  transactionId.GetComponent<TextMeshProUGUI>().text = productReceipt.transactionID;

            if (isAddingSubscription && transactionIdReceived != "")
            {
                AddSubscriptionData();
                isAddingSubscription = false;
            }
        }
        
        
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("Purchase of " + product.definition.id + " failed due to" + reason);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {

        bool validPurchase = true;
        // transactionId.GetComponent<TextMeshProUGUI>().text = "true";//productReceipt.transactionID;
        //             transactionId.GetComponent<TextMeshProUGUI>().text = e.purchasedProduct.definition.id;


        // if (String.Equals(e.purchasedProduct.definition.id, productName, StringComparison.Ordinal))
        // {
        //     Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", e.purchasedProduct.definition.id));
        //     // TODO: The non-consumable item has been successfully purchased, grant this item to the player.
        //     transactionId.GetComponent<TextMeshProUGUI>().text = e.purchasedProduct.definition.id;
        // }
        // else
        // {
        //     Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", e.purchasedProduct.definition.id));
        //                 transactionId.GetComponent<TextMeshProUGUI>().text = e.purchasedProduct.definition.id;

        // }


        // if (Application.platform == RuntimePlatform.IPhonePlayer ||
        //     Application.platform == RuntimePlatform.tvOS) {
        //         validPurchase = true;

        //     string transactionReceipt = m_AppleExtensions.GetTransactionReceiptForProduct (e.purchasedProduct);
        //     receiptReceived = transactionReceipt;
        //     // transactionId.GetComponent<TextMeshProUGUI>().text = transactionReceipt;
        //     // Console.WriteLine (transactionReceipt);
        //     // Send transaction receipt to server for validation
        // }    
        //  return (validPurchase) ? PurchaseProcessingResult.Complete : PurchaseProcessingResult.Pending;

        // string msg = "value is " + validPurchase;
        //  transactionId.GetComponent<TextMeshProUGUI>().text = msg;

        // OnPurchaseComplete.Invoke(e.purchasedProduct);
        // bool validPurchase = true;

        #if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        // Get a reference to IAppleConfiguration during IAP initialization.
        var appleConfig = builder.Configure<IAppleConfiguration>();
        var receiptData = System.Convert.FromBase64String(appleConfig.appReceipt);
        AppleReceipt receipt = new AppleValidator(AppleTangle.Data()).Validate(receiptData);

        Debug.Log(receipt.bundleID);
        Debug.Log(receipt.receiptCreationDate);
        foreach (AppleInAppPurchaseReceipt productReceipt in receipt.inAppPurchaseReceipts) {
            Debug.Log(productReceipt.transactionID);
            Debug.Log(productReceipt.productID);
            // transactionId.GetComponent<TextMeshProUGUI>().text = productReceipt.transactionID;
        }
        #endif

        // #if !DEBUG_STOREKIT_TEST
        // var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
        //         AppleTangle.Data(), Application.identifier);
        // #else
        //     var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
        //         AppleStoreKitTestTangle.Data(), Application.identifier);
        // #endif
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
                // transactionId.GetComponent<TextMeshProUGUI>().text = productReceipt.transactionID;

                GooglePlayReceipt google = productReceipt as GooglePlayReceipt;
                if (null != google)
                {
                    // This is Google's Order ID.
                    // Note that it is null when testing in the sandbox
                    // because Google's sandbox does not provide Order IDs.
                    Debug.Log(google.transactionID);
                    Debug.Log(google.purchaseDate);
                    Debug.Log(google.purchaseState);
                    Debug.Log(google.purchaseToken);
                    transactionIdReceived = google.transactionID;

                    // transactionId.GetComponent<TextMeshProUGUI>().text = google.transactionID;
                    // AddSubscriptionData();

                }


                AppleInAppPurchaseReceipt apple = productReceipt as AppleInAppPurchaseReceipt;
                if (null != apple)
                {
                    Debug.Log(apple.transactionID);
                    Debug.Log(apple.purchaseDate);
                    Debug.Log(apple.originalTransactionIdentifier);
                    Debug.Log(apple.subscriptionExpirationDate);
                    Debug.Log(apple.cancellationDate);
                    Debug.Log(apple.quantity);
                    transactionIdReceived = apple.originalTransactionIdentifier;
                    // transactionId.GetComponent<TextMeshProUGUI>().text = apple.originalTransactionIdentifier;
                    // AddSubscriptionData();
                }
            }

        }
        catch (IAPSecurityException)
        {
            Debug.Log("Invalid receipt, not unlocking content");
            validPurchase = false;
        }

        if (validPurchase)
        {
            // transactionId.GetComponent<TextMeshProUGUI>().text = transactionIdReceived;

            AddSubscriptionData();
        }

        return PurchaseProcessingResult.Complete;



        // Unlock the appropriate content here.

        // if (e.purchasedProduct.receipt != null)
        // {
        //     if (e.purchasedProduct.definition.type == ProductType.Subscription)
        //     {
        //         // if (checkIfProductIsAvailableForSubscriptionManager(e.purchasedProduct.receipt))
        //         // {
        //         // string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(item.definition.storeSpecificId)) ? null : introductory_info_dict[item.definition.storeSpecificId];
        //         // SubscriptionManager p = new SubscriptionManager(e.purchasedProduct, intro_json);
        //         // SubscriptionInfo info = p.getSubscriptionInfo();
        //         // Debug.Log("product id is: " + info.getProductId());
        //         // Debug.Log("purchase date is: " + info.getPurchaseDate());
        //         // Debug.Log("subscription next billing date is: " + info.getExpireDate());
        //         // Debug.Log("is subscribed? " + info.isSubscribed().ToString());
        //         // Debug.Log("is expired? " + info.isExpired().ToString());
        //         // Debug.Log("is cancelled? " + info.isCancelled());
        //         // Debug.Log("product is in free trial peroid? " + info.isFreeTrial());
        //         // Debug.Log("product is auto renewing? " + info.isAutoRenewing());
        //         // Debug.Log("subscription remaining valid time until next billing date is: " + info.getRemainingTime());
        //         // Debug.Log("is this product in introductory price period? " + info.isIntroductoryPricePeriod());
        //         // Debug.Log("the product introductory localized price is: " + info.getIntroductoryPrice());
        //         // Debug.Log("the product introductory price period is: " + info.getIntroductoryPricePeriod());
        //         // Debug.Log("the number of product introductory price period cycles is: " + info.getIntroductoryPricePeriodCycles());
        //             transactionId.GetComponent<TextMeshProUGUI>().text = "Checking subscription info";
        //             AddSubscriptionData();

        //         // }
        //         // else
        //         // {
        //         //     Debug.Log("This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class.");
        //         // }
        //     }
        // }




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