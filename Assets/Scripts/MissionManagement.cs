using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;

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
    public GameObject dutSentencePrefab2;
     [SerializeField]
    public GameObject dutSentencePrefab3;


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

    [SerializeField]
    public Button submitButton;

    [SerializeField]
    public GameObject passageScrollBar;

    private Vector3 sentenceRealPos;
    
    string auth_key;
    int dayLevelId;
    int levelId;

    int nextNumber = 1;
    int parameterCount = 0; //set after checking values

    
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
        public RevisionConversation revisionConversation;
        public Question[] questions;
        public ConversationQuestion[] conversationQuestions;
        public Passage[] passages;
    }

    [Serializable]
    public class RevisionConversation
    {
        public int id;
        public int day_level_id;
        public string description;
        public int type;
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

    [Serializable]
    public class BadRequest
    {
        public int status;
        public string title;
        public string message;
    }

    Dictionary<string, bool> availableData = new Dictionary<string, bool>()
        {
            {"isNewWordAvailable", false},
            {"isRevisionWordsAvailable", false},
            {"Questions", false},
            {"Conversation", false},
            {"Passages", false}
        };

     public Dictionary<string, bool> dataDisplayed = new Dictionary<string, bool>()
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
        {"isGeneralMCQDone", false},
        {"isNewWordDetailsDone", false}
    };

    public Dictionary<string, bool> copyOfdataDisplayed = new Dictionary<string, bool>()
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
        {"isGeneralMCQDone", false},
        {"isNewWordDetailsDone", false}
    };

    Dictionary<string, bool> copyOfavailableData = new Dictionary<string, bool>()
     {
            {"isNewWordAvailable", false},
            {"isRevisionWordsAvailable", false},
            {"Questions", false},
            {"Conversation", false},
            {"Passages", false}
        };

    public List<GameObject> sentencePrefabsArray = new List<GameObject>();
    public List<GameObject> convoWithMCQPrefabsArray = new List<GameObject>();

    // public GameObject[] sentencePrefabsArray;

    public bool isSettingCanvas = false;

    public int parameterCountControlCheck = 0;
    public int screenCount = 1;
    string listOfrevisionWords = "";
    
    public int newWordNumber = 0; // check if all new word screens are done
    public int revisionWordReference = 0;
    public int numberOfRevisionWords = 0;
    public int rwdataCount = 0;
    public int tempDataCount = 0;
    public bool revisionListDisplayed = false;
    public int totalNumber = 0;    // total count of dictionary - this is not including provision for multiple screen of individual parameter
    public int newWordDataCount = 0;
    // public int revisionDataCount = 0;
    public int revisionList = 0;
    // public int convoPlusList = 0;


    public List<Action<int>> methodCallArray = new List<Action<int>>();
    public List<int> parameterValueArray = new List<int>();
    public List<int> revisionCountArray = new List<int>();
    Dictionary<int, Button[]> mcqButtonsArray = new Dictionary<int, Button[]>();

    int buttonArrayNumber = 0;
    public int optionNumber = 0;

    int questionNumber = 0;
    public int generalMCQcount = 0;
    int tempGeneralMCQCount = 0;
  
    [Serializable]
    public class QuestionResponse
    {
        public int question_id;
        public SelectedOption[] selected_options;
    }

    [Serializable]
    public class SelectedOption
    {
        public int option_id;
    }

    [Serializable]
    public class ResponseSubmission
    {
        public List<QuestionResponse> response;
    }

    public List<int> selectedOptionList = new List<int>();
    public Dictionary<int, List<int>> questionResponseDict = new Dictionary<int, List<int>>();

    [Serializable]
     public class ResponseResult
    {
        public int id;
        public int student_id;
        public int day_level_id;
        public int created_at;
        public int updated_at;
        public int age_group_id;
        public int passed_at;
        public int is_passed;
        public double score_percentage;
        public int total_question;
    }

    public TextMeshProUGUI missionTitle;
    public int noOfAttempts = 0;
    int answerClicked = 0;
    bool updateDUTSentencePosition = false;
    float calHeight = 0.0f;
    bool isQuestion = false;

    string fixJson(string value)            // Added object type to JSON
    {
        value = "{\"items\":" + value + "}";
        return value;
    }

    string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";
  
    // string baseURL = "http://165.22.219.198/edugogy/api/v1/";


    // Start is called before the first frame update
    void Awake()
    {
         // start mission after fetching id from detail all api
        sentenceRealPos = new Vector2(singleSentenceBoard.transform.position.x,singleSentenceBoard.transform.position.y - 50.0f);
        singleSentenceBoard.gameObject.SetActive(true);
        multipleSentenceBoard.gameObject.SetActive(false);
        dutBoard.gameObject.SetActive(false);
        baseParentBoard.gameObject.SetActive(false);

         if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
        }
        // auth_key = "Bearer shBuqKWlYHGCss7Il4B0-L_3QpRO5L3Z";  //mine

        // auth_key = "Bearer usFEr6V4JK0P4OUz_eoZVvYMrzIRxATo";  // Ridhima - Mehak Key
        auth_key = "Bearer DTYp7oipE2vzpRvlNv-hJ4mRuR1skyrg"; // Ridhi di's - Komal
        // auth_key = "Bearer tUOc6R-eobQl-a6DbrW8NYiqUI-D6Gvr"; // Ridhima testing
    }

    async void Start ()
    {
        if (PlayerPrefs.HasKey("StartLevelID"))
        {
            dayLevelId = PlayerPrefs.GetInt("StartLevelID");
            Debug.Log("dayLevelId " + dayLevelId);
            levelId = PlayerPrefs.GetInt("LevelId");
            missionTitle.text = "Mission " + levelId.ToString();
            StartMission();
        }
       
    }


    // void Update()
    // {
    //     if (updateDUTSentencePosition == true)
    //     {
    //         dutSentencePrefab2.transform.position = new Vector2(dutSentencePrefab.transform.position.x, calHeight);
    //         updateDUTSentencePosition = false;

    //     }
    // }

    IEnumerator DownloadImage(string mediaUrl)
    {   
        //  var mediaUrl = "http://165.22.219.198/edugogy/frontend/web/uploads/word/thumb-Screen_Shot_2022-04-28_at_10.44.30_AM-4.png";

        if (mediaUrl != "")
        {
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
        // else
        // {
        //     wordImage.gameObject.SetActive(false);
        // }
        
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

        string uri = baseURL + "student-levels/start-level";
        Debug.Log(uri);

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", auth_key);


        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: in start mission" + request.error);
            Debug.Log(request.downloadHandler.text);
            string jsonString = request.downloadHandler.text;
           BadRequest badRequestDetails = new BadRequest();

            badRequestDetails = JsonUtility.FromJson<BadRequest>(jsonString);

            Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    badRequestDetails.message,
                    "Cancel",
                    "Home",
                    GoBackToDashboard
                    );
        }
        else
        {
            Debug.Log("Start Mission " + request.result);
            Debug.Log(request.downloadHandler.text);
            //  GetDataCount(); temporary 
        }
        request.Dispose();
         GetDataCount(); // need to check & fix content

    }

    void resetAction()
    {
        Debug.Log("can't start mission");
    }

    IEnumerator GetDataCount_Coroutine()
    {
        string uri = baseURL + "day-levels/data-count/" + levelId.ToString();

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
           
            dataCountDetails = JsonUtility.FromJson<DataCount>(jsonString);
            // 
            
            calculateTotalCount();

        }
        request.Dispose();

    }

    public void calculateTotalCount()
    {
        int revisionDataCount = 0;
        generalMCQcount = dataCountDetails.mcq_count; // 2
        Debug.Log("generalMCQcount is " + generalMCQcount);
       
        if  (dataCountDetails.new_word_data.new_word_count != 0)
        {
            newWordDataCount  = dataCountDetails.new_word_data.more_data[0].noun_count +
            dataCountDetails.new_word_data.more_data[0].verb_count + dataCountDetails.new_word_data.more_data[0].adverb_count +
            dataCountDetails.new_word_data.more_data[0].adjective_count +
            dataCountDetails.new_word_data.more_data[0].other_way_using_count
            + dataCountDetails.new_word_data.more_data[0].idiom_count
            + dataCountDetails.new_word_data.more_data[0].use_multiple_count
            + dataCountDetails.new_word_data.more_data[0].synonym_count
            + dataCountDetails.new_word_data.more_data[0].antonym_count
            + dataCountDetails.conversation_new_word_count;

            if (dataCountDetails.new_word_data.more_data[0].daily_use_tip_count != 0)
            {
                newWordDataCount = newWordDataCount + 1;
            }
        }
        
        Debug.Log("newWordDataCount is " + newWordDataCount);
        totalNumber = generalMCQcount + newWordDataCount; // 14

        Debug.Log("total number is" + totalNumber);
        if  (dataCountDetails.revision_word_data.revison_word_count != 0)
        {
            
            // Debug.Log("total number is" + totalNumber);
            revisionList = 1; // for list
            numberOfRevisionWords = dataCountDetails.revision_word_data.revison_word_count;
            for(int i = 0; i < dataCountDetails.revision_word_data.more_data.Length; i++)
            {
                revisionDataCount = dataCountDetails.revision_word_data.more_data[i].other_way_using_count + 
                     dataCountDetails.revision_word_data.more_data[i].idiom_count
                + dataCountDetails.revision_word_data.more_data[i].use_multiple_count
                + dataCountDetails.revision_word_data.more_data[i].synonym_count
                + dataCountDetails.revision_word_data.more_data[i].antonym_count;
                //5
                Debug.Log("revisionDataCount for i is " + i + " " + revisionDataCount);
                revisionCountArray.Add(revisionDataCount);

                revisionList += revisionDataCount;
            }
                      //for revision word list = 15 
        }


        Debug.Log("revisionList " + revisionList);

        if (dataCountDetails.conversation_mcq_count != 0)   // as mcqs related to 1 conversation will be displayed on one screen so it will be counted as 1 only, also as per client requirement - there is one converstaion with mcq only
        {
            totalNumber += 1;
        }
        
        //no. of revision words i.e. dataCountDetails.revision_word_data.revison_word_count
        //total count for each revision word
        // for back button  - working on calling method with parameter
       //15 + 5 + 2
       
        // int convoPlusList = dataCountDetails.conversation_revision_word_count + revisionList;
        // Debug.Log("convoPlusList " + convoPlusList);

        revisionList = dataCountDetails.conversation_revision_word_count + revisionList;

        totalNumber = totalNumber + dataCountDetails.passage_data.passage_count + revisionList;

        
        // 22 + 1
       Debug.Log("total number is" + totalNumber);
        GetAllDetails();

    }

    public void HideSpeakerAndImage()
    {
        speakerButton.gameObject.SetActive(false);
        wordImage.gameObject.SetActive(false);
    }

     public void DisplaySpeakerandImage()
    {
        speakerButton.gameObject.SetActive(true);
        Debug.Log(allDetailData.newWords[0].image_url);
                if (allDetailData.newWords[0].image_url != null)
                {
                    string imageURL = allDetailData.newWords[0].image_url;
                    wordImage.gameObject.SetActive(true);
                    StartCoroutine(DownloadImage(imageURL));
                }
                else if (allDetailData.newWords[0].image_url == null)
                {
                    Debug.Log("url is null");
                    // wordImage.gameObject.SetActive(false);
                    
                }
        // wordImage.gameObject.SetActive(true);
    }

    public void HideSentences()
    {
     
        singleSentenceBoard.gameObject.SetActive(false);
        multipleSentenceBoard.gameObject.SetActive(false);

    }

    public void DestroyPrefabs()
    {
       int prefabCount = parent.transform.childCount;
    //    Debug.Log("prefabCount is " + prefabCount);
       if (prefabCount > 1)
       {
            for (int i = 1; i < prefabCount; i++)
            {
                    Debug.Log("value of i " + i);
                    Destroy(parent.transform.GetChild(i).gameObject);
            }
       }

    //    int dutPrefabCount = dutParent.transform.childCount;
    //    if (dutPrefabCount > 1)
    //    {
    //         for (int i = 1; i < dutPrefabCount; i++)
    //         {
    //                 Debug.Log("value of i " + i);
    //                 Destroy(dutParent.transform.GetChild(i).gameObject);
    //         }
    //    }
       
       sentencePrefabsArray.Clear();
    }

    public void DestroyConvoPrefabs()
    {
        GameObject parentObject = convoPassageMCQPrefabParent.transform.gameObject;
        Debug.Log(parentObject.name);
        
        int parentPrefabCount = parentObject.transform.childCount;
        if (parentPrefabCount == 1)
        {
            GameObject contentObject = parentObject.transform.GetChild(0).gameObject;
            Debug.Log(contentObject.name);
            int prefabCount = contentObject.transform.childCount;
            Debug.Log("prefabCount is " + prefabCount);
            if (prefabCount > 1)
            {
                for (int i = 2; i < prefabCount; i++)
                {
                        Debug.Log("value of i " + i);
                        Destroy(contentObject.transform.GetChild(i).gameObject);
                }
            }
        }
        else if (parentPrefabCount > 1)
        {
            for (int i = 1; i < parentPrefabCount; i++)
                {
                        Debug.Log("value of i " + i);
                        Destroy(parentObject.transform.GetChild(i).gameObject);
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
        string uri = baseURL + "day-levels/" + levelId.ToString() + "?expand=newWords,revisionWords,newWords.nouns,newWords.nouns.nounSentences,newWords.verbs,newWords.verbs.verbSentences,newWords.adverbs,newWords.adverbs.adverbSentences,newWords.adjectives,newWords.adjectives.adjectiveSentences,newWords.dailyUseTips,newWords.otherWayUsingWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords.otherWayUsingWordSentences,newWords.idioms,newWords.idioms.idiomSentences,newWords.useMultipleWords,newWords.useMultipleWords.useMultipleWordSentences,newWords.synonyms,newWords.synonyms.synonymSentences,newWords.antonyms,newWords.antonyms.antonymSentences,revisionWords.nouns,revisionWords.nouns.nounSentences,revisionWords.verbs,revisionWords.verbs.verbSentences,revisionWords.adverbs,revisionWords.adverbs.adverbSentences,revisionWords.adjectives,revisionWords.adjectives.adjectiveSentences,revisionWords.dailyUseTips,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords.otherWayUsingWordSentences,revisionWords.idioms,revisionWords.idioms.idiomSentences,revisionWords.useMultipleWords,revisionWords.useMultipleWords.useMultipleWordSentences,revisionWords.synonyms,revisionWords.synonyms.synonymSentences,revisionWords.antonyms,revisionWords.antonyms.antonymSentences,questions,questions.questionOptions,conversation,conversationQuestions,conversationQuestions.questionOptions,passages,passages.questions,passages.questions.questionOptions,revisionConversation";
        // "newWords,revisionWords,newWords.nouns,newWords.nouns.nounSentences,newWords.verbs,newWords.verbs.verbSentences,newWords.adverbs,newWords.adverbs.adverbSentences,newWords.adjectives,newWords.adjectives.adjectiveSentences,newWords.dailyUseTips,newWords.otherWayUsingWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords.otherWayUsingWordSentences,newWords.idioms,newWords.idioms.idiomSentences,newWords.useMultipleWords,newWords.useMultipleWords.useMultipleWordSentences,newWords.synonyms,newWords.synonyms.synonymSentences,newWords.antonyms,newWords.antonyms.antonymSentences,revisionWords.nouns,revisionWords.nouns.nounSentences,revisionWords.verbs,revisionWords.verbs.verbSentences,revisionWords.adverbs,revisionWords.adverbs.adverbSentences,revisionWords.adjectives,revisionWords.adjectives.adjectiveSentences,revisionWords.dailyUseTips,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords.otherWayUsingWordSentences,revisionWords.idioms,revisionWords.idioms.idiomSentences,revisionWords.useMultipleWords,revisionWords.useMultipleWords.useMultipleWordSentences,revisionWords.synonyms,revisionWords.synonyms.synonymSentences,revisionWords.antonyms,revisionWords.antonyms.antonymSentences,questions,questions.questionOptions,conversation,conversationQuestions,conversationQuestions.questionOptions,passages,passages.questions,passages.questions.questionOptions,revisionConversation";

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
            DecideTypeOfDataAvailable();
            

        }
        // request.Dispose();
    }

    public void DecideTypeOfDataAvailable()
    {
        if (allDetailData.newWords.Length > 0)
        {
            availableData["isNewWordAvailable"] = true;
            //new word length will always be 1
        }
        if (allDetailData.revisionWords.Length > 0)
        {
            availableData["isRevisionWordsAvailable"] = true;
        }

        if  (dataCountDetails.new_word_data.new_word_count != 0)
        {
            Debug.Log(allDetailData.newWords[0].image_url);
            if (allDetailData.newWords[0].image_url != null)
            {
                string imageURL = allDetailData.newWords[0].image_url;
                
                StartCoroutine(DownloadImage(imageURL));
            }
            else if (allDetailData.newWords[0].image_url == null)
            {
                Debug.Log("url is null");
                wordImage.gameObject.SetActive(false);
                
            }
        }

        SetBottomTitleLabel();
        SetUpBaseCanvas();
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
            isQuestion = false;
            Debug.Log("new word details");
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
               Debug.Log("dut board covered");
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
            
            if (newWordNumber == 1 && dataCountDetails.interactive_line_new_word != "")
            {
                ShowInteractivePopup();
            }
            return;
            
        }

        Debug.Log("Checking for revision world " + newWordNumber + newWordDataCount);
        if ((newWordDataCount != 0 && newWordNumber == newWordDataCount) || newWordDataCount == 0) 
        {
            isQuestion = false;
            dataDisplayed["isNewWordDetailsDone"] = true;

             HideSpeakerAndImage();

            DestroyPrefabs();
            // new word displaying is done start checking for revision word list and related data
            Debug.Log("Entering to revision world & value of " + dataDisplayed["isRevisionWordListDone"] + " " + dataDisplayed["isRevisionWordContentDone"]);
            

            // if list is displayed run for loop, all methods for first word and so on
            if ((availableData["isRevisionWordsAvailable"] == true) && (dataDisplayed["isRevisionWordListDone"] == false) )
            {       
                
                parameterValueArray.Add(0);
                methodCallArray.Add(RevisionWordList);  
                RevisionWordList(0);   
                Debug.Log("revisionList " + revisionList); 
                bool valueAssigned = false;
                for(int x = 0; x < numberOfRevisionWords; x++)
                {
                    Debug.Log("number of revision words are " + numberOfRevisionWords);
                    Debug.Log("revisionCountArray[x] " + revisionCountArray[x]);
                    if (revisionCountArray[x] != 0 && valueAssigned == false)
                    {
                        revisionWordReference = x;
                        Debug.Log("value of revision word reference " + revisionWordReference);
                        valueAssigned = true;
                        return;

                    }
                    // return;

                }   
                return;
               
            }
            else if (dataDisplayed["isRevisionWordListDone"] == true && dataDisplayed["isRevisionWordContentDone"] == false && revisionList - 1 > 0 )//&& revisionCountArray[revisionWordReference] > 0 )
            {          
                    Debug.Log("Going to check data");
                    revisionWordDetails = allDetailData.revisionWords[revisionWordReference];
                    Debug.Log("revisionWordReference " + revisionCountArray[revisionWordReference]);
                    Debug.Log("count of conversation" + dataCountDetails.conversation_revision_word_count);

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
                        Debug.Log("Checking for other of using word");
                        tempDataCount += 1;
                        parameterValueArray.Add(parameterCountControlCheck);
                        methodCallArray.Add(AnotherWayOfUsingWordSetup); 
                        AnotherWayOfUsingWordSetup(parameterCountControlCheck);
                    
                    }
                    else if ((dataCountDetails.revision_word_data.more_data[revisionWordReference].use_multiple_count != 0) && (dataDisplayed["isRevisionWordUsingMultipleWordsDone"] == false))
                    {
                        Debug.Log("Calling for Multiple Word Setup " + revisionWordReference);
            
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

                        Debug.Log("reading conversation");               
                         tempDataCount += 1; 
                        parameterValueArray.Add(parameterCountControlCheck);
                        methodCallArray.Add(ConversationSetup);     
                        ConversationSetup(parameterCountControlCheck);
                       
                    }
                    
                    Debug.Log(tempDataCount + "tempDataCount");
                    Debug.Log(numberOfRevisionWords + " numberOfRevisionWords");



                    if (tempDataCount == 1 && revisionCountArray[revisionWordReference] == 0)
                    {
                        parameterCountControlCheck = 0;
                        dataDisplayed["isRevisionWordContentDone"] = true;
                        Debug.Log("isRevisionWordContentDone with conversation");
                    }
                     
                     if (revisionWordReference == 0)
                     {
                        Debug.Log("going in first loop");
                        if (tempDataCount == revisionCountArray[revisionWordReference] + dataCountDetails.conversation_revision_word_count)// && tempDataCount != 0)
                        {
                            tempDataCount = 0; // 0 for first word content
                            
                            if (revisionWordReference == numberOfRevisionWords - 1)
                            {
                                parameterCountControlCheck = 0;

                                dataDisplayed["isRevisionWordContentDone"] = true;
                                Debug.Log("isRevisionWordContentDone ");

                            }  
                            else if (revisionWordReference < numberOfRevisionWords - 1)
                            {
                                Debug.Log("isRevisionWordContentDone with ");
                                Debug.Log("check account of word reference" + revisionCountArray[revisionWordReference]);

                                int nextReferenceNumber = revisionWordReference + 1;
                                for(int x = nextReferenceNumber; x < numberOfRevisionWords; x++)
                                {
                                   Debug.Log("number of revision words are " + numberOfRevisionWords);
                                    if (revisionCountArray[x] != 0)
                                    {
                                        revisionWordReference = x;
                                        Debug.Log("value of revision word reference " + revisionWordReference);
                                        dataDisplayed.Clear();
                                        dataDisplayed = copyOfdataDisplayed;
                                        dataDisplayed["isRevisionWordListDone"] = true;
                                        return;

                                    }
                                    else if (x == numberOfRevisionWords - 1)
                                    {
                                        parameterCountControlCheck = 0;
                                        dataDisplayed["isRevisionWordContentDone"] = true;
                                        Debug.Log("isRevisionWordContentDone ");

                                    } 
                                }
                            } 
                        }
                     }
                     else
                     {
                        Debug.Log("going in second loop");
                        if (tempDataCount == revisionCountArray[revisionWordReference] + dataCountDetails.conversation_revision_word_count)
                        {
                            // if (tempDataCount == revisionList - 1)
                            // {
                                
                                  tempDataCount = 0; // 0 for first word content

                                    if (revisionWordReference == numberOfRevisionWords - 1)
                                    {

                                        // if (dataCountDetails.conversation_revision_word_count != 0 && dataDisplayed["isRevisionWordConversationDone"] == true)
                                        // {
                                            parameterCountControlCheck = 0;
                                            dataDisplayed["isRevisionWordContentDone"] = true;
                                            Debug.Log("isRevisionWordContentDone ");
                                        // }
            
                                    }  
                                    else if (revisionWordReference < numberOfRevisionWords - 1)
                                    {
                                        Debug.Log("isRevisionWordContentDone with ");

                                        int nextReferenceNumber = revisionWordReference + 1;
                                        for(int x = nextReferenceNumber; x < numberOfRevisionWords; x++)
                                        {
                                        Debug.Log("number of revision words are " + numberOfRevisionWords);
                                            if (revisionCountArray[x] != 0)
                                            {
                                                revisionWordReference = x;
                                                Debug.Log("value of revision word reference " + revisionWordReference);
                                                dataDisplayed.Clear();
                                                dataDisplayed = copyOfdataDisplayed;
                                                dataDisplayed["isRevisionWordListDone"] = true;
                                                return;

                                            }
                                            else if (x == numberOfRevisionWords - 1)
                                            {
                                            
                                                parameterCountControlCheck = 0;
                                                dataDisplayed["isRevisionWordContentDone"] = true;
                                                Debug.Log("isRevisionWordContentDone ");

                                            } 
                                        }
                                    } 
                            // }
                          
                        }
                     }
                    
                        
                    
                     return;
                    
            }
             else if (dataDisplayed["isConversationMCQDone"] == false && dataCountDetails.conversation_mcq_count != 0 ) //dataDisplayed["isRevisionWordContentDone"] == true && dataDisplayed["isConversationMCQDone"] == false)
            {
                Debug.Log("Check for convo mcq ");
                //call for mcq
                
                    Debug.Log("conversation_mcq_count" + dataCountDetails.conversation_mcq_count);
                    parameterValueArray.Add(0);
                    methodCallArray.Add(ConversationWithMCQSetup);     
                    
                    ConversationWithMCQSetup(0); // conversation mcq will be one only for now as per requirement from client
                    return;
                    // // make a method to display these mcqs & dataDisplayed["isConversationMCQDone"] is true after displaying and taking answer
                    // dataDisplayed["isConversationMCQDone"] = true;
                
             }
            else if (dataDisplayed["isPassageMCQDone"] == false && dataCountDetails.passage_data.passage_count != 0) // && dataDisplayed["isRevisionWordContentDone"] == true) // && dataDisplayed["isConversationMCQDone"] == true)
            {
                Debug.Log("Check for passage mcq " );
               
                // DestroyConvoPrefabs();
                Debug.Log("passage_count" + dataCountDetails.passage_data.passage_count);
                parameterValueArray.Add(parameterCountControlCheck);
                methodCallArray.Add(PassageMCQSetup);  
                
                PassageMCQSetup(parameterCountControlCheck);
                return;

                // parameterCountControlCheck pending
              
            }
            else if (dataDisplayed["isGeneralMCQDone"] == false && dataCountDetails.mcq_count != 0 ) // && dataDisplayed["isRevisionWordContentDone"] == true) // dataDisplayed["isPassageMCQDone"] == true && 
            {
            
                 Debug.Log("Check for general mcq");
                parameterValueArray.Add(tempGeneralMCQCount);
                methodCallArray.Add(GeneralMCQSetup); 
                
                GeneralMCQSetup(tempGeneralMCQCount);

                return;
            } 
        }
    }

    public void GeneralMCQSetup(int parameter)
    {
        isQuestion = true; 
        DestroyConvoPrefabs();
        baseParentBoard.gameObject.SetActive(false);
        conversationWithMCQBoard.gameObject.SetActive(false);
        generalMCQBoard.gameObject.SetActive(true);
        revisionWordBoard.gameObject.SetActive(false);
        conversationBoard.gameObject.SetActive(false);

        GameObject topImage = generalMCQContent.transform.GetChild(0).gameObject;
        TMPro.TMP_Text mcqInteractiveStatement = topImage.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();

        if (allDetailData.interactive_line_mcq != "")
        {
            mcqInteractiveStatement.text = allDetailData.interactive_line_mcq;
        }
        else
        {
            mcqInteractiveStatement.text = "Let's revise what we have already learnt.";
        }

        GameObject mcqBoard = generalMCQContent.transform.GetChild(1).gameObject;
        VerticalLayoutGroup mcqPathVlg = mcqBoard.GetComponent<VerticalLayoutGroup>();
        GameObject questionBoard = mcqBoard.transform.GetChild(0).gameObject;
        GameObject bg = questionBoard.transform.GetChild(0).gameObject;
        VerticalLayoutGroup bgPathVlg = bg.GetComponent<VerticalLayoutGroup>();

        TMPro.TMP_Text questionText = bg.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
        questionText.text = allDetailData.questions[parameter].title;
        float questionLength = allDetailData.questions[parameter].title.Length;

        GameObject optionBoard = mcqBoard.transform.GetChild(1).gameObject;
        GameObject optionContainer = optionBoard.transform.GetChild(0).gameObject;
        VerticalLayoutGroup pathVlg = optionContainer.GetComponent<VerticalLayoutGroup>();


        int answerOptions = allDetailData.questions[parameter].questionOptions.Length;
        
        Debug.Log("answer options is " + answerOptions);
        int noOfAttempts = 0;
        for(int x = 0; x < answerOptions; x++)
        {
            int value = allDetailData.questions[parameter].questionOptions[x].value;

            if (value == 1)
            {
                noOfAttempts = noOfAttempts + 1;
                Debug.Log("because value is 1");
            }
        }

        int children = optionContainer.transform.childCount;
        
        Button[] otherMCQbuttons = new Button[answerOptions];


        questionNumber = parameter;

        for(int j = 0; j < children; j++ )
        {
            Button thisButton = optionContainer.transform.GetChild(j).GetComponent<Button>();

            thisButton.gameObject.SetActive(true);
            thisButton.enabled = true;
            if (j <= answerOptions-1)
            {
                GameObject container = thisButton.transform.GetChild(0).gameObject;
                TMPro.TMP_Text answerOption = container.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                // Debug.Log("parameter value is " + parameter);
                //  Debug.Log("j value is " + j);
                // Debug.Log(allDetailData.questions[parameter].questionOptions[j].option);
                answerOption.text = allDetailData.questions[parameter].questionOptions[j].option;
                GameObject rightWrongImage = container.transform.GetChild(2).gameObject;
                rightWrongImage.SetActive(false);
                otherMCQbuttons[j] = thisButton;
                thisButton.onClick.AddListener(delegate{CheckRightMCQAnswer(thisButton,questionNumber, otherMCQbuttons, noOfAttempts);});
            }
            else
            {
                thisButton.gameObject.SetActive(false);            
            }

            pathVlg.spacing = 80;
            Debug.Log("length of question is " + questionLength);

            if (questionLength <= 30)
            {
                
                mcqPathVlg.spacing = -240;
                if (answerOptions == 4)
                {
                    mcqPathVlg.spacing = -80;
                }
                else if (answerOptions > 3)
                {
                    mcqPathVlg.spacing = -160;
                }

               
                
            }
            else if (questionLength >= 32 && questionLength <= 38)
            {

                mcqPathVlg.spacing = -120;
                if (answerOptions == 2)
                {
                    mcqPathVlg.spacing = -260;
                }
            }
            else if (questionLength < 40)
            {
                 mcqPathVlg.spacing = -100; // -60
            }
            else if (questionLength > 70 && questionLength < 200)
            {
                bgPathVlg.spacing = 40;
                bgPathVlg.padding.bottom = 40;
                if (answerOptions == 2)
                {
                     mcqPathVlg.spacing = -300;
                     
                }
                else
                {
                    mcqPathVlg.spacing = -180;
                }
            }
            else if (questionLength < 70)
            {
                mcqPathVlg.spacing = -240;
            }
            else if (questionLength > 200)
            {
                mcqPathVlg.spacing = -100;
            }
            else 
            {
                mcqPathVlg.spacing = -80;
            }
            

            // if (questionLength > 120)
            // {
            //     bgPathVlg.padding.bottom = 0;
            //     bgPathVlg.padding.top = 0;
            // }
            // else
            // {
            //     bgPathVlg.padding.bottom = 30;
            //     bgPathVlg.padding.top = 30;
            // }

            // 32 - "-400", in some cases 31 , -60
            // 60, 37, 25 - "-60"
            //77, -180
            // 126, -60, 53 -60
            //47, -240

            // if (j <= 3)
            // {
            //     mcqPathVlg.spacing = -60;
            //     // - 400 for less length of question string
            // }
            // else
            // {
            //     mcqPathVlg.spacing = 60;
            // }
        }

        if (isSettingCanvas == true)
        {
            if (tempGeneralMCQCount == generalMCQcount - 1)
            {
                submitButton.gameObject.SetActive(true);
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

    public void CheckRightMCQAnswer(Button button, int questionNumber, Button[] mcqButtonArray, int thisnoOfAttempts)
    {
        // int questionNumber - check the answer value 1 for particular question
        // allDetailData.conversationQuestions[0].questionOptions[j].option - j is option number
        Button[] buttonArray = mcqButtonArray;
        
        int tag = System.Convert.ToInt32(button.tag);
        int value = allDetailData.questions[questionNumber].questionOptions[tag].value;
        Debug.Log(button.tag);
        Debug.Log("value of" + value);
        GameObject container = button.transform.GetChild(0).gameObject;

        GameObject rightWrongImage = container.transform.GetChild(2).gameObject;
        Image myImage = rightWrongImage.GetComponent<Image>();

        int questionId = allDetailData.questions[questionNumber].id;
        int selectedOptionsId = allDetailData.questions[questionNumber].questionOptions[tag].id;   
        int answerClicked = 0;
        noOfAttempts = thisnoOfAttempts;

        if (value == 1)
        {
            
            myImage.sprite = tickSprite;
            SoundManagerScript.RightAnswerSound();
        }
        else if (value == 0)
        {
            
            myImage.sprite = wrongSprite;
            SoundManagerScript.WrongAnswerSound();
        }
        answerClicked += 1;
        rightWrongImage.SetActive(true);
        
       Debug.Log("No of times answer clicked is " + answerClicked);
        
        Debug.Log("No of attempts is " + noOfAttempts);
       if (answerClicked == noOfAttempts)
        {
            for (int j = 0; j < buttonArray.Length; j++)
            {
                buttonArray[j].enabled = false;
            }
            answerClicked = 0;
            noOfAttempts = 0;
            // buttonArray.Clear();
            // Destroy(buttonArray);
        }

        if (questionResponseDict.ContainsKey(questionId))       // Check if dictionary contains question id as key
        {
             Debug.Log("Adding for next time" + selectedOptionsId);

            // if yes check for selected option id otherwise add
            List<int> tempList = questionResponseDict[questionId];
            if (tempList.Contains(selectedOptionsId))
            {
                //do nothing
            }
            else
            {
                tempList.Add(selectedOptionsId);
                questionResponseDict[questionId] = tempList;               
            }

        }
        else  // if doesn't contain key set key and add selected option id as well
        {
            Debug.Log("Adding for first time " + selectedOptionsId);
            List <int> selectedIdList = new List<int>();
            selectedIdList.Add(selectedOptionsId);
            questionResponseDict.Add(questionId, selectedIdList);
        }
    }


    public void PassageMCQSetup(int parameter)
    {
        isQuestion = true;
        // Scrollbar bar = passageScrollBar.GetComponent<Scrollbar>();
        // bar.value = 1;
        // passageScrollBar.value = 0.0f;
        DestroyConvoPrefabs();
        Debug.Log("setting up second passage " + parameter);
        baseParentBoard.gameObject.SetActive(false);
        conversationWithMCQBoard.gameObject.SetActive(true);
        generalMCQBoard.gameObject.SetActive(false);
        revisionWordBoard.gameObject.SetActive(false);
        conversationBoard.gameObject.SetActive(false);

        int passageCount = dataCountDetails.passage_data.passage_count;

        
        if (allDetailData.passages[parameter].questions.Length != 0)
        {
             convoWithMCQPrefabsArray.Add(convoContentPrefab);

             GameObject convoBoard = convoContentPrefab.transform.GetChild(0).gameObject;

            TMPro.TMP_Text mytext = convoBoard.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            mytext.text = "Passage " + (parameter + 1);

            GameObject scrollBar = convoBoard.transform.GetChild(2).gameObject;
            GameObject descContainer = scrollBar.transform.GetChild(0).gameObject;
            
            TMPro.TMP_Text descriptionText = descContainer.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
            descriptionText.text = allDetailData.passages[parameter].description;

            Debug.Log("debugging value of parameter here " + parameter);
            GameObject questionBoard;
                GameObject newPrefab;
                GameObject mcqBoard;
                GameObject optionBoard;
                GameObject optionContainer;
                VerticalLayoutGroup pathVlg;
            for (var i = 0; i < allDetailData.passages[parameter].questions.Length; i++)
            {
                // questionNumberValue += 1;
                // questionNumber = i;
                

                if (i == 0)
                {
                    //  SetUpSinglePassageWithMCQ(parameter);

                        // GameObject 
                        mcqBoard = convoContentPrefab.transform.GetChild(1).gameObject;
                        
                        // pathVlg = mcqBoard.GetComponent<VerticalLayoutGroup>();

                        // GameObject 
                        questionBoard = mcqBoard.transform.GetChild(0).gameObject;
                optionBoard = mcqBoard.transform.GetChild(1).gameObject;
                        optionContainer = optionBoard.transform.GetChild(0).gameObject;
                
                }
                else
                {

                    Vector2 prefabPosition = convoWithMCQPrefabsArray[i - 1].transform.position;

                    // GameObject 
                    newPrefab = Instantiate(mcqPrefab).gameObject;
                    newPrefab.transform.SetParent(convoContentPrefab.transform, true);
                    newPrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 100.0f);//(height + 200.0f)); //600.0f

                     convoWithMCQPrefabsArray.Add(newPrefab);

                                        //  GameObject 
                    questionBoard = newPrefab.transform.GetChild(0).gameObject;

                optionBoard = newPrefab.transform.GetChild(1).gameObject;
                         optionContainer = optionBoard.transform.GetChild(0).gameObject;
                }

               
                
                    // if (i == 1)
                    // {
                    //     newPrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 800.0f);

                    // }
                    // else
                    // {

                    // }
                     

                        //    GameObject mcqBoard = newPrefab.transform.GetChild(1).gameObject;
                    

                    GameObject bg = questionBoard.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text questionHeader = bg.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();

                    int questionNumber = i + 1;
                    questionHeader.text = "Question " + questionNumber.ToString();

                     TMPro.TMP_Text question = bg.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();

                    Debug.Log("debugging value of parameter for questions " + parameter + " & question number " + i );

                    question.text = allDetailData.passages[parameter].questions[i].title;

                    int answerOptions = allDetailData.passages[parameter].questions[i].questionOptions.Length;
                    Debug.Log(answerOptions);

                    int noOfAttempts = 0;
                    for(int x = 0; x < answerOptions; x++)
                    {
                        int value = allDetailData.passages[parameter].questions[i].questionOptions[x].value;

                        if (value == 1)
                        {
                            noOfAttempts += 1;
                        }
                    }
                    int children = optionContainer.transform.childCount;
                    Button[] otherPassagebuttons = new Button[answerOptions];

                    Debug.Log("value of i here is " + i);
                    // questionNumber = i;
                    
                    for(int j = 0; j < children; j++ )
                    {
                        Button thisButton = optionContainer.transform.GetChild(j).GetComponent<Button>();
                          thisButton.gameObject.SetActive(true);
                          thisButton.enabled = true;
                        if (j <= answerOptions-1)
                        {
                            GameObject container = thisButton.transform.GetChild(0).gameObject;

                            TMPro.TMP_Text answerOption = container.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                            answerOption.text = allDetailData.passages[parameter].questions[i].questionOptions[j].option;
                            GameObject rightWrongImage = container.transform.GetChild(2).gameObject;
                            rightWrongImage.SetActive(false);
                            int pcc = parameterCountControlCheck;
                            int k = i;
                            otherPassagebuttons[j] = thisButton;

                            thisButton.onClick.AddListener(delegate{CheckRightAnswerForPassageQuestion(thisButton, pcc,k,otherPassagebuttons, noOfAttempts);});

                        }
                        else
                        {
                            thisButton.gameObject.SetActive(false);
                        }
                          
                    }
                    convoContentPrefab.transform.SetAsFirstSibling();

                    if (i > 0)
                    {
                        pathVlg = convoWithMCQPrefabsArray[i].GetComponent<VerticalLayoutGroup>();

                        RectTransform rt = (RectTransform)convoWithMCQPrefabsArray[i].transform;
                         float height = rt.rect.height;
                         Debug.Log("height of MCQ is " + height);
                        
                        if (answerOptions == 2)
                        {
                            pathVlg.padding.left = 0;
                            pathVlg.padding.top = 400;
                            pathVlg.padding.bottom = 600;
                            pathVlg.spacing = -200;
                            if (i == 1)
                            {
                                pathVlg.padding.top = 600;
                                // pathVlg.padding.bottom = 600;
                            }
                            else if (i == 2)
                            {
                                pathVlg.padding.top = 800;
                                // pathVlg.padding.bottom = 600;
                            }
                        }
                        else if (answerOptions == 3)
                        {
                            pathVlg.padding.left = 0;
                            pathVlg.padding.top = 800;
                            pathVlg.padding.bottom = 800;
                            pathVlg.spacing = -200;
                        }
                        else if (answerOptions == 4)
                        {
                            pathVlg.padding.left = 0;
                            pathVlg.padding.top = 1000;
                            pathVlg.padding.bottom = 800;
                            pathVlg.spacing = -200;
                        }
                        
                        if (parameter > 0 && i >= 2)
                        {
                            pathVlg.padding.top = 1200;
                        }


                    }
                    else
                    {
                        mcqBoard = convoContentPrefab.transform.GetChild(1).gameObject;
                        
                        pathVlg = mcqBoard.GetComponent<VerticalLayoutGroup>();

                        if (answerOptions == 2)
                        {
                            pathVlg.padding.bottom = 400;
                        }
                        else if (answerOptions >= 3)
                        {
                   
                            pathVlg.padding.bottom = 1000;
                        }
                        // else if (answerOptions == 4)
                        // {
                            
                        //     pathVlg.padding.bottom = 1000;
                           
                        // }
                    }
            }
        }
        // else
        // {
        //     // questionNumberValue += 1;
        //     SetUpSinglePassageWithMCQ(parameter);

        // } 

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
                Debug.Log("parameterCountControlCheck " + parameterCountControlCheck);
            }
            
             isSettingCanvas = false;
        }
        
        
    }

    public void SetUpSinglePassageWithMCQ(int parameter)
    {
        // GameObject convoBoard = convoContentPrefab.transform.GetChild(0).gameObject;

        //     TMPro.TMP_Text mytext = convoBoard.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
        //     mytext.text = "Passage";

        //     GameObject scrollBar = convoBoard.transform.GetChild(2).gameObject;
        //     GameObject container = scrollBar.transform.GetChild(0).gameObject;
            
        //     TMPro.TMP_Text descriptionText = container.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
        //     descriptionText.text = allDetailData.passages[parameter].description; //displayed passage

            // if (parameter == 0)
            // {
                GameObject mcqBoard = convoContentPrefab.transform.GetChild(1).gameObject;
                GameObject questionBoard = mcqBoard.transform.GetChild(0).gameObject;

            //  convoWithMCQPrefabsArray.Add(mcqBoard);
            // }
            // else
            // {
                
            // }
           

            GameObject bg = questionBoard.transform.GetChild(0).gameObject;
            TMPro.TMP_Text questionHeader = bg.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
            questionHeader.text = "Question 1";

            TMPro.TMP_Text question = bg.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();

            question.text = allDetailData.passages[parameter].questions[0].title;
            Debug.Log(allDetailData.passages[parameter].questions[0].title);

            int answerOptions = allDetailData.passages[parameter].questions[0].questionOptions.Length;
            Debug.Log(answerOptions);
            int noOfAttempts = 0;
            for(int x = 0; x < answerOptions; x++)
            {
                int value = allDetailData.passages[parameter].questions[0].questionOptions[x].value;

                if (value == 1)
                {
                    noOfAttempts += 1;
                }
            }
            GameObject optionBoard = mcqBoard.transform.GetChild(1).gameObject;
            GameObject optionContainer = optionBoard.transform.GetChild(0).gameObject;

            int children = optionContainer.transform.childCount;
            Button[] passageButtons = new Button[answerOptions];

            questionNumber = 0;
            for(int j = 0; j < children; j++ )
            {
                Button thisButton = optionContainer.transform.GetChild(j).GetComponent<Button>();
                  thisButton.gameObject.SetActive(true);
                  thisButton.enabled = true;

                if (j <= answerOptions-1)
                {
                     GameObject horizontalContainer = thisButton.transform.GetChild(0).gameObject;

                    TMPro.TMP_Text answerOption = horizontalContainer.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                    answerOption.text = allDetailData.passages[parameter].questions[0].questionOptions[j].option;
                    GameObject rightWrongImage = horizontalContainer.transform.GetChild(2).gameObject;
                    rightWrongImage.SetActive(false);
                    passageButtons[j] = thisButton;

                    thisButton.onClick.AddListener(delegate{CheckRightAnswerForPassageQuestion(thisButton, 0, 0, passageButtons,noOfAttempts);});

                }
                else
                {
                    thisButton.gameObject.SetActive(false);
                }
            }

    }

    public void ConversationWithMCQSetup(int parameter)
    {
        // Scrollbar bar = passageScrollBar.GetComponent<Scrollbar>();
        // bar.value = 1;
        // passageScrollBar.value = 0.0f;
        isQuestion = true;
        baseParentBoard.gameObject.SetActive(false);
        conversationWithMCQBoard.gameObject.SetActive(true);
        generalMCQBoard.gameObject.SetActive(false);
        revisionWordBoard.gameObject.SetActive(false);
        conversationBoard.gameObject.SetActive(false);


        int convoMCQcount = dataCountDetails.conversation_mcq_count;

        if (allDetailData.conversationQuestions.Length > 1)
        {
            convoWithMCQPrefabsArray.Add(convoContentPrefab);

            GameObject questionBoard;
                GameObject newPrefab;
                GameObject mcqBoard;
                GameObject optionBoard;
                GameObject optionContainer;
                VerticalLayoutGroup pathVlg;

            for (var i = 0; i < allDetailData.conversationQuestions.Length; i++)
            {
                //  questionNumber = i;
                if (i == 0)
                {
                    // questionNumberValue = 0;
                    SetUpSingleConvoWithMCQ();
                    GameObject convoBoard = convoContentPrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = convoBoard.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                    int dialogueNumber = i + 1;
                    mytext.text = "Conversation - Dialogue " + dialogueNumber;
                }
                else
                {
                    // questionNumberValue += 1;
                    Vector2 prefabPosition = convoWithMCQPrefabsArray[i - 1].transform.position;
                    newPrefab = Instantiate(convoContentPrefab).gameObject;
                    newPrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y-1666f); // -1666f
                    newPrefab.transform.SetParent(convoPassageMCQPrefabParent, true);

                    GameObject convoBoard = newPrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = convoBoard.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                    int dialogueNumber = i + 1;
                    mytext.text = "Conversation - Dialogue " + dialogueNumber;

                    GameObject scrollBar = convoBoard.transform.GetChild(2).gameObject;
                    GameObject container = scrollBar.transform.GetChild(0).gameObject;
                    
                    TMPro.TMP_Text descriptionText = container.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                    descriptionText.text = allDetailData.conversationQuestions[i].conversation;


                   mcqBoard = newPrefab.transform.GetChild(1).gameObject;
                    questionBoard = mcqBoard.transform.GetChild(0).gameObject;

                    GameObject bg = questionBoard.transform.GetChild(0).gameObject;
            TMPro.TMP_Text questionHeader = bg.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();

                    // TMPro.TMP_Text questionHeader = questionBoard.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                    int questionNumber = i + 1;
                    questionHeader.text = "Question " + questionNumber;


            TMPro.TMP_Text question = bg.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                    question.text = allDetailData.conversationQuestions[i].title;
                    int noOfAttempts = 0;
                    int answerOptions = allDetailData.conversationQuestions[i].questionOptions.Length;
                    Debug.Log(answerOptions);

                     for(int x = 0; x < answerOptions; x++)
                    {
                        int value = allDetailData.conversationQuestions[i].questionOptions[x].value;

                        if (value == 1)
                        {
                            noOfAttempts += 1;
                        }
                    }

                    optionBoard = mcqBoard.transform.GetChild(1).gameObject;
                    optionContainer = optionBoard.transform.GetChild(0).gameObject;

                        
                    int children = optionContainer.transform.childCount;
                    Button[] otherQbuttons = new Button[answerOptions];
                    for(int j = 0; j < children; j++ )
                    {
                        
                        Button thisButton = optionContainer.transform.GetChild(j).GetComponent<Button>();
                          thisButton.gameObject.SetActive(true);
                          thisButton.enabled = true;
                        if (j <= answerOptions-1)
                        {
                            GameObject horizontalContainer = thisButton.transform.GetChild(0).gameObject;

                            TMPro.TMP_Text answerOption = horizontalContainer.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                            answerOption.text = allDetailData.conversationQuestions[i].questionOptions[j].option;
                            GameObject rightWrongImage = horizontalContainer.transform.GetChild(2).gameObject;
                            rightWrongImage.SetActive(false);
                            int k = i;
                            otherQbuttons[j] = thisButton;
                            thisButton.onClick.AddListener(delegate{CheckRightAnswerOfQuestion(thisButton, k, otherQbuttons, noOfAttempts);});
                            

                        }
                        else
                        {
                            thisButton.gameObject.SetActive(false);
                        }
                    }
                    convoContentPrefab.transform.SetAsFirstSibling();

                    convoWithMCQPrefabsArray.Add(newPrefab);

                     if (i > 0)
                    {
                        pathVlg = convoWithMCQPrefabsArray[i].GetComponent<VerticalLayoutGroup>();

                        RectTransform rt = (RectTransform)convoWithMCQPrefabsArray[i].transform;
                         float height = rt.rect.height;
                         Debug.Log("height of MCQ is " + height);
                        
                        if (answerOptions == 2)
                        {
                            pathVlg.padding.left = 0;
                            pathVlg.padding.top = 400;
                            pathVlg.padding.bottom = 600;
                            pathVlg.spacing = -200;
                            if (i == 1)
                            {
                                pathVlg.padding.top = 600;
                                // pathVlg.padding.bottom = 600;
                            }
                            else if (i == 2)
                            {
                                pathVlg.padding.top = 800;
                                // pathVlg.padding.bottom = 600;
                            }
                        }
                        else if (answerOptions == 3)
                        {
                            pathVlg.padding.left = 0;
                            pathVlg.padding.top = 800;
                            pathVlg.padding.bottom = 800;
                            pathVlg.spacing = -200;
                        }
                        else if (answerOptions == 4)
                        {
                            pathVlg.padding.left = 0;
                            pathVlg.padding.top = 1000;
                            pathVlg.padding.bottom = 800;
                            pathVlg.spacing = -200;
                        }
                        
                        if (parameter > 0 && i >= 2)
                        {
                            pathVlg.padding.top = 1200;
                        }


                    }
                    
                    
                }  

            }
        }
        else
        {
            // questionNumberValue = 0;
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
            parameterCountControlCheck = 0;
            dataDisplayed["isConversationMCQDone"] = true;
            Debug.Log("Value of conversation MCQ done ");
             isSettingCanvas = false;
        }

        
    }



    public void SetUpSingleConvoWithMCQ()
    {
        GameObject convoBoard = convoContentPrefab.transform.GetChild(0).gameObject; // conversation board
        // GameObject board = convoBoard.transform.GetChild(0).gameObject;
        TMPro.TMP_Text mytext = convoBoard.transform.GetChild(0).GetComponent<TMPro.TMP_Text>(); 
        mytext.text = "Conversation";

            GameObject scrollBar = convoBoard.transform.GetChild(2).gameObject;
            GameObject container = scrollBar.transform.GetChild(0).gameObject;
            
            TMPro.TMP_Text descriptionText = container.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
            descriptionText.text = allDetailData.conversationQuestions[0].conversation;

            GameObject mcqBoard = convoContentPrefab.transform.GetChild(1).gameObject;
            GameObject questionBoard = mcqBoard.transform.GetChild(0).gameObject;

            GameObject bg = questionBoard.transform.GetChild(0).gameObject;
            TMPro.TMP_Text questionHeader = bg.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();

            questionHeader.text = "Question 1";

            TMPro.TMP_Text question = bg.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
            question.text = allDetailData.conversationQuestions[0].title;

            int answerOptions = allDetailData.conversationQuestions[0].questionOptions.Length;
            Debug.Log(answerOptions);

            int noOfAttempts = 0;
            for(int x = 0; x < answerOptions; x++)
            {
                int value = allDetailData.conversationQuestions[0].questionOptions[x].value;

                if (value == 1)
                {
                    noOfAttempts += 1;
                }
            }

            GameObject optionBoard = mcqBoard.transform.GetChild(1).gameObject;
            GameObject optionContainer = optionBoard.transform.GetChild(0).gameObject;

            int children = optionContainer.transform.childCount;
            Button[] firstQbuttons = new Button[answerOptions];

            questionNumber = 0;

            for(int j = 0; j < children; j++ )
            {
                Button thisButton = optionContainer.transform.GetChild(j).GetComponent<Button>();
                  thisButton.gameObject.SetActive(true);
                  thisButton.enabled = true;
                if (j <= answerOptions-1)
                {
                    GameObject horizontalContainer = thisButton.transform.GetChild(0).gameObject;

                    TMPro.TMP_Text answerOption = horizontalContainer.transform.GetChild(1).GetComponent<TMPro.TMP_Text>();
                    answerOption.text = allDetailData.conversationQuestions[0].questionOptions[j].option;
                    GameObject rightWrongImage = horizontalContainer.transform.GetChild(2).gameObject;
                    rightWrongImage.SetActive(false);
                    firstQbuttons[j] = thisButton;
                    thisButton.onClick.AddListener(delegate{CheckRightAnswerOfQuestion(thisButton, 0, firstQbuttons, noOfAttempts);});
                    

                }
                else
                {
                    thisButton.gameObject.SetActive(false);
                }
            }

            
                        
                VerticalLayoutGroup pathVlg = mcqBoard.GetComponent<VerticalLayoutGroup>();

                if (answerOptions == 2)
                {
                    pathVlg.padding.bottom = 400;
                }
                else if (answerOptions >= 3)
                {
            
                    pathVlg.padding.bottom = 1000;
                }
                        
                    

    }

    public void CheckRightAnswerForPassageQuestion(Button button,int passageNumber, int questionNumber, Button[] mcqButtonArray, int thisnoOfAttempts)
    {
        Button[] btnArray = mcqButtonArray;
        int rightAnswers = thisnoOfAttempts;
        // int questionNumber - check the answer value 1 for particular question
        // allDetailData.conversationQuestions[0].questionOptions[j].option - j is option number
        int tag = System.Convert.ToInt32(button.tag);
        Debug.Log(button.tag + "Button tag");
        Debug.Log(passageNumber + "passageNumber");
        Debug.Log(questionNumber + "questionNumber");
        int value = allDetailData.passages[passageNumber].questions[questionNumber].questionOptions[tag].value;
        
        Debug.Log("value of" + value);
        GameObject container = button.transform.GetChild(0).gameObject;

        GameObject rightWrongImage = container.transform.GetChild(2).gameObject;
        Image myImage = rightWrongImage.GetComponent<Image>();

        int questionId = allDetailData.passages[passageNumber].questions[questionNumber].id;
        int selectedOptionsId = allDetailData.passages[passageNumber].questions[questionNumber].questionOptions[tag].id;   
        int answerClicked = 0;
        if (value == 1)
        {
            answerClicked += 1;
            myImage.sprite = tickSprite;
            SoundManagerScript.RightAnswerSound();
        }
        else
        {
            answerClicked += 1;
            myImage.sprite = wrongSprite;
            SoundManagerScript.WrongAnswerSound();

        }       
        rightWrongImage.SetActive(true);

        if (answerClicked == rightAnswers)
        {
            for (int j = 0; j < btnArray.Length; j++)
            {
                btnArray[j].enabled = false;
            }
            answerClicked = 0;
            noOfAttempts = 0;
            rightAnswers = 0;
            // Destroy(buttonArray);
        }
        
        if (questionResponseDict.ContainsKey(questionId))       // Check if dictionary contains question id as key
        {
            // if yes check for selected option id otherwise add
            List<int> tempList = questionResponseDict[questionId];
            if (tempList.Contains(selectedOptionsId))
            {
                //do nothing
            }
            else
            {

                tempList.Add(selectedOptionsId);
                questionResponseDict[questionId] = tempList;
            }

        }
        else  // if doesn't contain key set key and add selected option id as well
        {
            List <int> selectedIdList = new List<int>();
            selectedIdList.Add(selectedOptionsId);
            questionResponseDict.Add(questionId, selectedIdList);
        }
    }

    private IEnumerator coroutine;
    public void CheckRightAnswerOfQuestion(Button button, int questionNumber, Button[] mcqButtonArray, int thisnoOfAttempts)
    {
         Button[] btnArray = mcqButtonArray;
        int rightAnswers = thisnoOfAttempts;
        // int questionNumber - check the answer value 1 for particular question
        // allDetailData.conversationQuestions[0].questionOptions[j].option - j is option number
        int tag = System.Convert.ToInt32(button.tag);
        int value = allDetailData.conversationQuestions[questionNumber].questionOptions[tag].value;
        Debug.Log(button.tag);
        Debug.Log("value of" + value);
        GameObject container = button.transform.GetChild(0).gameObject;

        GameObject rightWrongImage = container.transform.GetChild(2).gameObject;
        Image myImage = rightWrongImage.GetComponent<Image>();

        int questionId = allDetailData.conversationQuestions[questionNumber].id;
        int selectedOptionsId = allDetailData.conversationQuestions[questionNumber].questionOptions[tag].id;
        int numberOfOptions = allDetailData.conversationQuestions[questionNumber].questionOptions.Length;
        // int[] optionArray = new int[numberOfOptions];
        Debug.Log("question number is " + questionNumber + " & its id is " + questionId);

        int answerClicked = 0;
        if (value == 1)
        {
            answerClicked += 1;
            myImage.sprite = tickSprite;
            SoundManagerScript.RightAnswerSound();
        }
        else
        {
            answerClicked += 1;
            myImage.sprite = wrongSprite;
            SoundManagerScript.WrongAnswerSound();
            
        }
       
        rightWrongImage.SetActive(true);

        if (answerClicked == rightAnswers)
        {
            for (int j = 0; j < btnArray.Length; j++)
            {
                btnArray[j].enabled = false;
            }
            answerClicked = 0;
            noOfAttempts = 0;
            rightAnswers = 0;
            // Destroy(buttonArray);
        }

        if (questionResponseDict.ContainsKey(questionId))       // Check if dictionary contains question id as key
        {
            // if yes check for selected option id otherwise add
            List<int> tempList = questionResponseDict[questionId];
            if (tempList.Contains(selectedOptionsId))
            {
                //do nothing
            }
            else
            {
                tempList.Add(selectedOptionsId);
                questionResponseDict[questionId] = tempList;
            }

        }
        else  // if doesn't contain key set key and add selected option id as well
        {
            List <int> selectedIdList = new List<int>();
            selectedIdList.Add(selectedOptionsId);
            questionResponseDict.Add(questionId, selectedIdList);
        }
        // optionArray[0] = selectedOptionsId;
        // SubmitQuestionResponse(questionId, optionArray);
        // Debug.Log(questionResponseDict);
        
       
    }

    public void SubmitResponse() => StartCoroutine(PrintResponseDict_Coroutine());
    public void CompleteMission() => StartCoroutine(CompleteThisMission_Coroutine());


    public IEnumerator PrintResponseDict_Coroutine()
    {

        ResponseSubmission newResponse = new ResponseSubmission();
        newResponse.response = new List<QuestionResponse>();


       foreach(var item in questionResponseDict)
        {   

            Debug.Log("Checking for Item");
            for(int i = 0; i < item.Value.Count; i++)
            {

                Debug.Log("Value of i");
                List<int> optionList = item.Value;
                QuestionResponse newQuestionResponse = new QuestionResponse();
                newQuestionResponse.question_id = item.Key;

                Debug.Log("Count of option List " + optionList.Count);
                newQuestionResponse.selected_options = new SelectedOption[item.Value.Count];


                for (int j = 0; j < optionList.Count; j++)
                {
                     Debug.Log("Value of j");
                    SelectedOption newOption = new SelectedOption{option_id = optionList[j]};
                    newQuestionResponse.selected_options[j] = newOption;
                }
                
                newResponse.response.Add(newQuestionResponse);
        

            }
            

        } 

        string json = JsonUtility.ToJson(newResponse);
        Debug.Log(json);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        string uri = baseURL + "student-results/question-response?day_level_id=" + dayLevelId.ToString(); //+ dayLevelId;

        Debug.Log(uri);

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", auth_key);


        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);
             string userJson = request.downloadHandler.text;
             Debug.Log(userJson);

        }
        else
        {
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

            string userJson = request.downloadHandler.text;
            ResponseResult result = new ResponseResult();
            result = JsonUtility.FromJson<ResponseResult>(userJson);


            string message = "";
            // string scores = result.score_percentage.ToString();
            double scores = result.score_percentage;
            Debug.Log("scores is " + scores);
            // scores = Mathf.Round((float)scores * 100f) / 100f;
            // string roundedScore = scores.ToString();
            int thousandths = (int)(Mathf.Round((float)scores * 100f) / 100f);
            string roundedScore = thousandths.ToString();


            if (result.score_percentage >= 70.0)
            {

                message = "Congratulations! Mission accomplished!";
                SoundManagerScript.SuccessSound();
            }
            else
            {
                message = "Let’s give it another shot!";
                SoundManagerScript.UnsucessSound();

            }

            ScorePopUp popup = UIController.Instance.CreateScorePopUp();
			popup.Init(UIController.Instance.MainCanvas,
				roundedScore,
				message,
                CompleteMission,
                Reset
				);
        }
        request.Dispose();

    }

    // public void ResettingData()  => StartCoroutine(Reset());

    void Reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // newWordNumber = 0;
        // tempDataCount = 0;
        // parameterCountControlCheck = 0;
        // revisionListDisplayed = false;
        // questionResponseDict.Clear();
        // screenCount = 1;
        // Debug.Log("Resetting values");
        
        // availableData.Clear();
        // dataDisplayed.Clear();
        // yield return dataDisplayed = copyOfdataDisplayed;
        // yield return availableData = copyOfavailableData;
        // baseParentBoard.gameObject.SetActive(true);
        // generalMCQBoard.gameObject.SetActive(false);
        // submitButton.gameObject.SetActive(false);
        // parameterValueArray.Clear();
        // methodCallArray.Clear();
        // DecideTypeOfDataAvailable();
        // SetUpBaseCanvas();
        // SetBottomTitleLabel();
    }

    void GoBackToDashboard()
    {
        SceneManager.LoadScene("Dashboard");
    }

    public IEnumerator CompleteThisMission_Coroutine()
    {
         MissionForm missionFormData = new MissionForm { day_level_id = dayLevelId };
        string json = JsonUtility.ToJson(missionFormData);

        Debug.Log(json);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        string uri = baseURL + "student-levels/mark-pass";

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
            string userJson = request.downloadHandler.text;
            ResponseResult result = new ResponseResult();
            result = JsonUtility.FromJson<ResponseResult>(userJson);

            if (result.is_passed == 1)
            {
                // PlayerPrefs.SetBool("isPassed", true);
                 int nextLevel = levelId + 1;
                PlayerPrefs.SetString("NextLevelWillBe", nextLevel.ToString());
                DateTime completionDateTime = System.DateTime.Now.Date;
                PlayerPrefs.SetString("completionDateTime", completionDateTime.ToString());
            }
            else
            {
                // PlayerPrefs.SetBool("isPassed", false);
            }
           
            GoBackToDashboard();

            
        }
        request.Dispose();
    }

    public void RevisionWordList(int parameter)
    {
        // if (revisionListDisplayed == false)
        // {
            baseParentBoard.gameObject.SetActive(false);
            revisionWordBoard.gameObject.SetActive(true);
            generalMCQBoard.gameObject.SetActive(false);
            conversationBoard.gameObject.SetActive(false);
            conversationWithMCQBoard.gameObject.SetActive(false);

            
            revisionWordList.text = "";
            listOfrevisionWords = "";

            if (dataCountDetails.interactive_line_revision_word != "")
            {
                GameObject childObj = revisionWordBoard.transform.GetChild(0).gameObject;
                GameObject childOfFirstParent = childObj.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childOfFirstParent.GetComponent<TMPro.TMP_Text>();
                    mytext.text = allDetailData.interactive_line_revision_word;
            }
            else
            {
                 GameObject childObj = revisionWordBoard.transform.GetChild(0).gameObject;
                GameObject childOfFirstParent = childObj.transform.GetChild(0).gameObject;

                TMPro.TMP_Text mytext = childOfFirstParent.GetComponent<TMPro.TMP_Text>();
                mytext.text = "Revision words for the Day";
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
        //     revisionListDisplayed = true;
        // }
       
    }
    

    public void NounSetup(int parameter)
    {
        DisplaySpeakerandImage();
        baseParentBoard.gameObject.SetActive(true);

        conversationWithMCQBoard.gameObject.SetActive(false);
        generalMCQBoard.gameObject.SetActive(false);
         conversationBoard.gameObject.SetActive(false);
        dutBoard.gameObject.SetActive(false);
        generalBaseBoard.gameObject.SetActive(true);


         singleSentenceBoard.transform.position = sentenceRealPos;
        multipleSentenceBoard.transform.position = sentenceRealPos;

        typeOfDay.text = "New Word";
        typeOfWord.text = "Noun";
        word.text = newWordDetails.name; 
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
                        
                        Vector2 prefabPosition = new Vector2(sentencePrefab.transform.position.x,sentencePrefab.transform.position.y);
                    // Vector2 prefabPosition = new Vector2(sentencePrefabsArray[i - 1].transform.position.x - 160f, sentencePrefabsArray[i - 1].transform.position.y - 164f);
                    GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                    newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f); //prefabPosition.x - 160f
                    
                    newSentencePrefab.transform.SetParent(parent, true);

                    GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newNoun.nounSentences[i].description;
                    newSentencePrefab.transform.localScale = new Vector3(1, 1, 1);
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
        DisplaySpeakerandImage();
        conversationWithMCQBoard.gameObject.SetActive(false);
        generalMCQBoard.gameObject.SetActive(false);
         conversationBoard.gameObject.SetActive(false);
        baseParentBoard.gameObject.SetActive(true);
         dutBoard.gameObject.SetActive(false);
         generalBaseBoard.gameObject.SetActive(true);


          singleSentenceBoard.transform.position = sentenceRealPos;
        multipleSentenceBoard.transform.position = sentenceRealPos;

        typeOfDay.text = "New Word";
        typeOfWord.text = "Verb";
        word.text = newWordDetails.name; 
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
                    float sentenceLength = 0.0f;
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
                            sentenceLength += newVerb.verbSentences[0].description.Length;                 
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
                            newSentencePrefab.transform.localScale = new Vector3(1, 1, 1);
                            Debug.Log("End of Checking in loop for second object");
                            sentenceLength += newVerb.verbSentences[i].description.Length;                 

                        }  

                         VerticalLayoutGroup pathVlg = parent.GetComponent<VerticalLayoutGroup>();
                        Debug.Log("sentence length here is " + sentenceLength);
                        if (sentenceLength > 300)
                        {
                            pathVlg.padding.top = 160;
                            pathVlg.spacing = 200;
                            pathVlg.padding.bottom = 100;
                        }
                        else
                        {
                            pathVlg.padding.top = 60;
                            pathVlg.spacing = 80;
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
        DisplaySpeakerandImage();
        conversationWithMCQBoard.gameObject.SetActive(false);
        generalMCQBoard.gameObject.SetActive(false);
         conversationBoard.gameObject.SetActive(false);
        baseParentBoard.gameObject.SetActive(true);
         dutBoard.gameObject.SetActive(false);
         generalBaseBoard.gameObject.SetActive(true);

          singleSentenceBoard.transform.position = sentenceRealPos;
        multipleSentenceBoard.transform.position = sentenceRealPos;

        typeOfDay.text = "New Word";
        typeOfWord.text = "Adverb";
        word.text = newWordDetails.name; 
        Adverb newAdverb = new Adverb();
        int adverbCount = dataCountDetails.new_word_data.more_data[0].adverb_count;
        newAdverb = newWordDetails.adverbs[parameter];
        meaningAsNoun.text = newAdverb.description;

        float sentenceLength = 0.0f;

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
                    sentenceLength += newAdverb.adverbSentences[0].description.Length;
                
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
                    newSentencePrefab.transform.localScale = new Vector3(1, 1, 1);
                    Debug.Log("End of Checking in loop for second object");
                    sentenceLength += newAdverb.adverbSentences[i].description.Length;
                }  

                 VerticalLayoutGroup pathVlg = parent.GetComponent<VerticalLayoutGroup>();
                Debug.Log("sentence length here is " + sentenceLength);
                if (sentenceLength > 300)
                {
                    pathVlg.padding.top = 160;
                    pathVlg.spacing = 200;
                    pathVlg.padding.bottom = 100;
                }
                else
                {
                    pathVlg.padding.top = 60;
                    pathVlg.spacing = 60;
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
        Debug.Log("Working on adjective");
        DestroyPrefabs();
        DisplaySpeakerandImage();
        conversationWithMCQBoard.gameObject.SetActive(false);
        generalMCQBoard.gameObject.SetActive(false);
        conversationBoard.gameObject.SetActive(false);
        baseParentBoard.gameObject.SetActive(true);
        dutBoard.gameObject.SetActive(false);
        generalBaseBoard.gameObject.SetActive(true);

          singleSentenceBoard.transform.position = sentenceRealPos;
        multipleSentenceBoard.transform.position = sentenceRealPos;

        typeOfDay.text = "New Word";
        typeOfWord.text = "Adjective";
        word.text = newWordDetails.name; 
        Adjective newAdjective = new Adjective();
        int adjectiveCount = dataCountDetails.new_word_data.more_data[0].adjective_count;

            
        newAdjective = newWordDetails.adjectives[parameter];
        meaningAsNoun.text = newAdjective.description;

         float sentenceLength = 0.0f;

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
                    sentenceLength += newAdjective.adjectiveSentences[0].description.Length;                 
                }
                else
                {
                        
                        
                    Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                    GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                    newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 20f);//164f);
                    newSentencePrefab.transform.SetParent(parent, true);
                    GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newAdjective.adjectiveSentences[i].description;
                    newSentencePrefab.transform.localScale = new Vector3(1, 1, 1);
                    sentencePrefabsArray.Add(newSentencePrefab);
                    newSentencePrefab.transform.localScale = new Vector3(1, 1, 1);
                    Debug.Log("End of Checking in loop for second object");
                    sentenceLength += newAdjective.adjectiveSentences[i].description.Length;
                }  

                VerticalLayoutGroup pathVlg = parent.GetComponent<VerticalLayoutGroup>();
                Debug.Log("sentence length here is " + sentenceLength);
                if (sentenceLength > 300)
                {
                    pathVlg.padding.top = 160;
                    pathVlg.spacing = 200;
                    pathVlg.padding.bottom = 100;
                }
                else
                {
                    pathVlg.padding.top = 60;
                    pathVlg.spacing = 60;
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
        Debug.Log("ConversationHappening");
        DestroyPrefabs();
        
        baseParentBoard.gameObject.SetActive(false);
        conversationBoard.gameObject.SetActive(true);
        generalMCQBoard.gameObject.SetActive(false);
        revisionWordBoard.gameObject.SetActive(false);
        dutBoard.gameObject.SetActive(false);
        conversationText.gameObject.SetActive(true);
       
        float sentenceLength = 0.0f;

       
        VerticalLayoutGroup pathVlg = conversationBoard.GetComponent<VerticalLayoutGroup>();

        // conversationBoard.transform.position = new Vector2(conversationBoard.transform.position.x, conversationBoard.transform.position.y - 200.0f);
    
        Conversation convo = new Conversation();
        convo = allDetailData.conversation;

        if (convo != null)
        {
            conversationText.text = convo.description;
            if (convo.description != null)
            {
                 string desc = convo.description;
                Debug.Log("Length of string is " + desc.Length);
                if (desc.Length > 400.0f)
                {
                    pathVlg.padding.top = (int)desc.Length + 350;
                }
                else if (desc.Length < 220.0f)
                {
                    pathVlg.padding.top = (int)desc.Length;
                }
                else
                {
                    pathVlg.padding.top = (int)desc.Length + 200;
                }
                
            }
           
        }
        
        
       
        if (dataDisplayed["isRevisionWordListDone"] == true)
        {
            RevisionConversation revConvo = new RevisionConversation();
            revConvo = allDetailData.revisionConversation;
            conversationText.text = revConvo.description;
            string desc = revConvo.description;
            Debug.Log("Length of string is " + desc.Length);
            if (desc.Length > 400.0f)
            {
                pathVlg.padding.top = (int)desc.Length + 300;
            }
            else if (desc.Length < 220.0f)
            {
                pathVlg.padding.top = (int)desc.Length;
            }
            else
            {
                pathVlg.padding.top = (int)desc.Length + 200;
            }
            // pathVlg.padding.top = (int)desc.Length + 200;
            Debug.Log(revConvo.description);
            Debug.Log(conversationText.text);
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
        typeOfDay.text = "New Word";
        baseParentBoard.gameObject.SetActive(true);
        conversationBoard.gameObject.SetActive(false);
        conversationWithMCQBoard.gameObject.SetActive(false);

        generalBaseBoard.gameObject.SetActive(false);
        dutBoard.gameObject.SetActive(true);
        singleSentenceBoard.gameObject.SetActive(false);
        multipleSentenceBoard.gameObject.SetActive(false);
        generalMCQBoard.gameObject.SetActive(false);
        revisionWordBoard.gameObject.SetActive(false);

        VerticalLayoutGroup pathVlg = dutParent.GetComponent<VerticalLayoutGroup>();
        pathVlg.spacing = 200;
        pathVlg.padding.bottom = 80;
        pathVlg.padding.top = 80;

        // pathVlg.spacing = 100;
        // pathVlg.padding.bottom = 80;
        // pathVlg.padding.top = 80;

        // RectTransform rectTransform = dutSentencePrefab.GetComponent<RectTransform>();
        // RectTransform rectTransform2 = dutSentencePrefab2.GetComponent<RectTransform>();
        // RectTransform rectTransform3 = dutSentencePrefab3.GetComponent<RectTransform>();

        // float totalHeight = rectTransform.rect.height + rectTransform2.rect.height + rectTransform3.rect.height;
        
        // if (totalHeight > 300.0f)
        // {
        //     Debug.Log("totalHeight " + totalHeight);
        //     pathVlg.spacing = 400;
        //     pathVlg.padding.bottom = 160;
        //     pathVlg.padding.top = 160;
        // }

        float sentenceLength = 0.0f;

        word.text = newWordDetails.name; 

        if (newWordDetails.dailyUseTips.Length > 1)
        {
        
        sentencePrefabsArray.Add(dutSentencePrefab);    // have changed sentencePrefab to dutSentencePrefab
        // Vector2 firstSentencePosition = dutSentencePrefab.transform.position;
            for (var i = 0; i < newWordDetails.dailyUseTips.Length; i++)
            {
                // newDUT = newWordDetails.dailyUseTips[i];
                Debug.Log("Running in For loop " + i);
                    
                if (i == 0)
                {
                    Debug.Log("i is 0 here");
                     dutSentencePrefab2.gameObject.SetActive(false);
                    dutSentencePrefab3.gameObject.SetActive(false);
                    
                    GameObject childObj = dutSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newWordDetails.dailyUseTips[0].description; 
                    sentenceLength +=  newWordDetails.dailyUseTips[0].description.Length;                  
                }
                else if (i == 1)
                {
                    dutSentencePrefab2.gameObject.SetActive(true);
                    dutSentencePrefab3.gameObject.SetActive(false);


                    GameObject childObj = dutSentencePrefab2.transform.GetChild(0).gameObject;
                    
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newWordDetails.dailyUseTips[i].description;
                    sentenceLength +=  newWordDetails.dailyUseTips[i].description.Length;                  
  
                }
                else if (i == 2)
                {
                     dutSentencePrefab3.gameObject.SetActive(true);

                    GameObject childObj = dutSentencePrefab3.transform.GetChild(0).gameObject;

                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newWordDetails.dailyUseTips[i].description; 
                    sentenceLength +=  newWordDetails.dailyUseTips[i].description.Length;                  
 
                }
                Debug.Log("sentenceLength " + sentenceLength);

                if (sentenceLength > 300.0f)
                {
                    Debug.Log("sentenceLength second" + sentenceLength);
                    pathVlg.spacing = 360;
                    pathVlg.padding.bottom = 160;
                    pathVlg.padding.top = 160;
                }
                else if (sentenceLength > 200.0f)
                {
                    if (newWordDetails.dailyUseTips.Length == 3)
                    {
                        if (newWordDetails.dailyUseTips[1].description.Length +  newWordDetails.dailyUseTips[2].description.Length < 200 ) 
                        {
                            Debug.Log("sentenceLength third" + sentenceLength);
                            // pathVlg.spacing = 160;
                            // pathVlg.padding.bottom = 100;
                            // pathVlg.padding.top = 100;

                             pathVlg.spacing = 180;
                            pathVlg.padding.bottom = 100;
                            pathVlg.padding.top = 140;
                        }
                        else
                        {
                            Debug.Log("sentenceLength first is " + sentenceLength);
                            pathVlg.spacing = 300;
                            pathVlg.padding.bottom = 120;
                            pathVlg.padding.top = 140;
                        }
                    }
                    else
                    {
                          Debug.Log("sentenceLength fourth" + sentenceLength);
                            pathVlg.spacing = 300;
                            pathVlg.padding.bottom = 120;
                            pathVlg.padding.top = 120;
                    }
                    
                }
                else
                {
                    Debug.Log("sentenceLength fifth" + sentenceLength);
                    pathVlg.spacing = 160;
                    pathVlg.padding.bottom = 120;
                    pathVlg.padding.top = 120;
                }

                // else
                // {
                     
                      
                //     Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                //     GameObject newSentencePrefab = Instantiate(dutSentencePrefab).gameObject;
                //     newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);
                //     if (i == 1)
                //     {
                //         newSentencePrefab.transform.position = new Vector2(firstSentencePosition.x, firstSentencePosition.y - 164f);

                //     }
                //     else
                //     {
                //         newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 164f);

                //     }

                //     newSentencePrefab.transform.SetParent(dutParent, true);
                //     GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                //     TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                //     mytext.text = newWordDetails.dailyUseTips[i].description;
                //     sentencePrefabsArray.Add(newSentencePrefab);
                //     Debug.Log("End of Checking in loop for second object");
                // }  

            }
        }
         else
        {
            dutSentencePrefab2.gameObject.SetActive(false);
            dutSentencePrefab3.gameObject.SetActive(false);

            GameObject childObj = dutSentencePrefab.transform.GetChild(0).gameObject;
            TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
            mytext.text = newWordDetails.dailyUseTips[0].description;
        }

        

        dataDisplayed["isDUTDone"] = true;
    }

    public void AnotherWayOfUsingWordSetup(int parameter)
    {
        // Debug.Log("Defining this 1");
        DestroyPrefabs();
        HideSpeakerAndImage();
        revisionWordBoard.gameObject.SetActive(false);

        generalMCQBoard.gameObject.SetActive(false);
        conversationBoard.gameObject.SetActive(false);
        dutBoard.gameObject.SetActive(false);
        multipleSentenceBoard.gameObject.SetActive(false);
        
        singleSentenceBoard.transform.position = sentenceRealPos;
        generalBaseBoard.gameObject.SetActive(true);
        singleSentenceBoard.gameObject.SetActive(true);
        baseParentBoard.gameObject.SetActive(true);


        multipleSentenceBoard.transform.position = new Vector2(sentenceRealPos.x, sentenceRealPos.y - 50.0f);// - 200f);
        // multipleSentenceBoard.transform.position = sentenceRealPos; //+ 50f;

        int owuwCount = 0;
        OtherWayUsingWord newOwuw = new OtherWayUsingWord();

        if (dataDisplayed["isRevisionWordListDone"] == true)
        {
            typeOfDay.text = "Another way of using";
            typeOfWord.text = "Brief";
             owuwCount = dataCountDetails.revision_word_data.more_data[revisionWordReference].other_way_using_count;
             newOwuw = revisionWordDetails.otherWayUsingWords[parameter];
             meaningAsNoun.text = newOwuw.description;
             word.text = revisionWordDetails.name;
        }
        else
        {
            typeOfDay.text = "Another way of using";
            typeOfWord.text = "Brief";
             owuwCount = dataCountDetails.new_word_data.more_data[0].other_way_using_count;
             newOwuw = newWordDetails.otherWayUsingWords[parameter];
             meaningAsNoun.text = newOwuw.description;
              word.text = newWordDetails.name;
        }


        if (newOwuw.otherWayUsingWordSentences.Length > 1)
        {
            sentencePrefabsArray.Add(sentencePrefab);

            singleSentenceBoard.gameObject.SetActive(false);
            multipleSentenceBoard.gameObject.SetActive(true);

            float sentenceLength = 0.0f;

            for (var i = 0; i < newOwuw.otherWayUsingWordSentences.Length; i++)
            {
                    
                if (i == 0)
                {
                    Debug.Log("i is 0 here");
                    GameObject childObj = sentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newOwuw.otherWayUsingWordSentences[0].description; 
                    sentenceLength += newOwuw.otherWayUsingWordSentences[0].description.Length;                   
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
                    newSentencePrefab.transform.localScale = new Vector3(1, 1, 1);
                    Debug.Log("End of Checking in loop for second object");
                    sentenceLength += newOwuw.otherWayUsingWordSentences[i].description.Length;
                }  

                 VerticalLayoutGroup pathVlg = parent.GetComponent<VerticalLayoutGroup>();
                        Debug.Log("sentence length here is " + sentenceLength);
                        if (sentenceLength > 300)
                        {
                            pathVlg.padding.top = 160;
                            pathVlg.spacing = 200;
                            pathVlg.padding.bottom = 100;
                        }
                        else
                        {
                            pathVlg.padding.top = 60;
                            pathVlg.spacing = 60;
                        }


            }
        }
        else
        {
            singleSentenceBoard.gameObject.SetActive(true);
            multipleSentenceBoard.gameObject.SetActive(false);
            var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
            mytext.text = newOwuw.otherWayUsingWordSentences[0].description;
            Debug.Log(newOwuw.otherWayUsingWordSentences[0].description);
        }
        if (isSettingCanvas == true)
        {
            Debug.Log("owuwCount is " + owuwCount);
            if (parameterCountControlCheck == owuwCount - 1)
            {
                parameterCountControlCheck = 0;     //resetting 
                Debug.Log("OWUW is complete here");
                    if (dataDisplayed["isRevisionWordListDone"] == true)
                {
                    dataDisplayed["isRevisionWordOWUWordDone"] = true;
                }   
                else
                {
                    dataDisplayed["isnewWordOWUWordDone"] = true;
                }
                
            }
            else 
            {
                Debug.Log("Working on calling other way of using word again");
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
        conversationWithMCQBoard.gameObject.SetActive(false);
        generalMCQBoard.gameObject.SetActive(false);
        dutBoard.gameObject.SetActive(false);


        if (dataDisplayed["isRevisionWordUsingMultipleWordsDone"] == true || dataDisplayed["isnewWordOWUWordDone"] == true || dataDisplayed["isDUTDone"] == true)
        {
                //Change position of sentence boards
                // Vector3 singleSentenceBoardPos = singleSentenceBoard.transform.position;
                // singleSentenceBoardPos.y += 80f;
            singleSentenceBoard.transform.position = sentenceRealPos;
            multipleSentenceBoard.transform.position = sentenceRealPos;

            // Vector3 multipleSentenceBoardPos = multipleSentenceBoard.transform.position;
            // multipleSentenceBoardPos.y += 80f;
        }    

        typeOfDay.text = "Idiom";
        
        typeOfWord.text = "Meaning";
         Idiom newIdiom = new Idiom();
        int idiomCount = 0;

        if (dataDisplayed["isRevisionWordListDone"] == true)
        {
             idiomCount = dataCountDetails.revision_word_data.more_data[revisionWordReference].idiom_count;    
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
       

        float sentenceLength = 0.0f;
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
                    sentenceLength += newIdiom.idiomSentences[0].description.Length;                  
                }
                else
                {
                     
                      
                    Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
                    GameObject newSentencePrefab = Instantiate(sentencePrefab).gameObject;
                    newSentencePrefab.transform.position = new Vector2(prefabPosition.x, prefabPosition.y - 50f); // 164f
                    newSentencePrefab.transform.SetParent(parent, true);
                    GameObject childObj = newSentencePrefab.transform.GetChild(0).gameObject;
                    TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newIdiom.idiomSentences[i].description;
                    sentencePrefabsArray.Add(newSentencePrefab);
                    newSentencePrefab.transform.localScale = new Vector3(1, 1, 1);
                    Debug.Log("End of Checking in loop for second object");
                    sentenceLength += newIdiom.idiomSentences[i].description.Length;                  

                }  


                 VerticalLayoutGroup pathVlg = parent.GetComponent<VerticalLayoutGroup>();
                        Debug.Log("sentence length here is " + sentenceLength);
                        if (sentenceLength > 300)
                        {
                            pathVlg.padding.top = 160;
                            pathVlg.spacing = 200;
                            pathVlg.padding.bottom = 100;
                        }
                        else
                        {
                            pathVlg.padding.top = 60;
                            pathVlg.spacing = 60;
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

        Debug.Log("Value of parameter here is " + parameter);
             Debug.Log("Value of revision reference number here is " + revisionWordReference);
             Debug.Log("revision word name" + revisionWordDetails.name);

        dutBoard.gameObject.SetActive(false);
        baseParentBoard.gameObject.SetActive(true);
        revisionWordBoard.gameObject.SetActive(false);
        generalBaseBoard.gameObject.SetActive(false);
        conversationWithMCQBoard.gameObject.SetActive(false);
        generalMCQBoard.gameObject.SetActive(false);
        conversationBoard.gameObject.SetActive(false);


        singleSentenceBoard.transform.position = new Vector2(sentenceRealPos.x, sentenceRealPos.y + 423f);

        multipleSentenceBoard.transform.position = new Vector2(sentenceRealPos.x, sentenceRealPos.y + 423f);

        int multipleWordCount = 0;
        UseMultipleWord multipleWordDetails = new UseMultipleWord();
        
        if (dataDisplayed["isRevisionWordListDone"] == true)
        {
             typeOfDay.text = "Lets’ try to use these together!";
        
            multipleWordCount = dataCountDetails.revision_word_data.more_data[revisionWordReference].use_multiple_count;
            Debug.Log("Value of multiple word count is " + multipleWordCount);
            Debug.Log("Value of parameter is " + parameter);
            
             multipleWordDetails = revisionWordDetails.useMultipleWords[parameter];
            word.text = multipleWordDetails.description;
        }
        else
        {
             typeOfDay.text = "Lets’ try to use these together!";
        
            multipleWordCount = dataCountDetails.new_word_data.more_data[0].use_multiple_count;
             multipleWordDetails = newWordDetails.useMultipleWords[parameter];
             word.text = multipleWordDetails.description;
        }
       

        float sentenceLength = 0.0f;

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
                    sentenceLength +=  multipleWordDetails.useMultipleWordSentences[0].description.Length; 
                    Debug.Log("sentence length  for 1 here is " + sentenceLength);                 
               
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
                    newSentencePrefab.transform.localScale = new Vector3(1, 1, 1);
                    Debug.Log("End of Checking in loop for second object");
                    sentenceLength +=  multipleWordDetails.useMultipleWordSentences[i].description.Length;                  

                }  

                VerticalLayoutGroup pathVlg = parent.GetComponent<VerticalLayoutGroup>();
                Debug.Log("sentence length here is " + sentenceLength);
                if (sentenceLength > 500)
                {
                    pathVlg.padding.top = 160;
                    pathVlg.spacing = 400;
                    pathVlg.padding.bottom = 180;
                }
                else
                {
                    pathVlg.padding.top = 60;
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
        dutBoard.gameObject.SetActive(false);
        revisionWordBoard.gameObject.SetActive(false);
        generalBaseBoard.gameObject.SetActive(true);
        conversationWithMCQBoard.gameObject.SetActive(false);
        generalMCQBoard.gameObject.SetActive(false);
        conversationBoard.gameObject.SetActive(false);

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
            // typeOfDay.text = "Antonym of " + revisionWordDetails.antonyms[revisionWordReference].description;
            typeOfDay.text = "Antonym of " + revisionWordDetails.name;

            typeOfWord.text = "Meaning";
            
            antonymCount = dataCountDetails.revision_word_data.more_data[revisionWordReference].antonym_count;
            Debug.Log("antonymCount " + antonymCount);
            Debug.Log("value of parameter is  " + parameter);
            Debug.Log("revisionWordReference is " + revisionWordReference);
            antonymDetails = revisionWordDetails.antonyms[parameter]; // replace parameter with revision word reference
            Debug.Log("value of antonymDetails is  " + antonymDetails);
            meaningAsNoun.text = antonymDetails.meaning;
            Debug.Log("value of meaning is  " + antonymDetails.meaning);

            word.text = revisionWordDetails.antonyms[parameter].description;
            
            // Debug.Log("Description is " + revisionWordDetails.antonyms[revisionWordReference].description);
        }
        else
        {
            typeOfDay.text = "Antonym of " + newWordDetails.name;
            typeOfWord.text = "Meaning";
            
            antonymCount = dataCountDetails.new_word_data.more_data[0].antonym_count;
            antonymDetails = newWordDetails.antonyms[parameter];
            meaningAsNoun.text = antonymDetails.meaning;
            word.text = newWordDetails.antonyms[0].description;
        }


        Debug.Log("Length of antonym sentences is " + antonymDetails.antonymSentences.Length);
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
                    newSentencePrefab.transform.localScale = new Vector3(1, 1, 1);
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
        dutBoard.gameObject.SetActive(false);
         baseParentBoard.gameObject.SetActive(true);
        revisionWordBoard.gameObject.SetActive(false);
        generalBaseBoard.gameObject.SetActive(true);
        conversationWithMCQBoard.gameObject.SetActive(false);
        generalMCQBoard.gameObject.SetActive(false);

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
            typeOfDay.text = "Synonym of " + revisionWordDetails.name;
            typeOfWord.text = "Meaning";
            
            synonymCount = dataCountDetails.revision_word_data.more_data[revisionWordReference].synonym_count;
             Debug.Log("value of parameter is  " + parameter);
            Debug.Log("revisionWordReference is " + revisionWordReference);
            Debug.Log("Value of parameter is " + parameter);
            synonymDetails = revisionWordDetails.synonyms[parameter];
            meaningAsNoun.text = synonymDetails.meaning;
            word.text = revisionWordDetails.synonyms[parameter].description;

        }
        else
        {
            typeOfDay.text = "Synonym of " + newWordDetails.name;
            word.text = newWordDetails.synonyms[0].description;
            typeOfWord.text = "Meaning";
            
            synonymCount = dataCountDetails.new_word_data.more_data[0].synonym_count;

            
            synonymDetails = newWordDetails.synonyms[parameter];
            meaningAsNoun.text = synonymDetails.meaning;
        }
    
        Debug.Log("Length of synonym sentences is " + synonymDetails.synonymSentences.Length);

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
                    newSentencePrefab.transform.localScale = new Vector3(1, 1, 1);
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
            if (screenCount != totalNumber)
            {
                    // submitButton.gameObject.SetActive(false);

                    screenCount = screenCount + 1;
                    
                    // Debug.Log(screenCount + " screen count" + methodCallArray.Count);

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
                        // Debug.Log("calling to set base canvas only");
                        SetUpBaseCanvas();
                    }
            }
            else if (screenCount == totalNumber)
            {
                Debug.Log("we are on last screen");
                submitButton.gameObject.SetActive(true);

            }
            
        }
        else
        {
            if (isQuestion == true)
            {
                 Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Are you sure you want to go back? Going back will clear all attempted answers, and you have to reattempt.",
                    "Cancel",
                    "Go Back",
                    GoBack
                    );
            }
            else
            {
                 GoBack();
            }
           
        }

        SetBottomTitleLabel();
       
    }

    public void SendInt(Action<int> action, int value)
    {
        action.Invoke(value);
    }

    void GoBack()
    {
        if (isQuestion == true)
        {
            questionResponseDict.Clear();
            isQuestion = false;
        }
        
        if (screenCount != 1)
        {
        
            screenCount = screenCount - 1;
            // Debug.Log(screenCount + " screen count");
            int parameter = parameterValueArray[screenCount-1];
            Debug.Log(parameter + "parameter value for" + methodCallArray[screenCount-1]);
            Action<int> unityAction = methodCallArray[screenCount-1];
            SendInt(unityAction,parameter);
        }
        else
        {
            Debug.Log("we are on first screen");
        }

        if (screenCount != totalNumber)
        {
            submitButton.gameObject.SetActive(false);
        }
    }

}
