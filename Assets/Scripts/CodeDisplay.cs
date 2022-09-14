using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Random=UnityEngine.Random;


public class CodeDisplay : MonoBehaviour
{
    [SerializeField] private Sprite[] digits;

    [SerializeField] private Image[] characters;

    [SerializeField] public TextMeshProUGUI codeText;

    private string codeSequence;
    // Start is called before the first frame update

    private int firstNumber;
    private int secondNumber;
    private int thirdNumber;

    private string numberValueInString = "";

    private string generatedCodeSequence = "";

    private string codeInString = "";

    private bool isReset = false;

    

    private void Awake()
    {
        firstNumber = RandomRangeExcept(0, 9, 0);
        secondNumber = RandomRangeExcept(0, 9, firstNumber);
        thirdNumber = RandomRangeExcept(0, 9, secondNumber);
        Debug.Log(firstNumber);
        Debug.Log(secondNumber);
        Debug.Log(thirdNumber);

        generatedCodeSequence = string.Format("{0}{1}{2}", firstNumber, secondNumber, thirdNumber);
        Debug.Log(generatedCodeSequence);

        generateNumberString(firstNumber);
        string firstNumberInWord = numberValueInString;

        generateNumberString(secondNumber);
        string secondNumberInWord = numberValueInString;

        generateNumberString(thirdNumber);
        string thirdNumberInWord = numberValueInString;


        codeInString = string.Join(", ", firstNumberInWord, secondNumberInWord, thirdNumberInWord);
        Debug.Log(codeInString);

    }

    private void generateNumberString(int numberGenerated)
    {
        switch (numberGenerated)
        {
            case 0:
                numberValueInString = "Zero";
                break;
            case 1:
                numberValueInString = "One";
                break;
            case 2:
                numberValueInString = "Two";
                break;
            case 3:
                numberValueInString = "Three";
                break;
            case 4:
                numberValueInString = "Four";
                break;
            case 5:
                numberValueInString = "Five";
                break;
            case 6:
                numberValueInString = "Six";
                break;
            case 7:
                numberValueInString = "Seven";
                break;
            case 8:
                numberValueInString = "Eight";
                break;
            case 9:
                numberValueInString = "Nine";
                break;
        }
    }



    public int RandomRangeExcept(int min, int max, int except)
    {
        int result = Random.Range(min, max - 1);
        if (result >= except) result += 1;
        return result;
    }

    void Start()
    {

        codeSequence = "";

        for (int i = 0; i <= characters.Length - 1; i++)
        {
            characters[i].sprite = digits[10];
        };

        PushButton.ButtonPressed += AddDigitToCodeSequence;

        codeText.GetComponent<TextMeshProUGUI>().text = codeInString;

    }

    private void AddDigitToCodeSequence(string digitEntered)
    {
        if (codeSequence.Length < 3)
        {
            switch (digitEntered)
            {
                case "Zero":
                    codeSequence += "0";
                    DisplayCodeSequence(0);
                    break;
                case "One":
                    codeSequence += "1";
                    DisplayCodeSequence(1);
                    break;
                case "Two":
                    codeSequence += "2";
                    DisplayCodeSequence(2);
                    break;
                case "Three":
                    codeSequence += "3";
                    DisplayCodeSequence(3);
                    break;
                case "Four":
                    codeSequence += "4";
                    DisplayCodeSequence(4);
                    break;
                case "Five":
                    codeSequence += "5";
                    DisplayCodeSequence(5);
                    break;
                case "Six":
                    codeSequence += "6";
                    DisplayCodeSequence(6);
                    break;
                case "Seven":
                    codeSequence += "7";
                    DisplayCodeSequence(7);
                    break;
                case "Eight":
                    codeSequence += "8";
                    DisplayCodeSequence(8);
                    break;
                case "Nine":
                    codeSequence += "9";
                    DisplayCodeSequence(9);
                    break;

            }


        }

        switch (digitEntered)
        {
            case "Delete":
                // CheckResults();
                ResetDisplay();
                break;
        }
    }



    void DisplayCodeSequence(int digitJustEntered)
    {
        Debug.Log(digitJustEntered);
        switch (codeSequence.Length)
        {
            case 1:
                characters[0].sprite = digits[digitJustEntered];
                break;
            case 2:
                characters[1].sprite = digits[digitJustEntered];
                break;
            case 3:
                characters[2].sprite = digits[digitJustEntered];
                break;

        }
    }

    public void CheckResults()
    {
        Debug.Log(codeSequence);
        Debug.Log(generatedCodeSequence);
        if (codeSequence == generatedCodeSequence)
        {
            Debug.Log("Working");
            CheckInformation();
            // Add check to move to correct screen
            // SceneManager.LoadScene("Login");
        }
        else
        {
            Debug.Log("Not Working");

            Popup popup = UIController.Instance.CreatePopup();
			//Init popup with params (canvas, text, text, text, action)
			popup.Init(UIController.Instance.MainCanvas,
				"Code entered is not correct, Please check",
				"Cancel",
				"Sure!",
				resetAction
				);
            
            
            // MessageBox().DisplayFormat("Warning", "Please enter correct digits");
        }
    }


    public void CheckInformation()
    {
        if (PlayerPrefs.HasKey("auth_key"))
        {
            string auth_key = PlayerPrefs.GetString("auth_key");
            if (auth_key != null)
            {
                if (PlayerPrefs.HasKey("childName"))
                {
                    if (PlayerPrefs.HasKey("isAgeSelected"))
                    {
                        if (PlayerPrefs.HasKey("isSubscribed"))
                        {
                            SceneManager.LoadScene("Dashboard");
                        }
                        else
                        {
                            SceneManager.LoadScene("IAPCatalog");

                        }
                    }
                    else
                    {
                        SceneManager.LoadScene("SelectAge");
                    }

                }
                else
                {
                    SceneManager.LoadScene("KidsName");
                }
            }
        }
        else
        {
            SceneManager.LoadScene("Description");
        }
    }

    // Action resetAction = resetActionMethod;
    void resetAction()
    {
        isReset = true;
        ResetDisplay();
    }
    
    
    
    
    void ResetDisplay()
    {
        // isReset = true - set before calling this function on next button pop up
        Debug.Log(codeSequence.Length);

        if (isReset == true)
        {
            codeSequence = "";
            Debug.Log("when code is incorrect and checking for results");
            Debug.Log(codeSequence.Length);

            // characters[0].sprite = digits[10];

            for (int i = 0; i <= characters.Length - 1; i++)
            {
                characters[i].sprite = digits[10];
            };
            isReset = false;
        }
        else if (codeSequence.Length > 2)
        {
            characters[2].sprite = digits[10];
            codeSequence = codeSequence.Substring(0, codeSequence.Length - 1);
            Debug.Log(codeSequence.Length);
        }
        else if (codeSequence.Length > 1)
        {
            characters[1].sprite = digits[10];
            codeSequence = codeSequence.Substring(0, codeSequence.Length - 1);
            Debug.Log(codeSequence.Length);
        }
        else
        {

            codeSequence = "";
            Debug.Log("At last digit");
            Debug.Log(codeSequence.Length);

            // characters[0].sprite = digits[10];

            for (int i = 0; i <= characters.Length - 1; i++)
            {
                characters[i].sprite = digits[10];
            };
            // PushButton.ButtonPressed += AddDigitToCodeSequence;

        }


    }

    private void OnDestroy()
    {
        PushButton.ButtonPressed -= AddDigitToCodeSequence;
    }
    // Update is called once per frame
    // void Update()
    // {

    // }
}
