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
    float textTimer = 0;
	float buttonTimer = 0;
	[SerializeField]
    int sceneToLoad;

    AsyncOperation operation;
    AudioManager audioManager;

    void Start()
    {
        //Load the hub scene in the background while the button fades in
        StartCoroutine(LoadScene());
    }
    void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    void Update()
    {
        //As each object fades in, sart fading in the next object
        if(backgroundTimer < 1)
        {
            backgroundTimer += Time.deltaTime;
        }
        background.color = new Color(background.color.r, background.color.g, background.color.b, backgroundTimer);
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
        Destroy(GameObject.Find("Player"));
        Destroy(GameObject.Find("ProphecyManager"));
        Destroy(GameObject.Find("GameplayUI"));
        Destroy(GameObject.Find("Quest Manager"));
        Destroy(GameObject.Find("Trinket Manager"));

        FindObjectOfType<SAIM>().data.ResetDifficulty();
        operation.allowSceneActivation = true;
        if (audioManager)
        {
            for (int i = 0; i < audioManager.GetMusics().Length; i++)
            {
                audioManager.GetMusics()[i].GetAudioSource().Stop();
            }
            audioManager.PlayMusic("Hub Room Music");
        }
    }

    public IEnumerator LoadScene()
    {
        operation = SceneManager.LoadSceneAsync(sceneToLoad);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {

            yield return null;
        }

    }
}
