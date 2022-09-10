using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using System.Text;


public class FacebookLogin : MonoBehaviour
{
    // public GameObject Panel_Add;
    //  public Text FB_userName;
    // public Image FB_useerDp;
    string name = "";
    string id = "";
    string email = "";
    bool showingResults = false;

    [Serializable]
    public class SocialLoginForm
    {
        public string social_id;
        public string social_media;
        public string app_validate_key;
        public string email;
        public string name;
    }

    [Serializable]
    public class GetAuthKey
    {
        public string auth_key;
    }

    // string baseURL = "https://api.edugogy.app/v1/";
        string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";

    private void Awake()
    {
        FB.Init(SetInit, onHidenUnity);
        // Panel_Add.SetActive(false);
    }
    void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("Facebook is Login!");
        }
        else
        {
            Debug.Log("Facebook is not Logged in!");
        }
        DealWithFbMenus(FB.IsLoggedIn);
    }

    void onHidenUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void FBLogin()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        permissions.Add("email");


        FB.LogInWithReadPermissions(permissions, AuthCallBack);
        //FB.Login("email,publish_actions", callback);
    }
    // Start is called before the first frame update
    void AuthCallBack(IResult result)
    {
        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("Facebook is Login!");
                // Panel_Add.SetActive(true);
            }
            else
            {
                Debug.Log("Facebook is not Logged in!");
            }
            DealWithFbMenus(FB.IsLoggedIn);
        }
    }

    void DealWithFbMenus(bool isLoggedIn)
    {
        if (isLoggedIn)
        {
            FB.API("/me?fields=first_name", HttpMethod.GET, DisplayUsername);
            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
            FB.API("/me?fields=id", HttpMethod.GET, DisplayID);
            FB.API("/me?fields=email", HttpMethod.GET, DisplayEmail);
            showingResults = true;
            // SceneManager.LoadScene("IAPCatalog");
            
            // SocialLogin_Coroutine(id, email, name);

        }
        
    }

    void Update()
    {
        if (showingResults == true)
        {
            socialLoginRequest();
        }
    }

    void DisplayID(IResult result)
    {
        if (result.Error == null)
        {
            Debug.Log(result.ResultDictionary);
            id = "" + result.ResultDictionary["id"];

            Debug.Log("" + id);
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    void DisplayEmail(IResult result)
    {
        if (result.Error == null)
        {
            Debug.Log(result.ResultDictionary);
            email = "" + result.ResultDictionary["email"];

            Debug.Log("" + email);
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    void DisplayUsername(IResult result)
    {
        if (result.Error == null)
        {
            name = "" + result.ResultDictionary["first_name"];

            //  FB_userName.text = name;

            Debug.Log("" + name);
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    void DisplayProfilePic(IGraphResult result)
    {
        if (result.Texture != null)
        {
            Debug.Log("Profile Pic");
            // FB_useerDp.sprite = Sprite.Create(result.Texture,new Rect(0,0,128,128),new Vector2());
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    public void socialLoginRequest() => StartCoroutine(SocialLogin_Coroutine(id, email, name));

    IEnumerator SocialLogin_Coroutine(string id, string email, string name)
    {
        showingResults = false;
        GetAuthKey getKey = new GetAuthKey();

        SocialLoginForm socialLoginFormData = new SocialLoginForm {social_id = id, social_media = "facebook", app_validate_key = "0H9K@FbQ3k*6", email = email, name = name};
        string json = JsonUtility.ToJson(socialLoginFormData);
        Debug.Log("Value of facebook json is");
        Debug.Log(json);

        string uri = baseURL + "students/social-login";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

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
            
            string responseJson = request.downloadHandler.text;

            getKey = JsonUtility.FromJson<GetAuthKey>(responseJson);
            Debug.Log(getKey.auth_key);
            Debug.Log("Received Auth Key");
            SavePlatform("facebook");
            SaveAuthKey(getKey.auth_key);
            MoveToSubscription();
        }
    }

    void SavePlatform(string platform)
    {
         PlayerPrefs.SetString("platform", platform);
    }

    void SaveAuthKey(string auth_key){
        PlayerPrefs.SetString("auth_key", "Bearer " + auth_key);
    }

     void MoveToSubscription()
    {
        SceneManager.LoadScene("IAPCatalog");
    }
}
