using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPCJournal : MonoBehaviour
{
    [SerializeField]
    NPCData data;
    [SerializeField]
    List<GameObject> completionMarkers;
    int oldStoryPos = 0;
    [SerializeField]
    TextMeshProUGUI descriptionText;

    private void Start()
    {
        DataError();
        oldStoryPos = data.storyPosition;
        UpdateCompletionMarkers();
    }

    void Update()
    {
        DataError();
        if(data.storyPosition != oldStoryPos)
        {
            oldStoryPos = data.storyPosition;
            UpdateCompletionMarkers();
        }

        if(data.onQuest)
        {
            //Give quest desc
            descriptionText.text = data.questDescriptions[oldStoryPos];
        }
        else
        {
            descriptionText.text = "I should talk to them some time...";
        }


    }

    void UpdateCompletionMarkers()
    {
        for (int i = 0; i < data.storyPosition; i++)
        {
            completionMarkers[i].transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void DataError()
    {
        if (!data)
        {
            Debug.LogError("Assign an NPC Data to the journal!");
            return;
        }
    }
}
