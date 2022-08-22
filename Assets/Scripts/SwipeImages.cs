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
    int currentValue = 0;
    int nextValue = 1;
   
    // Update is called once per frame
    void Update()
    {
        pos = new float[transform.childCount];
        float distance = 1f/ (pos.Length - 1f);
        for(int i = 0; i < pos.Length; i++)
        {
            pos[i] = distance * i;

        }

        if (Input.GetMouseButton(0))
        {
            scroll_pos = scrollBar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for(int i = 0; i < pos.Length; i++)
            {
                currentValue = i;
                if (scroll_pos < pos[i] + (distance/2) && scroll_pos > pos[i] - (distance/2) )
                {
                    scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for(int i = 0; i < pos.Length; i++)
        {
            currentValue = i;
            if (scroll_pos < pos[i] + (distance/2) && scroll_pos > pos[i] - (distance/2) )
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f,1f),0.1f);
                for(int a = 0; a < pos.Length; a++)
                {
                    nextValue = a;
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
        scrollBar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollBar.GetComponent<Scrollbar>().value, pos[currentValue], 0.1f);
        transform.GetChild(nextValue).localScale = Vector2.Lerp(transform.GetChild(nextValue).localScale, new Vector2(0.8f,0.8f),0.1f);
        nextValue += 1;
        currentValue += 1;
        // transform.GetChild(currentValue).localScale = Vector2.Lerp(transform.GetChild(currentValue).localScale, new Vector2(1f,1f),0.1f);
    }
}
