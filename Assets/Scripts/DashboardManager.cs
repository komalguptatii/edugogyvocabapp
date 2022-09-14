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
     Vector3 worldPosition;
     Vector2 newPosition;
    public ScrollRect scrollRect;
    public RectTransform scrollRectTransform;
    public RectTransform contentPanel;
    Button lastReachedLevel;
    RectTransform maskTransform;
    float offset = -1500.0f;
    string missionNumber;

    string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    // string baseURL = "http://165.22.219.198/edugogy/api/v1/";

    private void Awake() 
    {
        if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            Debug.Log(auth_key);
        }

        // auth_key = "Bearer usFEr6V4JK0P4OUz_eoZVvYMrzIRxATo";  // Ridhima - Mehak Key
        // auth_key = "Bearer pkCZmdJCpkHdH6QYT2G2q_qeFxzJtvj3";
        // auth_key = "Bearer qJkO9zzHU5z3w2gcYTln1YQhONkTMFKU";

        GetUserProfile();
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
        // cam = GetComponent<Camera>();
        cam = Camera.main;
        var dateAndTime = DateTime.Now;
        var date = dateAndTime.Date;
        Debug.Log(date);
        Debug.Log(System.DateTime.Now); // Format - 07/29/2022 08:33:35
        SpawnPath();

        
        // ScrollToCenter(scrollRect, target);

        if (PlayerPrefs.HasKey("NextLevelWillBe"))
        {
            string nextLevelWillbe = PlayerPrefs.GetString("NextLevelWillBe");
            Debug.Log(nextLevelWillbe);
             Debug.Log("Checking for TimeSpan");
            int levelNumber = int.Parse(nextLevelWillbe);
            int level = levelNumber - 1;
            Debug.Log("level number is " + level);

            missionNumber = level.ToString();

            lastReachedLevel = GameObject.Find(missionNumber).GetComponent<Button>();
            RectTransform thistarget = lastReachedLevel.GetComponent<RectTransform>();
            Reset(lastReachedLevel);
            screenPos = cam.WorldToScreenPoint(thistarget.position);  // 
            Debug.Log("target is " + screenPos.x + " pixels from the left" + screenPos.y); //target is -24996 pixels from the left-552576.1

            Debug.Log("Position is " + thistarget.position); // Position is (-133.00, -2883.00, 0.00)
            SnapTo(thistarget);

        
            DateTime thisTime = System.DateTime.Now.Date;
            Debug.Log("this date is " + thisTime);  //09-08-2022 13:34:10
            lastTimeClicked = DateTime.Parse(PlayerPrefs.GetString("completionDateTime"));
            Debug.Log(lastTimeClicked); //08/09/2022 13:33:38

            TimeSpan difference = thisTime.Subtract(lastTimeClicked);
            // Debug.Log("difference is " + difference); // 00:00:32.3895120

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
        if (islevelUnlocked)
        {
        
            characterAnim.Play("AstroMoving");
                // astronaut.transform.position = Vector3.MoveTowards(astronaut.transform.position, newPosition, Time.deltaTime * speed );// 

                // astronaut.transform.position = Vector3.MoveTowards(astronaut.transform.position, -screenPos, Time.deltaTime * speed );// 
           
            // Vector2 newPosition = new Vector2(astronaut.transform.position.x + 100f, astronaut.transform.position.y + 400f);
            // astronaut.transform.position = Vector3.MoveTowards(astronaut.transform.position, newPosition, Time.deltaTime * speed );// 
            //  astronaut.transform.position = Vector2.Lerp(astronaut.transform.position, newPosition, Time.deltaTime);
            islevelUnlocked = false;
        }

    }
    
    public void SnapTo(RectTransform target)
    {
        var itemCenterPositionInScroll = GetWorldPointInWidget(scrollRectTransform, GetWidgetWorldPoint(target));

        var targetPositionInScroll = GetWorldPointInWidget(scrollRectTransform, GetWidgetWorldPoint(maskTransform));

        Debug.Log("itemCenterPositionInScroll " + itemCenterPositionInScroll); // -736
        Debug.Log("targetPositionInScroll " + targetPositionInScroll); // -1266

        Vector2 contentPanelPosition = new Vector2(contentPanel.position.x, contentPanel.position.y);
        Debug.Log("contentPanelPosition " + contentPanelPosition);

        var difference = targetPositionInScroll + itemCenterPositionInScroll;
        Debug.Log("difference " + difference);

//  
        var value1 = Mathf.Abs(contentPanelPosition.y) - Mathf.Abs(itemCenterPositionInScroll.y);
        Debug.Log("value1 " + value1);

        var value2 = Mathf.Abs(targetPositionInScroll.y + value1);
        Debug.Log("value2 " + value2);

        var offsetResult = value2/screenPos.y;
        Debug.Log("offsetResult " + offsetResult);

        var value3 = Mathf.Abs(value1/(difference.y*10));
        Debug.Log("value3  " + value3);

        float newOffset = value3 - offsetResult;
        Debug.Log("newOffset " + newOffset);
//

        // Debug.Log("difference " +  difference);

         var normalizedDifference = new Vector2(
            difference.x / (contentPanel.rect.size.x - scrollRectTransform.rect.size.x),
            difference.y / (contentPanel.rect.size.y - scrollRectTransform.rect.size.y));
        Debug.Log("normalizedDifference" + normalizedDifference);

        var newNormalizedPosition = scrollRect.normalizedPosition - normalizedDifference;
        Debug.Log("newNormalizedPosition " + newNormalizedPosition);

        // var ratio = itemCenterPositionInScroll.y/contentPanelPosition.y;

        //  var newdifference = ratio - Mathf.Abs(newNormalizedPosition.y);
        // Debug.Log("difference " +  newdifference);

        float scrollValue = 1 - Mathf.Abs(itemCenterPositionInScroll.y)/scrollRectTransform.rect.height;
        Debug.Log("target.position.y " + itemCenterPositionInScroll.y);
        Debug.Log("scrollValue rect transform " + scrollRectTransform.rect.height);
         Debug.Log("scrollValue " + scrollValue);


        // Vector2 offset = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformPoint(targetPositionInScroll.position);
        // Vector2 anchor = contentPanel.anchoredPosition;
        // anchor.y = offset.y;
        // contentPanel.anchoredPosition = anchor;

        scrollRect.verticalScrollbar.value = scrollValue; // - ((value3 + newOffset)* 10);

        // if (int.Parse(missionNumber) > 25)
        // {
        //      scrollRect.verticalScrollbar.value = scrollValue + Mathf.Abs(newOffset);
        // }
        // else
        // {
        //     scrollRect.verticalScrollbar.value = scrollValue - newOffset;
        // }
         //scrollValue - 0.03f;

        // 0.04586824, 0.13151658 - 0.1014356= 0.08564

        // scrollRect.verticalScrollbar.value = (value3*10) - offsetResult; //newdifference - 0.02f;


        // var normalizedDifference = new Vector2(
        //     difference.x / (contentPanel.rect.size.x - scrollRectTransform.rect.size.x),
        //     difference.y / (contentPanel.rect.size.y - scrollRectTransform.rect.size.y));
        // Debug.Log("normalizedDifference" + normalizedDifference);

        // var newNormalizedPosition = scrollRect.normalizedPosition - normalizedDifference;
        // Debug.Log("newNormalizedPosition " + newNormalizedPosition);

        // var diff = contentPanel.rect.size.y - scrollRectTransform.rect.size.y;
        // Debug.Log("diff is " + diff);

        // var ratioWithDifference = Mathf.Abs(target.position.y/difference.y);
        // Debug.Log("ratioWithDifference " + ratioWithDifference);
        // var ratioWithDiff = Mathf.Abs(target.position.y/diff);
        // Debug.Log("ratioWithDiff " + ratioWithDiff);

        // var differenceOfRatios = ratioWithDifference - ratioWithDiff;
        //  Debug.Log("differenceOfRatios " + differenceOfRatios);
        
        // var valueRequiredToScroll = Mathf.Abs(ratioWithDifference - differenceOfRatios)/ratio;
        // Debug.Log("valueRequiredToScroll " + valueRequiredToScroll);

        // var valueRequiredToScroll = Mathf.Abs(diff/itemCenterPositionInScroll.y);
        // Debug.Log("valueRequiredToScroll " + valueRequiredToScroll);

        // scrollRect.verticalNormalizedPosition = valueRequiredToScroll - 0.0440511f;//Mathf.Abs(ratio/2);//valueRequiredToScroll - 0.0440511f;//newNormalizedPosition.y;
        // scrollRect.verticalScrollbar.value = newNormalizedPosition.y;

        // var contentPos = (Vector2)scrollRect.transform.InverseTransformPoint( scrollRect.content.position );
        // var childPos = (Vector2)scrollRect.transform.InverseTransformPoint( target.position );

        // // Vector2 newPosition = new Vector2(itemCenterPositionInScroll.position.x, itemCenterPositionInScroll.position.y);
        // Vector2 contentPanelPosition = new Vector2(contentPanel.position.x, contentPanel.position.y);
        // Debug.Log("contentPanelPosition " + contentPanelPosition);
        
        // contentPanel.anchoredPosition = new Vector2(contentPanel.localPosition.x, contentPos.y + childPos.y - 1000.0f); //contentPos + childPos;
            // (Vector2)scrollRect.transform.InverseTransformPoint(contentPos)
            // - (Vector2)scrollRect.transform.InverseTransformPoint(childPos);
       
    }


    private Vector3 GetWidgetWorldPoint(RectTransform target)
    {
        //pivot position + item size has to be included
        var pivotOffset = new Vector3(
            (0.5f - target.pivot.x) * target.rect.size.x,
            (0.5f - target.pivot.y) * target.rect.size.y,
            0f);
        var localPosition = target.localPosition + pivotOffset;
        return target.parent.TransformPoint(localPosition);
    }

    private Vector3 GetWorldPointInWidget(RectTransform target, Vector3 worldPoint)
    {
        return target.InverseTransformPoint(worldPoint);
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
    }

    void GetUserProfile() => StartCoroutine(GetUserProfile_Coroutine());

    // called this API for dev purpose only - may be remove it later
    IEnumerator GetUserProfile_Coroutine()
    {
        ProfileDetails profileData = new ProfileDetails();
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
