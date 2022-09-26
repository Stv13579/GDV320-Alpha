﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    List<BaseElementClass> primaryElements;
    [SerializeField]
    List<BaseElementClass> catalystElements;
    [SerializeField]
    List<ComboElementList> comboElements;

    public BaseElementClass blankElement;
    
    //Sets the combo elements based on the current base elements
    public BaseElementClass CalculateCombo(BaseElementClass primary, BaseElementClass catalyst)
    {
        BaseElementClass chosenCombo = new BaseElementClass();

        if(primary is FireElement)
        {
            if(catalyst is VoidElement)
            {
                chosenCombo = GetComponent<LandMineElement>();
            }
            else if(catalyst is NecroticElement)
            {
                chosenCombo = GetComponent<AcidCloudElement>();
            }
            else if(catalyst is EnergyElement)
            {
                chosenCombo = GetComponent<LaserBeamElement>();
            }
        }
        else if (primary is WaterElement)
        {
            if (catalyst is VoidElement)
            {
                chosenCombo = GetComponent<StasisTrapElement>();
            }
            else if (catalyst is NecroticElement)
            {
                chosenCombo = GetComponent<LifeStealElement>();
            }
            else if (catalyst is EnergyElement)
            {
                chosenCombo = GetComponent<IceSlashElement>();
            }
        }
        else if (primary is CrystalElement)
        {
            if (catalyst is VoidElement)
            {
                chosenCombo = GetComponent<CrystalGrenadeElement>();
            }
            else if (catalyst is NecroticElement)
            {
                chosenCombo = GetComponent<CurseElement>();
            }
            else if (catalyst is EnergyElement)
            {
                chosenCombo = GetComponent<ShardCannonElement>();
            }
        }

        return chosenCombo;
    }

    [Serializable]
    public struct ComboElementList
    {
        public List<BaseElementClass> comboElements;
    }

    int leftElementIndex = 0;
    int rightElementIndex = 0;

    bool inComboMode = false;

    bool ableToShoot = true;

    bool loadOutChosen;

    [SerializeField]
    bool tutorial;

    AudioManager audioManager;

    [SerializeField]
    private Transform leftOrbPos;

    [SerializeField]
    private Transform rightOrbPos;

    [SerializeField]
    private GameplayUI uiScript;

    // Getters
    public List<BaseElementClass> GetCatalystElements() { return catalystElements; }
    public List<BaseElementClass> GetPrimaryElements() { return primaryElements; }
    public List<ComboElementList> GetComboElements() { return comboElements; }
    public Sprite GetPrimaryElementSprite() { return primaryElements[leftElementIndex].uiSprite; }
    public Sprite GetNextPrimaryElementSprite() 
    { 
        if((leftElementIndex + 1) >= primaryElements.Count)
        {
            return primaryElements[0].uiSprite;
        }

        return primaryElements[leftElementIndex + 1].uiSprite; 
    }

    public Sprite GetCatalystElementSprite() { return catalystElements[rightElementIndex].uiSprite; }
    public Sprite GetNextCatalystElementSprite() 
    {
        if ((rightElementIndex + 1) >= catalystElements.Count)
        {
            return catalystElements[0].uiSprite;
        }

        return catalystElements[rightElementIndex + 1].uiSprite; 
    }

    public Sprite GetComboElementSprite() { return (comboElements[leftElementIndex].comboElements[rightElementIndex].uiSprite); }
    public Transform GetLeftOrbPos() { return leftOrbPos; }
    public Transform GetRightOrbPos() { return rightOrbPos; }
    public int GetLeftElementIndex() { return leftElementIndex; }
    public int GetRightElementIndex() { return rightElementIndex; }
    public bool GetInComboMode() { return inComboMode; }
    public void SetAbleToShoot(bool tempAbleToShoot) { ableToShoot = tempAbleToShoot; }
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

    public Vector2 GetNextLeftMana()
    {
        PlayerClass player = this.gameObject.GetComponent<PlayerClass>();

        int i;

        if ((leftElementIndex + 1) >= primaryElements.Count)
        {
            i = Array.FindIndex(player.manaTypes, item => item.manaName == primaryElements[0].GetManaName());

            return new Vector2(player.manaTypes[i].currentMana, player.manaTypes[i].maxMana);
        }

        i = Array.FindIndex(player.manaTypes, item => item.manaName == primaryElements[leftElementIndex + 1].GetManaName());

        return new Vector2(player.manaTypes[i].currentMana, player.manaTypes[i].maxMana);
    }

    public Vector2 GetRightMana()
    {
        PlayerClass player = this.gameObject.GetComponent<PlayerClass>();
        int i = Array.FindIndex(player.manaTypes, item => item.manaName == catalystElements[rightElementIndex].GetManaName());

        return new Vector2(player.manaTypes[i].currentMana, player.manaTypes[i].maxMana);
    }

    public Vector2 GetNextRightMana()
    {
        PlayerClass player = this.gameObject.GetComponent<PlayerClass>();

        int i;

        if ((rightElementIndex + 1) >= catalystElements.Count)
        {
            i = Array.FindIndex(player.manaTypes, item => item.manaName == catalystElements[0].GetManaName());

            return new Vector2(player.manaTypes[i].currentMana, player.manaTypes[i].maxMana);
        }

        i = Array.FindIndex(player.manaTypes, item => item.manaName == catalystElements[rightElementIndex + 1].GetManaName());

        return new Vector2(player.manaTypes[i].currentMana, player.manaTypes[i].maxMana);
    }
    public void SetLoadOutChosen(bool tempLoadOutChosen) { loadOutChosen = tempLoadOutChosen; }

    //A function that is called whenever a new scene is loaded while this object is enabled.
    public void OnLoadScene(Scene scene, LoadSceneMode mode) { }

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        // hard coded for now
        primaryElements[leftElementIndex].GetPlayerHand().SetInteger("ElementL", leftElementIndex + 1);
        catalystElements[rightElementIndex].GetPlayerHand().SetInteger("ElementR", rightElementIndex + 101);

        uiScript = FindObjectOfType<GameplayUI>();
        SceneManager.sceneLoaded += OnLoadScene;

        if(tutorial)
        {
            loadOutChosen = true;
        }
    }


    private void Update()
    {
        if(loadOutChosen)
        {
            primaryElements[leftElementIndex].AnimationSwitch(true);
            catalystElements[rightElementIndex].AnimationSwitch(false);
            DestroyLeftOrb();
            DestroyRightOrb();
            InstantiatePrimaryOrb();
            InstantiateCatalystOrb();
            loadOutChosen = false;
        }
        
        SwitchingElements();
        if (ableToShoot && (SceneManager.GetActiveScene().name != "Hub Scene"))
        {
            // if the player is out of combo mode
            if (!inComboMode)
            {
                // use primary and catalyst elements to shoot
                NonComboShooting();

                // plays shooting idle sound
                if(primaryElements[leftElementIndex].GetStartCoolDown() == false
                   || catalystElements[rightElementIndex].GetStartCoolDown() == false)
                {
                    if (audioManager)
                    {
                        audioManager.PlaySFX(primaryElements[leftElementIndex].GetIdleSFX());
                        audioManager.PlaySFX(catalystElements[rightElementIndex].GetIdleSFX());
                    }
                }
            }
            else
            {
                ComboShooting();

                // plays shooting idle sound
                if (comboElements[leftElementIndex].comboElements[rightElementIndex].GetStartCoolDown() == false)
                {
                    if (audioManager)
                    {
                        audioManager.PlaySFX(comboElements[leftElementIndex].comboElements[rightElementIndex].GetIdleSFX());
                    }
                }
            }
        }
        // life the shooting effects
        // mostly used for hold elements
        StopComboShooting();
        StopNonComboShooting();
    }

    // function to switch elements
    // to check if they have press any input
    // lifts elements effect
    // plays animations and sound effects
    // and changes orbs
    private void SwitchingElements()
    {
        // check if the player has pressed q
        if (Input.GetKeyUp(KeyCode.Q) && !GetPrimaryElements().Contains(blankElement))
        {
            // if in combomode lift the element effect that is a combo element
            // else stop idle sound for primary element
            if (inComboMode)
            {
                comboElements[leftElementIndex].comboElements[rightElementIndex].LiftEffect();
                DestroyLeftOrb();
                DestroyRightOrb();
            }
            else
            {
                if (audioManager)
                {
                    audioManager.StopSFX(primaryElements[leftElementIndex].GetIdleSFX());
                }
                DestroyLeftOrb();
            }
            
            // changes element
            leftElementIndex++;
            if (leftElementIndex >= primaryElements.Count)
            {
                leftElementIndex = 0;
            }

            if (audioManager)
            {
                // play audio of switching weapons
                audioManager.StopSFX(primaryElements[leftElementIndex].GetSwitchElementSFX());
                audioManager.PlaySFX(primaryElements[leftElementIndex].GetSwitchElementSFX());
            }

            if (!inComboMode)
            {
                // play animation to switch primary element
                primaryElements[leftElementIndex].AnimationSwitch(true);
                // instantiate primary element orbs 
                InstantiatePrimaryOrb();
            }
            else
            {
                // play animation to switch combo element
                comboElements[leftElementIndex].comboElements[rightElementIndex].AnimationSwitch(true);
                InstantiateComboOrbs();
            }
        }
        if (Input.GetKeyUp(KeyCode.E) && !GetCatalystElements().Contains(blankElement))
        {
            // if in combomode lift the element effect that is a combo element
            // else lift effect for catalyst and stop idle sound
            if (inComboMode)
            {
                comboElements[leftElementIndex].comboElements[rightElementIndex].LiftEffect();
                DestroyLeftOrb();
                DestroyRightOrb();
            }
            else
            {
                if (audioManager)
                {
                    audioManager.StopSFX(catalystElements[rightElementIndex].GetIdleSFX());
                }
                catalystElements[rightElementIndex].LiftEffect();
                DestroyRightOrb();
            }

            // changes element
            rightElementIndex++;
            if (rightElementIndex >= catalystElements.Count)
            {
                rightElementIndex = 0;
            }

            if (audioManager)
            {
                // play audio of switching weapons
                audioManager.StopSFX(catalystElements[rightElementIndex].GetSwitchElementSFX());
                audioManager.PlaySFX(catalystElements[rightElementIndex].GetSwitchElementSFX());
            }

            // if not in combo mode
            if (!inComboMode)
            {
                // play animation for catalyst
                catalystElements[rightElementIndex].AnimationSwitch(false);
                // instanciate catalyst orbs
                InstantiateCatalystOrb();
            }
            else
            {
                // play animation for combo elements
                comboElements[leftElementIndex].comboElements[rightElementIndex].AnimationSwitch(true);
                InstantiateComboOrbs();
            }
        }
        // if the player is in combo mode and has no mana
        // takes player out of combo mode and puts them to primary and catalyst elements
        // player also start to use health as mana
        if (inComboMode && GetLeftMana()[0] <= comboElements[leftElementIndex].comboElements[rightElementIndex].GetManaCost() ||
            inComboMode && GetRightMana()[0] <= comboElements[leftElementIndex].comboElements[rightElementIndex].GetManaCost() ||
            inComboMode && GetLeftMana()[0] <= comboElements[leftElementIndex].comboElements[rightElementIndex].GetManaCost()  && GetRightMana()[0] <= comboElements[leftElementIndex].comboElements[rightElementIndex].GetManaCost())
        {
            comboElements[leftElementIndex].comboElements[rightElementIndex].LiftEffect();
            primaryElements[leftElementIndex].AnimationSwitch(true);
            catalystElements[rightElementIndex].AnimationSwitch(false);
            uiScript.SetCombo(!inComboMode);
            inComboMode = false;
        }

        // if player has pressed combo button
        if (Input.GetKeyUp(KeyCode.F) && GetLeftMana()[0] >= comboElements[leftElementIndex].comboElements[rightElementIndex].GetManaCost() && GetRightMana()[0] >= comboElements[leftElementIndex].comboElements[rightElementIndex].GetManaCost())
        {
            // if in combo mode lift combo element effect
            // else lift catalyst effect
            if (inComboMode)
            {
                comboElements[leftElementIndex].comboElements[rightElementIndex].LiftEffect();
                DestroyLeftOrb();
                DestroyRightOrb();
            }
            else
            {
                catalystElements[rightElementIndex].LiftEffect();
                DestroyLeftOrb();
                DestroyRightOrb();
            }

            // sets the ui
            uiScript.SetCombo(!inComboMode);

            // changes if in combo or no combo mode (switch)
            inComboMode = !inComboMode;

            if (audioManager)
            {
                // stops the idle sound effect for the non combo elements
                audioManager.StopSFX(primaryElements[leftElementIndex].GetIdleSFX());
                audioManager.StopSFX(catalystElements[rightElementIndex].GetIdleSFX());
            }

            // if not in combo after switch
            if (!inComboMode)
            {
                if (audioManager)
                {
                    // stop combo idle sound effect
                    audioManager.StopSFX(comboElements[leftElementIndex].comboElements[rightElementIndex].GetIdleSFX());
                }

                // play non combo animations
                primaryElements[leftElementIndex].AnimationSwitch(true);
                catalystElements[rightElementIndex].AnimationSwitch(false);

                if (audioManager)
                {
                    // play switching element sound effects for non combo elements
                    audioManager.StopSFX(catalystElements[rightElementIndex].GetSwitchElementSFX());
                    audioManager.PlaySFX(catalystElements[rightElementIndex].GetSwitchElementSFX());
                    audioManager.StopSFX(primaryElements[leftElementIndex].GetSwitchElementSFX());
                    audioManager.PlaySFX(primaryElements[leftElementIndex].GetSwitchElementSFX());
                }

                // change orbs for non combo elements
                InstantiatePrimaryOrb();
                InstantiateCatalystOrb();
            }
            else
            {
                // play combo animation
                comboElements[leftElementIndex].comboElements[rightElementIndex].AnimationSwitch(true);

                if (audioManager)
                {
                    // play the switching element sound effect for combo elements
                    audioManager.StopSFX(comboElements[leftElementIndex].comboElements[rightElementIndex].GetSwitchElementSFX());
                    audioManager.PlaySFX(comboElements[leftElementIndex].comboElements[rightElementIndex].GetSwitchElementSFX());
                }
                InstantiateComboOrbs();
            }

        }
    }

    // shooting function for primary and catalyst
    // checks if player has press any input for mouse 0 and 1
    private void NonComboShooting()
    {
        //Starts the process of activating the element held in the left hand
        if (!primaryElements[leftElementIndex].GetStartCoolDown() && 
            Input.GetKey(KeyCode.Mouse0) &&
            primaryElements[leftElementIndex].GetPlayerHand().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            primaryElements[leftElementIndex].ActivateElement();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            catalystElements[rightElementIndex].ActivateElement();
        }
    }

    // lifts the shooting effects of the non combo elements
    // mostly used for hold elements
    private void StopNonComboShooting()
    {
        //Stops the process of activating the element held in the left hand
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            primaryElements[leftElementIndex].LiftEffect();
        }
        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            catalystElements[rightElementIndex].LiftEffect();
        }
    }

    // shooting function for combo elements
    private void ComboShooting()
    {
        if (!comboElements[leftElementIndex].comboElements[rightElementIndex].GetStartCoolDown() &&
            Input.GetKey(KeyCode.Mouse0) &&
            comboElements[leftElementIndex].comboElements[rightElementIndex].GetPlayerHand().GetCurrentAnimatorStateInfo(2).IsName("Idle"))
        {
            comboElements[leftElementIndex].comboElements[rightElementIndex].ActivateElement();
        }
    }

    // lift effect function for combo elements
    private void StopComboShooting()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            comboElements[leftElementIndex].comboElements[rightElementIndex].LiftEffect();
        }
    }

    private void DestroyRightOrb()
    {
        // destroy orbs
        if (rightOrbPos.childCount == 1)
        {
            Destroy(rightOrbPos.GetChild(0).gameObject);
        }
        // checks for wrist VFXs
        // then destroys it
        if (rightOrbPos.parent.parent.childCount == 2)
        {
            Destroy(rightOrbPos.parent.parent.GetChild(1).gameObject);
        }
    }
    private void DestroyLeftOrb()
    {
        // destroys the current orb
        if (leftOrbPos.childCount == 1)
        {
            Destroy(leftOrbPos.GetChild(0).gameObject);
        }
        // checks for wrist VFXs
        // then destroys it
        if (leftOrbPos.parent.parent.childCount == 2)
        {
            Destroy(leftOrbPos.parent.parent.GetChild(1).gameObject);
        }
    }

    private void InstantiateCatalystOrb()
    {
        // instanciate catalyst orbs
        if (rightOrbPos.childCount < 2)
        {
            Instantiate(catalystElements[rightElementIndex].GetHandVFX(), rightOrbPos);
        }
        if (catalystElements[rightElementIndex].GetWristVFX())
        {
            Instantiate(catalystElements[rightElementIndex].GetWristVFX(), rightOrbPos.parent.parent);
        }
    }
    private void InstantiatePrimaryOrb()
    {
        if (leftOrbPos.childCount < 2)
        {
            Instantiate(primaryElements[leftElementIndex].GetHandVFX(), leftOrbPos);
        }
        if (primaryElements[leftElementIndex].GetWristVFX())
        {
            Instantiate(primaryElements[leftElementIndex].GetWristVFX(), leftOrbPos.parent.parent);
        }
    }

    private void InstantiateComboOrbs()
    {
        // change orbs for combo elements
        if (leftOrbPos.transform.childCount < 2 && rightOrbPos.transform.childCount < 2)
        {
            Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].GetHandVFX(), leftOrbPos);
            Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].GetHandVFX(), rightOrbPos);
            if (comboElements[leftElementIndex].comboElements[rightElementIndex].GetWristVFX())
            {
                Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].GetWristVFX(), leftOrbPos.parent.parent);
                Instantiate(comboElements[leftElementIndex].comboElements[rightElementIndex].GetWristVFX(), rightOrbPos.parent.parent);
            }
        }
    }
}
