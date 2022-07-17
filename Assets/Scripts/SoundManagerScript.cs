using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{

    public static AudioClip popUpsound;
    public static AudioClip rightAnswersound;

    static AudioSource audioSrc;


    // Start is called before the first frame update
    void Start()
    {
        popUpsound = Resources.Load <AudioClip> ("popUp");
        rightAnswersound = Resources.Load <AudioClip> ("RightAnswer");
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
}
