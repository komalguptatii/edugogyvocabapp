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
    public Image wordImage;     //Image visible on noun ,verb, adverb, adjective only

    [Serializable]
    public class MissionForm
    {
        public int day_level_id;
    } 

    [SerializeField]
    public TextMeshProUGUI typeOfDay; // new or revision

    [SerializeField]
    public TextMeshProUGUI typeOfWord; // noun, verb etc

    [SerializeField]
    public TextMeshProUGUI word; 

    [SerializeField]
    public Button speakerButton;

    [SerializeField]
    public TextMeshProUGUI meaningAsNoun;

    [SerializeField]
    public TextMeshProUGUI sentenceOfNoun;

    [SerializeField]
    public Canvas dutCanvas;

//     //Verb
//     [SerializeField]
//     public TextMeshProUGUI meaningAsVerb;

//     [SerializeField]
//     public TextMeshProUGUI sentenceOfVerb;


//    //Adverb
//     [SerializeField]
//     public TextMeshProUGUI meaningAsAdverb;

//     [SerializeField]
//     public TextMeshProUGUI sentenceOfAdverb;

//        //Adjective
//     [SerializeField]
//     public TextMeshProUGUI meaningAsAdjective;

//     [SerializeField]
//     public TextMeshProUGUI sentenceOfAdjective;

//     //Conversation
//     [SerializeField]
//     public TextMeshProUGUI convoDescription;


//       //Anotherwayofusingword
//     [SerializeField]
//     public TextMeshProUGUI meaningAnotherwayofusingword;

//     [SerializeField]
//     public TextMeshProUGUI sentenceAnotherwayofusingword;

//     [SerializeField]
//     public TextMeshProUGUI sentenceOfDUT;

//  //Idiom
//     [SerializeField]
//     public TextMeshProUGUI meaningAsIdiom;

//     [SerializeField]
//     public TextMeshProUGUI sentenceOfIdiom;

//      //multiple words
//     [SerializeField]
//     public TextMeshProUGUI placeholderMultipleWords;

//     [SerializeField]
//     public TextMeshProUGUI sentenceOfMultipleWords;

//     //Synonym
//     [SerializeField]
//     public TextMeshProUGUI synonymTitle;

//     [SerializeField]
//     public TextMeshProUGUI wordOfSynonym;

//     [SerializeField]
//     public TextMeshProUGUI meaningAsSynonym;

//     [SerializeField]
//     public TextMeshProUGUI sentenceOfSynonym;

//     //Antonym
//     [SerializeField]
//     public TextMeshProUGUI antonymTitle;

//     [SerializeField]
//     public TextMeshProUGUI wordOfAntonym;
//     [SerializeField]
//     public TextMeshProUGUI meaningAsAntonym;

//     [SerializeField]
//     public TextMeshProUGUI sentenceOfAntonym;

//     [SerializeField]
//     public Image singleSentenceNounBoard;

