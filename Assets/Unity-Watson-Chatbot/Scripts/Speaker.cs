// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using System;
// using IBM.Cloud.SDK;
// using IBM.Cloud.SDK.Authentication.Iam;
// using IBM.Cloud.SDK.Utilities;
// using IBM.Watson.TextToSpeech.V1;
// // using IBM.Watson.DeveloperCloud.Services.TextToSpeech.v1;
// // using IBM.Watson.DeveloperCloud.Utilities;
// // using IBM.Watson.DeveloperCloud.Connection;
// // using IBM.Watson.DeveloperCloud.Logging;

// public class Speaker : MonoBehaviour
// {
//     public string url; // Your IBM Watson URL
//     public string user; // Your IBM Watson username
//     public string pass; // Your IBM Watson password
//     public string text = "Hello SALSA, I'm Watson, your IBM services representative, how can I help you?";
//     public bool play;

//     private Credentials credentials;
//     private TextToSpeech textToSpeech;
//     private AudioSource audioSrc;

//     void Start ()
//     {
//         credentials = new Credentials(user, pass, url);
//         textToSpeech = new TextToSpeech(credentials);
//         audioSrc = GetComponent<AudioSource>(); // Get the SALSA AudioSource from this GameObject
//     }

//     public void OnButtonClick()
//     {
//         if (play)
//         {
//             play = false;
//             GetTTS();
//         }
//     }
//     // private void Update()
//     // {
//     //     if (play)
//     //     {
//     //         play = false;
//     //         GetTTS();
//     //     }
//     // }

//     private void GetTTS()
//     {
//         textToSpeech.Voice = VoiceType.en_US_Michael;
//         textToSpeech.ToSpeech(OnSuccess, OnFail, text, false);
//     }

//     private void OnSuccess(AudioClip clip, Dictionary<string, object> customData)
//     {
//         if (Application.isPlaying && clip != null && audioSrc != null)
//         {
//             audioSrc.spatialBlend = 0.0f;
//             audioSrc.clip = clip;
//             audioSrc.Play();
//         }
//     }

//     private void OnFail(RESTConnector.Error error, Dictionary<string, object> customData)
//     {
//         Log.Error("ExampleTextToSpeech.OnFail()", "Error received: {0}", error.ToString());
//     }
// }
