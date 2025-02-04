using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class LoginAPIManager : MonoBehaviour
{
    public TMP_Dropdown countryCodeDropdown;
    
    public TMP_InputField phoneNumberInput;
    public TMP_InputField countryCodeInput;


    public GameObject fbBtn;
    public GameObject appleBtn;
    public GameObject googleBtn; 

    public int selectedCountryCode;

    [Serializable]
    public class CountryCodeData
    {
        public int id ;
        public string code;
        public string name;

        public string dial_code;
    }

    [Serializable]
    public class CountryCodeList {
 
         public CountryCodeData[] items;
    }
    
    [Serializable]
    public class LoginForm
    {
        public string phone;
        public int country_code_id;
    }

    [Serializable]
    public class ValidateOTPForm
    {
        public string phone;
        public int country_code_id;
        public int otp;
    }

    [Serializable]
    public class SignedUpUser
    {
        public string phone;
        public int id;
        public int country_code_id;
        public int is_existing_user;
       
    }

    [Serializable]
    public class Student
    {
        public int id;
        public string name;
        public string phone;
        public int age_group_id;
        public int country_code_id;
    }

    [Serializable]
    public class ResendOTP
    {
        public string phone;
        public int country_code_id;
        public int student_id;
        public Student student;
    }

    CountryCodeList listObject = new CountryCodeList();

     public string countryListjson;
      private Animator loadingIndicator;
    public GameObject Indicator;
    // public Canvas LoginCanvas;
    

      string baseURL = "https://api.edugogy.app/v1/";
    // string baseURL = "https://api.testing.edugogy.app/v1/";

    string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";

    string fixJson(string value)            // Added object type to JSON
    {
        value = "{\"items\":" + value + "}";
        return value;
    }


    void Awake()
    {
          loadingIndicator = Indicator.GetComponent<Animator>(); 
         loadingIndicator.enabled = false;
        Indicator.SetActive(false);

        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("will hide apple sign in button here later");
            appleBtn.SetActive(false);
            ChangeButtonPosition();
            // fbBtn.SetActive(false);
            // googleBtn.SetActive(false);
            
        }
        //change position of facebook and google button after hiding apple button
        // appleBtn.SetActive(false);
        // ChangeButtonPosition();
    }

    void Start()
    {
        //
        GetCountryCodeListData();
    }

    IEnumerator GetCountryCodeListData_Coroutine()
    {
        // outputArea.text = "Loading...";
        List<string> m_DropOptions = new List<string>();


        string uri = baseURL + "country-codes?per-page=0";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            
            // EditorUtility.DisplayDialog("Country List loaded", "Ok", "Cancel");
            //Put list of country codes in dropdown
            countryListjson = request.downloadHandler.text;
            string jsonString = fixJson(countryListjson);
            Debug.Log(jsonString);
            
            listObject = JsonUtility.FromJson<CountryCodeList>(jsonString);

            for (var i = 0; i < listObject.items.Length; i++) {
    
                countryCodeDropdown.options.Add (new TMP_Dropdown.OptionData() {text = listObject.items[i].dial_code});
            }
        }
    }

    public void dropDownItemSelected(){
        // countryCodeDropdown.Show();

        //  int index = countryCodeDropdown.value;
        // selectedCountryCode = listObject.items[index].id;
        // Debug.Log(selectedCountryCode);

        int index = countryCodeDropdown.value;
        var selectedValue = countryCodeDropdown.options[countryCodeDropdown.value].text;
        Debug.Log(countryCodeDropdown.options[countryCodeDropdown.value].text);
        
        // selectedCountryCode = listObject.items[index].id;

        for(int x = 0; x < listObject.items.Length;x++)
        {
               if (selectedValue == listObject.items[x].dial_code)
               {
                    selectedCountryCode = listObject.items[x].id;
                    countryCodeInput.text = selectedValue.ToString();
                    return;
               }

        }
        

        Debug.Log("selectedCountryCode is" + selectedCountryCode);
    }

    public void OnValueChanged(string code)
    {
        Debug.Log("value of country code is " + countryCodeInput.text);
       
        // Debug.Log("code is empty now");
        // countryCodeDropdown.ClearOptions();

        // for (var i = 0; i < listObject.items.Length; i++) {
        //     countryCodeDropdown.options.Add (new TMP_Dropdown.OptionData() {text = listObject.items[i].dial_code});
        // }

        
        // countryCodeDropdown.options = countryCodeDropdown.options.FindAll( option => option.text.IndexOf( code ) >= 0 );
       
        // countryCodeDropdown.Show();
     
         
    }

    void SearchCountryCodeId(string code)
    {
        Debug.Log("value of code is " + code);
        code = "+" + code;
        for(int x = 0; x < listObject.items.Length;x++)
        {
               if (code == listObject.items[x].dial_code)
               {
                    selectedCountryCode = listObject.items[x].id;
                    Debug.Log("code Id is " + selectedCountryCode);

                    // countryCodeInput.text = selectedValue.ToString();
                    
               }

        }
    }

    private void LateUpdate()
    {
        countryCodeInput.MoveTextEnd(true);
    }

     

    private void ChangeButtonPosition()
    {
        Vector3 fbPos = fbBtn.transform.position;
        fbPos.x += 100f;
        fbBtn.transform.position = fbPos;

        Vector3 googlePos = googleBtn.transform.position;
        googlePos.x -= 100f;
        googleBtn.transform.position = googlePos;
    }

    void GetCountryCodeListData() => StartCoroutine(GetCountryCodeListData_Coroutine());

    public void sendLoginRequest() => StartCoroutine(ProcessLoginRequest_Coroutine());


    public void resendOTPRequest() => StartCoroutine(ProcessResendMobileOTPRequest_Coroutine());


    public bool ValidateLoginData()
    {
       
        SearchCountryCodeId(countryCodeInput.text);
             Debug.Log(selectedCountryCode);
        if (countryCodeInput.text == "")//(selectedCountryCode == 0)
        {
             Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Please select country code",
                    "Cancel",
                    "Sure!",
                    resetAction
                    );
                return false;
        }
        else if (phoneNumberInput.text == "")
        {
             Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Please enter mobile number",
                    "Cancel",
                    "Sure!",
                    resetAction
                    );
                return false;
        }
        else if (!Regex.Match(phoneNumberInput.text, "^[0-9]{10}$").Success)
        {
             Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Mobile number should be of 10 digits",
                    "Cancel",
                    "Sure!",
                    resetAction
                    );
                    return false;
        }
        else
        {
            return true;
        }
    }

    public void resetAction()
    {
        Debug.Log("checking for valid data");
    }

    IEnumerator ProcessLoginRequest_Coroutine()  // Actually this is API to sign up
    {
        loadingIndicator.enabled = true;
        Indicator.SetActive(true);
        // LoginCanvas.interactable = false;

        SignedUpUser userSignUp = new SignedUpUser();
        //Validation for Login via mobile number
        bool isDataValid = ValidateLoginData();
        if (isDataValid == true)
        {
            // "9855940600", 88 - country code id not code
        LoginForm loginFormData = new LoginForm { phone = phoneNumberInput.text, country_code_id = selectedCountryCode };
        string json = JsonUtility.ToJson(loginFormData);

        Debug.Log(json);
        string uri = baseURL + "students";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.error != null)
        {
             loadingIndicator.enabled = false;
            Indicator.SetActive(false);

            Debug.Log("Error: " + request.error);
            if (request.responseCode == 422)
            {
             Debug.Log(request.downloadHandler.text);
            
// {"error":[{"code":0,"source":"phone","title":"Phone \"9855940600\" has already been taken.","detail":"Phone \"9855940600\" has already been taken."}]}
                resendOTPRequest();
            }
            else if (request.responseCode == 404)
            {
                 Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Your Edugogy registration is still pending! Please make sure that you type the correct country code and don’t have DND on your messages",
                    "Cancel",
                    "Sure!",
                    resetAction
                    );
            }
        }
        else
        {
            Debug.Log("Status Code: " + request.responseCode);
            
            Debug.Log(request.downloadHandler.text);
            
            string userJson = request.downloadHandler.text;
            string jsonString = fixJson(userJson);
            Debug.Log(jsonString);
            userSignUp = JsonUtility.FromJson<SignedUpUser>(jsonString);

            Debug.Log(userSignUp);
            
            // if (userSignUp.is_existing_user == 0)
            // {
            //     resendOTPRequest();
            // }

     
            SavePhoneNumber(userSignUp.phone);
            // MoveToVerifyOTP();

            // MoveToVerifyOTP();
            // "phone": "9855940600",
            // "country_code_id": 88,
            // "id": 3,
            // "is_existing_user": 0
            // { "auth_key":"3VcmTskZ5jRINDiaO_489b0pdVsbTEy6"}

        }
        }
        else
        {
            loadingIndicator.enabled = false;
        Indicator.SetActive(false);
        }
    }

   

    void MoveToVerifyOTP()
    {
        SceneManager.LoadScene("VerifyOTP");
    }

    IEnumerator ProcessResendMobileOTPRequest_Coroutine()  //Resend validate otp, also used for login
    {


        ResendOTP resendOTP = new ResendOTP();
        ValidateOTPForm validateOTPFormData = new ValidateOTPForm { phone = phoneNumberInput.text, country_code_id = selectedCountryCode };
        string json = JsonUtility.ToJson(validateOTPFormData);

        Debug.Log(json);

        string uri = baseURL + "students/resend-otp";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);

            if (request.responseCode == 404)
            {
                 Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Your Edugogy registration is still pending! Please make sure that you type the correct country code and don’t have DND on your messages",
                    "Cancel",
                    "Sure!",
                    resetAction
                    );
            }
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

            string resendOTPJson = request.downloadHandler.text;
           
            resendOTP = JsonUtility.FromJson<ResendOTP>(resendOTPJson);

            Debug.Log(resendOTP.phone);
            Debug.Log(resendOTP.student.name);
            SavePhoneNumber(resendOTP.phone);
            
            //save phone number

        }

    }


 void SavePhoneNumber(string phone){
        PlayerPrefs.SetString("phone", phoneNumberInput.text);
        PlayerPrefs.SetInt("countryCodeId", selectedCountryCode);
        MoveToVerifyOTP();
    }

     
}
