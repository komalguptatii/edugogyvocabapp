using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class AgeSelection : MonoBehaviour
{

    void Start()
    {
        GetAgeData();
    }

    void GetAgeData() => StartCoroutine(GetAgeData_Coroutine());

    IEnumerator GetAgeData_Coroutine()
    {
        // outputArea.text = "Loading...";
        string uri = "http://165.22.219.198/edugogy/api/v1/age-groups";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

            //Put data in dropdown


        }
    }
}