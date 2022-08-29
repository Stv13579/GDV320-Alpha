using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenObject : MonoBehaviour
{
  
    Quest relatedQuest;
    public void SetQuest(Quest nQ) { relatedQuest = nQ; }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            relatedQuest.FindHiddenObject();
        }

    }
}
