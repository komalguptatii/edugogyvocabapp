using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
// using JsonUtility;

public class LoginAPIManager : MonoBehaviour
{

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

    void Start()
    {
        GetCountryCodeListData();
        // resendOTPRequest();
    }

    void GetCountryCodeListData() => StartCoroutine(GetCountryCodeListData_Coroutine());

    public void sendLoginRequest() => StartCoroutine(ProcessLoginRequest_Coroutine());

    public void validateOTPRequest() => StartCoroutine(ProcessValidateOTPRequest_Coroutine());

    public void resendOTPRequest() => StartCoroutine(ProcessResendMobileOTPRequest_Coroutine());


    IEnumerator GetCountryCodeListData_Coroutine()
    {
        // outputArea.text = "Loading...";
        string uri = "http://165.22.219.198/edugogy/api/v1/country-codes?per-page=0";
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

            //Put list of country codes in dropdown
        }
    }

    IEnumerator ProcessLoginRequest_Coroutine()  // Actually this is API to sign up
    {
        // "9855940600", 88 - country code id not code
        LoginForm loginFormData = new LoginForm { phone = "9855940600", country_code_id = 88 };
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
