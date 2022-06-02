using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using TMPro;

public class MissionManagement : MonoBehaviour
{
    [SerializeField] 
    public Image wordImage;

    [Serializable]
    public class MissionForm
    {
        public int day_level_id;
    }

    [SerializeField]
    public TextMeshProUGUI newWord;

    [SerializeField]
    public TextMeshProUGUI meaningAsNoun;

    [SerializeField]
    public TextMeshProUGUI sentenceOfNoun;

    [SerializeField]
    public Button speakerButton;

    [SerializeField]
    public Image singleSentenceNounBoard;

    [SerializeField]
    public Image multipleSentenceNounBoard;

    [SerializeField]
    public Transform sentencePrefab;

    [SerializeField]
    public GameObject[] noOfSentences; 

     [SerializeField]
    public Canvas[] diffBoards; 

    [SerializeField]
    public Transform parent;


    
    string auth_key;
    int dayLevelId;

    int nextNumber = 0;
    int backNumber = 0;

    [Serializable]
     public class AllDetail
    {
        public int id;
        public int type;
        public int age_group_id;
        public int level;
        public NewWord[] newWords;
        public List<object> revisionWords;
        public Conversation conversation;
        public Question[] questions;
        public ConversationQuestion[] conversationQuestions;
        public Passage[] passages;
    }

    [Serializable]
    public class NewWord
    {
        public int id;
        public int day_level_id;
        public int type;
        public string name;
        public string image;
        public string image_url;
        public Verb[] verbs;
        public Noun[] nouns;
        public Adverb[] adverbs;
        public Adjective adjectives;
        public DailyUseTip[] dailyUseTips;
        public OtherWayUsingWord[] otherWayUsingWords;
        public Idiom[] idioms;
        public UseMultipleWord[] useMultipleWords;
        public Synonym[] synonyms;
        public Antonym[] antonyms;
    }

    [Serializable]
     public class Noun
    {
        public int id;
        public string description;
        public int word_id;
        public NounSentence[] nounSentences;
    }

    [Serializable]
    public class NounSentence
    {
        public int id;
        public string description;
        public int noun_id;
    }

    [Serializable]
    public class Verb
    {
        public int id;
        public string description;
        public int word_id;
        public VerbSentence[] verbSentences;
    }

    [Serializable]
    public class VerbSentence
    {
        public int id;
        public string description;
        public int verb_id;
    }

    [Serializable]
    public class Adjective
    {
        public int id;
        public string description;
        public int word_id;
        public AdjectiveSentence[] adjectiveSentences;
    }

    [Serializable]
    public class AdjectiveSentence
    {
        public int id;
        public string description;
        public int adjective_id;
    }

    [Serializable]
    public class Adverb
    {
        public int id;
        public string description;
        public int word_id;
        public AdverbSentence[] adverbSentences;
    }

    [Serializable]
    public class AdverbSentence
    {
        public int id;
        public string description;
        public int adverb_id;
    }

    [Serializable]
    public class Antonym
    {
        public int id;
        public string description;
        public string meaning;
        public int word_id;
        public AntonymSentence[] antonymSentences;
    }

    [Serializable]
    public class AntonymSentence
    {
        public int id;
        public string description;
        public int antonym_id;
    }

    [Serializable]
    public class Conversation
    {
        public int id;
        public int day_level_id;
        public string description;
    }

    [Serializable]
    public class ConversationQuestion
    {
        public int id;
        public string title;
        public int day_level_id;
        public int status;
        public int type;
        public string conversation;
        public object passage_id;
        public List<QuestionOption> questionOptions;
    }

    [Serializable]
    public class DailyUseTip
    {
        public int id;
        public string description;
        public int word_id;
    }

    [Serializable]
    public class Idiom
    {
        public int id;
        public string description;
        public string meaning;
        public int word_id;
        public IdiomSentence[] idiomSentences;
    }

    [Serializable]
    public class IdiomSentence
    {
        public int id;
        public string description;
        public int idiom_id;
    }

