using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;

public class DashboardManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Serializable]
    public class ProfileForm
    {
        public string name;
        public string phone;
        public int country_code_id;

        public int age_group_id;

    }

    void Start()
    {
        // GetUserProfileData();
        UpdateUserProfileData();
    }

    void GetUserProfileData() => StartCoroutine(GetUserProfile_Coroutine());

    void UpdateUserProfileData() => StartCoroutine(UpdateUserProfile_Coroutine());


    IEnumerator GetUserProfile_Coroutine()
    {

        string uri = "http://165.22.219.198/edugogy/api/v1/students/view";

        var request = new UnityWebRequest(uri, "GET");

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer 3VcmTskZ5jRINDiaO_489b0pdVsbTEy6");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

            // {"id":3,"name":"Komal","phone":"9855940600","age_group_id":2,"country_code_id":88,"total_level":2,"total_passed_level":0,"available_level":30}

        }
    }

    IEnumerator UpdateUserProfile_Coroutine()
    {
        ProfileForm profileFormData = new ProfileForm { name = "Komal Gupta", phone = "9855940600", country_code_id = 88, age_group_id = 2 };
        string json = JsonUtility.ToJson(profileFormData);

        Debug.Log(json);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        string uri = "http://165.22.219.198/edugogy/api/v1/students/update";

        var request = new UnityWebRequest(uri, "PUT");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer 3VcmTskZ5jRINDiaO_489b0pdVsbTEy6");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

            // { "id":3,"name":"Komal Gupta","phone":"9855940600","age_group_id":2,"country_code_id":88}

        }
    }
}
