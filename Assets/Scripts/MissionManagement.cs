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
    public Image singleSentenceBoard;

    [SerializeField]
    public Image multipleSentenceBoard;

     [SerializeField]
    public GameObject sentencePrefab;

      [SerializeField]
    public GameObject singleSentencePrefab;

     [SerializeField]
    public Transform parent;

    [SerializeField]
    public GameObject baseParentBoard;

    [SerializeField]
    public GameObject conversationBoard;

    [SerializeField]
    public TextMeshProUGUI conversationText;

    [SerializeField]
    public GameObject generalBaseBoard;

      [SerializeField]
    public GameObject dutBoard;

    [SerializeField]
    public GameObject dutSentencePrefab;

    [SerializeField]
    public Transform dutParent;

     [SerializeField]
    public GameObject revisionWordBoard;

    [SerializeField]
    public TextMeshProUGUI revisionWordList;

    [SerializeField]
    public GameObject conversationWithMCQBoard;

    [SerializeField]
    public Transform convoPassageMCQPrefabParent;

    [SerializeField]
    public GameObject convoContentPrefab;

    [SerializeField]
    public Sprite tickSprite;

    [SerializeField]
    public Sprite wrongSprite;

     [SerializeField]
    public GameObject mcqPrefab;

    [SerializeField]
    public GameObject generalMCQBoard;

    [SerializeField]
    public GameObject generalMCQContent;

    private Vector3 sentenceRealPos;
  

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
        public string interactive_line_new_word;
        public string interactive_line_revision_word;
        public string interactive_line_mcq;
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
        public QuestionOption[] questionOptions;
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
        public QuestionOption[] questionOptions;
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

    [Serializable]
    public class MoreDatum
    {
        public int word_id;
        public int noun_count;
        public int verb_count;
        public int adverb_count;
        public int adjective_count;
        public int daily_use_tip_count;
        public int other_way_using_count;
        public int idiom_count;
        public int use_multiple_count;
        public int synonym_count;
        public int antonym_count;
        public int passage_id;
        public int question_count;
    }

    [Serializable]
    public class NewWordData
    {
        public int new_word_count;
        public MoreDatum[] more_data;
    }

    [Serializable]
    public class PassageData
    {
        public int passage_count;
        public MoreDatum[] more_data;
    }

    [Serializable]
    public class RevisionWordData
    {
        public int revison_word_count;
        public MoreDatum[] more_data;
    }

    [Serializable]
    public class DataCount
    {
        public int id;
        public int type;
        public int age_group_id;
        public int level;
        public string interactive_line_new_word;
        public string interactive_line_revision_word;
        public string interactive_line_mcq;
        public NewWordData new_word_data;
        public RevisionWordData revision_word_data;
        public int conversation_revision_word_count;
        public int conversation_new_word_count;
        public int conversation_mcq_count;
        public PassageData passage_data;
        public int mcq_count;
    }


    Dictionary<string, bool> availableData = new Dictionary<string, bool>()
        {
            {"isNewWordAvailable", false},
            {"isRevisionWordsAvailable", false},
            {"Questions", false},
            {"Conversation", false},
            {"Passages", false}
        };

     Dictionary<string, bool> dataDisplayed = new Dictionary<string, bool>()
    {
         {"isNounDone", false},
        {"isVerbDone", false},
        {"isAdverbDone",false},
        {"isAdjectiveDone",false},
        {"isNewWordConverstaionDone",false},
        {"isDUTDone",false},
        {"isnewWordOWUWordDone",false},
        {"isnewWordIdiomDone",false},
        {"isnewWordUsingMultipleWordsDone",false},
        {"isnewWordSynonymDone",false},
        {"isnewWordAntonymDone",false},
        {"isRevisionWordListDone", false},
        {"isRevisionWordSynonymDone", false},
        {"isRevisionWordAntonymDone", false},
        {"isRevisionWordOWUWordDone", false},
        {"isRevisionWordUsingMultipleWordsDone", false},
        {"isRevisionWordIdiomsDone", false},
        {"isRevisionWordConversationDone", false},
        {"isRevisionWordContentDone", false},
        {"isConversationMCQDone",false},
        {"isPassageMCQDone",false},
        {"isGeneralMCQDone", false}
    };

    public List<GameObject> sentencePrefabsArray = new List<GameObject>();
    public List<GameObject> convoWithMCQPrefabsArray = new List<GameObject>();

    // public GameObject[] sentencePrefabsArray;

    public bool isSettingCanvas = false;

    public int parameterCountControlCheck = 0;
    public int screenCount = 1;
    string listOfrevisionWords = "";
    public int newWordDataCount = 0;
    public int revisionDataCount = 0;
    public int newWordNumber = 0; // check if all new word screens are done
    public int revisionWordReference = 0;
    public int numberOfRevisionWords = 0;
    public int rwdataCount = 0;
    public int tempDataCount = 0;

    public List<Action<int>> methodCallArray = new List<Action<int>>();
    public List<int> parameterValueArray = new List<int>();
    int questionNumber = 0;
    int generalMCQcount = 0;
    int tempGeneralMCQCount = 0;
    // Button[] otherQbuttons;
    // Button[] firstQbuttons;
    

    // public string nounDetails = "newWords,newWords.nouns,newWords.nouns.nounSentences";
    // public string verbDetails = "newWords,newWords.verbs,newWords.verbs.verbSentences";
    // public string adverbDetails = "newWords,newWords.adverbs,newWords.adverbs.adverbSentences";
    // public string adjectiveDetails = "newWords,newWords.adjectives,newWords.adjectives.adjectiveSentences";
    // public string dailyTipsDetails = "newWords,newWords.dailyUseTips";
    // public string otherWayUsingWords = "newWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords.otherWayUsingWordSentences";
    // public string idioms = "newWords,newWords.idioms,newWords.idioms.idiomSentences";
    // public string useMultipleWords = "newWords,newWords.useMultipleWords,newWords.useMultipleWords.useMultipleWordSentences";
    // public string synonyms = "newWords,newWords.synonyms,newWords.synonyms.synonymSentences";
    // public string antonyms = "newWords,newWords.synonyms,newWords.antonyms,newWords.antonyms.antonymSentences";
    // public string questions = "questions,questions.questionOptions";
    // public string conversation = "conversation,conversationQuestions,conversationQuestions.questionOptions";
    // public string passages = "passages,passages.questions,passages.questions.questionOptions";

    string fixJson(string value)            // Added object type to JSON
    {
        value = "{\"items\":" + value + "}";
        return value;
    }

    // Start is called before the first frame update
    void Awake()
    {
         // start mission after fetching id from detail all api
        sentenceRealPos = singleSentenceBoard.transform.position;
        singleSentenceBoard.gameObject.SetActive(true);
        multipleSentenceBoard.gameObject.SetActive(false);
        dutBoard.gameObject.SetActive(false);

         if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
        }

        if (PlayerPrefs.HasKey("StartLevelID"))
        {
            dayLevelId = PlayerPrefs.GetInt("StartLevelID");
            Debug.Log(dayLevelId);
            // StartMission();
        }

    }

    async void Start ()
    {
        GetDataCount();
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

    void GetDataCount() => StartCoroutine(GetDataCount_Coroutine());

    AllDetail allDetailData = new AllDetail();
    NewWord newWordDetails = new NewWord();
    RevisionWord revisionWordDetails = new RevisionWord();
    DataCount dataCountDetails = new DataCount();


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

    IEnumerator GetDataCount_Coroutine()
    {
        string uri = "http://165.22.219.198/edugogy/api/v1/day-levels/data-count/4";

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
           
            dataCountDetails = JsonUtility.FromJson<DataCount>(jsonString);
            // 
            
            calculateTotalCount();

        }

    }

    public void calculateTotalCount()
    {
        generalMCQcount = dataCountDetails.mcq_count;
        // totalNumber = dataCountDetails.mcq_count;
        newWordDataCount  = dataCountDetails.new_word_data.more_data[0].noun_count +
        dataCountDetails.new_word_data.more_data[0].verb_count + dataCountDetails.new_word_data.more_data[0].adverb_count +
        dataCountDetails.new_word_data.more_data[0].adjective_count + dataCountDetails.new_word_data.more_data[0].daily_use_tip_count +
        dataCountDetails.new_word_data.more_data[0].other_way_using_count
        + dataCountDetails.new_word_data.more_data[0].idiom_count
        + dataCountDetails.new_word_data.more_data[0].use_multiple_count
        + dataCountDetails.new_word_data.more_data[0].synonym_count
        + dataCountDetails.new_word_data.more_data[0].antonym_count;
        
        Debug.Log(newWordDataCount);
        totalNumber = generalMCQcount + newWordDataCount;

        if  (dataCountDetails.revision_word_data.revison_word_count != 0)
        {
            totalNumber = totalNumber + 1;          //for revision word list
            numberOfRevisionWords = dataCountDetails.revision_word_data.revison_word_count;
            for(int i = 0; i < dataCountDetails.revision_word_data.more_data.Length; i++)
            {
                revisionDataCount = dataCountDetails.revision_word_data.more_data[i].other_way_using_count + 
                     dataCountDetails.revision_word_data.more_data[i].idiom_count
                + dataCountDetails.revision_word_data.more_data[i].use_multiple_count
                + dataCountDetails.revision_word_data.more_data[i].synonym_count
                + dataCountDetails.revision_word_data.more_data[i].antonym_count;
             }
       
        }

        //no. of revision words i.e. dataCountDetails.revision_word_data.revison_word_count
        //total count for each revision word
        // for back button  - working on calling method with parameter
        
        Debug.Log(revisionDataCount);
       totalNumber = totalNumber + revisionDataCount + dataCountDetails.conversation_revision_word_count 
       + dataCountDetails.conversation_new_word_count  
       + dataCountDetails.passage_data.passage_count;

        if (dataCountDetails.conversation_mcq_count != 0)   // as mcqs related to 1 conversation will be displayed on one screen so it will be counted as 1 only, also as per client requirement - there is one converstaion with mcq only
        {
            totalNumber += 1;
        }

       Debug.Log(totalNumber);
         GetAllDetails();


    }

    public void HideSpeakerAndImage()
    {
        speakerButton.gameObject.SetActive(false);
        wordImage.gameObject.SetActive(false);
    }

    public void HideSentences()
    {
        speakerButton.gameObject.SetActive(false);
        wordImage.gameObject.SetActive(false);
    }

    public void DestroyPrefabs()
    {
       int prefabCount = parent.transform.childCount;
       Debug.Log("prefabCount is " + prefabCount);
       if (prefabCount > 1)
       {
            for (int i = 1; i < prefabCount; i++)
            {
                    Debug.Log("value of i " + i);
                    Destroy(parent.transform.GetChild(i).gameObject);
            }
       }
       
       sentencePrefabsArray.Clear();
    }

    public void DestroyConvoPrefabs()
    {
        int prefabCount = convoPassageMCQPrefabParent.transform.childCount;
       Debug.Log("prefabCount is " + prefabCount);
       if (prefabCount > 1)
       {
            for (int i = 1; i < prefabCount; i++)
            {
                    Debug.Log("value of i " + i);
                    Destroy(convoPassageMCQPrefabParent.transform.GetChild(i).gameObject);
            }
       }
       
       convoWithMCQPrefabsArray.Clear();
    }

    public void RevisionWordDataCount(int number)       //calculation of revision word data count
    {
       rwdataCount = dataCountDetails.revision_word_data.more_data[number].other_way_using_count + 
                     dataCountDetails.revision_word_data.more_data[number].idiom_count
                + dataCountDetails.revision_word_data.more_data[number].use_multiple_count
                + dataCountDetails.revision_word_data.more_data[number].synonym_count
                + dataCountDetails.revision_word_data.more_data[number].antonym_count;
    }

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
            if (allDetailData.revisionWords.Length > 0)
            {
                availableData["isRevisionWordsAvailable"] = true;
            }
            if (allDetailData.newWords[0].image_url != null)
            {
                string imageURL = allDetailData.newWords[0].image_url;
                StartCoroutine(DownloadImage(imageURL));
            }
            SetBottomTitleLabel();
            SetUpBaseCanvas();

        }
    }

    void ShowInteractivePopup()
    {
        SoundManagerScript.playPopUpsound();
        string message = allDetailData.interactive_line_new_word;
        InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
			//Init popup with params (canvas, text, text, text, action)
			popup.Init(UIController.Instance.MainCanvas,
				message,
				"Let's enjoy"
				);
    }

    public void SetUpBaseCanvas()
    {
       isSettingCanvas = true;  //bool to differentiate if setting canvas or making next or back calls
        if ( availableData["isNewWordAvailable"] == true && newWordDataCount != newWordNumber)
        {
            newWordDetails = allDetailData.newWords[0];
            if (newWordDetails.type == 1)
            {
                typeOfDay.text = "New Word";
                word.text = newWordDetails.name;                
            }
            
            // if ((dataCountDetails.new_word_data.more_data[0].noun_count != 0) && (parameterCountControlCheck != dataCountDetails.new_word_data.more_data[0].noun_count) && (dataDisplayed["isNounDone"] == false)) // check if data displayed or not, add check count condition
            if ((dataCountDetails.new_word_data.more_data[0].noun_count != 0) && (dataDisplayed["isNounDone"] == false)) // check if data displayed or not, add check count condition
            {
                Debug.Log("Calling Noun Setup");
                newWordNumber += 1;
               
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(NounSetup);
                NounSetup(parameterCountControlCheck);                
            }
            else if ((dataCountDetails.new_word_data.more_data[0].verb_count != 0)  && (dataDisplayed["isVerbDone"] == false))
            {
                
                Debug.Log("Calling verb Setup");
                newWordNumber += 1;
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(VerbSetup);
                VerbSetup(parameterCountControlCheck);
            }
            else if ((dataCountDetails.new_word_data.more_data[0].adverb_count != 0)  && (dataDisplayed["isAdverbDone"] == false))
            {
                
                Debug.Log("Calling adverb Setup");
                newWordNumber += 1;
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(AdverbSetup);
                AdverbSetup(parameterCountControlCheck);
            }
            else if ((dataCountDetails.new_word_data.more_data[0].adjective_count != 0)  && (dataDisplayed["isAdjectiveDone"] == false))
            {
                
                Debug.Log("Calling adjective Setup");
                newWordNumber += 1;
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(AdjectiveSetup);
                AdjectiveSetup(parameterCountControlCheck);
            }
            else if ((dataCountDetails.conversation_new_word_count != 0) && (dataDisplayed["isNewWordConverstaionDone"] == false))
            {
                
                newWordNumber += 1;
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(ConversationSetup);
                ConversationSetup(parameterCountControlCheck);
            }
            else if ((dataCountDetails.new_word_data.more_data[0].daily_use_tip_count != 0) && (dataDisplayed["isDUTDone"] == false))
            {
               
                newWordNumber += 1;
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(DailyTipsSetup);
                DailyTipsSetup(parameterCountControlCheck);

            }
            else if ((dataCountDetails.new_word_data.more_data[0].other_way_using_count != 0) && (dataDisplayed["isnewWordOWUWordDone"] == false))
            {
               
                newWordNumber += 1;
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(AnotherWayOfUsingWordSetup);
                AnotherWayOfUsingWordSetup(parameterCountControlCheck);

            }
            else if ((dataCountDetails.new_word_data.more_data[0].idiom_count != 0) && (dataDisplayed["isnewWordIdiomDone"] == false))
            {
                
                newWordNumber += 1;
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(IdiomSetup);
                IdiomSetup(parameterCountControlCheck);

            }
             else if ((dataCountDetails.new_word_data.more_data[0].use_multiple_count != 0) && (dataDisplayed["isnewWordUsingMultipleWordsDone"] == false))
            {
               
                newWordNumber += 1;
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(MultipleWordSetup);
                MultipleWordSetup(parameterCountControlCheck);

            }
            else if ((dataCountDetails.new_word_data.more_data[0].antonym_count != 0) && (dataDisplayed["isnewWordAntonymDone"] == false))
            {
                
                newWordNumber += 1;
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(AntonymSetup);
                AntonymSetup(parameterCountControlCheck);

            }
            else if ((dataCountDetails.new_word_data.more_data[0].synonym_count != 0) && (dataDisplayed["isnewWordSynonymDone"] == false))
            {
                   
                newWordNumber += 1;  
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(SynonymSetup);
                SynonymSetup(parameterCountControlCheck);
            }
            // else 
            if (newWordNumber == 1 && dataCountDetails.interactive_line_new_word != "")
                {
                    ShowInteractivePopup();
                }
        }

        Debug.Log("Checking for revision world" + newWordNumber);
        if ((newWordDataCount != 0 && newWordNumber == newWordDataCount) || newWordDataCount == 0) 
        {
             HideSpeakerAndImage();

            DestroyPrefabs();
            // new word displaying is done start checking for revision word list and related data
            Debug.Log("Entering to revision world");

            // if list is displayed run for loop, all methods for first word and so on
            // if (dataDisplayed["isRevisionWordListDone"] == true && revisionDataCount == 1) 
            if ((availableData["isRevisionWordsAvailable"] == true) && (dataDisplayed["isRevisionWordListDone"] == false))
            {
                    // display revision list
                    revisionDataCount += 1;
                    RevisionWordDataCount(revisionWordReference);
                    parameterValueArray.Add(0);
                    methodCallArray.Add(RevisionWordList);  

                    RevisionWordList(0);
            }
            else if (dataDisplayed["isRevisionWordListDone"] == true && dataDisplayed["isRevisionWordContentDone"] == false)
            {                        
                    revisionWordDetails = allDetailData.revisionWords[revisionWordReference];
                    Debug.Log(revisionWordDetails.id);

                    if ((dataCountDetails.revision_word_data.more_data[revisionWordReference].synonym_count != 0) && (dataDisplayed["isRevisionWordSynonymDone"] == false))
                    {

                         tempDataCount += 1;   
                         parameterValueArray.Add(parameterCountControlCheck);
                         methodCallArray.Add(SynonymSetup);  
                        SynonymSetup(parameterCountControlCheck);
                    }
                    else if ((dataCountDetails.revision_word_data.more_data[revisionWordReference].antonym_count != 0) && (dataDisplayed["isRevisionWordAntonymDone"] == false))
                    {
                     
                         tempDataCount += 1;  
                          parameterValueArray.Add(parameterCountControlCheck);
                         methodCallArray.Add(AntonymSetup);   
                        AntonymSetup(parameterCountControlCheck);
                    }
                    else if ((dataCountDetails.revision_word_data.more_data[revisionWordReference].other_way_using_count != 0) && (dataDisplayed["isRevisionWordOWUWordDone"] == false))
                    {
                    
                    tempDataCount += 1;
                    parameterValueArray.Add(parameterCountControlCheck);
                    methodCallArray.Add(AnotherWayOfUsingWordSetup); 
                    AnotherWayOfUsingWordSetup(parameterCountControlCheck);

                    }
                    else if ((dataCountDetails.revision_word_data.more_data[revisionWordReference].use_multiple_count != 0) && (dataDisplayed["isRevisionWordUsingMultipleWordsDone"] == false))
                    {
            
                         tempDataCount += 1; 
                         parameterValueArray.Add(parameterCountControlCheck);
                         methodCallArray.Add(MultipleWordSetup);    
                        MultipleWordSetup(parameterCountControlCheck);
                    }
                    else if ((dataCountDetails.revision_word_data.more_data[revisionWordReference].idiom_count != 0) && (dataDisplayed["isRevisionWordIdiomsDone"] == false))
                    {                          
                        tempDataCount += 1;
                        parameterValueArray.Add(parameterCountControlCheck);
                        methodCallArray.Add(IdiomSetup); 
                        IdiomSetup(parameterCountControlCheck);
                    }
                    else if ((dataCountDetails.conversation_revision_word_count != 0) && (dataDisplayed["isRevisionWordConversationDone"] == false))
                    {
                                                  
                         tempDataCount += 1; 
                         parameterValueArray.Add(parameterCountControlCheck);
                        methodCallArray.Add(ConversationSetup);     
                        ConversationSetup(parameterCountControlCheck);
                    }
                    
                
                    if ((tempDataCount == rwdataCount) )
                    {
                        dataDisplayed["isRevisionWordContentDone"] = true;
                        Debug.Log("isRevisionWordContentDone ");
                    }
               

                    Debug.Log("tempDataCount " + tempDataCount);
                    Debug.Log("rwdataCount " + rwdataCount);
                    Debug.Log("revisionWordReference" + revisionWordReference);   
            }
             else if (dataDisplayed["isRevisionWordContentDone"] == true && dataDisplayed["isConversationMCQDone"] == false)
            {
                //call for mcq
                if (dataCountDetails.conversation_mcq_count != 0)
                {
                    Debug.Log("conversation_mcq_count" + dataCountDetails.conversation_mcq_count);
                    parameterValueArray.Add(0);
                    methodCallArray.Add(ConversationWithMCQSetup);     

                    ConversationWithMCQSetup(0); // conversation mcq will be one only for now as per requirement from client
                    
                    // // make a method to display these mcqs & dataDisplayed["isConversationMCQDone"] is true after displaying and taking answer
                    // dataDisplayed["isConversationMCQDone"] = true;
                }
             }
            else if (dataDisplayed["isPassageMCQDone"] == false && dataDisplayed["isRevisionWordContentDone"] == true) // && dataDisplayed["isConversationMCQDone"] == true)
            {
                if (dataCountDetails.passage_data.passage_count != 0)
                {
                        DestroyConvoPrefabs();
                        Debug.Log("passage_count" + dataCountDetails.passage_data.passage_count);
                        parameterValueArray.Add(parameterCountControlCheck);
                         methodCallArray.Add(PassageMCQSetup);  

                        PassageMCQSetup(parameterCountControlCheck);
                        // parameterCountControlCheck pending
                }
            }
            else if (dataDisplayed["isGeneralMCQDone"] == false && dataDisplayed["isRevisionWordContentDone"] == true) // dataDisplayed["isPassageMCQDone"] == true && 
            {
                if (dataCountDetails.mcq_count != 0)
                {
                      parameterValueArray.Add(tempGeneralMCQCount);
                         methodCallArray.Add(GeneralMCQSetup);  
                    GeneralMCQSetup(tempGeneralMCQCount);
                    
                }
            } 
        }
    }

    public void GeneralMCQSetup(int parameter)
    {
         DestroyConvoPrefabs();
        conversationWithMCQBoard.gameObject.SetActive(false);
        generalMCQBoard.gameObject.SetActive(true);

        GameObject topImage = generalMCQContent.transform.GetChild(0).gameObject;
        TMPro.TMP_Text mcqInteractiveStatement = topImage.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();

        mcqInteractiveStatement.text = allDetailData.interactive_line_mcq;

        GameObject mcqBoard = generalMCQContent.transform.GetChild(1).gameObject;
        GameObject questionBoard = mcqBoard.transform.GetChild(0).gameObject;
        TMPro.TMP_Text questionText = questionBoard.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
        questionText.text = allDetailData.questions[parameter].title;

        GameObject optionBoard = mcqBoard.transform.GetChild(1).gameObject;
        GameObject optionContainer = optionBoard.transform.GetChild(0).gameObject;

        int answerOptions = allDetailData.questions[parameter].questionOptions.Length;
        Debug.Log(answerOptions);


        int children = optionContainer.transform.childCount;
        
            Button[] otherMCQbuttons = new Button[answerOptions];

        questionNumber = parameter;

        for(int j = 0; j < children; j++ )
        {
            Button thisButton = optionContainer.transform.GetChild(j).GetComponent<Button>();
            if (j <= answerOptions-1)
            {
                TMPro.TMP_Text answerOption = thisButton.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                answerOption.text = allDetailData.questions[parameter].questionOptions[j].option;
                GameObject rightWrongImage = thisButton.transform.GetChild(2).gameObject;
                rightWrongImage.SetActive(false);
                thisButton.onClick.AddListener(delegate{CheckRightMCQAnswer(thisButton,questionNumber);});
                otherMCQbuttons[j] = thisButton;

            }
            else
            {
                thisButton.gameObject.SetActive(false);
            }
        }

        if (isSettingCanvas == true)
        {
            if (tempGeneralMCQCount == generalMCQcount - 1)
            {
                Debug.Log("general mcq is complete here");
                dataDisplayed["isGeneralMCQDone"] = true;
                tempGeneralMCQCount = 0;     //resetting 
            }
            else //if (dataCountDetails.new_word_data.more_data[0].noun_count > 1)
            {
                Debug.Log("Working on calling another mcq");
                tempGeneralMCQCount = tempGeneralMCQCount + 1;
            }
            
             isSettingCanvas = false;
        }

        
    }

    public void CheckRightMCQAnswer(Button button, int questionNumber)
    {
        // int questionNumber - check the answer value 1 for particular question
        // allDetailData.conversationQuestions[0].questionOptions[j].option - j is option number
        int tag = System.Convert.ToInt32(button.tag);
        int value = allDetailData.questions[questionNumber].questionOptions[tag].value;
        Debug.Log(button.tag);
        Debug.Log("value of" + value);
        GameObject rightWrongImage = button.transform.GetChild(2).gameObject;
        Image myImage = rightWrongImage.GetComponent<Image>();

        if (value == 1)
        {
            myImage.sprite = tickSprite;
            SoundManagerScript.RightAnswerSound();
        }
        else
        {
            myImage.sprite = wrongSprite;
        }
       
        rightWrongImage.SetActive(true);

    }


    public void PassageMCQSetup(int parameter)
    {
        baseParentBoard.gameObject.SetActive(false);
        conversationWithMCQBoard.gameObject.SetActive(true);
        int passageCount = dataCountDetails.passage_data.passage_count;

        if (allDetailData.passages[parameter].questions.Length > 1)
        {
            convoWithMCQPrefabsArray.Add(convoContentPrefab);

    
            for (var i = 0; i < allDetailData.passages[parameter].questions.Length; i++)
            {
                if (i == 0)
                {
                     SetUpSinglePassageWithMCQ();
                
                }
                else
                {
                    
                    Vector2 prefabPosition = convoWithMCQPrefabsArray[i - 1].transform.position;
                    GameObject newPrefab = Instantiate(mcqPrefab).gameObject;
                    if (i == 1)
                    {
                        newPrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 1300f);

                    }
                    else
                    {
                        newPrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 400f);

                    }
                    newPrefab.transform.SetParent(convoPassageMCQPrefabParent, true);

                    GameObject questionBoard = newPrefab.transform.GetChild(0).gameObject;

                    TMPro.TMP_Text question = questionBoard.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                    question.text = allDetailData.passages[parameter].questions[i].title;

                    int answerOptions = allDetailData.passages[parameter].questions[i].questionOptions.Length;
                    Debug.Log(answerOptions);

                    GameObject optionBoard = newPrefab.transform.GetChild(1).gameObject;
                    GameObject optionContainer = optionBoard.transform.GetChild(0).gameObject;

                    int children = optionContainer.transform.childCount;
                    Button[] otherPassagebuttons = new Button[answerOptions];

                    Debug.Log("value of i here is " + i);
                    questionNumber = i;

                    for(int j = 0; j < children; j++ )
                    {
                        Button thisButton = optionContainer.transform.GetChild(j).GetComponent<Button>();
                        if (j <= answerOptions-1)
                        {
                            TMPro.TMP_Text answerOption = thisButton.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                            answerOption.text = allDetailData.passages[0].questions[0].questionOptions[j].option;
                            GameObject rightWrongImage = thisButton.transform.GetChild(2).gameObject;
                            rightWrongImage.SetActive(false);
                            thisButton.onClick.AddListener(delegate{CheckRightAnswerForPassageQuestion(thisButton, parameterCountControlCheck,questionNumber);});
                            otherPassagebuttons[j] = thisButton;

                        }
                        else
                        {
                            thisButton.gameObject.SetActive(false);
                        }
                    }
                    convoContentPrefab.transform.SetAsFirstSibling();

                    convoWithMCQPrefabsArray.Add(newPrefab);
                    
                }  

            }
        }
        else
        {
            
            SetUpSinglePassageWithMCQ();

        } 

        if (isSettingCanvas == true)
        {
            if (parameterCountControlCheck == passageCount - 1)
            {
                Debug.Log("passage mcq is complete here");
                dataDisplayed["isPassageMCQDone"] = true;
                parameterCountControlCheck = 0;     //resetting 
            }
            else 
            {
                Debug.Log("Working on calling another passage mcq");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
            
             isSettingCanvas = false;
        }
        
        
    }

    public void SetUpSinglePassageWithMCQ()
    {
        GameObject convoBoard = convoContentPrefab.transform.GetChild(0).gameObject;
            TMPro.TMP_Text mytext = convoBoard.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            mytext.text = "Passage";

            GameObject scrollBar = convoBoard.transform.GetChild(2).gameObject;
            GameObject container = scrollBar.transform.GetChild(0).gameObject;
            
            TMPro.TMP_Text descriptionText = container.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
            descriptionText.text = allDetailData.passages[0].description;

            GameObject mcqBoard = convoContentPrefab.transform.GetChild(1).gameObject;
            GameObject questionBoard = mcqBoard.transform.GetChild(0).gameObject;

            TMPro.TMP_Text question = questionBoard.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
            question.text = allDetailData.passages[0].questions[0].title;

            int answerOptions = allDetailData.passages[0].questions[0].questionOptions.Length;
            Debug.Log(answerOptions);

            GameObject optionBoard = mcqBoard.transform.GetChild(1).gameObject;
            GameObject optionContainer = optionBoard.transform.GetChild(0).gameObject;

            int children = optionContainer.transform.childCount;
            Button[] passageButtons = new Button[answerOptions];

            questionNumber = 0;

            for(int j = 0; j < children; j++ )
            {
                Button thisButton = optionContainer.transform.GetChild(j).GetComponent<Button>();
                if (j <= answerOptions-1)
                {
                    TMPro.TMP_Text answerOption = thisButton.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                    answerOption.text = allDetailData.passages[0].questions[0].questionOptions[j].option;
                    GameObject rightWrongImage = thisButton.transform.GetChild(2).gameObject;
                    rightWrongImage.SetActive(false);
                    thisButton.onClick.AddListener(delegate{CheckRightAnswerForPassageQuestion(thisButton, 0, 0);});
                    passageButtons[j] = thisButton;

                }
                else
                {
                    thisButton.gameObject.SetActive(false);
                }
            }

    }

    public void ConversationWithMCQSetup(int parameter)
    {
        baseParentBoard.gameObject.SetActive(false);
        conversationWithMCQBoard.gameObject.SetActive(true);
        int convoMCQcount = dataCountDetails.conversation_mcq_count;

        if (allDetailData.conversationQuestions.Length > 1)
        {
            convoWithMCQPrefabsArray.Add(convoContentPrefab);

    
            for (var i = 0; i < allDetailData.conversationQuestions.Length; i++)
            {
                if (i == 0)
                {
                     SetUpSingleConvoWithMCQ();
                     GameObject convoBoard = convoContentPrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = convoBoard.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                    int dialogueNumber = i + 1;
                    mytext.text = "Conversation - Dialogue " + dialogueNumber;
                }
                else
                {
                    
                    Vector2 prefabPosition = convoWithMCQPrefabsArray[i - 1].transform.position;
                    GameObject newPrefab = Instantiate(convoContentPrefab).gameObject;
                    newPrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 1666f);
                    newPrefab.transform.SetParent(convoPassageMCQPrefabParent, true);

                    GameObject convoBoard = newPrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = convoBoard.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                    int dialogueNumber = i + 1;
                    mytext.text = "Conversation - Dialogue " + dialogueNumber;

                    GameObject scrollBar = convoBoard.transform.GetChild(2).gameObject;
                    GameObject container = scrollBar.transform.GetChild(0).gameObject;
                    
                    TMPro.TMP_Text descriptionText = container.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                    descriptionText.text = allDetailData.conversationQuestions[i].conversation;


                    GameObject mcqBoard = newPrefab.transform.GetChild(1).gameObject;
                    GameObject questionBoard = mcqBoard.transform.GetChild(0).gameObject;

                    TMPro.TMP_Text question = questionBoard.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                    question.text = allDetailData.conversationQuestions[i].title;

                    int answerOptions = allDetailData.conversationQuestions[i].questionOptions.Length;
                    Debug.Log(answerOptions);

                    GameObject optionBoard = mcqBoard.transform.GetChild(1).gameObject;
                    GameObject optionContainer = optionBoard.transform.GetChild(0).gameObject;

                    int children = optionContainer.transform.childCount;
                    Button[] otherQbuttons = new Button[answerOptions];

                    questionNumber = i;

                    for(int j = 0; j < children; j++ )
                    {
                        Button thisButton = optionContainer.transform.GetChild(j).GetComponent<Button>();
                        if (j <= answerOptions-1)
                        {
                            TMPro.TMP_Text answerOption = thisButton.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                            answerOption.text = allDetailData.conversationQuestions[i].questionOptions[j].option;
                            GameObject rightWrongImage = thisButton.transform.GetChild(2).gameObject;
                            rightWrongImage.SetActive(false);
                            thisButton.onClick.AddListener(delegate{CheckRightAnswerOfQuestion(thisButton, questionNumber);});
                            otherQbuttons[j] = thisButton;

                        }
                        else
                        {
                            thisButton.gameObject.SetActive(false);
                        }
                    }
                    convoContentPrefab.transform.SetAsFirstSibling();

                    convoWithMCQPrefabsArray.Add(newPrefab);
                    
                }  

            }
        }
        else
        {
            
            SetUpSingleConvoWithMCQ();

        } 
        
         if (isSettingCanvas == true)
        {
            // if (parameterCountControlCheck == convoMCQcount - 1)
            // {
            //     Debug.Log("convo mcq is complete here");
                
            //     parameterCountControlCheck = 0;     //resetting 
            // }
            // else 
            // {
            //     Debug.Log("Working on calling another convo mcq");
            //     parameterCountControlCheck = parameterCountControlCheck + 1;
            // }
            dataDisplayed["isConversationMCQDone"] = true;
             isSettingCanvas = false;
        }

        
    }

    


    public void SetUpSingleConvoWithMCQ()
    {
        GameObject convoBoard = convoContentPrefab.transform.GetChild(0).gameObject;
            TMPro.TMP_Text mytext = convoBoard.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            mytext.text = "Conversation";

            GameObject scrollBar = convoBoard.transform.GetChild(2).gameObject;
            GameObject container = scrollBar.transform.GetChild(0).gameObject;
            
            TMPro.TMP_Text descriptionText = container.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
            descriptionText.text = allDetailData.conversationQuestions[0].conversation;

            GameObject mcqBoard = convoContentPrefab.transform.GetChild(1).gameObject;
            GameObject questionBoard = mcqBoard.transform.GetChild(0).gameObject;

            TMPro.TMP_Text question = questionBoard.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
            question.text = allDetailData.conversationQuestions[0].title;

            int answerOptions = allDetailData.conversationQuestions[0].questionOptions.Length;
            Debug.Log(answerOptions);

            GameObject optionBoard = mcqBoard.transform.GetChild(1).gameObject;
            GameObject optionContainer = optionBoard.transform.GetChild(0).gameObject;

            int children = optionContainer.transform.childCount;
            Button[] firstQbuttons = new Button[answerOptions];

            questionNumber = 0;

            for(int j = 0; j < children; j++ )
            {
                Button thisButton = optionContainer.transform.GetChild(j).GetComponent<Button>();
                if (j <= answerOptions-1)
                {
                    TMPro.TMP_Text answerOption = thisButton.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                    answerOption.text = allDetailData.conversationQuestions[0].questionOptions[j].option;
                    GameObject rightWrongImage = thisButton.transform.GetChild(2).gameObject;
                    rightWrongImage.SetActive(false);
                    thisButton.onClick.AddListener(delegate{CheckRightAnswerOfQuestion(thisButton, 0);});
                    firstQbuttons[j] = thisButton;

                }
                else
                {
                    thisButton.gameObject.SetActive(false);
                }
            }

    }

    public void CheckRightAnswerForPassageQuestion(Button button,int passageNumber, int questionNumber)
    {
        // int questionNumber - check the answer value 1 for particular question
        // allDetailData.conversationQuestions[0].questionOptions[j].option - j is option number
        int tag = System.Convert.ToInt32(button.tag);
        Debug.Log(button.tag + "Button tag");
        Debug.Log(passageNumber + "passageNumber");
        Debug.Log(questionNumber + "questionNumber");
        int value = allDetailData.passages[passageNumber].questions[questionNumber].questionOptions[tag].value;
        
        Debug.Log("value of" + value);
        GameObject rightWrongImage = button.transform.GetChild(2).gameObject;
        Image myImage = rightWrongImage.GetComponent<Image>();

        if (value == 1)
        {
            myImage.sprite = tickSprite;
            SoundManagerScript.RightAnswerSound();
        }
        else
        {
            myImage.sprite = wrongSprite;
        }
       
        rightWrongImage.SetActive(true);

    }

    public void CheckRightAnswerOfQuestion(Button button, int questionNumber)
    {
        // int questionNumber - check the answer value 1 for particular question
        // allDetailData.conversationQuestions[0].questionOptions[j].option - j is option number
        int tag = System.Convert.ToInt32(button.tag);
        int value = allDetailData.conversationQuestions[questionNumber].questionOptions[tag].value;
        Debug.Log(button.tag);
        Debug.Log("value of" + value);
        GameObject rightWrongImage = button.transform.GetChild(2).gameObject;
        Image myImage = rightWrongImage.GetComponent<Image>();

        if (value == 1)
        {
            myImage.sprite = tickSprite;
            SoundManagerScript.RightAnswerSound();
        }
        else
        {
            myImage.sprite = wrongSprite;
        }
       
        rightWrongImage.SetActive(true);

    }

    public void RevisionWordList(int parameter)
    {
        baseParentBoard.gameObject.SetActive(false);
        revisionWordBoard.gameObject.SetActive(true);

        if (dataCountDetails.interactive_line_revision_word != "")
        {
            GameObject childObj = revisionWordBoard.transform.GetChild(0).gameObject;
                TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                mytext.text = allDetailData.interactive_line_revision_word;
        }
       

         for(int i = 0; i < allDetailData.revisionWords.Length; i++)
            {
                string revisionWord = allDetailData.revisionWords[i].name;
                Debug.Log(revisionWord);
                listOfrevisionWords = listOfrevisionWords + revisionWord + "\n";
                Debug.Log(listOfrevisionWords);
            }
        revisionWordList.text = listOfrevisionWords;
        dataDisplayed["isRevisionWordListDone"] = true;
    }
    

    public void NounSetup(int parameter)
    {
        typeOfWord.text = "Noun";
        Noun newNoun = new Noun();
        int nounCount = dataCountDetails.new_word_data.more_data[0].noun_count;

            
        newNoun = newWordDetails.nouns[parameter];
        meaningAsNoun.text = newNoun.description;

        Debug.Log("Parameter count check " + parameterCountControlCheck);
 
        if (newNoun.nounSentences.Length > 1)
        {
            sentencePrefabsArray.Add(sentencePrefab);

            singleSentenceBoard.gameObject.SetActive(false);
            multipleSentenceBoard.gameObject.SetActive(true);
            Debug.Log("Length " + newNoun.nounSentences.Length);
                    for (var i = 0; i < newNoun.nounSentences.Length; i++)
                    {
                        Debug.Log("Running in For loop " + i);
                        Debug.Log(newNoun.nounSentences[0].description);
                         
                        if (i == 0)
                        {
                            Debug.Log("i is 0 here");
                            GameObject childObj = sentencePrefab.transform.GetChild(0).gameObject;
                            TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                            mytext.text = newNoun.nounSentences[0].description;                     
                        }
                        else
                        {
                             
                              
                            Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                            GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                            newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);
                            newSentencePrefab.transform.SetParent(parent, true);

                            GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                            TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                            mytext.text = newNoun.nounSentences[i].description;
                            sentencePrefabsArray.Add(newSentencePrefab);
                            Debug.Log("End of Checking in loop for second object");
                        }  

                    }
                }
                else
                {
                    singleSentenceBoard.gameObject.SetActive(true);
                    multipleSentenceBoard.gameObject.SetActive(false);
                    var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newNoun.nounSentences[0].description;
                }
            
            if (isSettingCanvas == true)
            {
                if (parameterCountControlCheck == nounCount - 1)
            {
                Debug.Log("noun is complete here");
                dataDisplayed["isNounDone"] = true;
                parameterCountControlCheck = 0;     //resetting 
            }
            else //if (dataCountDetails.new_word_data.more_data[0].noun_count > 1)
            {
                Debug.Log("Working on calling noun again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
            Debug.Log(dataDisplayed["isNounDone"] + "Noun is done");
            isSettingCanvas = false;
            }
            
    }

    public void VerbSetup(int parameter)
    {
        DestroyPrefabs();
        typeOfWord.text = "Verb";
        Verb newVerb = new Verb();
        int verbCount = dataCountDetails.new_word_data.more_data[0].verb_count;

        Debug.Log("Length of verb" + newWordDetails.verbs.Length);
                newVerb = newWordDetails.verbs[parameter];
                meaningAsNoun.text = newVerb.description;

                if (newVerb.verbSentences.Length > 1)
                {
                    sentencePrefabsArray.Add(sentencePrefab);

                    singleSentenceBoard.gameObject.SetActive(false);
                    multipleSentenceBoard.gameObject.SetActive(true);
                    Debug.Log("Length " + newVerb.verbSentences.Length);
                    for (var i = 0; i < newVerb.verbSentences.Length; i++)
                    {
                        Debug.Log("Running in For loop " + i);
                        Debug.Log(newVerb.verbSentences[0].description);
                         
                        if (i == 0)
                        {
                            Debug.Log("i is 0 here");
                            GameObject childObj = sentencePrefab.transform.GetChild(0).gameObject;
                            TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                            mytext.text = newVerb.verbSentences[0].description;                     
                        }
                        else
                        {
                             
                              
                            Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                            GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                            newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);
                            newSentencePrefab.transform.SetParent(parent, true);
                            GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                            TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                            mytext.text = newVerb.verbSentences[i].description;
                            sentencePrefabsArray.Add(newSentencePrefab);
                            Debug.Log("End of Checking in loop for second object");
                        }  

                    }
                }
                else
                {
                    singleSentenceBoard.gameObject.SetActive(true);
                    multipleSentenceBoard.gameObject.SetActive(false);
                    var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newVerb.verbSentences[0].description;
                }


            if (isSettingCanvas == true)
            {
            if (parameterCountControlCheck == verbCount - 1)
            {
                Debug.Log("verb is complete here");
                dataDisplayed["isVerbDone"] = true;
                parameterCountControlCheck = 0;     //resetting 
                // Array.Clear(sentencePrefabsArray, 0, sentencePrefabsArray.count);
            }
            else 
            {
                Debug.Log("Working on calling verb again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
            Debug.Log(dataDisplayed["isVerbDone"] + "Verb is done");
            isSettingCanvas = false;
            }
    }
    
    public void AdverbSetup(int parameter)
    {
        DestroyPrefabs();
        typeOfWord.text = "Adverb";
        Adverb newAdverb = new Adverb();
        int adverbCount = dataCountDetails.new_word_data.more_data[0].adverb_count;
        newAdverb = newWordDetails.adverbs[parameter];
        meaningAsNoun.text = newAdverb.description;

        if (newAdverb.adverbSentences.Length > 1)
        {
            Debug.Log(sentencePrefabsArray.Count);
            sentencePrefabsArray.Add(sentencePrefab);
            singleSentenceBoard.gameObject.SetActive(false);
            multipleSentenceBoard.gameObject.SetActive(true);
            Debug.Log("Length " + newAdverb.adverbSentences.Length);
            for (var i = 0; i < newAdverb.adverbSentences.Length; i++)
            {
                Debug.Log("Running in For loop " + i);
                Debug.Log(newAdverb.adverbSentences.Length + " newAdverb.adverbSentences");
                    
                if (i == 0)
                {
                    Debug.Log("i is 0 here");
                    GameObject childObj = sentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newAdverb.adverbSentences[0].description;  
                
                }
                else
                {
                    Debug.Log(sentencePrefabsArray.Count + "sentencePrefabsArray");
                    Debug.Log(i + " i value");
                    Vector2 prefabPosition = sentencePrefabsArray[i-1].transform.position;
                    GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                    newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);
                    newSentencePrefab.transform.SetParent(parent, true);
                    GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newAdverb.adverbSentences[i].description;
                    sentencePrefabsArray.Add(newSentencePrefab);
                    Debug.Log("End of Checking in loop for second object");
                }  

            }
            Debug.Log(sentencePrefabsArray.Count);
            
        }
        else
        {
            singleSentenceBoard.gameObject.SetActive(true);
            multipleSentenceBoard.gameObject.SetActive(false);
            var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
            mytext.text = newAdverb.adverbSentences[0].description;
        }

            if (isSettingCanvas == true)
            {
                 if (parameterCountControlCheck == adverbCount - 1)
            {
                Debug.Log("Adverb is complete here");
                dataDisplayed["isAdverbDone"] = true;
                parameterCountControlCheck = 0;     //resetting 
            }
            else 
            {
                Debug.Log("Working on calling Adverb again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
            Debug.Log(dataDisplayed["isAdverbDone"] + "Adverb is done");
                isSettingCanvas = false;
            }
           

    }

    public void AdjectiveSetup(int parameter)
    {
        DestroyPrefabs();
        typeOfWord.text = "Adjective";
        Adjective newAdjective = new Adjective();
        int adjectiveCount = dataCountDetails.new_word_data.more_data[0].adjective_count;

            
        newAdjective = newWordDetails.adjectives[parameter];
        meaningAsNoun.text = newAdjective.description;


        if (newAdjective.adjectiveSentences.Length > 1)
        {
            sentencePrefabsArray.Add(sentencePrefab);

            singleSentenceBoard.gameObject.SetActive(false);
            multipleSentenceBoard.gameObject.SetActive(true);
            Debug.Log("Length " + newAdjective.adjectiveSentences.Length);
                    for (var i = 0; i < newAdjective.adjectiveSentences.Length; i++)
                    {
                        Debug.Log("Running in For loop " + i);
                        Debug.Log(newAdjective.adjectiveSentences[0].description);
                         
                        if (i == 0)
                        {
                            Debug.Log("i is 0 here");
                            GameObject childObj = sentencePrefab.transform.GetChild(0).gameObject;
                            TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                            mytext.text = newAdjective.adjectiveSentences[0].description;                     
                        }
                        else
                        {
                             
                              
                            Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                            GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                            newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);
                            newSentencePrefab.transform.SetParent(parent, true);
                            GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                            TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                            mytext.text = newAdjective.adjectiveSentences[i].description;
                            sentencePrefabsArray.Add(newSentencePrefab);
                            Debug.Log("End of Checking in loop for second object");
                        }  

                    }
                }
                else
                {
                    singleSentenceBoard.gameObject.SetActive(true);
                    multipleSentenceBoard.gameObject.SetActive(false);
                    var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newAdjective.adjectiveSentences[0].description;
                }
            if (isSettingCanvas == true)
            {
                if (parameterCountControlCheck == adjectiveCount - 1)
                {
                    Debug.Log("adjective is complete here");
                    dataDisplayed["isAdjectiveDone"] = true;
                    parameterCountControlCheck = 0;     //resetting 
                }
                else 
                {
                    Debug.Log("Working on calling adjective again");
                    parameterCountControlCheck = parameterCountControlCheck + 1;
                }
                Debug.Log(dataDisplayed["isAdjectiveDone"] + "adjective is done");
                isSettingCanvas = false;
            }
            

    }

    public void ConversationSetup(int parameter)
    {
        DestroyPrefabs();
        baseParentBoard.gameObject.SetActive(false);
        conversationBoard.gameObject.SetActive(true);

        Conversation convo = new Conversation();
        convo = allDetailData.conversation;
        conversationText.text = convo.description;

        if (dataDisplayed["isRevisionWordListDone"] == true)
        {
            dataDisplayed["isRevisionWordConversationDone"] = true;
        }   
        else
        {
            dataDisplayed["isNewWordConverstaionDone"] = true;
        }
           
    }




    public void DailyTipsSetup(int parameter)
    {

        DestroyPrefabs();
        baseParentBoard.gameObject.SetActive(true);
        conversationBoard.gameObject.SetActive(false);
        generalBaseBoard.gameObject.SetActive(false);
        dutBoard.gameObject.SetActive(true);
        singleSentenceBoard.gameObject.SetActive(false);
        multipleSentenceBoard.gameObject.SetActive(false);
        
        if (newWordDetails.dailyUseTips.Length > 1)
        {
        
        sentencePrefabsArray.Add(sentencePrefab);

            for (var i = 0; i < newWordDetails.dailyUseTips.Length; i++)
            {
                // newDUT = newWordDetails.dailyUseTips[i];
                Debug.Log("Running in For loop " + i);
                    
                if (i == 0)
                {
                    Debug.Log("i is 0 here");
                    GameObject childObj = dutSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newWordDetails.dailyUseTips[0].description;                     
                }
                else
                {
                     
                      
                    Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                    GameObject newSentencePrefab = Instantiate(dutSentencePrefab).gameObject;
                    newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);
                    newSentencePrefab.transform.SetParent(dutParent, true);
                    GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newWordDetails.dailyUseTips[i].description;
                    sentencePrefabsArray.Add(newSentencePrefab);
                    Debug.Log("End of Checking in loop for second object");
                }  

            }
        }
         else
        {
            GameObject childObj = dutSentencePrefab.transform.GetChild(0).gameObject;
            TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
            mytext.text = newWordDetails.dailyUseTips[0].description;
        }

        dataDisplayed["isDUTDone"] = true;
    }

    public void AnotherWayOfUsingWordSetup(int parameter)
    {
        DestroyPrefabs();
        HideSpeakerAndImage();
        baseParentBoard.gameObject.SetActive(true);
        conversationBoard.gameObject.SetActive(false);
        generalBaseBoard.gameObject.SetActive(true);
        dutBoard.gameObject.SetActive(false);
        singleSentenceBoard.gameObject.SetActive(true);
        multipleSentenceBoard.gameObject.SetActive(false);
        
        int owuwCount = 0;
        OtherWayUsingWord newOwuw = new OtherWayUsingWord();

        if (dataDisplayed["isRevisionWordListDone"] == true)
        {
            typeOfDay.text = "Another way of using";
            typeOfWord.text = "Brief/Note";
             owuwCount = dataCountDetails.revision_word_data.more_data[0].other_way_using_count;
             newOwuw = revisionWordDetails.otherWayUsingWords[parameter];
             meaningAsNoun.text = newOwuw.description;
        }
        else
        {
            typeOfDay.text = "Another way of using";
            typeOfWord.text = "Brief/Note";
             owuwCount = dataCountDetails.new_word_data.more_data[0].other_way_using_count;
             newOwuw = newWordDetails.otherWayUsingWords[parameter];
             meaningAsNoun.text = newOwuw.description;
        }


        if (newOwuw.otherWayUsingWordSentences.Length > 1)
        {
            sentencePrefabsArray.Add(sentencePrefab);

            singleSentenceBoard.gameObject.SetActive(false);
            multipleSentenceBoard.gameObject.SetActive(true);
            for (var i = 0; i < newOwuw.otherWayUsingWordSentences.Length; i++)
            {
                    
                if (i == 0)
                {
                    Debug.Log("i is 0 here");
                    GameObject childObj = sentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newOwuw.otherWayUsingWordSentences[0].description;                     
                }
                else
                {
                     
                      
                    Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                    GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                    newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);
                    newSentencePrefab.transform.SetParent(parent, true);
                    GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newOwuw.otherWayUsingWordSentences[i].description;
                    sentencePrefabsArray.Add(newSentencePrefab);
                    Debug.Log("End of Checking in loop for second object");
                }  

            }
        }
        else
        {
            singleSentenceBoard.gameObject.SetActive(true);
            multipleSentenceBoard.gameObject.SetActive(false);
            var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
            mytext.text = newOwuw.otherWayUsingWordSentences[0].description;
        }
        if (isSettingCanvas == true)
        {
            if (parameterCountControlCheck == owuwCount - 1)
            {
                Debug.Log("OWUW is complete here");
                    if (dataDisplayed["isRevisionWordListDone"] == true)
                {
                    dataDisplayed["isRevisionWordOWUWordDone"] = true;
                }   
                else
                {
                dataDisplayed["isnewWordOWUWordDone"] = true;
                }
                parameterCountControlCheck = 0;     //resetting 
            }
            else 
            {
                Debug.Log("Working on calling adjective again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
            isSettingCanvas = false;
        }
        
    }

    public void IdiomSetup(int parameter)
    {
        DestroyPrefabs();
        HideSpeakerAndImage();
        baseParentBoard.gameObject.SetActive(true);
        revisionWordBoard.gameObject.SetActive(false);
        generalBaseBoard.gameObject.SetActive(true);

        if (dataDisplayed["isRevisionWordUsingMultipleWordsDone"] == true || dataDisplayed["isnewWordOWUWordDone"] == true || dataDisplayed["isDUTDone"] == true)
        {
                //Change position of sentence boards
                // Vector3 singleSentenceBoardPos = singleSentenceBoard.transform.position;
                // singleSentenceBoardPos.y += 80f;
            singleSentenceBoard.transform.position = sentenceRealPos;

            // Vector3 multipleSentenceBoardPos = multipleSentenceBoard.transform.position;
            // multipleSentenceBoardPos.y += 80f;
                multipleSentenceBoard.transform.position = sentenceRealPos;
        }    

        typeOfDay.text = "Idiom";
        
        typeOfWord.text = "Meaning";
         Idiom newIdiom = new Idiom();
        int idiomCount = 0;

        if (dataDisplayed["isRevisionWordListDone"] == true)
        {
             idiomCount = dataCountDetails.revision_word_data.more_data[0].idiom_count;    
            newIdiom = revisionWordDetails.idioms[parameter];
            word.text = newIdiom.description;
            meaningAsNoun.text = newIdiom.meaning;  
        }
        else
        {
             idiomCount = dataCountDetails.new_word_data.more_data[0].idiom_count;    
            newIdiom = newWordDetails.idioms[parameter];
             word.text = newIdiom.description;
             meaningAsNoun.text = newIdiom.meaning;
        }
       


        if (newIdiom.idiomSentences.Length > 1)
        {
            sentencePrefabsArray.Add(sentencePrefab);

            singleSentenceBoard.gameObject.SetActive(false);
            multipleSentenceBoard.gameObject.SetActive(true);
            for (var i = 0; i < newIdiom.idiomSentences.Length; i++)
            {
                    
                if (i == 0)
                {
                    Debug.Log("i is 0 here");
                    GameObject childObj = sentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newIdiom.idiomSentences[0].description;                     
                }
                else
                {
                     
                      
                    Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                    GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                    newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);
                    newSentencePrefab.transform.SetParent(parent, true);
                    GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newIdiom.idiomSentences[i].description;
                    sentencePrefabsArray.Add(newSentencePrefab);
                    Debug.Log("End of Checking in loop for second object");
                }  

            }
        }
        else
        {
            singleSentenceBoard.gameObject.SetActive(true);
            multipleSentenceBoard.gameObject.SetActive(false);
            var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
            mytext.text = newIdiom.idiomSentences[0].description;
        }

        if (isSettingCanvas == true)
        {
            if (parameterCountControlCheck == idiomCount - 1)
            {
                Debug.Log("Idiom is complete here");
                 if (dataDisplayed["isRevisionWordListDone"] == true)
             {
                    dataDisplayed["isRevisionWordIdiomsDone"] = true;
             }   
             else
             {
                dataDisplayed["isnewWordIdiomDone"] = true;
             }
                
                parameterCountControlCheck = 0;     //resetting 
            }
            else 
            {
                Debug.Log("Working on calling idiom again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
            isSettingCanvas = false;
        }
    }

    public void MultipleWordSetup(int parameter)
    {
         DestroyPrefabs();
        HideSpeakerAndImage();
           

        baseParentBoard.gameObject.SetActive(true);
        revisionWordBoard.gameObject.SetActive(false);
        generalBaseBoard.gameObject.SetActive(false);


        //Change position of sentence boards
        Vector3 singleSentenceBoardPos = singleSentenceBoard.transform.position;
        singleSentenceBoardPos.y += 423f;
        singleSentenceBoard.transform.position = singleSentenceBoardPos;

        Vector3 multipleSentenceBoardPos = multipleSentenceBoard.transform.position;
        multipleSentenceBoardPos.y += 423f;
        multipleSentenceBoard.transform.position = multipleSentenceBoardPos;

        int multipleWordCount = 0;
        UseMultipleWord multipleWordDetails = new UseMultipleWord();
        
        if (dataDisplayed["isRevisionWordListDone"] == true)
        {
             typeOfDay.text = "Using multiple words in a sentence";
        
             
            multipleWordCount = dataCountDetails.revision_word_data.more_data[0].use_multiple_count;
             multipleWordDetails = revisionWordDetails.useMultipleWords[parameter];
            word.text = multipleWordDetails.description;
        }
        else
        {
             typeOfDay.text = "Using multiple words in a sentence";
        
            multipleWordCount = dataCountDetails.new_word_data.more_data[0].use_multiple_count;
             multipleWordDetails = newWordDetails.useMultipleWords[parameter];
             word.text = multipleWordDetails.description;
        }
       

        if (multipleWordDetails.useMultipleWordSentences.Length > 1)
        {
            sentencePrefabsArray.Add(sentencePrefab);

            singleSentenceBoard.gameObject.SetActive(false);
            multipleSentenceBoard.gameObject.SetActive(true);

            for (var i = 0; i < multipleWordDetails.useMultipleWordSentences.Length; i++)
            {
                    
                if (i == 0)
                {
                    Debug.Log("i is 0 here");
                    GameObject childObj = sentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = multipleWordDetails.useMultipleWordSentences[0].description;                     
                }
                else
                {
                     
                      
                    Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                    GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                    newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);
                    newSentencePrefab.transform.SetParent(parent, true);
                    GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = multipleWordDetails.useMultipleWordSentences[i].description;
                    sentencePrefabsArray.Add(newSentencePrefab);
                    Debug.Log("End of Checking in loop for second object");
                }  

            }
        }
        else
        {
            singleSentenceBoard.gameObject.SetActive(true);
            multipleSentenceBoard.gameObject.SetActive(false);
            var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
            mytext.text = multipleWordDetails.useMultipleWordSentences[0].description;
        }

        if (isSettingCanvas == true)  
        {
            if (parameterCountControlCheck == multipleWordCount - 1)
            {
                if (dataDisplayed["isRevisionWordListDone"] == true)
             {
                    dataDisplayed["isRevisionWordUsingMultipleWordsDone"] = true;
             }   
             else
             {
                dataDisplayed["isnewWordUsingMultipleWordsDone"] = true;
             }
                parameterCountControlCheck = 0;     //resetting 
            }
            else //if (dataCountDetails.new_word_data.more_data[0].noun_count > 1)
            {
                Debug.Log("Working on calling multipleWords again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
            isSettingCanvas = false;
        }
    
    }

    public void AntonymSetup(int parameter)
    {

        DestroyPrefabs();
         HideSpeakerAndImage();
        baseParentBoard.gameObject.SetActive(true);
        revisionWordBoard.gameObject.SetActive(false);
        generalBaseBoard.gameObject.SetActive(true);

        if (dataDisplayed["isnewWordUsingMultipleWordsDone"] == true && (dataDisplayed["isnewWordAntonymDone"] == false && dataDisplayed["isnewWordSynonymDone"] == false && dataDisplayed["isRevisionWordSynonymDone"] == false))
        {
            //Change position of sentence boards
            // Vector3 singleSentenceBoardPos = singleSentenceBoard.transform.position;
            // singleSentenceBoardPos.y -= 423f;
            singleSentenceBoard.transform.position = sentenceRealPos;

            // Vector3 multipleSentenceBoardPos = multipleSentenceBoard.transform.position;
            // multipleSentenceBoardPos.y -= 423f;
            multipleSentenceBoard.transform.position = sentenceRealPos;
        }

        Antonym antonymDetails = new Antonym();
        int antonymCount = 0;
        if (dataDisplayed["isRevisionWordListDone"] == true)
        {
            typeOfDay.text = "Antonym of " + revisionWordDetails.antonyms[revisionWordReference].description;
            typeOfWord.text = "Meaning";
            
            antonymCount = dataCountDetails.revision_word_data.more_data[revisionWordReference].antonym_count;
            antonymDetails = revisionWordDetails.antonyms[parameter];
            meaningAsNoun.text = antonymDetails.meaning;
        }
        else
        {
            typeOfDay.text = "Antonym of " + newWordDetails.antonyms[0].description;
            typeOfWord.text = "Meaning";
            
            antonymCount = dataCountDetails.new_word_data.more_data[0].antonym_count;
            antonymDetails = newWordDetails.antonyms[parameter];
            meaningAsNoun.text = antonymDetails.meaning;
        }



        if (antonymDetails.antonymSentences.Length > 1)
        {
            sentencePrefabsArray.Add(sentencePrefab);

            singleSentenceBoard.gameObject.SetActive(false);
            multipleSentenceBoard.gameObject.SetActive(true);
            for (var i = 0; i < antonymDetails.antonymSentences.Length; i++)
            {                         
                if (i == 0)
                {
                    Debug.Log("i is 0 here");
                    GameObject childObj = sentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = antonymDetails.antonymSentences[0].description;                     
                }
                else
                {
                     
                      
                    Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                    GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                    newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);
                    newSentencePrefab.transform.SetParent(parent, true);
                    GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = antonymDetails.antonymSentences[i].description;
                    sentencePrefabsArray.Add(newSentencePrefab);
                    Debug.Log("End of Checking in loop for second object");
                }  

            }
        }
        else if (antonymDetails.antonymSentences.Length == 1)
        {
            singleSentenceBoard.gameObject.SetActive(true);
            multipleSentenceBoard.gameObject.SetActive(false);
            var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
            mytext.text = antonymDetails.antonymSentences[0].description;
        }
        else if (antonymDetails.antonymSentences.Length == 0)
        {
            HideSentences();
        }

        if (isSettingCanvas == true)
        {
            if (parameterCountControlCheck == antonymCount - 1)
            {
                Debug.Log("antonym is complete here");
                if (dataDisplayed["isRevisionWordListDone"] == true)
                {
                    dataDisplayed["isRevisionWordAntonymDone"] = true;
                }
                else
                {
                    dataDisplayed["isnewWordAntonymDone"] = true;
                }
                parameterCountControlCheck = 0;     //resetting 
            }
            else 
            {
                Debug.Log("Working on calling antonym again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
            isSettingCanvas = false;
        }
        
    }

    
    public void SynonymSetup(int parameter)
    {

         DestroyPrefabs();
        HideSpeakerAndImage();
         baseParentBoard.gameObject.SetActive(true);
        revisionWordBoard.gameObject.SetActive(false);
        generalBaseBoard.gameObject.SetActive(true);

        if (dataDisplayed["isnewWordUsingMultipleWordsDone"] == true && (dataDisplayed["isnewWordAntonymDone"] == false && dataDisplayed["isnewWordSynonymDone"] == false ))
        {
            //Change position of sentence boards
            // Vector3 singleSentenceBoardPos = singleSentenceBoard.transform.position;
            // singleSentenceBoardPos.y -= 423f;
            singleSentenceBoard.transform.position = sentenceRealPos;

            // Vector3 multipleSentenceBoardPos = multipleSentenceBoard.transform.position;
            // multipleSentenceBoardPos.y -= 423f;
            multipleSentenceBoard.transform.position = sentenceRealPos;
        } 

        Synonym synonymDetails = new Synonym();
        int synonymCount = 0;
        if (dataDisplayed["isRevisionWordListDone"] == true)
        {
            typeOfDay.text = "Synonym of " + revisionWordDetails.synonyms[revisionWordReference].description;
            typeOfWord.text = "Meaning";
            
            synonymCount = dataCountDetails.revision_word_data.more_data[revisionWordReference].synonym_count;

            
            synonymDetails = revisionWordDetails.synonyms[parameter];
            meaningAsNoun.text = synonymDetails.meaning;
        }
        else
        {
            typeOfDay.text = "Synonym of " + newWordDetails.synonyms[0].description;
            typeOfWord.text = "Meaning";
            
            synonymCount = dataCountDetails.new_word_data.more_data[0].synonym_count;

            
            synonymDetails = newWordDetails.synonyms[parameter];
            meaningAsNoun.text = synonymDetails.meaning;
        }
    

        if (synonymDetails.synonymSentences.Length > 1)
        {
            sentencePrefabsArray.Add(sentencePrefab);

            singleSentenceBoard.gameObject.SetActive(false);
            multipleSentenceBoard.gameObject.SetActive(true);
            for (var i = 0; i < synonymDetails.synonymSentences.Length; i++)
            {                         
                if (i == 0)
                {
                    Debug.Log("i is 0 here");
                    GameObject childObj = sentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = synonymDetails.synonymSentences[0].description;                     
                }
                else
                {
                     
                      
                    Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                    GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                    newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);
                    newSentencePrefab.transform.SetParent(parent, true);
                    GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = synonymDetails.synonymSentences[i].description;
                    sentencePrefabsArray.Add(newSentencePrefab);
                    Debug.Log("End of Checking in loop for second object");
                }  

            }
        }
        else if (synonymDetails.synonymSentences.Length == 1)
        {
            singleSentenceBoard.gameObject.SetActive(true);
            multipleSentenceBoard.gameObject.SetActive(false);
            var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
            mytext.text = synonymDetails.synonymSentences[0].description;
        }
        else if (synonymDetails.synonymSentences.Length == 0)
        {
            HideSentences();
        }

        if (isSettingCanvas == true)
        {
            if (parameterCountControlCheck == synonymCount - 1)
            {
                Debug.Log("synonym is complete here");

                if (dataDisplayed["isRevisionWordListDone"] == true)
                {
                    dataDisplayed["isRevisionWordSynonymDone"] = true;
                }
                else
                {
                    dataDisplayed["isnewWordSynonymDone"] = true;

                }
                parameterCountControlCheck = 0;     //resetting 
            }
            else 
            {
                Debug.Log("Working on calling synonym again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
            isSettingCanvas = false;
        }
    }

    public void SetBottomTitleLabel()
    {
        GameObject bottomPanel = GameObject.Find("BottomPanel");
        GameObject childObj = bottomPanel.transform.GetChild(0).gameObject;
        TMPro.TMP_Text numberText = childObj.GetComponent<TMPro.TMP_Text>();
        numberText.text = screenCount + "/" + totalNumber;
    }

    public void NextButton(Button button)
    {
        //Check screen count   
        if (button.tag == "Next")
        {
            screenCount = screenCount + 1;
            
            Debug.Log(screenCount + " screen count" + methodCallArray.Count);

            if (screenCount > 0 && screenCount <= methodCallArray.Count)
            {
                
                Debug.Log(screenCount + " screen count" + methodCallArray.Count);
                
                int parameter = parameterValueArray[screenCount-1];
                Debug.Log(parameter + "parameter value for" + methodCallArray[screenCount-1]);
                Action<int> unityAction = methodCallArray[screenCount-1];
                SendInt(unityAction,parameter);
            
            }
            else
            {
                SetUpBaseCanvas();
            }
            
        }
        else
        {
            if (screenCount != 1)
            {
             
                screenCount = screenCount - 1;
                Debug.Log(screenCount + " screen count");
                int parameter = parameterValueArray[screenCount-1];
                Debug.Log(parameter + "parameter value for" + methodCallArray[screenCount-1]);
                Action<int> unityAction = methodCallArray[screenCount-1];
                SendInt(unityAction,parameter);
            }
            else
            {
                Debug.Log("we are on first screen");
            }
        }

        SetBottomTitleLabel();
    }

    public void SendInt(Action<int> action, int value)
    {
        action.Invoke(value);
    }

}
