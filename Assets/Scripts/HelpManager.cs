using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening ;

public class HelpManager : MonoBehaviour
{
    [Serializable]
    public class HelpResponse
    {
        public int id;
        public string title;
        public string description;
        public int created_at;
        public int updated_at;
        public int created_by;
        public int updated_by;
    }

    [Serializable]
    public class FAQList {
 
         public HelpResponse[] items;
    }

    public GameObject collapsablePrefab;
    public Transform ScrollContentParent;
    public List<GameObject> collapsablePrefabArray = new List<GameObject>();

     string auth_key;

     string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";
     
    string fixJson(string value)            // Added object type to JSON
    {
        value = "{\"items\":" + value + "}";
        return value;
    }

    FAQList faqlistObject = new FAQList();

    // Start is called before the first frame update
    void Start()
    {
         if (PlayerPrefs.HasKey("auth_key"))
        {
            auth_key = PlayerPrefs.GetString("auth_key");
            
            Debug.Log(auth_key);

        }
        GetFAQs();
    }

    void GetFAQs() => StartCoroutine(GetFAQs_Coroutine());

     IEnumerator GetFAQs_Coroutine()
    {
        string uri = baseURL + "faqs?per-page=20";

        var request = new UnityWebRequest(uri, "GET");

        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", auth_key);
        // request.SetRequestHeader("Authorization", "Bearer a8HMPlzEWaj4uglc9xob-1WuI_smGj9t");


        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            string jsonString = request.downloadHandler.text;
            string faqjson = fixJson(jsonString);

            faqlistObject = JsonUtility.FromJson<FAQList>(faqjson);
            
            Vector2 prefabPosition = collapsablePrefab.transform.position;
            for (var i = 0; i < faqlistObject.items.Length; i++) { //faqlistObject.items.Length
                HelpResponse  faq = new HelpResponse();
                faq = faqlistObject.items[i];
                GameObject newPrefab = Instantiate(collapsablePrefab).gameObject;
                collapsablePrefabArray.Add(newPrefab);
                float yPosition = prefabPosition.y - (200f * i);
                if (i == 0)
                {
                    yPosition = -50f;
                }
                
                newPrefab.transform.position = new Vector2(prefabPosition.x, yPosition);

                newPrefab.transform.SetParent(ScrollContentParent, false);

                GameObject contentWindow = newPrefab.transform.GetChild(0).gameObject;
                GameObject header = contentWindow.transform.GetChild(0).gameObject;
                TMPro.TMP_Text question = header.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                question.text = faqlistObject.items[i].title;

                Button expandButton = header.transform.GetChild(1).GetComponent<Button>();
                Button collapseButton = header.transform.GetChild(2).GetComponent<Button>();
                expandButton.tag = i.ToString();
                collapseButton.tag = i.ToString();
                expandButton.onClick.AddListener(delegate{OnExpandButtonClick(expandButton);});
                collapseButton.onClick.AddListener(delegate{OnCollapseButtonClick(collapseButton);});


                GameObject body = contentWindow.transform.GetChild(1).gameObject;
                 TMPro.TMP_Text answer = body.transform.GetChild(0).GetComponent<TMPro.TMP_Text>();
                 answer.text = faqlistObject.items[i].description;

            }

        }

    }

    public void OnExpandButtonClick(Button button)
    {
        int tagValue = int.Parse(button.tag);
        for (int j = tagValue+1; j < collapsablePrefabArray.Count; j++ )
        {
            if (j > 0)
            {
                Debug.Log(j);
                Vector2 nextButtonPosition = collapsablePrefabArray[j].transform.position;
                Debug.Log(nextButtonPosition);
                Vector2 newPosition = new Vector2(nextButtonPosition.x, nextButtonPosition.y - (300f * collapsablePrefabArray.Count));
                Debug.Log(newPosition);  
                collapsablePrefabArray[j].transform.position = Vector2.Lerp(nextButtonPosition, newPosition, 0.2f);
            }

        }
    }

    public void OnCollapseButtonClick(Button button)
    {
        int tagValue = int.Parse(button.tag);
        for (int j = tagValue+1; j < collapsablePrefabArray.Count; j++ )
        {
            if (j != collapsablePrefabArray.Count && j != 0)
            {
                Debug.Log(j);
                Vector2 previousButtonPosition = collapsablePrefabArray[j].transform.position;
                Debug.Log(previousButtonPosition);
                Vector2 newPosition = new Vector2(previousButtonPosition.x, previousButtonPosition.y + (300f * collapsablePrefabArray.Count));
                Debug.Log(newPosition);  
                collapsablePrefabArray[j].transform.position = Vector2.Lerp(previousButtonPosition, newPosition, 0.2f);
            }

        }
    }
    
}
