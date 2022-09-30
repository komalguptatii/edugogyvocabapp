using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using TMPro;
using Random=UnityEngine.Random;
using UnityEngine.SceneManagement;

public class KidsProfileManager : MonoBehaviour
{
       
       
    public TMP_InputField KidsName;
        public TMP_InputField AgeGroupInput;

    string auth_key;

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
    }

    
    [Serializable]
    public class AgeGroup
    {

        public int id;
        public string title;
        public int sort_order;
        public int total_level;
    }

    [Serializable]
    public class AgeGroupList
    {
        public AgeGroup[] items;
    }

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
    public class SubscriptionForm
    {
        public string transaction_id;

        public string platform;

        public string platform_plan_id;
    }

    public Button[] ageButton;
    public Image[] ageImageTick;
    public Sprite buttonSprite;
    public Sprite deactivateButtonSprite;
    private int selectedButton;


    string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    // string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";
    // Start is called before the first frame update
    void Start()
    {
         if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            
            Debug.Log(auth_key);

        }
         
          for(int i = 0; i < ageImageTick.Length; i++)
        {
            ageImageTick[i].enabled = false;
            ageImageTick[i].tag = i.ToString();
            ageButton[i].tag = i.ToString();
        }
         auth_key = "Bearer shBuqKWlYHGCss7Il4B0-L_3QpRO5L3Z";  // for api.testing.edugogy.app

        //Get Kids details
        GetKidProfile();
    }

    string fixJson(string value)            // Added object type to JSON
    {
        value = "{\"items\":" + value + "}";
        return value;
    }

    void GetKidProfile() => StartCoroutine(GetKidProfile_Coroutine());
    // void GetAgeGroupList() => StartCoroutine(GetAgeGroupList_Coroutine());
    public void UpdateKidsProfile() => StartCoroutine(UpdateKidsName_Coroutine());
            public void AddTrial() => StartCoroutine(AddTrialSubscription_Coroutine());


    KidsProfile profile = new KidsProfile();
    AgeGroupList agegrouplist = new AgeGroupList();


    IEnumerator GetKidProfile_Coroutine()
    {
        // outputArea.text = "Loading...";

        string uri = baseURL + "students/view";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("Content-Type", "application/json");
             request.SetRequestHeader("Authorization", auth_key);
            // request.SetRequestHeader("Authorization", "Bearer M0hLI8d5GVceaFh32XrOsiaiFvFgxxRz");

            yield return request.SendWebRequest();
            
           
            string jsonString = request.downloadHandler.text;

            profile = JsonUtility.FromJson<KidsProfile>(jsonString);
            Debug.Log(profile.name);
            Debug.Log(profile.age_group_id);

            KidsName.text = profile.name;
            selectedButton = profile.age_group_id;
            ageButton[profile.age_group_id - 1].image.sprite = buttonSprite;
            ageImageTick[profile.age_group_id - 1].enabled = true;
            
            if (profile.remaining_trial == 2)
            {
                Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "You have 2 more chances left to change your level.",
                    "Cancel",
                    "Okay",
                    GoSubscribe
                    );
            }
            else
            {
                Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "It’s time to choose your subscription level! Let’s get started!",
                    "Cancel",
                    "Subscribe Now",
                    GoSubscribe
                    );
            }

            // GetAgeGroupList();
            request.Dispose();
        }

        
    }

     public void OnClick(Button button)
    {
        
        Debug.Log(button.tag);
        
        selectedButton = int.Parse(button.tag);
        ageButton[selectedButton].image.sprite = buttonSprite;
        ageImageTick[selectedButton].enabled = true;

        for(int i = 0; i < 7; i++)
        {
            if(i != selectedButton)
            {
                ageButton[i].image.sprite = deactivateButtonSprite;
                ageImageTick[i].enabled = false;

            }
        }

    }
    
    //On update button click, update button profile and take back to settings screen

    //  IEnumerator GetAgeGroupList_Coroutine()
    // {
    //     // outputArea.text = "Loading...";

    //     string uri = baseURL + "age-groups";
    //     using (UnityWebRequest request = UnityWebRequest.Get(uri))
    //     {

    //         yield return request.SendWebRequest();
            
    //         string ageGroupJson = request.downloadHandler.text;
    //         string jsonString = fixJson(ageGroupJson);
    //         Debug.Log(jsonString);            

    //         agegrouplist = JsonUtility.FromJson<AgeGroupList>(jsonString);

    //         for (var i = 0; i < agegrouplist.items.Length; i++) 
    //         {
    //             var id = agegrouplist.items[i].id;
    //             if (profile.age_group_id == id)
    //             {
    //                 Debug.Log(agegrouplist.items[i].title);
    //                 // AgeGroupInput.text = agegrouplist.items[i].title + " Years";

    //             }
    //         }
           
    //     }
    // }

    IEnumerator UpdateKidsName_Coroutine()  //validate otp
    {
        profile.name = KidsName.text;
        profile.age_group_id = selectedButton + 1;
        string json = JsonUtility.ToJson(profile);

        Debug.Log(json);

        string uri = baseURL + "students/update";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(uri, "PUT");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
         request.SetRequestHeader("Authorization", auth_key);
        // request.SetRequestHeader("Authorization", "Bearer M0hLI8d5GVceaFh32XrOsiaiFvFgxxRz");

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
            Debug.Log(request.downloadHandler.text);
        }
        else
        {
           
            Debug.Log("Status Code: " + request.responseCode + "Update");
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);
            AddTrial();
            // SceneManager.LoadScene("Settings");

        }

        request.Dispose();
            
    }

    IEnumerator AddTrialSubscription_Coroutine()
    {
             ErrorList list = new ErrorList();
        Error error = new Error();

        int randomNumber = Random.Range(1000, 2000);

        SubscriptionForm subscriptionFormData = new SubscriptionForm { transaction_id = randomNumber.ToString(), platform = "apple", platform_plan_id = "trial" };
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
            Debug.Log("Error: in adding trial subscription because of no. of chances: " + request.error);
            Debug.Log(request.downloadHandler.text);
            Debug.Log("Status Code: " + request.responseCode);

            //  string  errorJson = request.downloadHandler.text;
            // string jsonString = fixJson(errorJson);
            // Debug.Log(jsonString);
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
            Debug.Log("Successful trial subscripton");
            // SceneManager.LoadScene("KidsName");
            // PlayerPrefs.SetString("isSubscribed", "true");
            // SceneManager.LoadScene("Dashboard");
        }
        request.Dispose();
    }

     void GoSubscribe()
    {
        SceneManager.LoadScene("IAPCatalog");
    }
}
