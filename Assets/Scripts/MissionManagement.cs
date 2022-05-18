using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;

public class MissionManagement : MonoBehaviour
{
    public class MissionForm
    {
        public int day_level_id;
    }
    // Start is called before the first frame update
    void Awake()
    {
        // StartMission();
    }

    void Start()
    {
        GetNewWordData();
    }

    void StartMission() => StartCoroutine(StartMission_Coroutine());

    void GetNewWordData() => StartCoroutine(GetNounData_Coroutine());


    IEnumerator StartMission_Coroutine()
    {
        MissionForm missionFormData = new MissionForm { day_level_id = 4 };
        string json = JsonUtility.ToJson(missionFormData);

        Debug.Log(json);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        string uri = "http://165.22.219.198/edugogy/api/v1/student-levels/start-level";

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer 3VcmTskZ5jRINDiaO_489b0pdVsbTEy6");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

        }
    }

    IEnumerator GetNounData_Coroutine()
    {

        string uri = "http://165.22.219.198/edugogy/api/v1/day-levels/1?expand=newWords";

        var request = new UnityWebRequest(uri, "GET");

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer 3VcmTskZ5jRINDiaO_489b0pdVsbTEy6");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);


            // { "id":4,"type":1,"age_group_id":2,"level":1,"newWords":[{ "id":4,"day_level_id":4,"type":1,"name":"Orange","pronunciation":null,"sort_order":null,"created_at":null,"updated_at":null,"image":"Screen_Shot_2022-04-28_at_10.44.30_AM-4.png","image_url":"http://165.22.219.198/edugogy/frontend/web/uploads/word/thumb-Screen_Shot_2022-04-28_at_10.44.30_AM-4.png"}]}
        }
    }

}
