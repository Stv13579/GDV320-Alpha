using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawn : MonoBehaviour
{

    public bool triggered = false;

    [SerializeField]
    Transform spawnPosition;

    [SerializeField]
    GameObject boss;

    public bool bossDead = false;

    [SerializeField]
    GameObject hubPortal;

    [SerializeField]
    GameObject bridge, bossRing;

    // audio manager
    AudioManager audioManager;
    [SerializeField]
    private string initialMusic;
    [SerializeField]
    private string battleMusic;

    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        if (audioManager)
        {
            audioManager.FadeOutAndPlayMusic(initialMusic, battleMusic);
        }

        if (!triggered)
        {
            return;
        }
        if(bossDead)
        {
            hubPortal.SetActive(true);
            bridge.SetActive(true);
            bossRing.SetActive(false);

            //if the boss dies set this to true
            if (audioManager)
            {
                audioManager.SetCurrentStateToFadeOutAudio2();
            }
        }
    }

    public void StartFight()
    {
        if(triggered)
        {
            return;
        }
        triggered = true;

        bridge.SetActive(false);
        bossRing.SetActive(true);

        Instantiate(boss, spawnPosition.position, Quaternion.identity);

       // when the boss spawns set this to true
        if (audioManager)
        {
            audioManager.SetCurrentStateToFadeOutAudio1();
        }
    }

}
