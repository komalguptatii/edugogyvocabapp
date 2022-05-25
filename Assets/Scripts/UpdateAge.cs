using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class UpdateAge : MonoBehaviour
{
    [Serializable]
    public class SelectAgeForm
    {
        public int age_group_id;
    }

    // Start is called before the first frame update

    // public TextMeshProUGUI kidsNameTextInput;
    // public TMP_InputField kidsNameTextInput;
    public int ageGroupId;
    // public GameObject textDisplay;
    void Start()
    {
        //Add condition for first time login
        // SelectAge
    }

    // Update is called once per frame
    public void StoreKidsAgeGroup()
    {
        //validation
        if (ageGroupId != 0)
        {
            Debug.Log(ageGroupId);
            UpdateKidsAgeGroup();
        }
    }

    void UpdateKidsAgeGroup() => StartCoroutine(ProcessKidsAgeGroup_Coroutine());
    IEnumerator ProcessKidsAgeGroup_Coroutine()  //validate otp
    {

        SelectAgeForm validAgeGroupForm = new SelectAgeForm { age_group_id = ageGroupId };
        string json = JsonUtility.ToJson(validAgeGroupForm);

        Debug.Log(json);


        string uri = "http://165.22.219.198/edugogy/api/v1/students/update";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(uri, "PUT");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer 3VcmTskZ5jRINDiaO_489b0pdVsbTEy6");


        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);
            // { "auth_key":"3VcmTskZ5jRINDiaO_489b0pdVsbTEy6"}
            MoveToNextScreen();
        }

    }

    public void MoveToNextScreen()
    {
        SceneManager.LoadScene("SelectAge");
    }
}
