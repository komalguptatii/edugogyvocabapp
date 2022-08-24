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
                    currentIndexOfImageDisplayed = i;
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
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

    public void NextImage()
    {
        if (currentIndexOfImageDisplayed == 0)
        {
            currentIndexOfImageDisplayed += 1;
        }
        scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[currentIndexOfImageDisplayed], 0.1f);

    }
}
