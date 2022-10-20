using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialChecklist : MonoBehaviour
{
    //Take a list of serialzed lists of tutorial pointers and create them set by set as they are completed

    [SerializeField]
    List<Transform> pointers;
    int tutorialPoint = 0;


    private void Start()
    {
        foreach (Transform ga in pointers[0])
        {
            ga.gameObject.SetActive(true);
        }
    }

    //Move to the next set of tutorials checks
    //Call every time a tutorial is completed
    public void Progress()
    {
        if(CheckConditions(pointers[tutorialPoint]))
        {
            tutorialPoint++;
            StartNextBatch(pointers[tutorialPoint]);
        }
    }

    //Check each tut for if it has been completed
    public bool CheckConditions(Transform pointerList)
    {
        foreach(Transform tut in pointerList)
        {
            if(tut.gameObject.activeInHierarchy)
            {
                return false;
            }
        }

        return true;
    }

    public void StartNextBatch(Transform batch)
    {
        foreach (Transform tut in batch)
        {
            tut.gameObject.SetActive(true);
        }
    }

}
