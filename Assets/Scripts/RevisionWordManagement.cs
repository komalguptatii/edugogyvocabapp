using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using TMPro;
using UnityEngine.UI;


public class RevisionWordManagement : MonoBehaviour
{

     [SerializeField]
    public TextMeshProUGUI wordList;

    [Serializable]
    public class Antonym
    {
        public int id;
        public string description;
        public string meaning;
        public int word_id;
        public List<AntonymSentence> antonymSentences;
    }

    [Serializable]
    public class AntonymSentence
    {
        public int id;
        public string description;
        public int antonym_id;
    }

    [Serializable]
    public class OtherWayUsingWord
    {
        public int id;
        public string description;
        public int word_id;
        public List<OtherWayUsingWordSentence> otherWayUsingWordSentences;
    }

    [Serializable]
    public class OtherWayUsingWordSentence
    {
        public int id;
        public string description;
        public int other_way_using_word_id;
    }

    [Serializable]
    public class RevisionWord
    {
        public int id;
        public int day_level_id;
        public int type;
        public string name;
        public object image;
        public object image_url;
        public List<object> verbs;
        public List<object> nouns;
        public List<object> adverbs;
        public List<object> adjectives;
        public List<object> dailyUseTips;
        public List<OtherWayUsingWord> otherWayUsingWords;
        public List<object> idioms;
        public List<object> useMultipleWords;
        public List<object> synonyms;
        public List<Antonym> antonyms;
    }

    [Serializable]
    public class Root
    {
        public int id;
        public int type;
        public int age_group_id;
        public int level;
        public RevisionWord[] revisionWords;
    }

    string listOfrevisionWords = "";
    // Start is called before the first frame update
    void Start()
    {
        GetRevisionDetails();
    }

    void GetRevisionDetails() => StartCoroutine(GetRevisionDetails_Coroutine());

    IEnumerator GetRevisionDetails_Coroutine()   //To get level id - for initial use, value of level is 1
    {

        Root rootDetails = new Root();
        string uri = "http://165.22.219.198/edugogy/api/v1/day-levels/3?expand=revisionWords,revisionWords.nouns,revisionWords.nouns.nounSentences,revisionWords.verbs,revisionWords.verbs.verbSentences,revisionWords.adverbs,revisionWords.adverbs.adverbSentences,revisionWords.adjectives,revisionWords.adjectives.adjectiveSentences,revisionWords.dailyUseTips,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords.otherWayUsingWordSentences,revisionWords.idioms,revisionWords.idioms.idiomSentences,revisionWords.useMultipleWords,revisionWords.useMultipleWords.useMultipleWordSentences,revisionWords.synonyms,revisionWords.synonyms.synonymSentences,revisionWords.antonyms,revisionWords.antonyms.antonymSentences";
        var request = new UnityWebRequest(uri, "GET");

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer FXfb7epZDK-riK6XS7goQGK2MdezH2Fd");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

            string jsonString = request.downloadHandler.text;
           
            rootDetails = JsonUtility.FromJson<Root>(jsonString);
            Debug.Log(rootDetails.id);

            
            for(int i = 0; i < rootDetails.revisionWords.Length; i++)
            {
                string revisionWord = rootDetails.revisionWords[i].name;
                Debug.Log(revisionWord);
                listOfrevisionWords = listOfrevisionWords + revisionWord + "\n";
                Debug.Log(listOfrevisionWords);
            }
            wordList.text = listOfrevisionWords;   
        }
    }

}
