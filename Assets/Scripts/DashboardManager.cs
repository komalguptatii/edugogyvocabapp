using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

public class DashboardManager : MonoBehaviour
{
    // Start is called before the first frame update

    [Serializable]
    public class ProfileForm
    {
        public string name;
        public string phone;
        public int country_code_id;

        public int age_group_id;

    }

    [Serializable]
     public class AllDetail
    {
        public int id;
        public int type;
        public int age_group_id;
        public int level;
    }

    string levelId;
    // void Start()
    // {
     
    // }

    public void OnButtonClick()
    {
        //Get button text
        GameObject textobj = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject;
        TMP_Text mytext = textobj.GetComponent<TMP_Text>();
        levelId = mytext.text;
        Debug.Log(levelId);
        GetAllDetails();
        
    }

    void GetAllDetails() => StartCoroutine(GetAllDetailsForLevel_Coroutine());

     IEnumerator GetAllDetailsForLevel_Coroutine()   //To get level id - for initial use, value of level is 1
    {

        AllDetail allDetailData = new AllDetail();
        string uri = "http://165.22.219.198/edugogy/api/v1/day-levels/" + levelId + "?expand=newWords,revisionWords,newWords.nouns,newWords.nouns.nounSentences,newWords.verbs,newWords.verbs.verbSentences,newWords.adverbs,newWords.adverbs.adverbSentences,newWords.adjectives,newWords.adjectives.adjectiveSentences,newWords.dailyUseTips,newWords.otherWayUsingWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords.otherWayUsingWordSentences,newWords.idioms,newWords.idioms.idiomSentences,newWords.useMultipleWords,newWords.useMultipleWords.useMultipleWordSentences,newWords.synonyms,newWords.synonyms.synonymSentences,newWords.antonyms,newWords.antonyms.antonymSentences,revisionWords.nouns,revisionWords.nouns.nounSentences,revisionWords.verbs,revisionWords.verbs.verbSentences,revisionWords.adverbs,revisionWords.adverbs.adverbSentences,revisionWords.adjectives,revisionWords.adjectives.adjectiveSentences,revisionWords.dailyUseTips,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords.otherWayUsingWordSentences,revisionWords.idioms,revisionWords.idioms.idiomSentences,revisionWords.useMultipleWords,revisionWords.useMultipleWords.useMultipleWordSentences,revisionWords.synonyms,revisionWords.synonyms.synonymSentences,revisionWords.antonyms,revisionWords.antonyms.antonymSentences,questions,questions.questionOptions,conversation,conversationQuestions,conversationQuestions.questionOptions,passages,passages.questions,passages.questions.questionOptions";
        Debug.Log(uri);
        var request = new UnityWebRequest(uri, "GET");

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer a8HMPlzEWaj4uglc9xob-1WuI_smGj9t");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            string jsonString = request.downloadHandler.text;
           
            allDetailData = JsonUtility.FromJson<AllDetail>(jsonString);
            Debug.Log(allDetailData.id);        //4
            Debug.Log(allDetailData.type);      //1

            PlayerPrefs.SetInt("StartLevelID", allDetailData.id);
            //Move to scene type accordingly and start level 
            // if (allDetailData.type == 1)
            // {
                SceneManager.LoadScene("NewWordDay");
            // }
            // else 
            // {
            //      SceneManager.LoadScene("RevisionWordDay");
            // }

                  
        }
    }


    // called this API for dev purpose only - may be remove it later
    IEnumerator GetUserProfile_Coroutine()
    {

        string uri = "http://165.22.219.198/edugogy/api/v1/students/view";

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

            // {"id":3,"name":"Komal","phone":"9855940600","age_group_id":2,"country_code_id":88,"total_level":2,"total_passed_level":0,"available_level":30}

        }
    }

   
}
