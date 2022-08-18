using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip popUpsound;
    public static AudioClip rightAnswersound;
    public static AudioClip wrongAnswersound;
    public static AudioClip successsound;
    public static AudioClip unsucesssound;

    static AudioSource audioSrc;


    // Start is called before the first frame update
    void Start()
    {
        popUpsound = Resources.Load <AudioClip> ("popUp");
        rightAnswersound = Resources.Load <AudioClip> ("right");
        wrongAnswersound = Resources.Load <AudioClip> ("wrong");
        successsound = Resources.Load <AudioClip> ("success70");
        unsucesssound = Resources.Load <AudioClip> ("unsuccess70");
        audioSrc = GetComponent <AudioSource> ();
    }

    public static void playPopUpsound()
    {
        audioSrc.PlayOneShot(popUpsound);
    }

     public static void RightAnswerSound()
    {
        audioSrc.PlayOneShot(rightAnswersound);
    }

     public static void WrongAnswerSound()
    {
        audioSrc.PlayOneShot(wrongAnswersound);
    }

    public static void SuccessSound()
    {
        audioSrc.PlayOneShot(successsound);
    }

    public static void UnsucessSound()
    {
        audioSrc.PlayOneShot(unsucesssound);
    }

}
