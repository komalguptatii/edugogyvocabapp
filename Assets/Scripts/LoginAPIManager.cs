using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using TMPro;
// using UnityEditor;
// using JsonUtility;

public class LoginAPIManager : MonoBehaviour
{
    public TMP_Dropdown countryCodeDropdown;
    

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

         CountryCodeList listObject = new CountryCodeList();

    public int selectedCountryCode;
    string fixJson(string value)            // Added object type to JSON
    {
        value = "{\"items\":" + value + "}";
        return value;
    }


    IEnumerator GetCountryCodeListData_Coroutine()
    {
        // outputArea.text = "Loading...";
        List<string> m_DropOptions = new List<string>();


        string uri = "http://165.22.219.198/edugogy/api/v1/country-codes?per-page=0";
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
                Debug.Log(listObject.items.Length);
                Debug.Log(listObject.items[i]);

                Debug.Log("country code is " + listObject.items[i].code);

                Debug.Log("country id is " + listObject.items[i].id);
                //  var dropdownOptions = listObject.items.Select(i => new Dropdown.OptionData(i.id)).ToList();
                //  Debug.Log(dropdownOptions);
                // m_DropOptions.Add((listObject.items[i].id).ToString());
                // Debug.Log(m_DropOptions);
            countryCodeDropdown.options.Add (new TMP_Dropdown.OptionData() {text = listObject.items[i].dial_code});
            }
        }
    }

    public void dropDownItemSelected(){
        int index = countryCodeDropdown.value;
        selectedCountryCode = listObject.items[index].id;
        Debug.Log(selectedCountryCode);
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

    public string countryListjson;


    void Start()
    {
        // countryCodeData = new CountryCodeData();
        GetCountryCodeListData();

        // resendOTPRequest();
    }

    void GetCountryCodeListData() => StartCoroutine(GetCountryCodeListData_Coroutine());

    public void sendLoginRequest() => StartCoroutine(ProcessLoginRequest_Coroutine());

    public void validateOTPRequest() => StartCoroutine(ProcessValidateOTPRequest_Coroutine());

    public void resendOTPRequest() => StartCoroutine(ProcessResendMobileOTPRequest_Coroutine());




    IEnumerator ProcessLoginRequest_Coroutine()  // Actually this is API to sign up
    {
        // "9855940600", 88 - country code id not code
        LoginForm loginFormData = new LoginForm { phone = "9855940600", country_code_id = selectedCountryCode };
        string json = JsonUtility.ToJson(loginFormData);

        Debug.Log(json);


        string uri = "http://165.22.219.198/edugogy/api/v1/students";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.downloadHandler.text);


            // "phone": "9855940600",
            // "country_code_id": 88,
            // "id": 3,
            // "is_existing_user": 0

        }

    }

    IEnumerator ProcessValidateOTPRequest_Coroutine()  //validate otp
    {

        ValidateOTPForm validateOTPFormData = new ValidateOTPForm { phone = "9855940600", country_code_id = 88, otp = 6875 };
        string json = JsonUtility.ToJson(validateOTPFormData);

        Debug.Log(json);


        string uri = "http://165.22.219.198/edugogy/api/v1/students/validate-otp";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);
            // { "auth_key":"3VcmTskZ5jRINDiaO_489b0pdVsbTEy6"}
        }

    }

    IEnumerator ProcessResendMobileOTPRequest_Coroutine()  //Resend validate otp, also used for login
    {

        ValidateOTPForm validateOTPFormData = new ValidateOTPForm { phone = "9855940600", country_code_id = 88 };
        string json = JsonUtility.ToJson(validateOTPFormData);

        Debug.Log(json);


        string uri = "http://165.22.219.198/edugogy/api/v1/students/resend-otp";

        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        var request = new UnityWebRequest(uri, "POST");

        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Error: " + request.error);
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);


        }

    }


}
