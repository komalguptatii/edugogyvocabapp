using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwipeImages : MonoBehaviour
{
    // [SerializeField] 
    public GameObject scrollBar;
    float scroll_pos = 0;
    float[] pos;
    float distance;
    int currentIndexOfImageDisplayed = 1;
    int currentValue = 0;
    int nextValue = 1; 
    private Animator handAnim;
    public GameObject hand;
   

    private void Awake() 
    {
        handAnim = hand.GetComponent<Animator>(); 
    }

    // Update is called once per frame
    void Update()
    {
        ImageSwipe();
    }

    public void ImageSwipe()
    {

        pos = new float[transform.childCount];
        distance = 1f/ (pos.Length - 1f);
        for(int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;
           

        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollBar.GetComponent<Scrollbar>().value;
            Debug.Log(scroll_pos);
        }
        else
        {
            for(int i = 0; i < pos.Length; i++)
            {
                
                if (scroll_pos < pos[i] + (distance/2) && scroll_pos > pos[i] - (distance/2) )
                {
                    currentValue = i;
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                     if (i > 0)
                    {
                        handAnim.enabled = false;
                        hand.SetActive(false);
                    }
                    else if (i == 0)
                    {
                         handAnim.enabled = true;
                        hand.SetActive(true);
                    }
                }
            }
        }

        for(int i = 0; i < pos.Length; i++)
        {
            if (scroll_pos < pos[i] + (distance/2) && scroll_pos > pos[i] - (distance/2) )
            {
                currentIndexOfImageDisplayed = i;
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f,1f),0.1f);
                for(int a = 0; a < pos.Length; a++)
                {
                    
                    if (a != i)
                    {
                        transform.GetChild(a).localScale = Vector2.Lerp(transform.GetChild(a).localScale, new Vector2(0.8f,0.8f),0.1f);
                    }
                }
            }
        }
    }

    // public void NextImage()
    // {

        
    //     transform.GetChild(nextValue).localScale = Vector2.Lerp(transform.GetChild(nextValue).localScale, new Vector2(1f,1f),0.1f);
    //     scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[currentValue], 0.1f);



    // }
}
