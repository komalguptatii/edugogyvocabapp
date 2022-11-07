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

    public bool isLoggedIn = true;
    
    public bool isQuickLogin = false;

    [Serializable]
    public class KidsNameForm
    {
        public string name;
    }

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

     private Animator loadingIndicator;
    public GameObject Indicator;

    void Start()
    {
        loadingIndicator = Indicator.GetComponent<Animator>(); 
         loadingIndicator.enabled = false;
        Indicator.SetActive(false);

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

        if (showingResults == true || isQuickLogin == true)
        {
             loadingIndicator.enabled = true;
        Indicator.SetActive(true);
            socialLoginRequest();
        }
            // Updates the AppleAuthManager instance to execute
        // pending callbacks inside Unity's execution loop
        if (this.appleAuthManager != null)
        {
            this.appleAuthManager.Update();
            if (isLoggedIn)
            {
                QuickLogin();
                isLoggedIn = false;
            }
            // Debug.Log("If already logged in");
        }

    }

    public void QuickLogin()
    {
        Debug.Log("Quick Login going");
        var quickLoginArgs = new AppleAuthQuickLoginArgs();

        this.appleAuthManager.QuickLogin(
        quickLoginArgs,
        credential =>
        {
            Debug.Log("Received a valid credential!");
            // Received a valid credential!
            // Try casting to IAppleIDCredential or IPasswordCredential

            // Previous Apple sign in credential
            var appleIdCredential = credential as IAppleIDCredential; 

             if (appleIdCredential != null)
                {
                    // Apple User ID
                    // You should save the user ID somewhere in the device
                    var userId = appleIdCredential.User;
                    PlayerPrefs.SetString(AppleUserIdKey, userId);
                    id = userId;
                    // showingResults = true;
                    isQuickLogin = true;
                }
                else
                {
                    Debug.Log("Apple id credentials are null");
                }


            // Saved Keychain credential (read about Keychain Items)
            var passwordCredential = credential as IPasswordCredential;

             if (passwordCredential != null)
                {
                    // Apple User ID
                    // You should save the user ID somewhere in the device
                    var userId = passwordCredential.User;
                    PlayerPrefs.SetString(AppleUserIdKey, userId);
                    id = userId;

                    // var fullName = appleIdCredential.FullName;
                    // PlayerPrefs.SetString("AppleName", fullName.ToLocalizedString());

                    // Email (Received ONLY in the first login)
                    // var email = passwordCredential.Email;
                    // email = email;

                    // // Full name (Received ONLY in the first login)
                    // var fullName = passwordCredential.FullName;
                    // name = fullName.ToString();
                    Debug.Log("Quick Login happening");
                    // showingResults = true;
                    isQuickLogin = true;

                }
                else
                {
                    Debug.Log("Password credentials are null");
                }
            
        },
        error =>
        {
            // Quick login failed. The user has never used Sign in With Apple on your app. Go to login screen
        });
    }

    public void SignIn()
    {
         Debug.Log("If already logged in, trying to fetch details");
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
                     PlayerPrefs.SetString("AppleEmail", email);

                    email = email;

                    // Full name (Received ONLY in the first login)
                    var fullName = appleIdCredential.FullName;
                    PlayerPrefs.SetString("AppleName", fullName.ToLocalizedString());
                    Debug.Log("full name of apple user is " + fullName.ToLocalizedString(PersonNameFormatterStyle.Short));
                    Debug.Log("full name of apple user is " + fullName.ToLocalizedString(PersonNameFormatterStyle.Medium));
                    Debug.Log("full name of apple user is " + fullName.ToLocalizedString(PersonNameFormatterStyle.Long));

                    name = fullName.ToLocalizedString();

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
                else
                {
                    Debug.Log("Apple ID Credential is null");
                }
            },
            error =>
            {
                // Something went wrong
                var authorizationErrorCode = error.GetAuthorizationErrorCode();
                Debug.Log("authorizationErrorCode " + authorizationErrorCode);
            });
    }

    

    public void socialLoginRequest() => StartCoroutine(SocialLogin_Coroutine(id, email, name));

    IEnumerator SocialLogin_Coroutine(string id, string email, string name)
    {
        
        GetAuthKey getKey = new GetAuthKey();

        SocialLoginForm socialLoginFormData = new SocialLoginForm();
        socialLoginFormData.social_id = id;
        socialLoginFormData.social_media = "apple";
        socialLoginFormData.app_validate_key = "0H9K@FbQ3k*6";

        if (showingResults == true)
        {
            string emailId = "";
            string nameOfAppleUser = "";
            if (PlayerPrefs.HasKey("AppleEmail"))
            {
                emailId = PlayerPrefs.GetString("AppleEmail");
                nameOfAppleUser = PlayerPrefs.GetString("AppleName");
            }
             
            
            
            socialLoginFormData.email = emailId;
            socialLoginFormData.name = nameOfAppleUser;
            showingResults = false;
            Debug.Log("Value of apple json is name" + nameOfAppleUser + " & email is " + emailId + " id is " + id);


        }
        else if (isQuickLogin == true)
        {
            Debug.Log("Value of apple json is id " + id);

            isQuickLogin = false;
        }
        

        // SocialLoginForm socialLoginFormData = new SocialLoginForm {social_id = id, social_media = "apple", app_validate_key = "0H9K@FbQ3k*6", email = emailId, name = nameOfAppleUser};
        string json = JsonUtility.ToJson(socialLoginFormData);

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
            
        }
    }

    void SavePlatform(string platform)
    {
         PlayerPrefs.SetString("platform", platform);
    }

    void SaveAuthKey(string auth_key){
        PlayerPrefs.SetString("auth_key", "Bearer " + auth_key);

        if (PlayerPrefs.HasKey("AppleName"))
        {
            string nameOfAppleUser = PlayerPrefs.GetString("AppleName");
            UpdateKidsName();   
        }
        else
        {
            MoveToSubscription();
        }
        
    }

    void UpdateKidsName() => StartCoroutine(ProcessKidsName_Coroutine());

    IEnumerator ProcessKidsName_Coroutine()  //validate otp
    {

        string nameOfAppleUser = PlayerPrefs.GetString("AppleName");
        KidsNameForm validKidNameForm = new KidsNameForm { name = nameOfAppleUser };
        string json = JsonUtility.ToJson(validKidNameForm);

        Debug.Log(json);


        string uri = baseURL + "students/update";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(uri, "PUT");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", PlayerPrefs.GetString("auth_key"));


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

            MoveToSubscription();
        }

    }

     void MoveToSubscription()
    {
         loadingIndicator.enabled = false;
        Indicator.SetActive(false);
        //  Debug.Log("Value of apple json is");
        SceneManager.LoadScene("SelectAge"); // Don't go on Kids name as Apple is already providing that
    }
}
