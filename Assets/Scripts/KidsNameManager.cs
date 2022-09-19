using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Text;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;


public class KidsNameManager : MonoBehaviour
{

    [Serializable]
    public class KidsNameForm
    {
        public string name;
    }
     
    string baseURL = "https://api.edugogy.app/v1/";
        // string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";

    // Start is called before the first frame update

    // public TextMeshProUGUI kidsNameTextInput;
    public TMP_InputField kidsNameTextInput;
    public string theName;
    public string auth_key;
    // public GameObject textDisplay;
    void Start()
    {
        //Add condition for first time login
        // SelectAge

         if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            
            Debug.Log(auth_key);

        }
    }

    // Update is called once per frame
    public void StoreKidsName()
    {
        //validation
        if (kidsNameTextInput.text != "")
        {
            theName = kidsNameTextInput.text;
            Debug.Log(theName);
            UpdateKidsName();
        }
        else
        {
            Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Please enter child's name",
                    "Cancel",
                    "Sure!",
                    resetAction
                    );
        }

        // textDisplay.GetComponent<TextMeshProUGUI>().text = theName;
    }

    public void resetAction()
    {
        Debug.Log("empty textfield");
    }

    void UpdateKidsName() => StartCoroutine(ProcessKidsName_Coroutine());

    IEnumerator ProcessKidsName_Coroutine()  //validate otp
    {

        KidsNameForm validKidNameForm = new KidsNameForm { name = kidsNameTextInput.text };
        string json = JsonUtility.ToJson(validKidNameForm);

        Debug.Log(json);


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
        }
        else
        {
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);
            PlayerPrefs.SetString("childName", validKidNameForm.name);

            MoveToNextScreen();
        }

    }

    public void MoveToNextScreen()
    {
        SceneManager.LoadScene("SelectAge");
    }
}
