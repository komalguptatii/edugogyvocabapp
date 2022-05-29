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

    public Button[] ageButton;
    public Image[] ageImageTick;
    public int ageGroupId;
    string auth_key;
    public Sprite buttonSprite;
    
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
        }
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

    public void OnClick()
    {
        var indexOfButton = 0;
        ageButton[indexOfButton].image.sprite = buttonSprite;
        ageImageTick[indexOfButton].enabled = true;
        // ageButton[indexOfButton].text.color = Color(141, 41, 2555);
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
        request.SetRequestHeader("Authorization", auth_key);


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
            // MoveToNextScreen();
        }

    }

    // public void MoveToNextScreen()
    // {
    //     SceneManager.LoadScene("SelectAge");
    // }
}
