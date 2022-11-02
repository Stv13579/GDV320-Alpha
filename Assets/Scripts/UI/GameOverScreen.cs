using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class GameOverScreen : MonoBehaviour
{
	[SerializeField]
	Image background;
	[SerializeField]
	TextMeshProUGUI text;
	[SerializeField]
    GameObject button;
    float backgroundTimer = 0;
    float backgroundalpha = 0;
    float textTimer = 0;
	float buttonTimer = 0;
	[SerializeField]
    int sceneToLoad;

    AsyncOperation operation;
    AudioManager audioManager;

    void Start()
    {
        //Load the hub scene in the background while the button fades in
	    LoadingScreen.SetSceneToLoad(sceneToLoad);
    }
    void Awake()
    {
	    audioManager = AudioManager.GetAudioManager();
    }

    void Update()
    {
        backgroundTimer += Time.deltaTime;
        //As each object fades in, sart fading in the next object
        if (backgroundTimer > 4)
        {
            backgroundalpha += Time.deltaTime;
        }
        background.color = new Color(background.color.r, background.color.g, background.color.b, backgroundalpha);
        if(background.color.a >= 1)
        {
            if(textTimer < 1)
            {
                textTimer += Time.deltaTime / 2;
            }
            text.color = new Color(255, 0, 0, textTimer);
        }
        if(text.color.a >= 1)
        {
            button.SetActive(true);
            if (buttonTimer < 1)
            {
                buttonTimer += Time.deltaTime;
            }
            button.GetComponent<Image>().color = new Color(255, 255, 255, buttonTimer);
            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = new Color(0, 0, 0, buttonTimer);
        }
    }

    public void ReturnToHub()
    {
	    Destroy(PlayerClass.GetPlayerClass().gameObject);
	    Destroy(ProphecyManager.GetProphecyManager().gameObject);
	    Destroy(GameplayUI.GetGameplayUI().gameObject);
	    Destroy(TrinketManager.GetTrinketManager().gameObject);
	    if(FindObjectOfType<SAIM>())
	    {
		    FindObjectOfType<SAIM>().data.ResetDifficulty();
	    }
	    QuestManager.GetQuestManager().FinishRunUpdate();
	    SceneManager.LoadScene(6);
        if (audioManager)
        {
            for (int i = 0; i < audioManager.GetMusics().Length; i++)
            {
                audioManager.GetMusics()[i].GetAudioSource().Stop();
            }
            for (int i = 0; i < audioManager.GetSounds().Length; i++)
            {
                audioManager.GetSounds()[i].GetAudioSource().Stop();
            }
            audioManager.PlayMusic("Hub Room Music");
        }
    }
}
