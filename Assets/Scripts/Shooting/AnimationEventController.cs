using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEventController : MonoBehaviour
{
    [SerializeField]
    List<BaseElementClass> elements;

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.GetAudioManager();
    }
    public void Activate(int activateType)
    {
        if (activateType <= elements.Count)
        {
            elements[activateType].ActivateVFX();
            elements[activateType].ElementEffect();
        }
    }

    public void Lifted(int activateType)
    {
        if (activateType <= elements.Count)
        {           
            elements[activateType].LiftEffect();
        }
    }    

    public void CurseCracking()
    {
        if (audioManager)
        {
            audioManager.PlaySFX("Curse Cracking");
        }
    }
}
