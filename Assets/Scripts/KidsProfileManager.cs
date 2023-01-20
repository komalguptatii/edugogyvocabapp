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
        public int remaining_level_for_day;
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
    int previousAgeGroupId;
    bool isCallingAfterUpdate = false;
    private Animator loadingIndicator;
    public GameObject Indicator;
    public Button updateButton;


    string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    // string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";
    // Start is called before the first frame update
    void Start()
    {
        loadingIndicator = Indicator.GetComponent<Animator>(); 
        updateButton.enabled = false;

         if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            
            Debug.Log(auth_key);

        }
         
        // auth_key = "Bearer DB7wRBv6G8atorfi2FqkEw3w-Zo-gYAb"; 

          for(int i = 0; i < ageImageTick.Length; i++)
        {
            ageImageTick[i].enabled = false;
            ageImageTick[i].tag = i.ToString();
            ageButton[i].tag = i.ToString();
        }
          

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

    public void takeUpdateConfirmation()
    {
        //Pending -  if already is_trial_subscription is true - don't ask any pop up, if subscribe then this pop up - Also here issue is is_trial_subscription coming false yet
        if (profile.is_trial_subscription == true)
        {
            UpdateKidsProfile();
        }
        else
        {
            Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Changing your level now will end your current subscription. Do you still want to continue?",
                    "Cancel",
                    "Continue",
                    UpdateKidsProfile
                    );
        }
        
    }

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
            Debug.Log(jsonString);

            profile = JsonUtility.FromJson<KidsProfile>(jsonString);

            Debug.Log(profile.name);
            Debug.Log(profile.age_group_id);
            previousAgeGroupId = profile.age_group_id;
            
            if (!isCallingAfterUpdate)
            {
                updateButton.enabled = true;
                     loadingIndicator.enabled = false;
        Indicator.SetActive(false);
        
                KidsName.text = profile.name;
            selectedButton = profile.age_group_id;
            ageButton[profile.age_group_id - 1].image.sprite = buttonSprite;
            ageImageTick[profile.age_group_id - 1].enabled = true;
            
            if (profile.is_trial_subscription == true)
            {
                if (profile.remaining_trial > 0)
                {

                    Popup popup = UIController.Instance.CreatePopup();
                    popup.Init(UIController.Instance.MainCanvas,
                        "You have " + profile.remaining_trial + " more chances left to change your level.",
                        "Cancel",
                        "Okay",
                        StayOnPage
                        );
                }
                else if (profile.remaining_trial == 0)
                {
                    // should something like that Trial period is over
                    Popup popup = UIController.Instance.CreatePopup();
                    popup.Init(UIController.Instance.MainCanvas,
                        "It’s time to choose your subscription level! Let’s get started! As trial period is over.",
                        "Cancel",
                        "Choose level",
                        StayOnPage
                        );
                }
            }
            

            }
            else
            {
                Debug.Log("profile.subscription_remaining_day " + profile.subscription_remaining_day);
                 int nextLevel = profile.total_passed_level + 1;
                   PlayerPrefs.SetString("NextLevelWillBe", nextLevel.ToString());
                // NextLevelWillBe
                if (profile.available_level == 0)
                {
                   
                    GoSubscribe();
                    
                }
                else
                {
                    //when not subscribed, and value of remaining days is null
                    SceneManager.LoadScene("Dashboard");
                    
                }
            }
            
            // GetAgeGroupList();
            request.Dispose();
        }

        
    }

     public void OnClick(Button button)
    {
        
        Debug.Log(button.tag);
        
        selectedButton = int.Parse(button.tag) + 1;
        ageButton[selectedButton - 1].image.sprite = buttonSprite;
        ageImageTick[selectedButton - 1].enabled = true;

        for(int i = 0; i < 7; i++)
        {
            if(i != selectedButton - 1)
            {
                ageButton[i].image.sprite = deactivateButtonSprite;
                ageImageTick[i].enabled = false;

            }
        }

    }
    
   
    IEnumerator UpdateKidsName_Coroutine()  //validate otp
    {
        profile.name = KidsName.text;
        profile.age_group_id = selectedButton;
        int currentAgeGroupId = selectedButton;
        
        string json = JsonUtility.ToJson(profile);

        // Debug.Log(json);

        string uri = baseURL + "students/update";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(uri, "PUT");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
         request.SetRequestHeader("Authorization", auth_key);

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
            if (previousAgeGroupId != currentAgeGroupId)
            {
                //ResetPlayerPrefs data to start with Mission 1

                PlayerPrefs.DeleteKey("NextLevelWillBe");
                PlayerPrefs.DeleteKey("numberOfLevelsPerDay");
                PlayerPrefs.DeleteKey("completionDateTime");
                PlayerPrefs.DeleteKey("isReattempting");
                PlayerPrefs.DeleteKey("totalLevelsPassed");
                Debug.Log("Deleting Keys");
            }
            isCallingAfterUpdate = true;
            GetKidProfile();
            // GoSubscribe();
           

    

        }

        request.Dispose();
            
    }

     void GoSubscribe()
    {
        SceneManager.LoadScene("IAPCatalog");
    }

    void StayOnPage()
    {
        Debug.Log("Stay on page, Update level");
    }
}
