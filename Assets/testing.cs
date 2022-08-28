using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{

    AudioManager audioManager;
    [SerializeField]
    private string initialMusic;
    [SerializeField]
    private string battleMusic;
    private bool fadeOutAmbientAudio = false;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (audioManager)
        {
            audioManager.FadeOutAndPlayMusic(battleMusic, initialMusic);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (audioManager)
            {
                audioManager.SetCurrentStateToFadeOut();
            }
        }
    }
}
