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
    private string spaceExplorationID = "com.techies.edugogy.onemonth";
    private string spaceWalkID = "com.techies.edugogy.threemonth";
    private string spaceCaptureID = "com.techies.edugogy.sixmonth";

    string auth_key;

    [SerializeField] public TextMeshProUGUI transactionId;
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

     string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";

     [SerializeField]
    public Button subscribeLater;
    [SerializeField]
    public Button restoreButton;

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
    string processedTransactionId = "";



    private void Awake()
    {
        // subscribeNowButton.enabled = false;
        if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
        }
         InitializePurchasing();
    }

    private void Start()
    {
        //     StandardPurchasingModule.Instance().useFakeStoreAlways = true;
      
       if (Application.platform == RuntimePlatform.Android)
        {
            typeOfPlatform = "google";
        }
        else
        {
            typeOfPlatform = "apple";
        }
         
        GetProfile();

    }

     void InitializePurchasing()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());

             builder.AddProduct("com.techies.edugogy.onemonth", ProductType.Subscription);
            builder.AddProduct("com.techies.edugogy.threemonth", ProductType.Subscription);
            builder.AddProduct("com.techies.edugogy.sixmonth", ProductType.Subscription);

            UnityPurchasing.Initialize(this, builder);
        }

    public void OnInitialized (IStoreController controller, IExtensionProvider extensions)
    {
         m_StoreController = controller;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();
       
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
        transactionId.GetComponent<TextMeshProUGUI>().text = "Initialization failed";
    }

    public void ListProducts()
    {
        // transactionId.GetComponent<TextMeshProUGUI>().text = "checking for all products";
        foreach (UnityEngine.Purchasing.Product item in m_StoreController.products.all)
        {
            // transactionId.GetComponent<TextMeshProUGUI>().text = "coming into loop";
            if (item.receipt != null)
            {
                Debug.Log("Receipt found for Product = " + item.definition.id.ToString());
                transactionId.GetComponent<TextMeshProUGUI>().text = "Receipt found for Product = " + item.definition.id.ToString() + "transaction id is " + item.transactionID.ToString();

                transactionIdReceived = item.transactionID.ToString();
                selectedId = item.definition.id.ToString();
                AddSubscriptionData();
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

            // if (profile.available_level == 0)
            // {
            //     restoreButton.gameObject.SetActive(false);
            //     subscribeNowButton.gameObject.SetActive(true);
            // }

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
            SceneManager.LoadScene("Dashboard");

            // {"transaction_id":"14277687","platform":"apple","platform_plan_id":"com.techies.edugogy.onemonth","student_id":3,"age_group_id":2,"plan_id":2,"plan_title":"1 Month","plan_term":"30 day","number_of_level":30,"start_at":1655356249,"expire_at":1657948249,"created_at":1652797078,"updated_at":1652797078,"id":4}
        }
    }

    IEnumerator AddTrialSubscription_Coroutine()
    {
        ErrorList list = new ErrorList();
        Error error = new Error();

        int randomNumber = Random.Range(10000, 90000);

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
        // transactionId.GetComponent<TextMeshProUGUI>().text = "Completing Purchase";
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

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("Purchase of " + product.definition.id + " failed due to" + reason);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
         var product = e.purchasedProduct;

            Debug.Log($"Processing Purchase: {product.definition.id}");

            if (Application.platform == RuntimePlatform.IPhonePlayer ||
                Application.platform == RuntimePlatform.tvOS)
            {
                var thisreceipt = m_AppleExtensions.GetTransactionReceiptForProduct(product);
                Debug.Log($"Product receipt for deferred purchase: {thisreceipt}");
                // transactionId.GetComponent<TextMeshProUGUI>().text = thisreceipt;
                // Send transaction receipt to server for validation
            }

        processedTransactionId = e.purchasedProduct.transactionID;
        Debug.Log("getting Purchase transaction id" + e.purchasedProduct.transactionID);
        transactionIdReceived = e.purchasedProduct.transactionID;
        transactionId.GetComponent<TextMeshProUGUI>().text = "tid is " + transactionIdReceived;
         selectedId = product.definition.id;
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
    
        // #if UNITY_ANDROID || UNITY_IOS || UNITY_STANDALONE_OSX
        // var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        // // Get a reference to IAppleConfiguration during IAP initialization.
        // var appleConfig = builder.Configure<IAppleConfiguration>();
        // var receiptData = System.Convert.FromBase64String(appleConfig.appReceipt);
        // AppleReceipt receipt = new AppleValidator(AppleTangle.Data()).Validate(receiptData);

        // Debug.Log(receipt.bundleID);
        // Debug.Log(receipt.receiptCreationDate);
        // foreach (AppleInAppPurchaseReceipt productReceipt in receipt.inAppPurchaseReceipts) {
        //     Debug.Log(productReceipt.transactionID);
        //     Debug.Log(productReceipt.productID);
        //     // transactionId.GetComponent<TextMeshProUGUI>().text = productReceipt.transactionID;
        // }
        // #endif

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
                //     transactionId.GetComponent<TextMeshProUGUI>().text = "apple identifier " + apple.originalTransactionIdentifier + " tid is " + apple.transactionID;
                //     // transactionId.GetComponent<TextMeshProUGUI>().text = apple.originalTransactionIdentifier;
                //     // AddSubscriptionData();
                // }
            }

        }
        catch (IAPSecurityException)
        {
            Debug.Log("Invalid receipt, not unlocking content");
            validPurchase = false;
        }

        // if (validPurchase)
        // {
        //     // transactionId.GetComponent<TextMeshProUGUI>().text = transactionIdReceived;

        //     AddSubscriptionData();
        // }

        return PurchaseProcessingResult.Complete;

    }

    public void RestoreTransactions() {

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
                transactionId.GetComponent<TextMeshProUGUI>().text = "transaction id is " + apple.transactionID + " product id is " + apple.productID;

            }
            else
            {
                transactionId.GetComponent<TextMeshProUGUI>().text = "false";
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