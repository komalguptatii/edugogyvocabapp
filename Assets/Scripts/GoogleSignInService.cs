using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Firebase;
using Firebase.Auth;
using Google;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Text;

public class GoogleSignInService : MonoBehaviour
{
    // public Text infoText;
    public string webClientId = "849410546096-t5gubl7o7bbe3osbbg586quj6cjdef2b.apps.googleusercontent.com";

    public FirebaseAuth auth;
    public FirebaseApp app;
    private GoogleSignInConfiguration configuration;

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

    public string email = "";
    public string name = "";
    public bool isSignInDone = false;

    string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";

    private void Awake()
    {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
        // var dependencyResult = FirebaseApp.CheckAndFixDependenciesAsync();
        // if(dependencyResult == DependencyStatus.Available)
        // {
        //     app = FirebaseApp.DefaultInstance;
        //     OnFirebaseInitialized.Invoke(); //This simply loads the next scene.
        // }
    }
     
    void Start()
    {
    //    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
       auth = FirebaseAuth.DefaultInstance;
       
        // auth.StateChanged += AuthStateChanged;
        // CheckFirebaseDependencies();
    }

    public void CheckFirebaseDependencies()
    {
        // auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Debug.Log("auth value is " + auth);
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            Debug.Log("checked for dependency  " + auth);
            if (task.IsCompleted)
            {
                Debug.Log("After task completion");
                if (task.Result == DependencyStatus.Available)
                {
                    // auth = FirebaseAuth.DefaultInstance;
                     Debug.Log("if auth is get instantiated");

                    auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
                }
                else
                {
                    AddToInformation("Could not resolve all Firebase dependencies: " + task.Result.ToString());
                }
            }
            else
            {
                AddToInformation("Dependency check was not completed. Error : " + task.Exception.Message);
            }
        });
    }

    public void SignInWithGoogle() { OnSignIn(); }
    public void SignOutFromGoogle() { OnSignOut(); }

    private void OnSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void OnSignOut()
    {
        AddToInformation("Calling SignOut");
        GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect()
    {
        AddToInformation("Calling Disconnect");
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            using (IEnumerator<Exception> enumerator = task.Exception.InnerExceptions.GetEnumerator())
            {
                if (enumerator.MoveNext())
                {
                    GoogleSignIn.SignInException error = (GoogleSignIn.SignInException)enumerator.Current;
                    AddToInformation("Got Error: " + error.Status + " " + error.Message);
                }
                else
                {
                    AddToInformation("Got Unexpected Exception?!?" + task.Exception);
                }
            }
        }
        else if (task.IsCanceled)
        {
            AddToInformation("Canceled");
        }
        else
        {
            // AddToInformation("Email = " + task.Result.Email);
            // AddToInformation("Google ID Token = " + task.Result.IdToken);
            email = task.Result.Email;
            name = task.Result.DisplayName;
            isSignInDone = true;
            
            // socialLoginRequest();
            // CallSocialLoginCoroutine();
            // AddToInformation("Welcome: " + task.Result.DisplayName + "!");

            // socialLoginRequest();
            // SignInWithGoogleOnFirebase(task.Result.IdToken);

            
        }



    }

    public void CallSocialLoginCoroutine()
    {
        Debug.Log("Entered this function to create user on firebase");
        socialLoginRequest();

    }

    void Update()
    {
        if (isSignInDone == true)
        {
            socialLoginRequest();
        }
    }

    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Debug.Log("Entered this function to create user on firebase");
        socialLoginRequest();

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
        Debug.Log("credential is" + credential);

        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
       
            AddToInformation("Sign In Successful.");
        //     AggregateException ex = task.Exception;
        //     Debug.Log("Value of ex is " + ex);
        //     if (ex != null)
        //     {
        //         if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
        //             AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
        //     }
        //     else
        //     {
        //         AddToInformation("Sign In Successful.");
        //         socialLoginRequest();

        //     }
         });
       
    }

    public void OnSignInSilently()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        AddToInformation("Calling SignIn Silently");

        GoogleSignIn.DefaultInstance.SignInSilently().ContinueWith(OnAuthenticationFinished);
    }

    public void OnGamesSignIn()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = true;
        GoogleSignIn.Configuration.RequestIdToken = false;

        AddToInformation("Calling Games SignIn");

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    private void AddToInformation(string str)
    {
        // infoText.text += "\n" + str; 
        Debug.Log(str);
    }

    public void socialLoginRequest() => StartCoroutine(SocialLogin_Coroutine(email, email, name));

    IEnumerator SocialLogin_Coroutine(string id, string email, string name)
    {
        isSignInDone = false;
        Debug.Log("Starting Coroutine to Login");
        GetAuthKey getKey = new GetAuthKey();

        SocialLoginForm socialLoginFormData = new SocialLoginForm {social_id = id, social_media = "google", app_validate_key = "0H9K@FbQ3k*6", email = email, name = name};
        string json = JsonUtility.ToJson(socialLoginFormData);

        Debug.Log("Value of google json is");
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
            SavePlatform("google");
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
        SceneManager.LoadScene("KidsName");
    }
}