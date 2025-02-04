// // namespace LoginInWithGoogle
// // {


// using System;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// // using Google;
// using UnityEngine;
// using UnityEngine.UI;

// public class LoginInWithGoogle : MonoBehaviour
// {

//     public Text statusText;

//     public string webClientId = "http://22065395251-ss0vk4iio7vdlnfund8i05sk67b9ir7g.apps.googleusercontent.com/";

//     private GoogleSignInConfiguration configuration;

//     // Defer the configuration creation until Awake so the web Client ID
//     // Can be set via the property inspector in the Editor.
//     void Awake()
//     {
//         configuration = new GoogleSignInConfiguration
//         {
//             WebClientId = webClientId,
//             RequestIdToken = true
//         };
//     }

//     public void OnSignIn()
//     {

//         GoogleSignIn.Configuration = configuration;
//         GoogleSignIn.Configuration.UseGameSignIn = false;
//         GoogleSignIn.Configuration.RequestIdToken = true;
//         AddStatusText("Calling SignIn");

//         GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
//           OnAuthenticationFinished);
//     }

//     public void OnSignOut()
//     {
//         AddStatusText("Calling SignOut");
//         GoogleSignIn.DefaultInstance.SignOut();
//     }

//     public void OnDisconnect()
//     {
//         AddStatusText("Calling Disconnect");
//         GoogleSignIn.DefaultInstance.Disconnect();
//     }

//     internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
//     {
//         if (task.IsFaulted)
//         {
//             using (IEnumerator<System.Exception> enumerator =
//                     task.Exception.InnerExceptions.GetEnumerator())
//             {
//                 if (enumerator.MoveNext())
//                 {
//                     GoogleSignIn.SignInException error =
//                             (GoogleSignIn.SignInException)enumerator.Current;
//                     AddStatusText("Got Error: " + error.Status + " " + error.Message);
//                 }
//                 else
//                 {
//                     AddStatusText("Got Unexpected Exception?!?" + task.Exception);
//                 }
//             }
//         }
//         else if (task.IsCanceled)
//         {
//             AddStatusText("Canceled");
//         }
//         else
//         {
//             AddStatusText("Welcome: " + task.Result.DisplayName + "!");
//         }
//     }

//     public void OnSignInSilently()
//     {
//         GoogleSignIn.Configuration = configuration;
//         GoogleSignIn.Configuration.UseGameSignIn = false;
//         GoogleSignIn.Configuration.RequestIdToken = true;
//         AddStatusText("Calling SignIn Silently");

//         GoogleSignIn.DefaultInstance.SignInSilently()
//               .ContinueWith(OnAuthenticationFinished);
//     }


//     public void OnGamesSignIn()
//     {
//         GoogleSignIn.Configuration = configuration;
//         GoogleSignIn.Configuration.UseGameSignIn = true;
//         GoogleSignIn.Configuration.RequestIdToken = false;

//         AddStatusText("Calling Games SignIn");

//         GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
//           OnAuthenticationFinished);
//     }

//     private List<string> messages = new List<string>();
//     void AddStatusText(string text)
//     {
//         if (messages.Count == 5)
//         {
//             messages.RemoveAt(0);
//         }
//         messages.Add(text);
//         string txt = "";
//         foreach (string s in messages)
//         {
//             txt += "\n" + s;
//         }
//         statusText.text = txt;
//         Debug.Log(txt);
//     }
// }
// // }