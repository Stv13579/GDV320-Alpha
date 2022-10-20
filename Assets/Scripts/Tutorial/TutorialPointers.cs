using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPointers : MonoBehaviour
{
    /// <summary>
    /// Lower is faster
    /// </summary>

    float slideSpeed = 1;

    float currentSlideTime = 0;


    [SerializeField]
    bool finished = false;
    protected void Update()
    {
        CheckConditions();

        if(finished)
        {
            if(currentSlideTime == 0)
            {
                gameObject.SetActive(false);
                GetComponentInParent<TutorialChecklist>().Progress();
            }
            currentSlideTime -= Time.deltaTime;

        }
        else
        {
            currentSlideTime += Time.deltaTime;
        }

        if (currentSlideTime > slideSpeed)
            currentSlideTime = slideSpeed;
        else if(currentSlideTime < 0)
            currentSlideTime = 0;

        GetComponent<RectTransform>().anchoredPosition = 
            new Vector3(Mathf.Lerp(0, -GetComponent<RectTransform>().rect.width, currentSlideTime / slideSpeed), transform.localPosition.y, transform.localPosition.z);

    }

    //Check if this element of the tutorial has been completed
    public virtual void CheckConditions(bool done = false)
    {
        if(done)
        {
            finished = true;
        }
    }

    public virtual void FinishPoint()
    {
        GetComponentInParent<TutorialChecklist>().Progress();
        gameObject.SetActive(false);
    }
}
