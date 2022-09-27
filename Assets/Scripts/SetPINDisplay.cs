using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class SetPINDisplay : MonoBehaviour
{
    [SerializeField] private Sprite[] digits;

    [SerializeField] private Image[] characters;

    [SerializeField] public TextMeshProUGUI setPINDescription;

    private string codeSequence;

    private bool isReset = false;
    
    private void Awake() {
        // // FinalPIN
        if (PlayerPrefs.HasKey("FinalPIN"))
        {
            setPINDescription.text = "Enter your secret pin";
        }
    }
    
    void Start()
    {

        codeSequence = "";

        for (int i = 0; i <= characters.Length - 1; i++)
        {
            characters[i].sprite = digits[10];
        };

        PushButton.ButtonPressed += AddDigitToCodeSequence;
        Debug.Log("Hello");

    }

    private void AddDigitToCodeSequence(string digitEntered)
    {
        if (codeSequence.Length < 4)
        {
                    Debug.Log("Hello2");

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
            case "Next":
                CheckResults();
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
            case 4:
                characters[3].sprite = digits[digitJustEntered];
                break;

        }
    }

    public void CheckResults()
    {
        Debug.Log(codeSequence);
        
        if (setPINDescription.text == "Set your secret pin")
        {
            //Save set pin
            if (codeSequence == "" || codeSequence.Length != 4)
            {
                 Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Please set secret pin to proceed",
                    "Cancel",
                    "Sure!",
                    resetAction
                    );
            }
            else
            {
                PlayerPrefs.SetString("SetPIN", codeSequence);
                isReset = true;
                ResetDisplay();
                setPINDescription.text = "Confirm your secret pin";

            }

        }
        else if (PlayerPrefs.HasKey("SetPIN"))
        {
            Debug.Log(codeSequence);
            var alreadySetPIN = PlayerPrefs.GetString("SetPIN");
            if (codeSequence == "" || codeSequence.Length != 4)
            {

                 Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Please confirm secret pin to proceed",
                    "Cancel",
                    "Sure!",
                    resetAction
                    );
            }
            else if (codeSequence == alreadySetPIN)
            {
                PlayerPrefs.SetString("FinalPIN", codeSequence);
                PlayerPrefs.DeleteKey("SetPIN");
                SceneManager.LoadScene("KidsProfile");
            }
        }

        if (PlayerPrefs.HasKey("FinalPIN"))
        {
            Debug.Log(codeSequence);
            var alreadySetPIN = PlayerPrefs.GetString("FinalPIN");
            if (codeSequence == "" || codeSequence.Length != 4)
            {
                 Popup popup = UIController.Instance.CreatePopup();
                popup.Init(UIController.Instance.MainCanvas,
                    "Please enter secret pin to proceed",
                    "Cancel",
                    "Sure!",
                    resetAction
                    );
            }
            else if (codeSequence == alreadySetPIN)
            {
                //Moveto Kids profile
                SceneManager.LoadScene("KidsProfile");
            }
        }

    }

    void resetAction()
    {
        isReset = true;
        ResetDisplay();
    }

    void ResetDisplay()
    {
        Debug.Log(codeSequence.Length);

        if (isReset == true)
        {
            codeSequence = "";
            Debug.Log("when code is incorrect and checking for results");
            Debug.Log(codeSequence.Length);

            for (int i = 0; i <= characters.Length - 1; i++)
            {
                characters[i].sprite = digits[10];
            };
            isReset = false;
        }
        else if (codeSequence.Length > 3)
        {
            characters[3].sprite = digits[10];
            codeSequence = codeSequence.Substring(0, codeSequence.Length - 1);
            Debug.Log(codeSequence.Length);
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
