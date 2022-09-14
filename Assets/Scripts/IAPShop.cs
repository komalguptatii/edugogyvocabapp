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
    private string timePeriod = "com.techies.edugogy.sixmonth";
    private string subPeriod = "com.techies.edugogy.threemonth";
    string auth_key;
    [SerializeField] public TextMeshProUGUI transactionId;

     string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";

    public class SubscriptionForm
    {
        public string transaction_id;

        public string platform;

        public string platform_plan_id;
    }
    private void Start()
    {
        //     StandardPurchasingModule.Instance().useFakeStoreAlways = true;
        if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
            // auth_key = "Bearer KWDs6ZofHH8-obBDw3rOb4VYeHq-QR55";
        }
        // AddSubscriptionData(); // otherwise call on receipt validation and information received

    }

    public void AddSubscriptionData() => StartCoroutine(AddSubscription_Coroutine());

    IEnumerator AddSubscription_Coroutine()
    {
        int randomNumber = Random.Range(2000, 3000);

        SubscriptionForm subscriptionFormData = new SubscriptionForm { transaction_id = randomNumber.ToString(), platform = "apple", platform_plan_id = "com.techies.edugogy.sixmonth" };
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
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);
            // SceneManager.LoadScene("KidsName");
            PlayerPrefs.SetString("isSubscribed", "true");
            SceneManager.LoadScene("Dashboard");

            // {"transaction_id":"14277687","platform":"apple","platform_plan_id":"com.techies.edugogy.onemonth","student_id":3,"age_group_id":2,"plan_id":2,"plan_title":"1 Month","plan_term":"30 day","number_of_level":30,"start_at":1655356249,"expire_at":1657948249,"created_at":1652797078,"updated_at":1652797078,"id":4}
        }
    }

    public void OnPurchaseComplete(Product product)
    {
        if (product.definition.id == timePeriod)
        {
            Debug.Log("Subscription successful for one month");

        }

        if (product.definition.id == subPeriod)
        {
            Debug.Log("Subscription successful for three month");
        }
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason reason)
    {
        Debug.Log("Purchase of " + product.definition.id + " failed due to" + reason);
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        bool validPurchase = true;
        var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                AppleTangle.Data(), Application.identifier);
        Debug.Log("Checking for tangle data under purchase process");
        Debug.Log(validator + " value of validator");

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
                    transactionId.GetComponent<TextMeshProUGUI>().text = google.transactionID;

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
                    transactionId.GetComponent<TextMeshProUGUI>().text = apple.transactionID;

                }
            }

        }
        catch (IAPSecurityException)
        {
            Debug.Log("Invalid receipt, not unlocking content");
            validPurchase = false;
        }



        return PurchaseProcessingResult.Complete;



        // Unlock the appropriate content here.

        // if (e.purchasedProduct.receipt != null)
        // {
        //     if (e.purchasedProduct.definition.type == ProductType.Subscription)
        //     {
        //         // if (checkIfProductIsAvailableForSubscriptionManager(e.purchasedProduct.receipt))
        //         // {
        //         string intro_json = (introductory_info_dict == null || !introductory_info_dict.ContainsKey(item.definition.storeSpecificId)) ? null : introductory_info_dict[item.definition.storeSpecificId];
        //         SubscriptionManager p = new SubscriptionManager(e.purchasedProduct, intro_json);
        //         SubscriptionInfo info = p.getSubscriptionInfo();
        //         Debug.Log("product id is: " + info.getProductId());
        //         Debug.Log("purchase date is: " + info.getPurchaseDate());
        //         Debug.Log("subscription next billing date is: " + info.getExpireDate());
        //         Debug.Log("is subscribed? " + info.isSubscribed().ToString());
        //         Debug.Log("is expired? " + info.isExpired().ToString());
        //         Debug.Log("is cancelled? " + info.isCancelled());
        //         Debug.Log("product is in free trial peroid? " + info.isFreeTrial());
        //         Debug.Log("product is auto renewing? " + info.isAutoRenewing());
        //         Debug.Log("subscription remaining valid time until next billing date is: " + info.getRemainingTime());
        //         Debug.Log("is this product in introductory price period? " + info.isIntroductoryPricePeriod());
        //         Debug.Log("the product introductory localized price is: " + info.getIntroductoryPrice());
        //         Debug.Log("the product introductory price period is: " + info.getIntroductoryPricePeriod());
        //         Debug.Log("the number of product introductory price period cycles is: " + info.getIntroductoryPricePeriodCycles());
        //         // }
        //         // else
        //         // {
        //         //     Debug.Log("This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class.");
        //         // }
        //     }
        // }




    }
}
