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

    [Serializable]
    public class ProfileDetails
    {
        public int id;
        public string name;
        public string phone;
        public int age_group_id;
        public int country_code_id;
        public object social_id;
        public object social_media;
        public object email;
        public int total_level;
        public int total_passed_level;
        public int available_level;
    }

    public GameObject pathPrefab;
    public Transform contentParent;
    [SerializeField] public Button[] levelButtonArray = new Button[5];

    string levelId;
    int numberOfLevelsPerDay = 0;
    int totalNumberOfLevels = 0;

    private void Awake() {
        GetUserProfile();
    }

    void Start()
    {
        Debug.Log(System.DateTime.Now); // Format - 07/29/2022 08:33:35
        SpawnPath();

        //scroll to specific level - unlock next level, check for time period
        //calculate time difference between previousLevel and in next level 
    }

    public void SpawnPath()
    {
        Vector2 currentPosition = pathPrefab.transform.position;
        // float height = pathPrefab.transform.localScale.height;
        // Debug.Log(height);
        int z = 0;

        for(int i = 0; i < 36; i++)
        {
            GameObject nextPath = Instantiate(pathPrefab).gameObject;
            nextPath.transform.position = new Vector2(currentPosition.x, -(currentPosition.y + 1939f));
             // + pathPrefab.localScale.height);
             nextPath.transform.SetParent(contentParent, true);
            currentPosition = nextPath.transform.position;

            for (int j = 0; j < nextPath.transform.childCount; j++ )
            {
                Button button = nextPath.transform.GetChild(j).GetComponent<Button>();
                TMPro.TMP_Text levelNumber = button.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                button.tag = "Locked";
                button.onClick.AddListener(delegate{OnButtonClick();});
                int k = z + 1;
                levelNumber.text = k.ToString();
                if (k == 1)
                {
                    GameObject lockImage = button.transform.GetChild(1).gameObject;
                    lockImage.SetActive(false);
                    button.tag = "Unlocked";
                }
                z = k;
            }
       
        }
    }

    public void OnButtonClick()
    {
        //Get button text to paas level id
        GameObject textobj = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).gameObject;
        TMP_Text mytext = textobj.GetComponent<TMP_Text>();
        levelId = mytext.text;
        Debug.Log(levelId);

        if (numberOfLevelsPerDay == 2) // combine this with checking for next level ID
        {
            // quota over for the day
            //save datetime here and check for next level date time, when entered next date set  quota to 0
             InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
			    popup.Init(UIController.Instance.MainCanvas,
				"Well done!You have unleashed your true potential. Meet us tomorrow to unlock the next mission!",
				"Ok"
				);
        }

        if (PlayerPrefs.HasKey("NextLevelWillBe"))
        {
            string nextLevelWillbe = PlayerPrefs.GetString("NextLevelWillBe");
            Debug.Log(nextLevelWillbe);
            if (levelId == nextLevelWillbe)
            {
                DateTime thisTime = System.DateTime.Now;
                Debug.Log("this date is " + thisTime);
                long temp = Convert.ToInt64(PlayerPrefs.GetString("completionDateTime"));
                DateTime oldDate = DateTime.FromBinary(temp);
                Debug.Log("old date is " + oldDate);
                TimeSpan difference = thisTime.Subtract(oldDate);
                Debug.Log("difference is " + difference);
                //Check for difference here
            }
        }
        else
        {
            //complete previous level - 
        //      // // save & get details only if entering to level also unlock it based on conditions
            if (button.tag == "Unlocked")
            {
                SaveDataForPreviousLevel(); 
                GetAllDetails();
                numberOfLevelsPerDay = numberOfLevelsPerDay + 1;
                PlayerPrefs.SetInt("numberOfLevelsPerDay", numberOfLevelsPerDay);
            }
            
        }

        int sevendsDaysBefore = totalNumberOfLevels - 7;
        int dayBefore = totalNumberOfLevels - 1;
        //next level will be
        if (levelId == totalNumberOfLevels.ToString()) // for free trial
        {
            Debug.Log("Your subscription is over, today is the last day"); //pop up - start with level , you haven't subscribed, complete previous level 
            // check for subscription period - 30, 90, 180 may change adding free trial
            InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
			    popup.Init(UIController.Instance.MainCanvas,
				"Your subscription is over, today is the last day",
				"Ok"
				);
            
        } 
        else if (levelId == sevendsDaysBefore.ToString() || levelId == dayBefore.ToString())
        {
            InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
			    popup.Init(UIController.Instance.MainCanvas,
				"Your subscription is getting over",
				"Ok"
				);
        }
        
        // for testing purpose
         GetAllDetails();
            numberOfLevelsPerDay = numberOfLevelsPerDay + 1;
            PlayerPrefs.SetInt("numberOfLevelsPerDay", numberOfLevelsPerDay);
        
    }

    public void SaveDataForPreviousLevel()
    {
        PlayerPrefs.SetString("PreviousLevelId", levelId);
        DateTime dateTimeOfPreviousLevel = System.DateTime.Now;
        PlayerPrefs.SetString("dateTimeOfPreviousLevel", dateTimeOfPreviousLevel.ToString());
    }

    public void CheckForDateTimePeriod()
    {
        //Keep reference of last "level Id", Its date time when passed 
        // date time at new clicked level id 
        //if difference between date time is > 0 then carry on with 2nd levels
        //set quota done if two levels are completed - as per day only 2 levels are allowed

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
            PlayerPrefs.SetInt("LevelId",int.Parse(levelId));
            
            SceneManager.LoadScene("NewWordDay");
            
                  
        }
    }

    void GetUserProfile() => StartCoroutine(GetUserProfile_Coroutine());

    // called this API for dev purpose only - may be remove it later
    IEnumerator GetUserProfile_Coroutine()
    {
        ProfileDetails profileData = new ProfileDetails();
        string uri = "http://165.22.219.198/edugogy/api/v1/students/view";

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
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

            string responseJson = request.downloadHandler.text;
            profileData = JsonUtility.FromJson<ProfileDetails>(responseJson);

            if (profileData.total_passed_level == 0)
            {
                InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
			    popup.Init(UIController.Instance.MainCanvas,
				"Welcome aboard, astronaut! Your space mission is about to begin.",
				"Ready to take off?"
				);
            }
            else
            {
                Debug.Log(profileData.total_passed_level);
            }

            totalNumberOfLevels = profileData.total_level;

            // {"id":3,"name":"Komal","phone":"9855940600","age_group_id":2,"country_code_id":88,"total_level":2,"total_passed_level":0,"available_level":30}

        }
    }

   
}
