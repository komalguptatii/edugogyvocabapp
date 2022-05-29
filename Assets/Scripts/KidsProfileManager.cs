using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using TMPro;
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
        public int total_level;
        public int total_passed_level;
        public int subscription_id;
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

    // Start is called before the first frame update
    void Start()
    {
         if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            
            Debug.Log(auth_key);

        }
        //Get Kids details
        GetKidProfile();
    }

    void GetKidProfile() => StartCoroutine(GetKidProfile_Coroutine());
    void GetAgeGroupList() => StartCoroutine(GetAgeGroupList_Coroutine());
    public void UpdateKidsProfile() => StartCoroutine(UpdateKidsName_Coroutine());

    KidsProfile profile = new KidsProfile();
    AgeGroupList agegrouplist = new AgeGroupList();


    IEnumerator GetKidProfile_Coroutine()
    {
        // outputArea.text = "Loading...";

        string uri = "http://165.22.219.198/edugogy/api/v1/students/view";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", auth_key);

            yield return request.SendWebRequest();
            
            string jsonString = request.downloadHandler.text;
            
            profile = JsonUtility.FromJson<KidsProfile>(jsonString);
            Debug.Log(profile.name);
            Debug.Log(profile.age_group_id);
            KidsName.text = profile.name;
            GetAgeGroupList();

        }
    }
    //On update button click, update button profile and take back to settings screen

    string fixJson(string value)            // Added object type to JSON
    {
        value = "{\"items\":" + value + "}";
        return value;
    }

     IEnumerator GetAgeGroupList_Coroutine()
    {
        // outputArea.text = "Loading...";

        string uri = "http://165.22.219.198/edugogy/api/v1/age-groups";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {

            yield return request.SendWebRequest();
            
            string ageGroupJson = request.downloadHandler.text;
            string jsonString = fixJson(ageGroupJson);
            Debug.Log(jsonString);            

            agegrouplist = JsonUtility.FromJson<AgeGroupList>(jsonString);

            for (var i = 0; i < agegrouplist.items.Length; i++) 
            {
                var id = agegrouplist.items[i].id;
                if (profile.age_group_id == id)
                {
                    Debug.Log(agegrouplist.items[i].title);
                    AgeGroupInput.text = agegrouplist.items[i].title + " Years";

                }
            }
           
        }
    }

    IEnumerator UpdateKidsName_Coroutine()  //validate otp
    {
        profile.name = KidsName.text;
        string json = JsonUtility.ToJson(profile);

        Debug.Log(json);

        string uri = "http://165.22.219.198/edugogy/api/v1/students/update";

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
            SceneManager.LoadScene("Settings");

        }

    }


}