    [Serializable]
    public class OtherWayUsingWord
    {
        public int id;
        public string description;
        public int word_id;
        public OtherWayUsingWordSentence[] otherWayUsingWordSentences;
    }

    [Serializable]
    public class OtherWayUsingWordSentence
    {
        public int id;
        public string description;
        public int other_way_using_word_id;
    }

    [Serializable]
    public class Passage
    {
        public int id;
        public string description;
        public int day_level_id;
        public Question[] questions;
    }

    [Serializable]
    public class Question
    {
        public int id;
        public string title;
        public int day_level_id;
        public int status;
        public int type;
        public object passage_id;
        public List<QuestionOption> questionOptions;
    }

    [Serializable]
    public class QuestionOption
    {
        public int id;
        public string option;
        public int value;
    }
   
    [Serializable]    
    public class Synonym
    {
        public int id;
        public string description;
        public string meaning;
        public int word_id;
        public SynonymSentence[] synonymSentences;
    }

    [Serializable]
    public class SynonymSentence
    {
        public int id;
        public string description;
        public int synonym_id;
    }

    [Serializable]
    public class UseMultipleWord
    {
        public int id;
        public string description;
        public int word_id;
        public UseMultipleWordSentence[] useMultipleWordSentences;
    }

    [Serializable]
    public class UseMultipleWordSentence
    {
        public int id;
        public string description;
        public int use_multiple_word_id;
    }

    
   
    string fixJson(string value)            // Added object type to JSON
    {
        value = "{\"items\":" + value + "}";
        return value;
    }

    // Start is called before the first frame update
    void Awake()
    {
        // StartMission(); // start mission after fetching id from detail all api

         if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
        }

    }

    async void Start ()
    {
        GetAllDetails();
    }


