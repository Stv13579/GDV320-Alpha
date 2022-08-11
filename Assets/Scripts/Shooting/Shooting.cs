﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Shooting : MonoBehaviour
{
    public List<BaseElementClass> primaryElements;
    public List<BaseElementClass> catalystElements;

    [Serializable]
    public struct ComboElementList
    {
        public List<BaseElementClass> comboElements;
    }

    public List<ComboElementList> comboElements;
    int leftElementIndex = 0;
    int rightElementIndex = 0;

    bool inComboMode = false;

    public bool ableToShoot = true;

    bool canChangeHoldElements;
    
    AudioManager audioManager;

    [SerializeField]
    Transform leftOrbPos;

    [SerializeField]
    Transform rightOrbPos;

    [SerializeField]
    GameplayUI uiScript;
    public Sprite GetPrimaryElementSprite() { return primaryElements[leftElementIndex].uiSprite; }

    public Sprite GetCatalystElementSprite() { return catalystElements[rightElementIndex].uiSprite; }

    public Sprite GetComboElementSprite() { return (comboElements[leftElementIndex].comboElements[rightElementIndex].uiSprite); }
    public Transform GetLeftOrbPos() { return leftOrbPos; }
    public Transform GetRightOrbPos() { return rightOrbPos; }
    public int GetLeftElementIndex() { return leftElementIndex; }
    public int GetRightElementIndex() { return rightElementIndex; }
    public bool GetInComboMode() { return inComboMode; }
    public Sprite GetCrosshair()
    {
        if (inComboMode)
        {
            return (comboElements[leftElementIndex].comboElements[rightElementIndex].crosshair);
        }
        else
        {
            return (primaryElements[leftElementIndex].crosshair);
        }
    }
    public Vector2 GetLeftMana()
    {
        PlayerClass player = this.gameObject.GetComponent<PlayerClass>();
        int i = Array.FindIndex(player.manaTypes, item => item.manaName == primaryElements[leftElementIndex].GetManaName());

        return new Vector2(player.manaTypes[i].currentMana, player.manaTypes[i].maxMana);
    }
    public Vector2 GetRightMana()
    {
        PlayerClass player = this.gameObject.GetComponent<PlayerClass>();
        int i = Array.FindIndex(player.manaTypes, item => item.manaName == catalystElements[rightElementIndex].GetManaName());

        return new Vector2(player.manaTypes[i].currentMana, player.manaTypes[i].maxMana);
    }
    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        // hard coded for now
        primaryElements[leftElementIndex].GetPlayerHand().SetInteger("ElementL", leftElementIndex + 1);
        catalystElements[rightElementIndex].GetPlayerHand().SetInteger("ElementR", rightElementIndex + 101);
        canChangeHoldElements = true;
    }
    private void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if(ableToShoot)
        {
            if (!inComboMode)
            {
                NonComboShooting();
            }
            else
            {
                ComboShooting();
            }
        }
        
        SwitchingElements();
    }
    void SwitchingElements()
    {
            if (Input.GetKeyUp(KeyCode.Q))
            {
                leftElementIndex++;
                // play audio of switching weapons
                audioManager.Stop("Change Element");
                audioManager.Play("Change Element");
                if (leftElementIndex >= primaryElements.Count)
                {
                    leftElementIndex = 0;
                }
                Destroy(leftOrbPos.GetChild(0).gameObject);
                if (leftOrbPos.parent.parent.childCount == 2)
                {
                    Destroy(leftOrbPos.parent.parent.GetChild(1).gameObject);
                }
                if (!inComboMode)
                {
                    primaryElements[leftElementIndex].AnimationSwitch(true);
                    Instantiate(primaryElements[leftElementIndex].handVFX, leftOrbPos);
                    if (primaryElements[leftElementIndex].wristVFX)
                    {
                        Instantiate(primaryElements[leftElementIndex].wristVFX, leftOrbPos.parent.parent);
                    }
                }
                else
                {
                    comboElements[leftElementIndex].comboElements[rightElementIndex].AnimationSwitch(true);
                    Destroy(rightOrbPos.GetChild(0).gameObject);
                    if (rightOrbPos.parent.parent.childCount == 2)
                    {
                        Destroy(rightOrbPos.parent.parent.GetChild(1).gameObject);
                    }
                    Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].handVFX, leftOrbPos);
                    Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].handVFX, rightOrbPos);
                    if (comboElements[leftElementIndex].comboElements[rightElementIndex].wristVFX)
                    {
                        Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].wristVFX, leftOrbPos.parent.parent);
                        Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].wristVFX, rightOrbPos.parent.parent);
                    }

                }

            }
            if (canChangeHoldElements)
            {
                if (Input.GetKeyUp(KeyCode.E))
                {
                    rightElementIndex++;
                    // play audio of switching weapons
                    audioManager.Stop("Change Element");
                    audioManager.Play("Change Element");
                    if (rightElementIndex >= catalystElements.Count)
                    {
                        rightElementIndex = 0;
                    }
                    Destroy(rightOrbPos.GetChild(0).gameObject);
                    if (rightOrbPos.parent.parent.childCount == 2)
                    {
                        Destroy(rightOrbPos.parent.parent.GetChild(1).gameObject);
                    }
                    if (!inComboMode)
                    {
                        catalystElements[rightElementIndex].AnimationSwitch(false);
                        Instantiate(catalystElements[rightElementIndex].handVFX, rightOrbPos);
                        if (catalystElements[rightElementIndex].wristVFX)
                        {
                            Instantiate(catalystElements[rightElementIndex].wristVFX, rightOrbPos.parent.parent);
                        }
                    }
                    else
                    {
                        comboElements[leftElementIndex].comboElements[rightElementIndex].AnimationSwitch(true);
                        Destroy(leftOrbPos.GetChild(0).gameObject);
                        if (leftOrbPos.parent.parent.childCount == 2)
                        {
                            Destroy(leftOrbPos.parent.parent.GetChild(1).gameObject);
                        }
                        Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].handVFX, leftOrbPos);
                        Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].handVFX, rightOrbPos);
                        if (comboElements[leftElementIndex].comboElements[rightElementIndex].wristVFX)
                        {
                            Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].wristVFX, leftOrbPos.parent.parent);
                            Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].wristVFX, rightOrbPos.parent.parent);
                        }
                    }

                }
            }
        
        if (canChangeHoldElements)
        {
            if (Input.GetKeyUp(KeyCode.F))
            {
                uiScript.SetCombo(!inComboMode);
                inComboMode = !inComboMode;
                //Activate an animation trigger?
                Destroy(leftOrbPos.GetChild(0).gameObject);
                if (leftOrbPos.parent.parent.childCount == 2)
                {
                    Destroy(leftOrbPos.parent.parent.GetChild(1).gameObject);
                }
                Destroy(rightOrbPos.GetChild(0).gameObject);
                if (rightOrbPos.parent.parent.childCount == 2)
                {
                    Destroy(rightOrbPos.parent.parent.GetChild(1).gameObject);
                }
                comboElements[leftElementIndex].comboElements[rightElementIndex].AnimationSwitch(true);
                if (!inComboMode)
                {
                    primaryElements[leftElementIndex].AnimationSwitch(true);
                    catalystElements[rightElementIndex].AnimationSwitch(false);
                    Instantiate(primaryElements[leftElementIndex].handVFX, leftOrbPos);
                    if (primaryElements[leftElementIndex].wristVFX)
                    {
                        Instantiate(primaryElements[leftElementIndex].wristVFX, leftOrbPos.parent.parent);
                    }
                    Instantiate(catalystElements[rightElementIndex].handVFX, rightOrbPos);
                    if (catalystElements[rightElementIndex].wristVFX)
                    {
                        Instantiate(catalystElements[rightElementIndex].wristVFX, rightOrbPos.parent.parent);
                    }
                }
                else
                {
                    Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].handVFX, leftOrbPos);
                    Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].handVFX, rightOrbPos);
                    if (comboElements[leftElementIndex].comboElements[rightElementIndex].wristVFX)
                    {
                        Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].wristVFX, leftOrbPos.parent.parent);
                        Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].wristVFX, rightOrbPos.parent.parent);
                    }
                }

            }
        }
        
    }
    void NonComboShooting()
    {
        //Starts the process of activating the element held in the left hand
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            primaryElements[leftElementIndex].ActivateElement();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            catalystElements[rightElementIndex].ActivateElement();
            canChangeHoldElements = false;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            primaryElements[leftElementIndex].LiftEffect();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            catalystElements[rightElementIndex].LiftEffect();
            canChangeHoldElements = true;
        }
    }
    void ComboShooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            comboElements[leftElementIndex].comboElements[rightElementIndex].ActivateElement();
            canChangeHoldElements = false;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            comboElements[leftElementIndex].comboElements[rightElementIndex].LiftEffect();
            canChangeHoldElements = true;
        }
    }
}
