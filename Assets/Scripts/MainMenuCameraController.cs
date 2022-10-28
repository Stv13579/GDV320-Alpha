using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class MainMenuCameraController : MonoBehaviour
{
    enum ViewPosition
    {
        Blaze,
        Silvain,
        Lilly,
        Freya
    }

    [SerializeField]
    ViewPosition vP;

    [SerializeField]
    List<Transform> positions;

    [SerializeField]
    Image mmBackground;

    [SerializeField]
    float transitionTiming;
    float currentTransTime;

    bool fadeTransition = false;
    bool fading = false;

    Color bgCol;

    private void Start()
    {
        UpdatePosition();


    }

    private void Update()
    {
        currentTransTime += Time.deltaTime;

        if(currentTransTime > transitionTiming)
        {
            currentTransTime = 0;
            fadeTransition = true;
        }

        if(fadeTransition)
        {
            bgCol = mmBackground.color;
            bgCol.a += Time.deltaTime;
            mmBackground.color = bgCol;
            if (bgCol.a >= 1)
            {
                fadeTransition = false;
                fading = true;
                NewPosition();
            }
        }

        if(fading && !fadeTransition)
        {
            bgCol = mmBackground.color;
            bgCol.a -= Time.deltaTime;
            mmBackground.color = bgCol;
            if(bgCol.a <= 0)
            {
                
                fading = false;
            }
        }

    }

    public void UpdatePosition()
    {
        transform.SetPositionAndRotation(positions[(int)vP].position, positions[(int)vP].rotation);
    }

    public void NewPosition()
    {
        vP++;
        if((int)vP >= System.Enum.GetNames(typeof(ViewPosition)).Length)
        {
            vP = 0;
        }
        UpdatePosition();
    }

    private void OnValidate()
    {
        UpdatePosition();
    }


}
