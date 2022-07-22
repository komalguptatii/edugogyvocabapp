using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{

    public void OnClick()
    {
        PlayerPrefs.DeleteAll();
         SceneManager.LoadScene("Login");
    }

    public void OnMusicOnOff()
    {
        AudioListener.pause = !AudioListener.pause;

    }
}
