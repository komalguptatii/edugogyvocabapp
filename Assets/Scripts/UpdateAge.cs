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

    public Button[] ageButton;
    public Image[] ageImageTick;
    public int ageGroupId;
    private string auth_key;
    public Sprite buttonSprite;
    public Sprite deactivateButtonSprite;
    private int selectedButton;
    public bool isAgeSelected = false;
    
     string baseURL = "https://api.edugogy.app/v1/";
        // string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";

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
    }

    // Update is called once per frame
    public void StoreKidsAgeGroup()
    {
        //validation
        // if (ageGroupId != 0)
        Debug.Log("isAgeSelected" + isAgeSelected);
        if (isAgeSelected == true)
        {
            Debug.Log(ageGroupId);
            GetKidsAgeGroup();

        }
        else
        {
            // Display Validation
            Debug.Log("Please select age group");
            Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Please select age group",
                    "Cancel",
                    "Sure!",
                    resetAction
                    );
        }
    }

    public void resetAction()
    {
        Debug.Log("Please select age group");
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

         isAgeSelected = true;
        // ageButton[indexOfButton].text.color = Color(141, 41, 2555);
    }

    public void UpdateOnClick()
    {
        StoreKidsAgeGroup();

    }

    void UpdateKidsAgeGroup() => StartCoroutine(ProcessKidsAgeGroup_Coroutine());
    void GetKidsAgeGroup() => StartCoroutine(GetAgeGroupList_Coroutine());


    string fixJson(string value)            // Added object type to JSON
    {
        value = "{\"items\":" + value + "}";
        return value;
    }

    IEnumerator GetAgeGroupList_Coroutine()
    {
        // outputArea.text = "Loading...";
        AgeGroupList agegrouplist = new AgeGroupList();

        string uri = baseURL + "age-groups";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {

            yield return request.SendWebRequest();
            
            string ageGroupJson = request.downloadHandler.text;
            string jsonString = fixJson(ageGroupJson);
            Debug.Log(jsonString);            

            agegrouplist = JsonUtility.FromJson<AgeGroupList>(jsonString);

            ageGroupId = agegrouplist.items[selectedButton].id;
            Debug.Log(ageGroupId + " Age Group ID");
            UpdateKidsAgeGroup();

        }
    }


    IEnumerator ProcessKidsAgeGroup_Coroutine()  //validate otp
    {

        Debug.Log(ageGroupId);
        SelectAgeForm validAgeGroupForm = new SelectAgeForm { age_group_id = ageGroupId };
        string json = JsonUtility.ToJson(validAgeGroupForm);

        Debug.Log("SelectAgeForm" + json);


        string uri = baseURL + "students/update";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(uri, "PUT");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", auth_key);

        Debug.Log(auth_key);


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
            PlayerPrefs.SetInt("isAgeSelected", isAgeSelected ? 1 : 0);
            MoveToNextScreen();

        }

    }

    public void MoveToNextScreen()
    {
        SceneManager.LoadScene("Dashboard");
    }
}
