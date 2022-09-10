using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AgeSelection : MonoBehaviour
{

    string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1";

    void Start()
    {
        GetAgeData();
    }

    void GetAgeData() => StartCoroutine(GetAgeData_Coroutine());

    IEnumerator GetAgeData_Coroutine()
    {
        // outputArea.text = "Loading...";
        string uri = baseURL +"/age-groups";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

            //Put data in dropdown


        }
    }
}