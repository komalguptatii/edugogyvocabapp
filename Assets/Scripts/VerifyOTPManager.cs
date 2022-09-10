using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text;
using TMPro;
using UnityEngine.SceneManagement;

public class VerifyOTPManager : MonoBehaviour
{
    public TMP_InputField firstDigit;
    public TMP_InputField secondDigit;
    public TMP_InputField thirdDigit;
    public TMP_InputField fourthDigit;

    string phoneNumber;
    int countryCodeId;
    public string otpEntered;

    [Serializable]
    public class ValidateOTPForm
    {
        public string phone;
        public int country_code_id;
        public int otp;
    }

    [Serializable]
    public class GetAuthKey
    {
        public string auth_key;
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

    [SerializeField]
    public TextMeshProUGUI description; 

     string baseURL = "https://api.edugogy.app/v1/";
    //     string baseURL = "https://api.testing.edugogy.app/v1/";

    // string baseURLTest = "http://165.22.219.198/edugogy/api/v1/";

    string fixJson(string value)            // Added object type to JSON
    {
        value = "{\"items\":" + value + "}";
        return value;
    }


    void Awake()
    {
        SetInputCharacterLength();
        if (PlayerPrefs.HasKey("phone"))
        {
            phoneNumber = PlayerPrefs.GetString("phone");
            countryCodeId = PlayerPrefs.GetInt("countryCodeId");
            Debug.Log(phoneNumber);
            Debug.Log(countryCodeId);

        }


    }

    void Start() {
        // phoneNumber = "9855940600";

        int phoneLength = phoneNumber.Length;
        string returnNumber = "";

        for (int i = 1; i <= 5; i++)
        {
            int value = phoneLength - i;
            Debug.Log(value);
            returnNumber = phoneNumber.Remove(value);
            if (i == 5)
            {
                returnNumber = returnNumber + "XXXXX";
            }
        }


        Debug.Log(returnNumber);
        description.text = "Kindly enter the OTP sent by SMS on " +  returnNumber   +" for  your space flight.";
    }

    void SetInputCharacterLength()
    {
        firstDigit.characterLimit = 1;
        secondDigit.characterLimit = 1;
        thirdDigit.characterLimit = 1;
        fourthDigit.characterLimit = 1;

        firstDigit.Select();

    }

    public void OnTextValueChanged(string data)
    {
        Debug.Log(firstDigit.text);
        Debug.Log(data);
        if (firstDigit.text.Length == 1)
        {
            secondDigit.Select();
            secondDigit.ActivateInputField();
            otpEntered = (firstDigit.text).ToString();
        }

       


         
    }


    public void secondDigitValueChanged(string data)
    {
         if (secondDigit.text.Length == 1)
        {
            thirdDigit.Select();
            thirdDigit.ActivateInputField();
            otpEntered = otpEntered + (secondDigit.text).ToString();
        }
    }

    public void thirdDigitValueChanged(string data)
    {
        
        if (thirdDigit.text.Length == 1)
        {
            fourthDigit.Select();
            fourthDigit.ActivateInputField();
            otpEntered = otpEntered + (thirdDigit.text).ToString();
        }
    }

    public void fourthDigitValueChanged(string data)
    {
       if (fourthDigit.text.Length == 1)
        {
            
            otpEntered = otpEntered + (fourthDigit.text).ToString();
            Debug.Log(otpEntered);
        }
    }

    public void validateOTPRequest() => StartCoroutine(ProcessValidateOTPRequest_Coroutine());

    public void resendOTPRequest() => StartCoroutine(ProcessResendMobileOTPRequest_Coroutine());
  

    void SaveAuthKey(string auth_key){
        PlayerPrefs.SetString("auth_key", "Bearer " + auth_key);
    }

     void MoveToSubscription()
    {
        SceneManager.LoadScene("IAPCatalog");
    }

    IEnumerator ProcessValidateOTPRequest_Coroutine()  //validate otp
    {
        var otp = int.Parse(otpEntered);
        Debug.Log(otp);
        GetAuthKey getKey = new GetAuthKey();
        
        ValidateOTPForm validateOTPFormData = new ValidateOTPForm { phone = phoneNumber, country_code_id = countryCodeId, otp = otp };
        string json = JsonUtility.ToJson(validateOTPFormData);

        Debug.Log(json);


        string uri = baseURL + "students/validate-otp";

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
            // { "auth_key":"zJMqRh3THK3WTaTh1x7CAMO1T3s0Cxes"}

             string validateOTPJson = request.downloadHandler.text;

            getKey = JsonUtility.FromJson<GetAuthKey>(validateOTPJson);
            Debug.Log(getKey.auth_key);
            SaveAuthKey(getKey.auth_key);
            MoveToSubscription();
        }

    }

    IEnumerator ProcessResendMobileOTPRequest_Coroutine()  //Resend validate otp, also used for login
    {

        ResetInputs();
        ResendOTP resendOTP = new ResendOTP();
        ValidateOTPForm validateOTPFormData = new ValidateOTPForm { phone = phoneNumber, country_code_id = countryCodeId };
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
        }
        else
        {
            Debug.Log("All OK");
            Debug.Log("Status Code: " + request.responseCode);
            Debug.Log(request.result);
            Debug.Log(request.downloadHandler.text);

            string resendOTPJson = request.downloadHandler.text;
            // string jsonString = fixJson(userJson);
            // Debug.Log(jsonString);
            resendOTP = JsonUtility.FromJson<ResendOTP>(resendOTPJson);
            Debug.Log(resendOTP.phone);
            Debug.Log(resendOTP.student.name);
           
        }

    }

    public void ResetInputs()
    {
        firstDigit.text = "";
        secondDigit.text = "";
        thirdDigit.text = "";
        fourthDigit.text = "";
        otpEntered = "";
    }

}
