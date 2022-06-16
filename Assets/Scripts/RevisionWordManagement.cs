using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using TMPro;
using UnityEngine.UI;
using System.Text;


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

    [Serializable]
    public class MissionForm
    {
        public int day_level_id;
    }

    string listOfrevisionWords = "";

    int dayLevelId;
    string auth_key;

    public string nounDetails = "revisionWords,revisionWords.nouns,revisionWords.nouns.nounSentences";
    public string verbDetails = "revisionWords,revisionWords.verbs,revisionWords.verbs.verbSentences";
    public string adverbDetails = "revisionWords,revisionWords.adverbs,revisionWords.adverbs.adverbSentences";
    public string adjectiveDetails = "revisionWords,revisionWords.adjectives,revisionWords.adjectives.adjectiveSentences";
    public string dailyTipsDetails = "revisionWords,revisionWords.dailyUseTips";
    public string otherWayUsingWords = "revisionWords,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords.otherWayUsingWordSentences";
    public string idioms = "revisionWords,revisionWords.idioms,revisionWords.idioms.idiomSentences";
    public string useMultipleWords = "revisionWords,revisionWords.useMultipleWords,revisionWords.useMultipleWords.useMultipleWordSentences";
    public string synonyms = "revisionWords,revisionWords.synonyms,revisionWords.synonyms.synonymSentences";
    public string antonyms = "revisionWords,revisionWords.synonyms,revisionWords.antonyms,revisionWords.antonyms.antonymSentences";
    public string questions = "questions,questions.questionOptions";
    public string conversation = "conversation,conversationQuestions,conversationQuestions.questionOptions";
    public string passages = "passages,passages.questions,passages.questions.questionOptions";

    // Start is called before the first frame update

    void Awake()
    {
        if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
        }

        if (PlayerPrefs.HasKey("StartLevelID"))
        {
            dayLevelId = PlayerPrefs.GetInt("StartLevelID");
            Debug.Log(dayLevelId);
            StartMission();
        }
    }

    void Start()
    {
        // GetRevisionDetails();
         Dictionary<string, bool> revisionDataAvailableData = new Dictionary<string, bool>()
        {
            {"noun", false},
            {"verb", false},
            {"adverbs", false},
            {"adjectives", false},
            {"dailyUseTips", false},
            {"otherWayUsingWords", false},
            {"idioms", false},
            {"useMultipleWords", false},
            {"synonyms",false},
            {"antonyms", false},
            {"questions", false},
            {"conversation", false},
            {"passages", false}
        };
        

        Debug.Log(revisionDataAvailableData.Count);
    }

    void StartMission() => StartCoroutine(StartMission_Coroutine());

    void GetRevisionDetails() => StartCoroutine(GetRevisionDetails_Coroutine());

     IEnumerator StartMission_Coroutine()
    {
        MissionForm missionFormData = new MissionForm { day_level_id = dayLevelId };
        string json = JsonUtility.ToJson(missionFormData);

        Debug.Log(json);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        string uri = "http://165.22.219.198/edugogy/api/v1/student-levels/start-level";

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", auth_key);

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