//     [SerializeField]
//     public Image multipleSentenceNounBoard;

    // [SerializeField]
    // public Transform sentencePrefab;

    // [SerializeField]
    // public GameObject[] noOfSentences; 

    //  [SerializeField]
    // public Canvas[] diffBoards; 

    // [SerializeField]
    // public Transform parent;


    
    string auth_key;
    int dayLevelId;

    int nextNumber = 1;
    int parameterCount = 0; //set after checking values

    int totalNumber = 0;    // total count of dictionary - this is not including provision for multiple screen of individual parameter

    [Serializable]
     public class AllDetail
    {
        public int id;
        public int type;
        public int age_group_id;
        public int level;
        public NewWord[] newWords;
        public RevisionWord[] revisionWords;
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
        public Adjective[] adjectives;
        public DailyUseTip[] dailyUseTips;
        public OtherWayUsingWord[] otherWayUsingWords;
        public Idiom[] idioms;
        public UseMultipleWord[] useMultipleWords;
        public Synonym[] synonyms;
        public Antonym[] antonyms;
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
        public List<Verb> verbs;
        public List<Noun> nouns;
        public List<Adverb> adverbs;
        public List<Adjective> adjectives;
        public List<DailyUseTip> dailyUseTips;
        public List<OtherWayUsingWord> otherWayUsingWords;
        public List<Idiom> idioms;
        public List<UseMultipleWord> useMultipleWords;
        public List<Synonym> synonyms;
        public List<Antonym> antonyms;
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

    Dictionary<string, bool> availableData = new Dictionary<string, bool>()
        {
            {"isNewWordAvailable", false},
            {"newWordNoun", false},
            {"newWordVerb", false},
            {"newWordAdverbs", false},
            {"newWordAdjectives", false},
            {"newWordDailyUseTips", false},
            {"newWordOtherWayUsingWords", false},
            {"newWordIdioms", false},
            {"newWordUseMultipleWords", false},
            {"newWordSynonyms",false},
            {"newWordAntonyms", false},
            {"isRevisionWordsAvailable", false},
            {"revisionNoun", false},
            {"revisionVerb", false},
            {"revisionAdverbs", false},
            {"revisionAdjectives", false},
            {"revisionDailyUseTips", false},
            {"revisionOtherWayUsingWords", false},
            {"revisionIdioms", false},
            {"revisionUseMultipleWords", false},
            {"revisionSynonyms",false},
            {"revisionAntonyms", false},
            {"Questions", false},
            {"Conversation", false},
            {"Passages", false}
        };

    public string nounDetails = "newWords,newWords.nouns,newWords.nouns.nounSentences";
    public string verbDetails = "newWords,newWords.verbs,newWords.verbs.verbSentences";
    public string adverbDetails = "newWords,newWords.adverbs,newWords.adverbs.adverbSentences";
    public string adjectiveDetails = "newWords,newWords.adjectives,newWords.adjectives.adjectiveSentences";
    public string dailyTipsDetails = "newWords,newWords.dailyUseTips";
    public string otherWayUsingWords = "newWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords.otherWayUsingWordSentences";
    public string idioms = "newWords,newWords.idioms,newWords.idioms.idiomSentences";
    public string useMultipleWords = "newWords,newWords.useMultipleWords,newWords.useMultipleWords.useMultipleWordSentences";
    public string synonyms = "newWords,newWords.synonyms,newWords.synonyms.synonymSentences";
    public string antonyms = "newWords,newWords.synonyms,newWords.antonyms,newWords.antonyms.antonymSentences";
    public string questions = "questions,questions.questionOptions";
    public string conversation = "conversation,conversationQuestions,conversationQuestions.questionOptions";
    public string passages = "passages,passages.questions,passages.questions.questionOptions";

    string fixJson(string value)            // Added object type to JSON
    {
        value = "{\"items\":" + value + "}";
        return value;
    }

    // Start is called before the first frame update
    void Awake()
    {
         // start mission after fetching id from detail all api

        // singleSentenceNounBoard.gameObject.SetActive(true);
        // multipleSentenceNounBoard.gameObject.SetActive(false);

         if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
        }

        // if (PlayerPrefs.HasKey("StartLevelID"))
        // {
        //     dayLevelId = PlayerPrefs.GetInt("StartLevelID");
        //     Debug.Log(dayLevelId);
        //     StartMission();
        // }

    }

    async void Start ()
    {
        Debug.Log(availableData.Count);
        totalNumber = availableData.Count;
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

        string uri = "http://165.22.219.198/edugogy/api/v1/day-levels/4?expand=newWords,revisionWords,newWords.nouns,newWords.nouns.nounSentences,newWords.verbs,newWords.verbs.verbSentences,newWords.adverbs,newWords.adverbs.adverbSentences,newWords.adjectives,newWords.adjectives.adjectiveSentences,newWords.dailyUseTips,newWords.otherWayUsingWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords.otherWayUsingWordSentences,newWords.idioms,newWords.idioms.idiomSentences,newWords.useMultipleWords,newWords.useMultipleWords.useMultipleWordSentences,newWords.synonyms,newWords.synonyms.synonymSentences,newWords.antonyms,newWords.antonyms.antonymSentences,revisionWords.nouns,revisionWords.nouns.nounSentences,revisionWords.verbs,revisionWords.verbs.verbSentences,revisionWords.adverbs,revisionWords.adverbs.adverbSentences,revisionWords.adjectives,revisionWords.adjectives.adjectiveSentences,revisionWords.dailyUseTips,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords.otherWayUsingWordSentences,revisionWords.idioms,revisionWords.idioms.idiomSentences,revisionWords.useMultipleWords,revisionWords.useMultipleWords.useMultipleWordSentences,revisionWords.synonyms,revisionWords.synonyms.synonymSentences,revisionWords.antonyms,revisionWords.antonyms.antonymSentences,questions,questions.questionOptions,conversation,conversationQuestions,conversationQuestions.questionOptions,passages,passages.questions,passages.questions.questionOptions";

        var request = new UnityWebRequest(uri, "GET");

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        // request.SetRequestHeader("Authorization", auth_key);
        request.SetRequestHeader("Authorization", "Bearer a8HMPlzEWaj4uglc9xob-1WuI_smGj9t");


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
            if (allDetailData.newWords.Length > 0)
            {
                availableData["isNewWordAvailable"] = true;
                //new word length will always be 1
            }
            
            if (availableData["isNewWordAvailable"] == true)
            {
                if (allDetailData.newWords[0].nouns.Length > 0)
                {
                    availableData["newWordNoun"] = true;
                }
                if (allDetailData.newWords[0].verbs.Length > 0)
                {
                    availableData["newWordVerb"] = true;
                }
                if (allDetailData.newWords[0].adverbs.Length > 0)
                {
                    availableData["newWordAdverbs"] = true;
                }
                if (allDetailData.newWords[0].adjectives.Length > 0)
                {
                    availableData["newWordAdjectives"] = true;
                }
                if (allDetailData.newWords[0].dailyUseTips.Length > 0)
                {
                    availableData["newWordDailyUseTips"] = true;
                }
                if (allDetailData.newWords[0].otherWayUsingWords.Length > 0)
                {
                    availableData["newWordOtherWayUsingWords"] = true;
                }
                if (allDetailData.newWords[0].idioms.Length > 0)
                {
                    availableData["newWordIdioms"] = true;
                }
                if (allDetailData.newWords[0].useMultipleWords.Length > 0)
                {
                    availableData["newWordUseMultipleWords"] = true;
                }
                if (allDetailData.newWords[0].synonyms.Length > 0)
                {
                    availableData["newWordSynonyms"] = true;
                }
                if (allDetailData.newWords[0].antonyms.Length > 0)
                {
                    availableData["newWordAntonyms"] = true;
                }

            }

            foreach(bool value in availableData.Values)
            {
                if (value == true)
                {
                    parameterCount = parameterCount + 1;
                }
            }
            Debug.Log("Parameter count is " + parameterCount);
          Debug.Log(availableData["isNewWordAvailable"]);
          SetBottomTitleLabel();
          SetUpBaseCanvas();

        }
    }

    public void SetUpBaseCanvas()
    {
        //Check for new word availability and its correspondent parameter

        if ( availableData["isNewWordAvailable"] == true)
        {
            NewWord newWordDetails = new NewWord();
            newWordDetails = allDetailData.newWords[0];
            if (newWordDetails.type == 1)
            {
                typeOfDay.text = "New Word";
                word.text = newWordDetails.name;
            }

            if (availableData["newWordNoun"] == true)
            {
                typeOfWord.text = "Noun";
                Noun newNoun = new Noun();
                newNoun = newWordDetails.nouns[0];
                meaningAsNoun.text = newNoun.description;
                sentenceOfNoun.text = newNoun.nounSentences[0].description; // can be multiple check for it
            }
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
            // GetNounDetails();
            // NewWordSetup();

        }
    }

    IEnumerator GetNounData_Coroutine()
    {

        string uri = "http://165.22.219.198/edugogy/api/v1/day-levels/1?expand=newWords.nouns,newWords.nouns.nounSentences";

        var request = new UnityWebRequest(uri, "GET");

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
            string jsonString = request.downloadHandler.text;
            allDetailData = JsonUtility.FromJson<AllDetail>(jsonString);
            Debug.Log("Check no. of new words");
           
            Debug.Log(allDetailData.newWords.Length);
            // NewWordSetup();
            
        }
    }

    public void NounSetup()
    {
        
    }

    // public void VerbSetup()
    // {
    //      if (allDetailData.newWords.Length == 1)
    //         {

    //             newWord.text = allDetailData.newWords[0].name;
    //             StartCoroutine(DownloadImage(allDetailData.newWords[0].image_url));
            
    //             NewWord newWordDetails = new NewWord();
    //             newWordDetails = allDetailData.newWords[0];
    //             title.text = "Verb";
    //             Verb verbDetail = new Verb();
    //             verbDetail = newWordDetails.verbs[0];
    //             meaningAsVerb.text = verbDetail.description;
                
    //             sentenceOfVerb.text = verbDetail.verbSentences[0].description;
    //         }
    // }
    
    // public void AdverbSetup()
    // {
    //      if (allDetailData.newWords.Length == 1)
    //         {

    //             newWord.text = allDetailData.newWords[0].name;
    //             StartCoroutine(DownloadImage(allDetailData.newWords[0].image_url));
            
    //             NewWord newWordDetails = new NewWord();
    //             newWordDetails = allDetailData.newWords[0];
    //             title.text = "Adverb";
    //             Adverb adverbDetail = new Adverb();
    //             adverbDetail = newWordDetails.adverbs[0];
    //             meaningAsAdverb.text = adverbDetail.description;
                
    //             sentenceOfAdverb.text = adverbDetail.adverbSentences[0].description;
    //         }
    // }

    // public void AdjectiveSetup()
    // {
    //      if (allDetailData.newWords.Length == 1)
    //         {

    //             newWord.text = allDetailData.newWords[0].name;
    //             StartCoroutine(DownloadImage(allDetailData.newWords[0].image_url));
            
    //             NewWord newWordDetails = new NewWord();
    //             newWordDetails = allDetailData.newWords[0];
    //             title.text = "Adjective";
    //             Adjective adjectiveDetail = new Adjective();
    //             adjectiveDetail = newWordDetails.adjectives[0];
    //             meaningAsAdjective.text = adjectiveDetail.description;
                
    //             sentenceOfAdjective.text = adjectiveDetail.adjectiveSentences[0].description;
    //         }
    // }

    // public void ConversationSetup()
    // {
    //      if (allDetailData.newWords.Length == 1)
    //         {            
                
    //             Conversation convo = new Conversation();
    //             convo = allDetailData.conversation;
    //             Debug.Log(convo.description);
    //             convoDescription.text = convo.description;
                
    //         }
    // }


    // public void DailyTipsSetup()
    // {
    //      if (allDetailData.newWords.Length == 1)
    //         {

    //             newWord.text = allDetailData.newWords[0].name;
    //             // StartCoroutine(DownloadImage(allDetailData.newWords[0].image_url));
            
    //             NewWord newWordDetails = new NewWord();
    //             newWordDetails = allDetailData.newWords[0];
    //             title.text = "Daily Use Tips";
    //             DailyUseTip dailyUseTipsDetails = new DailyUseTip();
    //             dailyUseTipsDetails = newWordDetails.dailyUseTips[0];
                
    //             sentenceOfDUT.text = dailyUseTipsDetails.description;

    
    //         }
    // }

    // public void AnotherWayOfUsingWordSetup()
    // {
    //     if (allDetailData.newWords.Length == 1)
    //         {

    //             newWord.text = allDetailData.newWords[0].name;
            
    //             NewWord newWordDetails = new NewWord();
    //             newWordDetails = allDetailData.newWords[0];
    //             // title.text = "Another Way of using word";
    //             OtherWayUsingWord anotherwayDetail = new OtherWayUsingWord();
    //             anotherwayDetail = newWordDetails.otherWayUsingWords[0];
    //             meaningAnotherwayofusingword.text = anotherwayDetail.description;
                
    //             sentenceAnotherwayofusingword.text = anotherwayDetail.otherWayUsingWordSentences[0].description;
    //         }
    // }

    // public void IdiomSetup()
    // {
    //     if (allDetailData.newWords.Length == 1)
    //         {

    //             newWord.text = allDetailData.newWords[0].name;
            
    //             NewWord newWordDetails = new NewWord();
    //             newWordDetails = allDetailData.newWords[0];
    //             // title.text = "Another Way of using word";
    //             Idiom idiomDetails = new Idiom();
    //             idiomDetails = newWordDetails.idioms[0];
    //             meaningAsIdiom.text = idiomDetails.description;
                
    //             sentenceOfIdiom.text = idiomDetails.idiomSentences[0].description;
    //         }
    // }

    // public void MultipleWordSetup()
    // {
    //     if (allDetailData.newWords.Length == 1)
    //         {

    //             newWord.text = allDetailData.newWords[0].name;
            
    //             NewWord newWordDetails = new NewWord();
    //             newWordDetails = allDetailData.newWords[0];
    //             // title.text = "Another Way of using word";
    //             UseMultipleWord multipleWordDetails = new UseMultipleWord();
    //             multipleWordDetails = newWordDetails.useMultipleWords[0];
    //             placeholderMultipleWords.text = multipleWordDetails.description;
                
    //             sentenceOfMultipleWords.text = multipleWordDetails.useMultipleWordSentences[0].description;
    //         }
    // }

    // public void AntonymSetup()
    // {
    //     if (allDetailData.newWords.Length == 1)
    //         {

    //             newWord.text = allDetailData.newWords[0].name;
            
    //             NewWord newWordDetails = new NewWord();
    //             newWordDetails = allDetailData.newWords[0];
    //             // title.text = "Another Way of using word";
    //             Antonym antonymDetails = new Antonym();
    //             antonymDetails = newWordDetails.antonyms[0];
    //             antonymTitle.text = antonymDetails.description;
    //             meaningAsAntonym.text = antonymDetails.meaning;
                
    //             sentenceOfAntonym.text = antonymDetails.antonymSentences[0].description;
    //         }
    // }
    // public void SynonymSetup()
    // {
    //     if (allDetailData.newWords.Length == 1)
    //         {

    //             newWord.text = allDetailData.newWords[0].name;
            
    //             NewWord newWordDetails = new NewWord();
    //             newWordDetails = allDetailData.newWords[0];
    //             // title.text = "Another Way of using word";
    //             Synonym synonymDetails = new Synonym();
    //             synonymDetails = newWordDetails.synonyms[0];
    //             synonymTitle.text = synonymDetails.description;
    //             meaningAsSynonym.text = synonymDetails.meaning;
                
    //             sentenceOfSynonym.text = synonymDetails.synonymSentences[0].description;
    //         }
    // }

    public void SetBottomTitleLabel()
    {
        GameObject bottomPanel = GameObject.Find("BottomPanel");
        GameObject childObj = bottomPanel.transform.GetChild(0).gameObject;
        TMPro.TMP_Text numberText = childObj.GetComponent<TMPro.TMP_Text>();
        numberText.text = nextNumber + "/" + parameterCount;
    }

    public void NextButton(Button button)
    {

        // if (button.tag == "Next")
        // {
        //     nextNumber = nextNumber + 1;
        // }
        // else
        // {
        //     if (nextNumber > 1)
        //     {
        //         nextNumber = nextNumber - 1;
        //     }
        // }
        // var currentBoardNumber = nextNumber - 1;

        // for(int i = 0; i < diffBoards.Length; i++)
        // {
        //     if (i != currentBoardNumber)
        //     {
        //         diffBoards[i].gameObject.SetActive(false);
        //     }
        // }

        // diffBoards[currentBoardNumber].gameObject.SetActive(true);

        // Debug.Log(diffBoards[currentBoardNumber].gameObject.name);

        // SetBottomTitleLabel();

        // if (nextNumber == 1)
        // {
        //     NewWordSetup();
        // }
        // else if (nextNumber == 2)
        // {
        //     VerbSetup();
        // }
        // else if (nextNumber == 3)
        // {
        //     AdverbSetup();
        // }
        // else if (nextNumber == 4)
        // {
        //     AdjectiveSetup();
        // }
        // else if (nextNumber == 5)
        // {
        //     ConversationSetup();
        // }
        // else if (nextNumber == 6)
        // {
        //     DailyTipsSetup();
        // }
        // else if (nextNumber == 7)
        // {
        //     AnotherWayOfUsingWordSetup();
        // }
        // else if (nextNumber == 8)
        // {
        //     IdiomSetup();
        // }
        // else if (nextNumber == 9)
        // {
        //     MultipleWordSetup();
        // }
        // else if (nextNumber == 10)
        // {
        //     SynonymSetup();
        // }
        // else if (nextNumber == 11)
        // {
        //     AntonymSetup();
        // }
    }

    

}
