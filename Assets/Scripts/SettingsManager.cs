using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using UnityEngine.UI;
using Google;

public class SettingsManager : MonoBehaviour
{

    public void OnClick()
    {
        if (PlayerPrefs.HasKey("platform"))
        {
            string typeOfPlatform = PlayerPrefs.GetString("platform");
            if (typeOfPlatform == "facebook")
            {
                // if 
                FB.LogOut();
            }
            else if (typeOfPlatform == "google")
            {
                OnSignOut();
            }
        }
        
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("Login");
    }

    public void OnMusicOnOff()
    {
        AudioListener.pause = !AudioListener.pause;

    }

     private void OnSignOut()
    {
        GoogleSignIn.DefaultInstance.SignOut();
    }
}
