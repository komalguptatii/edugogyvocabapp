using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class MicrophoneAccess : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

         #if PLATFORM_ANDROID
         if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
         {
            Permission.RequestUserPermission(Permission.Microphone);
         }
        #elif UNITY_IOS
         if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
         {
             Application.RequestUserAuthorization(UserAuthorization.Microphone);
         }
        #endif

        
    }

     public void CheckInformation()
    {
        if (PlayerPrefs.HasKey("auth_key"))
        {
            string auth_key = PlayerPrefs.GetString("auth_key");
            if (auth_key != null)
            {
                if (PlayerPrefs.HasKey("childName"))
                {
                    if (PlayerPrefs.HasKey("isAgeSelected"))
                    {
                        if (PlayerPrefs.HasKey("isSubscribed"))
                        {
                            SceneManager.LoadScene("Dashboard");
                        }
                        else
                        {
                            SceneManager.LoadScene("IAPCatalog");

                        }
                    }
                    else
                    {
                        SceneManager.LoadScene("SelectAge");
                    }

                }
                else
                {
                    SceneManager.LoadScene("KidsName");
                }
            }
        }
        else
        {
            SceneManager.LoadScene("Description");
        }
    }

}
