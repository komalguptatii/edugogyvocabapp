using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

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

}
