using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPointers : MonoBehaviour
{
    [SerializeField]
    float slideSpeed;

    float currentSlideTime = 0;

    bool finished = false;
    protected void Update()
    {
        CheckConditions();

        currentSlideTime += Time.deltaTime;

        //Slide it out at start
        if(currentSlideTime < 1)
        {
            transform.localPosition = new Vector3(Mathf.Lerp(0, -GetComponent<RectTransform>().rect.width, currentSlideTime/1), transform.localPosition.y, transform.localPosition.z);
        }

        //Slide it in to end
        if (currentSlideTime < 1 && finished)
        {
            transform.position += new Vector3(slideSpeed * Time.deltaTime, 0, 0);
        }
    }

    //Check if this element of the tutorial has been completed
    public virtual void CheckConditions()
    {

    }

    public virtual void FinishPoint()
    {
        GetComponentInParent<TutorialChecklist>().Progress();
        gameObject.SetActive(false);
    }
}
