using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MicrophoneAccess : MonoBehaviour
{

    string auth_key;

     string baseURL = "https://api.edugogy.app/v1/";
    
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

    // Start is called before the first frame update
    void Start()
    {

         #if PLATFORM_ANDROID
         if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
         {
            Permission.RequestUserPermission(Permission.Microphone);
         }
        #elif UNITY_IOS
         if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
         {
             Application.RequestUserAuthorization(UserAuthorization.Microphone);
         }
        #endif

        
    }

      void GetKidProfile() => StartCoroutine(GetKidProfile_Coroutine());

     public void CheckInformation()
    {
        if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            if (auth_key != null)
            {
                GetKidProfile();
                // if (PlayerPrefs.HasKey("childName"))
                // {
                //     if (PlayerPrefs.HasKey("isAgeSelected"))
                //     {
                //         if (PlayerPrefs.HasKey("isSubscribed"))
                //         {
                //             SceneManager.LoadScene("Dashboard");
                //         }
                //         else
                //         {
                //             SceneManager.LoadScene("IAPCatalog");

                //         }
                //     }
                //     else
                //     {
                //         SceneManager.LoadScene("SelectAge");
                //     }

                // }
                // else
                // {
                //     SceneManager.LoadScene("KidsName");
                // }
            }
        }
        else
        {
            SceneManager.LoadScene("Description");
        }
    }

    IEnumerator GetKidProfile_Coroutine()
    {
        
        KidsProfile profile = new KidsProfile();
         auth_key = PlayerPrefs.GetString("auth_key");
        string uri = baseURL + "students/view";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            // auth_key = "Bearer shBuqKWlYHGCss7Il4B0-L_3QpRO5L3Z";
             request.SetRequestHeader("Authorization", auth_key);

            yield return request.SendWebRequest();
            
           
            string jsonString = request.downloadHandler.text;

            profile = JsonUtility.FromJson<KidsProfile>(jsonString);
            
            if (profile.name != "" && profile.age_group_id != 0)
            {
                 PlayerPrefs.SetString("childName", profile.name);
                 PlayerPrefs.SetInt("isAgeSelected", 1); //isAgeSelected ? 1 : 0
                 if (profile.available_level > 0)
                 {
                     SceneManager.LoadScene("Dashboard");
                 }
                 else
                 {
                    SceneManager.LoadScene("IAPCatalog");
                 }
               
            }
            else
            {
                SceneManager.LoadScene("KidsName");
            }
        
            request.Dispose();
        }

        
    }

}
