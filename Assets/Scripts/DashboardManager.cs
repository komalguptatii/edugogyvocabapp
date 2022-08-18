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
    int levelsPassed = 0;

    DateTime lastTimeClicked;

    private Animator animator;
    string auth_key;

    public GameObject astronaut;
    private Animator characterAnim;

    bool islevelUnlocked = false;
    public float speed = 40.0f;
    Vector3 targetPosition;
     Camera cam;
     Vector3 screenPos;
     Vector2 newPosition;


    private void Awake() {
         if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
        }
        GetUserProfile();
    }

    void Start()
    {
        
        // cam = GetComponent<Camera>();
        var dateAndTime = DateTime.Now;
        var date = dateAndTime.Date;
        Debug.Log(date);
        Debug.Log(System.DateTime.Now); // Format - 07/29/2022 08:33:35
        SpawnPath();

        if (PlayerPrefs.HasKey("NextLevelWillBe"))
        {
            Debug.Log("Checking for TimeSpan");
            string nextLevelWillbe = PlayerPrefs.GetString("NextLevelWillBe");
            Debug.Log(nextLevelWillbe);
            int levelNumber = int.Parse(nextLevelWillbe);
            Debug.Log("level number is " + levelNumber);
            
            // if (levelId == nextLevelWillbe)
            // {
            DateTime thisTime = System.DateTime.Now.Date;
            Debug.Log("this date is " + thisTime);  //09-08-2022 13:34:10
            lastTimeClicked = DateTime.Parse(PlayerPrefs.GetString("completionDateTime"));
            Debug.Log(lastTimeClicked); //08/09/2022 13:33:38

            TimeSpan difference = thisTime.Subtract(lastTimeClicked);
            Debug.Log("difference is " + difference); // 00:00:32.3895120

            if (difference.TotalHours == 0)
            {
                
                // if (numberOfLevelsPerDay != 2)
                // {
                    // unlock
                    //show unlock animation and move character to that level
                    Debug.Log("unlock next level");
                    Button button = GameObject.Find(nextLevelWillbe).GetComponent<Button>();
                     GameObject lockImage = button.transform.GetChild(1).gameObject;
                    // lockImage.SetActive(false);
                    button.tag = "Unlocked";
                    animator = lockImage.GetComponent<Animator>();
                    animator.Play("LockUnlock");
                    characterAnim = astronaut.GetComponent<Animator>();
                    characterAnim.Play("AstroMoving");
                    islevelUnlocked = true;

                    // targetPosition = lockImage.transform.position;
                    // screenPos = cam.ScreenToWorldPoint(targetPosition);

                    if (levelNumber%2 == 0)
                    {
                        newPosition = new Vector2(astronaut.transform.position.x + 590f, astronaut.transform.position.y + 424f);

                    }
                    else
                    {
                        newPosition = new Vector2(astronaut.transform.position.x - 44f, astronaut.transform.position.y + 403f);

                    }

                    // Vector2 newPosition = new Vector2(astronaut.transform.position.x + 100f, astronaut.transform.position.y + 400f);
                    // astronaut.transform.position = Vector2.Lerp(astronaut.transform.position, newPosition, Time.deltaTime);
                    // Vector2.Lerp(previousButtonPosition, newPosition, 0.2f);
                // }
                // else 
                // {
                
                //             //     //save datetime here and check for next level date time, when entered next date set  quota to 0
                //     InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
                //     popup.Init(UIController.Instance.MainCanvas,
                //     "Well done!You have unleashed your true potential. Meet us tomorrow to unlock the next mission!",
                //     "Ok"
                //     );
                // }
            }
                
            // }
        }
       
        //scroll to specific level - unlock next level, check for time period
        //calculate time difference between previousLevel and in next level 
    }

    void Update() {
        // if (islevelUnlocked)
        // {
            

           
                characterAnim.Play("AstroMoving");
                astronaut.transform.position = Vector3.MoveTowards(astronaut.transform.position, newPosition, Time.deltaTime * speed );// 

                // astronaut.transform.position = Vector3.MoveTowards(astronaut.transform.position, -screenPos, Time.deltaTime * speed );// 
           
            // Vector2 newPosition = new Vector2(astronaut.transform.position.x + 100f, astronaut.transform.position.y + 400f);
            // astronaut.transform.position = Vector3.MoveTowards(astronaut.transform.position, newPosition, Time.deltaTime * speed );// 
            //  astronaut.transform.position = Vector2.Lerp(astronaut.transform.position, newPosition, Time.deltaTime);
            // islevelUnlocked = false;
        // }

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
                Debug.Log("Adding Listener");
                int k = z + 1;
                button.name = k.ToString();
                levelNumber.text = k.ToString();
                int removeStartNumber = (k - (i * 5));
                if (k == 1)
                {
                    GameObject lockImage = button.transform.GetChild(1).gameObject;
                    lockImage.SetActive(false);
                    button.tag = "Unlocked";

                    
                }
                else if (removeStartNumber == 1)
                {
                    GameObject startButton = button.transform.GetChild(2).gameObject;
                    startButton.SetActive(false);
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
        Debug.Log("level id is " + levelId);

        // if (EventSystem.current.currentSelectedGameObject.tag == "Unlocked")
        // {
            SaveDataForPreviousLevel(); 
            GetAllDetails();
            numberOfLevelsPerDay = numberOfLevelsPerDay + 1;
            PlayerPrefs.SetInt("numberOfLevelsPerDay", numberOfLevelsPerDay);
        // }
        // else
        // {
        //     Debug.Log("Can't unlock this mission yet");
        // }
        
    
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
        string uri = "http://165.22.219.198/edugogy/api/v1/day-levels/" + levelId + "?expand=newWords,revisionWords,newWords.nouns,newWords.nouns.nounSentences,newWords.verbs,newWords.verbs.verbSentences,newWords.adverbs,newWords.adverbs.adverbSentences,newWords.adjectives,newWords.adjectives.adjectiveSentences,newWords.dailyUseTips,newWords.otherWayUsingWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords.otherWayUsingWordSentences,newWords.idioms,newWords.idioms.idiomSentences,newWords.useMultipleWords,newWords.useMultipleWords.useMultipleWordSentences,newWords.synonyms,newWords.synonyms.synonymSentences,newWords.antonyms,newWords.antonyms.antonymSentences,revisionWords.nouns,revisionWords.nouns.nounSentences,revisionWords.verbs,revisionWords.verbs.verbSentences,revisionWords.adverbs,revisionWords.adverbs.adverbSentences,revisionWords.adjectives,revisionWords.adjectives.adjectiveSentences,revisionWords.dailyUseTips,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords.otherWayUsingWordSentences,revisionWords.idioms,revisionWords.idioms.idiomSentences,revisionWords.useMultipleWords,revisionWords.useMultipleWords.useMultipleWordSentences,revisionWords.synonyms,revisionWords.synonyms.synonymSentences,revisionWords.antonyms,revisionWords.antonyms.antonymSentences,questions,questions.questionOptions,conversation,conversationQuestions,conversationQuestions.questionOptions,passages,passages.questions,passages.questions.questionOptions,revisionConversation";
        // "?expand=newWords,revisionWords,newWords.nouns,newWords.nouns.nounSentences,newWords.verbs,newWords.verbs.verbSentences,newWords.adverbs,newWords.adverbs.adverbSentences,newWords.adjectives,newWords.adjectives.adjectiveSentences,newWords.dailyUseTips,newWords.otherWayUsingWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords.otherWayUsingWordSentences,newWords.idioms,newWords.idioms.idiomSentences,newWords.useMultipleWords,newWords.useMultipleWords.useMultipleWordSentences,newWords.synonyms,newWords.synonyms.synonymSentences,newWords.antonyms,newWords.antonyms.antonymSentences,revisionWords.nouns,revisionWords.nouns.nounSentences,revisionWords.verbs,revisionWords.verbs.verbSentences,revisionWords.adverbs,revisionWords.adverbs.adverbSentences,revisionWords.adjectives,revisionWords.adjectives.adjectiveSentences,revisionWords.dailyUseTips,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords.otherWayUsingWordSentences,revisionWords.idioms,revisionWords.idioms.idiomSentences,revisionWords.useMultipleWords,revisionWords.useMultipleWords.useMultipleWordSentences,revisionWords.synonyms,revisionWords.synonyms.synonymSentences,revisionWords.antonyms,revisionWords.antonyms.antonymSentences,questions,questions.questionOptions,conversation,conversationQuestions,conversationQuestions.questionOptions,passages,passages.questions,passages.questions.questionOptions";
        Debug.Log(uri);
        var request = new UnityWebRequest(uri, "GET");

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        //  request.SetRequestHeader("Authorization", auth_key);
        request.SetRequestHeader("Authorization", "Bearer MaKLCWXbSS8yl9CA_UrnrZJmu-gYlFTK");

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
        //  request.SetRequestHeader("Authorization", auth_key);
        request.SetRequestHeader("Authorization", "Bearer MaKLCWXbSS8yl9CA_UrnrZJmu-gYlFTK");

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

            totalNumberOfLevels = profileData.total_level;
            levelsPassed = profileData.total_passed_level;

            NotifyAboutSubscriptionStatus();
            // {"id":3,"name":"Komal","phone":"9855940600","age_group_id":2,"country_code_id":88,"total_level":2,"total_passed_level":0,"available_level":30}

        }
    }

    void NotifyAboutSubscriptionStatus()
    {
        int sevendsDaysBefore = totalNumberOfLevels - 7;
        int dayBefore = totalNumberOfLevels - 1;

        // if (levelsPassed == 0)
        // {
        //     InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
        //     popup.Init(UIController.Instance.MainCanvas,
        //     "Welcome aboard, astronaut! Your space mission is about to begin.",
        //     "Ready to take off?"
        //     );
        // }
        // else  if (levelsPassed == totalNumberOfLevels) // for free trial
        // {
        //     Debug.Log("Your subscription is over, today is the last day"); //pop up - start with level , you haven't subscribed, complete previous level 
        //     // check for subscription period - 30, 90, 180 may change adding free trial
        //     InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
        //         popup.Init(UIController.Instance.MainCanvas,
        //         "Your subscription is over, today is the last day",
        //         "Ok"
        //         );
            
        // } 
        // else if (levelsPassed == dayBefore)
        // {
        //     InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
        //         popup.Init(UIController.Instance.MainCanvas,
        //         "Your subscription is getting over in a day",
        //         "Ok"
        //         );
        // }
        // else if (levelsPassed == sevendsDaysBefore)
        // {
        //     InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
        //         popup.Init(UIController.Instance.MainCanvas,
        //         "Your subscription is getting over in coming 7 days",
        //         "Ok"
        //         );
        // }
    }
   
}