IEnumerator DownloadImage(string mediaUrl)
{   
    //  var mediaUrl = "http://165.22.219.198/edugogy/frontend/web/uploads/word/thumb-Screen_Shot_2022-04-28_at_10.44.30_AM-4.png";

    UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
    yield return request.SendWebRequest();
    if(request.isNetworkError || request.isHttpError) 
        Debug.Log(request.error);
    else
    {
        Texture2D myTexture = ((DownloadHandlerTexture)request.downloadHandler).texture;

        wordImage.sprite = Sprite.Create(myTexture, new Rect(0, 0, myTexture.width, myTexture.height), new Vector2(0, 0));
    }
    request.Dispose();
    
} 

    // void DownloadPicture() => StartCoroutine(DownloadImage(string));

    void StartMission() => StartCoroutine(StartMission_Coroutine());

    void GetAllDetails() => StartCoroutine(GetAllDetailsForLevel_Coroutine());

    void GetNounDetails() => StartCoroutine(GetNounData_Coroutine());


         AllDetail allDetailData = new AllDetail();

    IEnumerator GetAllDetailsForLevel_Coroutine()   //To get level id - for initial use, value of level is 1
    {


        string uri = "http://165.22.219.198/edugogy/api/v1/day-levels/1?expand=newWords,revisionWords,newWords.nouns,newWords.nouns.nounSentences,newWords.verbs,newWords.verbs.verbSentences,newWords.adverbs,newWords.adverbs.adverbSentences,newWords.adjectives,newWords.adjectives.adjectiveSentences,newWords.dailyUseTips,newWords.otherWayUsingWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords.otherWayUsingWordSentences,newWords.idioms,newWords.idioms.idiomSentences,newWords.useMultipleWords,newWords.useMultipleWords.useMultipleWordSentences,newWords.synonyms,newWords.synonyms.synonymSentences,newWords.antonyms,newWords.antonyms.antonymSentences,revisionWords.nouns,revisionWords.nouns.nounSentences,revisionWords.verbs,revisionWords.verbs.verbSentences,revisionWords.adverbs,revisionWords.adverbs.adverbSentences,revisionWords.adjectives,revisionWords.adjectives.adjectiveSentences,revisionWords.dailyUseTips,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords.otherWayUsingWordSentences,revisionWords.idioms,revisionWords.idioms.idiomSentences,revisionWords.useMultipleWords,revisionWords.useMultipleWords.useMultipleWordSentences,revisionWords.synonyms,revisionWords.synonyms.synonymSentences,revisionWords.antonyms,revisionWords.antonyms.antonymSentences,questions,questions.questionOptions,conversation,conversationQuestions,conversationQuestions.questionOptions,passages,passages.questions,passages.questions.questionOptions";

        var request = new UnityWebRequest(uri, "GET");

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer a0iG_UYvMEB2AqI2Mu4FtFGdrylGdwBo");

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
            // var details = JSON.Parse(jsonString);
            // Debug.Log(details["id"]);
            // string detailJson = fixJson(jsonString);
            allDetailData = JsonUtility.FromJson<AllDetail>(jsonString);
            dayLevelId = allDetailData.id;
            Debug.Log(allDetailData.conversation.id);
            StartMission();            
        }
    }

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
        request.SetRequestHeader("Authorization", "Bearer a0iG_UYvMEB2AqI2Mu4FtFGdrylGdwBo");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);
            GetNounDetails();

        }
    }

    IEnumerator GetNounData_Coroutine()
    {

        string uri = "http://165.22.219.198/edugogy/api/v1/day-levels/1?expand=newWords.nouns,newWords.nouns.nounSentences";

        var request = new UnityWebRequest(uri, "GET");

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer a0iG_UYvMEB2AqI2Mu4FtFGdrylGdwBo");

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
            allDetailData = JsonUtility.FromJson<AllDetail>(jsonString);
            Debug.Log("Check no. of new words");
           
            Debug.Log(allDetailData.newWords.Length);
            NewWordSetup();
            
        }
    }

    public void NewWordSetup()
    {
         if (allDetailData.newWords.Length == 1)
            {
                Debug.Log("Noun data");
                Debug.Log(allDetailData.newWords[0].name);

                Debug.Log(allDetailData.newWords[0].id);
                newWord.text = allDetailData.newWords[0].name;
                StartCoroutine(DownloadImage(allDetailData.newWords[0].image_url));
                Debug.Log("Check length of noun");
                Debug.Log(allDetailData.newWords[0].nouns.Length);
                NewWord newWordDetails = new NewWord();
                newWordDetails = allDetailData.newWords[0];
                Debug.Log(newWordDetails.nouns[0].description);
                Noun newNoun = new Noun();
                newNoun = newWordDetails.nouns[0];
                meaningAsNoun.text = newNoun.description;
                if (newNoun.nounSentences.Length > 1)
                {
                    for (var i = 0; i < newNoun.nounSentences.Length; i++)
                    {
                        singleSentenceNounBoard.enabled = false;
                        multipleSentenceNounBoard.enabled = true;
                        Vector2 prefabPosition = sentencePrefab.transform.position;
                        RectTransform rt = (RectTransform)sentencePrefab.transform;
                        var height = rt.rect.height;
                        var calculatedHeight = prefabPosition.y - height - 27f;
                        GameObject secondSentencePrefab = Instantiate(sentencePrefab).gameObject;
                        secondSentencePrefab.transform.position = new Vector2(prefabPosition.x, calculatedHeight);
                        secondSentencePrefab.transform.SetParent(parent, true);
                        noOfSentences[i] = secondSentencePrefab;
                        GameObject childObj = secondSentencePrefab.transform.GetChild(0).gameObject;
                        TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                        mytext.text = newNoun.nounSentences[i].description;
                    }
                }
                else
                {
                    singleSentenceNounBoard.enabled = true;
                    multipleSentenceNounBoard.enabled = false;
                    sentenceOfNoun.text = newNoun.nounSentences[0].description;
                }
            }
    }

    public void NextButton()
    {
        nextNumber = nextNumber + 1;

    }

    public void BackButton()
    {
        if (backNumber > 0)
        {
        backNumber = backNumber - 1;

        }
    }

}
