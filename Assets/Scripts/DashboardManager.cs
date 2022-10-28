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
        public bool is_trial_subscription;
        public string subscription_remaining_day;
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
     Vector3 worldPosition;
     Vector2 newPosition;
    public Vector3 astronautOriginalPosition;
    public ScrollRect scrollRect;
    public RectTransform scrollRectTransform;
    public RectTransform contentPanel;
    Button lastReachedLevel;
    RectTransform maskTransform;
    float offset = -1500.0f;
    string missionNumber;
    bool isTrial = false;


    string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    // string baseURL = "http://165.22.219.198/edugogy/api/v1/";

    float leftSide, rightSide;
    float clampValue = 150.0f;
    bool isUnleashPotential = false;

    ProfileDetails profileData = new ProfileDetails();

    private void Awake() 
    {
        if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
        }

        auth_key = "Bearer bBb-TBDt6rzkIddwUdEer-CMfJbncvSr";  
        UpdateDeviceBasedUI();

        GetUserProfile();
    }

    public void UpdateDeviceBasedUI()
    {
        if (Screen.width == 750)
        {
            clampValue = 85.0f;
        }
        else if (Screen.width == 828 || Screen.width == 768)
        {
            clampValue = 95.0f;
        }
    }

    private void Reset(Button levelButton)
    {
        if (maskTransform == null)
        {
            var mask = GetComponentInChildren<Mask>(true);
            if (mask)
            {
                maskTransform = mask.rectTransform;
            }
            if (maskTransform == null)
            {
                var mask2D = GetComponentInChildren<RectMask2D>(true);
                if (mask2D)
                {
                    maskTransform = mask2D.rectTransform;
                }
            }
        }
    }

    void Start()
    {
        leftSide = Camera.main.ViewportToWorldPoint(new Vector3(0,0,0)).x;
        rightSide = Camera.main.ViewportToWorldPoint(new Vector3(1,1,1)).x;
        // cam = GetComponent<Camera>();
        cam = Camera.main;
        var dateAndTime = DateTime.Now;
        var date = dateAndTime.Date;
        Debug.Log(date);
        Debug.Log(System.DateTime.Now); // Format - 07/29/2022 08:33:35
        SpawnPath();


        if (PlayerPrefs.HasKey("NextLevelWillBe"))
        {
            string nextLevelWillbe = PlayerPrefs.GetString("NextLevelWillBe");
            Debug.Log(nextLevelWillbe);
             Debug.Log("Checking for TimeSpan");
            int levelNumber = int.Parse(nextLevelWillbe);
            int totalLevelsPassed = PlayerPrefs.GetInt("totalLevelsPassed");
            Debug.Log("total levels passed is " + totalLevelsPassed);
            int level = levelNumber - 1;

            if (level < totalLevelsPassed)
            {
                for (int z = 1; z <= totalLevelsPassed; z++)
                {
                    Button button = GameObject.Find(z.ToString()).GetComponent<Button>();
                    GameObject lockImage = button.transform.GetChild(1).gameObject;
                    lockImage.SetActive(false);
                    button.tag = "Unlocked";
                }
            }
            else
            {
                for (int z = 1; z <= level; z++)
                {
                    Button button = GameObject.Find(z.ToString()).GetComponent<Button>();
                    GameObject lockImage = button.transform.GetChild(1).gameObject;
                    lockImage.SetActive(false);
                    button.tag = "Unlocked";
                }
            }
             
            
           
            
            
            Debug.Log("level number is " + level);

            missionNumber = level.ToString();

            if (level != 0)
            {
                lastReachedLevel = GameObject.Find(missionNumber).GetComponent<Button>();
                
            }
           else
           {
                lastReachedLevel = GameObject.Find("1").GetComponent<Button>();
              
           }

               RectTransform thistarget = lastReachedLevel.GetComponent<RectTransform>();
                Reset(lastReachedLevel);
                screenPos = cam.WorldToScreenPoint(thistarget.position);  // 
                Debug.Log("target is " + screenPos.x + " pixels from the left" + screenPos.y); //target is -24996 pixels from the left-552576.1

                Debug.Log("Position is " + thistarget.position); // Position is (-133.00, -2883.00, 0.00)
                GetNormalizePosition(thistarget);

            DateTime thisTime = System.DateTime.Now.Date;
            Debug.Log("this date is " + thisTime);  //09-08-2022 13:34:10
            lastTimeClicked = DateTime.Parse(PlayerPrefs.GetString("completionDateTime"));
            Debug.Log(lastTimeClicked); //08/09/2022 13:33:38

            TimeSpan difference = thisTime.Subtract(lastTimeClicked);
            Debug.Log("difference is " + difference); // 00:00:32.3895120
            // total hour difference is coming on same day is 0 & next day as 1 - reset numberOFLevelsPerday
            Debug.Log("numberOfLevelsPerDay is " + numberOfLevelsPerDay); // 00:00:32.3895120
 
            numberOfLevelsPerDay = PlayerPrefs.GetInt("numberOfLevelsPerDay");
            Debug.Log("numberOfLevelsPerDay is " + numberOfLevelsPerDay); // 00:00:32.3895120

            if (profileData.is_trial_subscription == false)
            {
                if (difference.TotalHours == 0)
                {
                    
                    if (numberOfLevelsPerDay < 2) // changing 2 to 15
                    {
                        // unlock
                        //show unlock animation and move character to that level
                        Debug.Log("unlock next level");
                        Button button = GameObject.Find(nextLevelWillbe).GetComponent<Button>();
                        GameObject lockImage = button.transform.GetChild(1).gameObject;
                        // lockImage.SetActive(false);
                        button.tag = "Unlocked";
                        animator = lockImage.GetComponent<Animator>();
                        // animator.Play("LockUnlock");
                        characterAnim = astronaut.GetComponent<Animator>();
                        // characterAnim.Play("AstroMoving");
                        islevelUnlocked = true;

                        targetPosition = lockImage.transform.position;
                        // screenPos = cam.ScreenToWorldPoint(targetPosition);

                        newPosition = new Vector2(targetPosition.x + 228f, targetPosition.y);
                        //changing astronaut position in Update function
                    }
                    else if (numberOfLevelsPerDay >= 2 )
                    {
                    
                    Debug.Log("You have unleashed your true potential");
                    isUnleashPotential = true;
                    
                        // //     //save datetime here and check for next level date time, when entered next date set  quota to 0
                        // InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
                        // popup.Init(UIController.Instance.MainCanvas,
                        // "Well done!You have unleashed your true potential. Meet us tomorrow to unlock the next mission!",
                        // "Ok"
                        // );
                    }
                }
                else if (difference.TotalHours == 1)
                {
                    numberOfLevelsPerDay = 0;
                    PlayerPrefs.SetInt("numberOfLevelsPerDay", numberOfLevelsPerDay);
                    Debug.Log("unlock next level");
                        Button button = GameObject.Find(nextLevelWillbe).GetComponent<Button>();
                        GameObject lockImage = button.transform.GetChild(1).gameObject;
                        // lockImage.SetActive(false);
                        button.tag = "Unlocked";
                        animator = lockImage.GetComponent<Animator>();
                        // animator.Play("LockUnlock");
                        characterAnim = astronaut.GetComponent<Animator>();
                        // characterAnim.Play("AstroMoving");
                        islevelUnlocked = true;

                        targetPosition = lockImage.transform.position;
                        // screenPos = cam.ScreenToWorldPoint(targetPosition);

                        newPosition = new Vector2(targetPosition.x + 228f, targetPosition.y);

                }   
            }
            else 
            {
                if (level < 6)
                {
                    PlayerPrefs.SetInt("numberOfLevelsPerDay", numberOfLevelsPerDay);
                    Debug.Log("unlock next level");
                        Button button = GameObject.Find(nextLevelWillbe).GetComponent<Button>();
                        GameObject lockImage = button.transform.GetChild(1).gameObject;
                        // lockImage.SetActive(false);
                        button.tag = "Unlocked";
                        animator = lockImage.GetComponent<Animator>();
                        // animator.Play("LockUnlock");
                        characterAnim = astronaut.GetComponent<Animator>();
                        // characterAnim.Play("AstroMoving");
                        islevelUnlocked = true;

                        targetPosition = lockImage.transform.position;
                        // screenPos = cam.ScreenToWorldPoint(targetPosition);

                        newPosition = new Vector2(targetPosition.x + 228f, targetPosition.y);

                }
                else
                {
                    Popup popup = UIController.Instance.CreatePopup();
                        popup.Init(UIController.Instance.MainCanvas,
                        "You have completed your trial phase. It’s time to choose your level and subscribe!",
                        "Cancel",
                        "Subscribe Now",
                        GoSubscribe
                        );
                }
            }
            

                
            // }
        }
       
        //scroll to specific level - unlock next level, check for time period
        //calculate time difference between previousLevel and in next level 
    }

    void Update() {
        if (islevelUnlocked)
        {
        
            characterAnim.Play("AstroMoving");
            Vector2 desiredPos = newPosition;

            float height = Camera.main.orthographicSize * 2;
            float width = height * Screen.width / Screen.height;
            Bounds bounds = new Bounds (Vector3.zero, new Vector3(width, height, 0));

            Vector2 clampedPosition = new Vector2(
                Mathf.Clamp(desiredPos.x, 0, Screen.width - clampValue),
                Mathf.Clamp(desiredPos.y, 0, Screen.height) );
            
            Vector2 smoothPos = Vector2.Lerp(astronaut.transform.position, clampedPosition, Time.deltaTime);
            astronaut.transform.position = smoothPos;
            
            // astronaut.transform.position = Vector2.Lerp(astronaut.transform.position, newPosition, Time.deltaTime);
            animator.Play("LockUnlock");
        //     islevelUnlocked = false;
        }

        if (isUnleashPotential)
        {
            InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
            popup.Init(UIController.Instance.MainCanvas,
            "Well done!You have unleashed your true potential. Meet us tomorrow to unlock the next mission!",
            "Okay"
            );
            isUnleashPotential = false;
        }

    }

    public void GetNormalizePosition(RectTransform target)
    {
        int missionNumber = int.Parse(target.transform.gameObject.name);
        missionNumber = missionNumber - 1;

        float normalizePosition = (float)missionNumber/ 180;
        scrollRect.verticalNormalizedPosition = normalizePosition;//1-normalizePosition;
    }

    public void SpawnPath()
    {
        Vector2 currentPosition = pathPrefab.transform.position;
        // float height = pathPrefab.transform.localScale.height;
        // Debug.Log(height);
         RectTransform rt = pathPrefab.GetComponent<Image>().rectTransform;
        float height = rt.rect.height;
        Debug.Log("height of rect is " + height);
        Debug.Log("local scale is" + pathPrefab.transform.localScale.y);
        int z = 0;


        for(int i = 0; i < 36; i++)
        {
            GameObject nextPath = Instantiate(pathPrefab).gameObject;
            nextPath.transform.position = new Vector2(currentPosition.x, -(currentPosition.y + height));//pathPrefab.transform.localScale.y));//+ 1939f));
            VerticalLayoutGroup pathVlg = contentParent.GetComponent<VerticalLayoutGroup>();

            if (Screen.width >= 1440)
            {
                pathVlg.spacing = -500;
            }
            else if (Screen.width >= 1284)
            {
                pathVlg.spacing = -400;
            }
             else if (Screen.width >= 1242)
            {
                pathVlg.spacing = -300;
            }
            else if (Screen.width >= 1170)
            {
                pathVlg.spacing = -150;
            }
            else if (Screen.width >= 1125)
            {
                pathVlg.spacing = -50;
            }
            else if (Screen.width >= 1080)
            {
                pathVlg.spacing = 0;
                pathVlg.padding.bottom = 500;
            }
            else if (Screen.width >= 828)
            {
                pathVlg.spacing = 600;
                pathVlg.padding.bottom = 900;
            }
            else if (Screen.width >= 720)
            {
                pathVlg.spacing = 800;
                pathVlg.padding.bottom = 900;
            }
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

        if (EventSystem.current.currentSelectedGameObject.tag == "Unlocked")
        {
            if (numberOfLevelsPerDay < 2 && profileData.is_trial_subscription == false)
            {
                numberOfLevelsPerDay = numberOfLevelsPerDay + 1;
                PlayerPrefs.SetInt("numberOfLevelsPerDay", numberOfLevelsPerDay);
                PlayerPrefs.SetInt("isReattempting", 0);
                SaveDataForPreviousLevel(); 
                GetAllDetails();
            }
            else if (profileData.is_trial_subscription == true && int.Parse(levelId) < 6)
            {
                SaveDataForPreviousLevel(); 
                GetAllDetails();
            }
            else
            {
                int totalLevelsPassed = PlayerPrefs.GetInt("totalLevelsPassed");

                if (int.Parse(levelId) <= totalLevelsPassed)
                {
                    PlayerPrefs.SetInt("isReattempting", 1);
                }
                
                GetAllDetails();
            }
        }
        else
        {
            Debug.Log("Can't unlock this mission yet");
            InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
            popup.Init(UIController.Instance.MainCanvas,
            "Can't unlock this mission yet",
            "Okay"
            );

        }
    
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
        string uri = baseURL + "day-levels/" + levelId + "?expand=newWords,revisionWords,newWords.nouns,newWords.nouns.nounSentences,newWords.verbs,newWords.verbs.verbSentences,newWords.adverbs,newWords.adverbs.adverbSentences,newWords.adjectives,newWords.adjectives.adjectiveSentences,newWords.dailyUseTips,newWords.otherWayUsingWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords.otherWayUsingWordSentences,newWords.idioms,newWords.idioms.idiomSentences,newWords.useMultipleWords,newWords.useMultipleWords.useMultipleWordSentences,newWords.synonyms,newWords.synonyms.synonymSentences,newWords.antonyms,newWords.antonyms.antonymSentences,revisionWords.nouns,revisionWords.nouns.nounSentences,revisionWords.verbs,revisionWords.verbs.verbSentences,revisionWords.adverbs,revisionWords.adverbs.adverbSentences,revisionWords.adjectives,revisionWords.adjectives.adjectiveSentences,revisionWords.dailyUseTips,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords.otherWayUsingWordSentences,revisionWords.idioms,revisionWords.idioms.idiomSentences,revisionWords.useMultipleWords,revisionWords.useMultipleWords.useMultipleWordSentences,revisionWords.synonyms,revisionWords.synonyms.synonymSentences,revisionWords.antonyms,revisionWords.antonyms.antonymSentences,questions,questions.questionOptions,conversation,conversationQuestions,conversationQuestions.questionOptions,passages,passages.questions,passages.questions.questionOptions,revisionConversation";

        // "?expand=newWords,revisionWords,newWords.nouns,newWords.nouns.nounSentences,newWords.verbs,newWords.verbs.verbSentences,newWords.adverbs,newWords.adverbs.adverbSentences,newWords.adjectives,newWords.adjectives.adjectiveSentences,newWords.dailyUseTips,newWords.otherWayUsingWords,newWords.otherWayUsingWords,newWords.otherWayUsingWords.otherWayUsingWordSentences,newWords.idioms,newWords.idioms.idiomSentences,newWords.useMultipleWords,newWords.useMultipleWords.useMultipleWordSentences,newWords.synonyms,newWords.synonyms.synonymSentences,newWords.antonyms,newWords.antonyms.antonymSentences,revisionWords.nouns,revisionWords.nouns.nounSentences,revisionWords.verbs,revisionWords.verbs.verbSentences,revisionWords.adverbs,revisionWords.adverbs.adverbSentences,revisionWords.adjectives,revisionWords.adjectives.adjectiveSentences,revisionWords.dailyUseTips,revisionWords.otherWayUsingWords,revisionWords.otherWayUsingWords.otherWayUsingWordSentences,revisionWords.idioms,revisionWords.idioms.idiomSentences,revisionWords.useMultipleWords,revisionWords.useMultipleWords.useMultipleWordSentences,revisionWords.synonyms,revisionWords.synonyms.synonymSentences,revisionWords.antonyms,revisionWords.antonyms.antonymSentences,questions,questions.questionOptions,conversation,conversationQuestions,conversationQuestions.questionOptions,passages,passages.questions,passages.questions.questionOptions";
        Debug.Log(uri);
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
            string jsonString = request.downloadHandler.text;
           
            allDetailData = JsonUtility.FromJson<AllDetail>(jsonString);
            Debug.Log(allDetailData.id);        //4
            Debug.Log(allDetailData.type);      //1

           

            PlayerPrefs.SetInt("StartLevelID", allDetailData.id);
            PlayerPrefs.SetInt("LevelId",int.Parse(levelId));
            
            SceneManager.LoadScene("NewWordDay");
            
                  
        }
        request.Dispose();
    }

    void GetUserProfile() => StartCoroutine(GetUserProfile_Coroutine());

    // called this API for dev purpose only - may be remove it later
    IEnumerator GetUserProfile_Coroutine()
    {
        string uri = baseURL + "students/view";

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

            string responseJson = request.downloadHandler.text;
            profileData = JsonUtility.FromJson<ProfileDetails>(responseJson);   
            isTrial = profileData.is_trial_subscription;         

            if (profileData.available_level == 0)
            {
                Popup popup = UIController.Instance.CreatePopup();
                        popup.Init(UIController.Instance.MainCanvas,
                        "You are not authorized to this level unless subscribed for. Please subscribe",
                        "Cancel",
                        "Subscribe Now",
                        GoSubscribe
                        );
            }
            else
            {
                totalNumberOfLevels = profileData.available_level;
                levelsPassed = profileData.total_passed_level;
                
                if (profileData.is_trial_subscription == true)
                {
                    if (int.Parse(profileData.subscription_remaining_day) > 0)
                    {
                        string displayMessageForTrial = "You are left with " + profileData.subscription_remaining_day + " missions of this level"; // missions in thia leve

                        InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
                        popup.Init(UIController.Instance.MainCanvas,
                        displayMessageForTrial,
                        "Okay"
                        );
                    }
                    else
                    {
                        // if (chances == 0)
                        Popup popup = UIController.Instance.CreatePopup();
                        popup.Init(UIController.Instance.MainCanvas,
                        "You have completed your trial phase. It’s time to choose your level and subscribe!",
                        "Cancel",
                        "Subscribe Now",
                        GoSubscribe
                        );
                    }
                    
                }

            }
            
            NotifyAboutSubscriptionStatus();
        }
        request.Dispose();
    }

    void GoSubscribe()
    {
        SceneManager.LoadScene("IAPCatalog");
    }

    void NotifyAboutSubscriptionStatus()
    {
        int sevendsDaysBefore = totalNumberOfLevels - 7;
        int dayBefore = totalNumberOfLevels - 1;

        if (levelsPassed == 0)
        {
            InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
            popup.Init(UIController.Instance.MainCanvas,
            "Welcome aboard, astronaut! Your space mission is about to begin.",
            "Ready to take off?"
            );
        }
        else  if (levelsPassed == totalNumberOfLevels) // for free trial
        {
            Debug.Log("Your subscription is over, today is the last day"); //pop up - start with level , you haven't subscribed, complete previous level 
            // check for subscription period - 30, 90, 180 may change adding free trial
            InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
                popup.Init(UIController.Instance.MainCanvas,
                "Your subscription is over, today is the last day",
                "Ok"
                );
            
        } 
        else if (levelsPassed == dayBefore)
        {
            InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
                popup.Init(UIController.Instance.MainCanvas,
                "Your subscription is getting over in a day",
                "Ok"
                );
        }
        else if (levelsPassed == sevendsDaysBefore)
        {
            InteractivePopUp popup = UIController.Instance.CreateInteractivePopup();
                popup.Init(UIController.Instance.MainCanvas,
                "Your subscription is getting over in coming 7 days",
                "Ok"
                );
        }
    }
   
}
