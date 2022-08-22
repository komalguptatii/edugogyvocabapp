using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicrophoneAccess : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        //  #if PLATFORM_ANDROID
        //  if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        //  {
        //     Permission.RequestUserPermission(Permission.Microphone);
        //  }
        // #el
        #if UNITY_IOS
         if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
         {
             Application.RequestUserAuthorization(UserAuthorization.Microphone);
         }
        #endif

        
    }

}
