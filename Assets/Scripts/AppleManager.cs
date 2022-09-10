using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using System.Text;
using AppleAuth.Extensions;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;


public class AppleManager : MonoBehaviour
{
    private IAppleAuthManager appleAuthManager;

    public string AppleUserIdKey { get; private set; }

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

    string id = "";
    string email = "";
    string name = "";
    bool showingResults = false;

    string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1";


    void Start()
    {

        // If the current platform is supported
        if (AppleAuthManager.IsCurrentPlatformSupported)
        {
            // Creates a default JSON deserializer, to transform JSON Native responses to C# instances
            var deserializer = new PayloadDeserializer();
            // Creates an Apple Authentication manager with the deserializer
            this.appleAuthManager = new AppleAuthManager(deserializer);
        }

    }

    void Update()
    {

        if (showingResults == true)
        {
            socialLoginRequest();
        }
            // Updates the AppleAuthManager instance to execute
        // pending callbacks inside Unity's execution loop
        if (this.appleAuthManager != null)
        {
            this.appleAuthManager.Update();
        }

    }

    public void SignIn()
    {
        var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

        this.appleAuthManager.LoginWithAppleId(
            loginArgs,
            credential =>
            {
                // Obtained credential, cast it to IAppleIDCredential
                var appleIdCredential = credential as IAppleIDCredential;
                if (appleIdCredential != null)
                {
                    // Apple User ID
                    // You should save the user ID somewhere in the device
                    var userId = appleIdCredential.User;
                    PlayerPrefs.SetString(AppleUserIdKey, userId);
                    id = userId;

                    // Email (Received ONLY in the first login)
                    var email = appleIdCredential.Email;
                    email = email;

                    // Full name (Received ONLY in the first login)
                    var fullName = appleIdCredential.FullName;
                    name = fullName.ToString();

                    // Identity token
                    var identityToken = Encoding.UTF8.GetString(
                        appleIdCredential.IdentityToken,
                        0,
                        appleIdCredential.IdentityToken.Length);

                    // Authorization code
                    var authorizationCode = Encoding.UTF8.GetString(
                        appleIdCredential.AuthorizationCode,
                        0,
                        appleIdCredential.AuthorizationCode.Length);

                    Debug.Log("Signing in with Apple");
                    Debug.Log(userId);
                    Debug.Log(email);
                    Debug.Log(fullName);
                    Debug.Log(identityToken);
                    Debug.Log("It's done");
                    showingResults = true;
                    // MoveToSubscription();
                    // socialLoginRequest();
                    // SocialLogin_Coroutine(userId, email, fullName.ToString());
                    // socialLoginRequest();
                    // SceneManager.LoadScene("IAPCatalog");


                    // And now you have all the information to create/login a user in your system
                }
            },
            error =>
            {
                // Something went wrong
                var authorizationErrorCode = error.GetAuthorizationErrorCode();
            });
    }

    

    public void socialLoginRequest() => StartCoroutine(SocialLogin_Coroutine(id, email, name));

    IEnumerator SocialLogin_Coroutine(string id, string email, string name)
    {
        showingResults = false;
        GetAuthKey getKey = new GetAuthKey();

        SocialLoginForm socialLoginFormData = new SocialLoginForm {social_id = id, social_media = "apple", app_validate_key = "0H9K@FbQ3k*6", email = email, name = name};
        string json = JsonUtility.ToJson(socialLoginFormData);

        Debug.Log("Value of apple json is");
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
            SavePlatform("apple");
            SaveAuthKey(getKey.auth_key);
            Debug.Log("Received Auth Key");
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
        //  Debug.Log("Value of apple json is");
        SceneManager.LoadScene("IAPCatalog");
    }
}
