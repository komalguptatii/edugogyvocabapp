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
        {"isnewWordAntonymDone",false}
    };

    public List<GameObject> sentencePrefabsArray = new List<GameObject>();

    public int parameterCountControlCheck = 0;
    public int screenCount = 1;

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
            calculateTotalCount();

        }

    }

    public void calculateTotalCount()
    {
        totalNumber = dataCountDetails.mcq_count + dataCountDetails.new_word_data.more_data[0].noun_count +
        dataCountDetails.new_word_data.more_data[0].verb_count + dataCountDetails.new_word_data.more_data[0].adverb_count +
        dataCountDetails.new_word_data.more_data[0].adjective_count + dataCountDetails.new_word_data.more_data[0].daily_use_tip_count +
        dataCountDetails.new_word_data.more_data[0].other_way_using_count
        + dataCountDetails.new_word_data.more_data[0].idiom_count
        + dataCountDetails.new_word_data.more_data[0].use_multiple_count
        + dataCountDetails.new_word_data.more_data[0].synonym_count
        + dataCountDetails.new_word_data.more_data[0].antonym_count
        + 1;

        int revisionCount = 0;
        for(int i = 0; i < dataCountDetails.revision_word_data.more_data.Length; i++)
        {
            revisionCount = dataCountDetails.revision_word_data.more_data[i].other_way_using_count + 
            dataCountDetails.revision_word_data.more_data[i].idiom_count
            + dataCountDetails.revision_word_data.more_data[i].use_multiple_count
            + dataCountDetails.revision_word_data.more_data[i].synonym_count
            + dataCountDetails.revision_word_data.more_data[i].antonym_count;
        }
       
       totalNumber = totalNumber + revisionCount + dataCountDetails.conversation_revision_word_count 
       + dataCountDetails.conversation_new_word_count + dataCountDetails.conversation_mcq_count 
       + dataCountDetails.passage_data.passage_count;

       Debug.Log(totalNumber);
         GetAllDetails();


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
            SetBottomTitleLabel();
            SetUpBaseCanvas();

        }
    }

    public void SetUpBaseCanvas()
    {
        //Check for new word availability and its correspondent parameter

        if ( availableData["isNewWordAvailable"] == true)
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

                NounSetup();                
            }
            else if ((dataCountDetails.new_word_data.more_data[0].verb_count != 0)  && (dataDisplayed["isVerbDone"] == false))
            {
                Debug.Log("Calling verb Setup");
                VerbSetup();
            }
            else if ((dataCountDetails.new_word_data.more_data[0].adverb_count != 0)  && (dataDisplayed["isAdverbDone"] == false))
            {
                Debug.Log("Calling adverb Setup");
                AdverbSetup();
            }
            else if ((dataCountDetails.new_word_data.more_data[0].adjective_count != 0)  && (dataDisplayed["isAdjectiveDone"] == false))
            {
                Debug.Log("Calling adjective Setup");
                AdjectiveSetup();
            }
            else if ((dataCountDetails.conversation_new_word_count != 0) && (dataDisplayed["isNewWordConverstaionDone"] == false))
            {
                baseParentBoard.gameObject.SetActive(false);
                conversationBoard.gameObject.SetActive(true);
                ConversationSetup();
            }
            else if ((dataCountDetails.new_word_data.more_data[0].daily_use_tip_count != 0) && (dataDisplayed["isDUTDone"] == false))
            {
                baseParentBoard.gameObject.SetActive(true);
                conversationBoard.gameObject.SetActive(false);
                generalBaseBoard.gameObject.SetActive(false);
                dutBoard.gameObject.SetActive(true);
                singleSentenceBoard.gameObject.SetActive(false);
                multipleSentenceBoard.gameObject.SetActive(false);
                DailyTipsSetup();

            }
            else if ((dataCountDetails.new_word_data.more_data[0].other_way_using_count != 0) && (dataDisplayed["isnewWordOWUWordDone"] == false))
            {
                baseParentBoard.gameObject.SetActive(true);
                conversationBoard.gameObject.SetActive(false);
                generalBaseBoard.gameObject.SetActive(true);
                dutBoard.gameObject.SetActive(false);
                singleSentenceBoard.gameObject.SetActive(true);
                multipleSentenceBoard.gameObject.SetActive(false);
                AnotherWayOfUsingWordSetup();

            }
            else if ((dataCountDetails.new_word_data.more_data[0].idiom_count != 0) && (dataDisplayed["isnewWordIdiomDone"] == false))
            {
                IdiomSetup();

            }
             else if ((dataCountDetails.new_word_data.more_data[0].use_multiple_count != 0) && (dataDisplayed["isnewWordUsingMultipleWordsDone"] == false))
            {
                generalBaseBoard.gameObject.SetActive(false);
                //-373
                //Change position of sentence boards
                Vector3 singleSentenceBoardPos = singleSentenceBoard.transform.position;
                singleSentenceBoardPos.y += 423f;
                singleSentenceBoard.transform.position = singleSentenceBoardPos;

                Vector3 multipleSentenceBoardPos = multipleSentenceBoard.transform.position;
                multipleSentenceBoardPos.y += 423f;
                multipleSentenceBoard.transform.position = multipleSentenceBoardPos;
                MultipleWordSetup();

            }
            else if ((dataCountDetails.new_word_data.more_data[0].antonym_count != 0) && (dataDisplayed["isnewWordAntonymDone"] == false))
            {
                generalBaseBoard.gameObject.SetActive(true);

                if (dataDisplayed["isnewWordUsingMultipleWordsDone"] == true)
                {
                    //Change position of sentence boards
                    Vector3 singleSentenceBoardPos = singleSentenceBoard.transform.position;
                    singleSentenceBoardPos.y -= 423f;
                    singleSentenceBoard.transform.position = singleSentenceBoardPos;

                    Vector3 multipleSentenceBoardPos = multipleSentenceBoard.transform.position;
                    multipleSentenceBoardPos.y -= 423f;
                    multipleSentenceBoard.transform.position = multipleSentenceBoardPos;
                }
                
                AntonymSetup();

            }
            else if ((dataCountDetails.new_word_data.more_data[0].synonym_count != 0) && (dataDisplayed["isnewWordSynonymDone"] == false))
            {
                generalBaseBoard.gameObject.SetActive(true);

                if (dataDisplayed["isnewWordUsingMultipleWordsDone"] == true && dataDisplayed["isnewWordAntonymDone"] == false)
                {
                    //Change position of sentence boards
                    Vector3 singleSentenceBoardPos = singleSentenceBoard.transform.position;
                    singleSentenceBoardPos.y -= 423f;
                    singleSentenceBoard.transform.position = singleSentenceBoardPos;

                    Vector3 multipleSentenceBoardPos = multipleSentenceBoard.transform.position;
                    multipleSentenceBoardPos.y -= 423f;
                    multipleSentenceBoard.transform.position = multipleSentenceBoardPos;
                }       
                SynonymSetup();
            }
            // else if ()
            // {
                    // display revision list
            // }
        }

        

    }

    

    public void NounSetup()
    {
        typeOfWord.text = "Noun";
        Noun newNoun = new Noun();
        int nounCount = dataCountDetails.new_word_data.more_data[0].noun_count;

            
        newNoun = newWordDetails.nouns[parameterCountControlCheck];
        meaningAsNoun.text = newNoun.description;


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
                            Debug.Log("i is 1 here");
                            Debug.Log("Checking in loop for second object");
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
    }

    public void VerbSetup()
    {
         
        typeOfWord.text = "Verb";
        Verb newVerb = new Verb();
        int verbCount = dataCountDetails.new_word_data.more_data[0].verb_count;

        Debug.Log("Length of verb" + newWordDetails.verbs.Length);
                newVerb = newWordDetails.verbs[parameterCountControlCheck];
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
                            Debug.Log("i is 1 here");
                            Debug.Log("Checking in loop for second object");
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
    }
    
    public void AdverbSetup()
    {
        typeOfWord.text = "Adverb";
        Adverb newAdverb = new Adverb();
        int adverbCount = dataCountDetails.new_word_data.more_data[0].adverb_count;

            
        newAdverb = newWordDetails.adverbs[parameterCountControlCheck];
        meaningAsNoun.text = newAdverb.description;


        if (newAdverb.adverbSentences.Length > 1)
        {
            sentencePrefabsArray.Add(sentencePrefab);

            singleSentenceBoard.gameObject.SetActive(false);
            multipleSentenceBoard.gameObject.SetActive(true);
            Debug.Log("Length " + newAdverb.adverbSentences.Length);
                    for (var i = 0; i < newAdverb.adverbSentences.Length; i++)
                    {
                        Debug.Log("Running in For loop " + i);
                        Debug.Log(newAdverb.adverbSentences[0].description);
                         
                        if (i == 0)
                        {
                            Debug.Log("i is 0 here");
                            GameObject childObj = sentencePrefab.transform.GetChild(0).gameObject;
                            TMPro.TMP_Text mytext = childObj.GetComponent<TMPro.TMP_Text>();
                            mytext.text = newAdverb.adverbSentences[0].description;                     
                        }
                        else
                        {
                            Debug.Log("i is 1 here");
                            Debug.Log("Checking in loop for second object");
                            Vector2 prefabPosition = sentencePrefabsArray[i - 1].transform.position;
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
                }
                else
                {
                    singleSentenceBoard.gameObject.SetActive(true);
                    multipleSentenceBoard.gameObject.SetActive(false);
                    var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
                    mytext.text = newAdverb.adverbSentences[0].description;
                }
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

    }

    public void AdjectiveSetup()
    {
        typeOfWord.text = "Adjective";
        Adjective newAdjective = new Adjective();
        int adjectiveCount = dataCountDetails.new_word_data.more_data[0].adjective_count;

            
        newAdjective = newWordDetails.adjectives[parameterCountControlCheck];
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
                            Debug.Log("i is 1 here");
                            Debug.Log("Checking in loop for second object");
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

    }

    public void ConversationSetup()
    {
        
        Conversation convo = new Conversation();
        convo = allDetailData.conversation;
        conversationText.text = convo.description;
        dataDisplayed["isNewWordConverstaionDone"] = true;
           
    }




    public void DailyTipsSetup()
    {

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
                            Debug.Log("i is 1 here");
                            Debug.Log("Checking in loop for second object");
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

    public void AnotherWayOfUsingWordSetup()
    {
        
        typeOfDay.text = "Another way of using";
        typeOfWord.text = "Brief/Note";
        OtherWayUsingWord newOwuw = new OtherWayUsingWord();
        int owuwCount = dataCountDetails.new_word_data.more_data[0].other_way_using_count;

            
        newOwuw = newWordDetails.otherWayUsingWords[parameterCountControlCheck];
        meaningAsNoun.text = newOwuw.description;


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
                            Debug.Log("i is 1 here");
                            Debug.Log("Checking in loop for second object");
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
            if (parameterCountControlCheck == owuwCount - 1)
            {
                Debug.Log("OWUW is complete here");
                dataDisplayed["isnewWordOWUWordDone"] = true;
                parameterCountControlCheck = 0;     //resetting 
            }
            else 
            {
                Debug.Log("Working on calling adjective again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
    }

    public void IdiomSetup()
    {
        typeOfDay.text = "Idiom";
        
        typeOfWord.text = "Meaning";
        Idiom newIdiom = new Idiom();
        int idiomCount = dataCountDetails.new_word_data.more_data[0].idiom_count;
       
            
        newIdiom = newWordDetails.idioms[parameterCountControlCheck];
        word.text = newIdiom.description;
        meaningAsNoun.text = newIdiom.meaning;


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
                            Debug.Log("i is 1 here");
                            Debug.Log("Checking in loop for second object");
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
            if (parameterCountControlCheck == idiomCount - 1)
            {
                Debug.Log("Idiom is complete here");
                dataDisplayed["isnewWordIdiomDone"] = true;
                parameterCountControlCheck = 0;     //resetting 
            }
            else 
            {
                Debug.Log("Working on calling idiom again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
    }

    public void MultipleWordSetup()
    {
        
        typeOfDay.text = "Using multiple words in a sentence";
        
        UseMultipleWord multipleWordDetails = new UseMultipleWord();
        int multipleWordCount = dataCountDetails.new_word_data.more_data[0].use_multiple_count;
        multipleWordDetails = newWordDetails.useMultipleWords[parameterCountControlCheck];
        word.text = multipleWordDetails.description;

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
                            Debug.Log("i is 1 here");
                            Debug.Log("Checking in loop for second object");
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
            if (parameterCountControlCheck == multipleWordCount - 1)
            {
                dataDisplayed["isnewWordUsingMultipleWordsDone"] = true;
                parameterCountControlCheck = 0;     //resetting 
            }
            else //if (dataCountDetails.new_word_data.more_data[0].noun_count > 1)
            {
                Debug.Log("Working on calling multipleWords again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
    }

    public void AntonymSetup()
    {
 
        typeOfDay.text = "Antonym of " + newWordDetails.antonyms[0].description;
        typeOfWord.text = "Meaning";
        Antonym antonymDetails = new Antonym();
        int antonymCount = dataCountDetails.new_word_data.more_data[0].antonym_count;

            
        antonymDetails = newWordDetails.antonyms[parameterCountControlCheck];
        meaningAsNoun.text = antonymDetails.meaning;


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
                            Debug.Log("i is 1 here");
                            Debug.Log("Checking in loop for second object");
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
                else
                {
                    singleSentenceBoard.gameObject.SetActive(true);
                    multipleSentenceBoard.gameObject.SetActive(false);
                    var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
                    mytext.text = antonymDetails.antonymSentences[0].description;
                }
            if (parameterCountControlCheck == antonymCount - 1)
            {
                Debug.Log("antonym is complete here");
                dataDisplayed["isnewWordAntonymDone"] = true;
                parameterCountControlCheck = 0;     //resetting 
            }
            else 
            {
                Debug.Log("Working on calling antonym again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
            }
    }

    
    public void SynonymSetup()
    {
        typeOfDay.text = "Synonym of " + newWordDetails.synonyms[0].description;
        typeOfWord.text = "Meaning";
        Synonym synonymDetails = new Synonym();
        int synonymCount = dataCountDetails.new_word_data.more_data[0].synonym_count;

            
        synonymDetails = newWordDetails.synonyms[parameterCountControlCheck];
        meaningAsNoun.text = synonymDetails.meaning;


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
                            Debug.Log("i is 1 here");
                            Debug.Log("Checking in loop for second object");
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
                else
                {
                    singleSentenceBoard.gameObject.SetActive(true);
                    multipleSentenceBoard.gameObject.SetActive(false);
                    var mytext = singleSentencePrefab.GetComponent<TMPro.TMP_Text>();
                    mytext.text = synonymDetails.synonymSentences[0].description;
                }
            if (parameterCountControlCheck == synonymCount - 1)
            {
                Debug.Log("synonym is complete here");
                dataDisplayed["isnewWordSynonymDone"] = true;
                parameterCountControlCheck = 0;     //resetting 
            }
            else 
            {
                Debug.Log("Working on calling synonym again");
                parameterCountControlCheck = parameterCountControlCheck + 1;
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
        screenCount = screenCount + 1;
        
        if (button.tag == "Next")
        {
            SetUpBaseCanvas();
        }
        SetBottomTitleLabel();
    }

}
