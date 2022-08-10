﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyElement : BaseElementClass
{
    [SerializeField]
    List<GameObject> containedEnemies = new List<GameObject>();

    public GameObject energyShield;

    [SerializeField]
    // might change to this method depending if current way works
    private enum shieldState
    {
        shieldUp,
        parrying,
        shieldDown
    }
    [SerializeField]
    private shieldState shieldStateChange = shieldState.shieldDown;

    [SerializeField]
    private float timeToParry;
    private bool useShield = false;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        if (upgraded == true)
        {
            switch (shieldStateChange)
            {
                case shieldState.shieldDown:
                    {
                        if (!Input.GetKey(KeyCode.Mouse1))
                        {
                            DeactivateEnergyShield();
                        }
                        if (playerHand.GetCurrentAnimatorStateInfo(0).IsName("Hold"))
                        {
                            if (Input.GetKeyUp(KeyCode.Mouse1) || !PayCosts(Time.deltaTime))
                            {
                                DeactivateEnergyShield();
                            }
                        }
                        if (Input.GetKey(KeyCode.Mouse1) && useShield == true)
                        {
                            shieldStateChange = shieldState.parrying;
                        }
                    }
                    break;
                case shieldState.parrying:
                    {
                        timeToParry += Time.deltaTime;
                        if (timeToParry >= 0.25f)
                        {
                            shieldStateChange = shieldState.shieldUp;
                            timeToParry = 0.0f;
                            //parrycode;
                        }
                    }
                    break;
                case shieldState.shieldUp:
                    {
                        if (!Input.GetKey(KeyCode.Mouse1))
                        {
                            shieldStateChange = shieldState.shieldDown;
                        }
                        if (playerHand.GetCurrentAnimatorStateInfo(1).IsName("EnergyHold"))
                        {
                            if (Input.GetKeyUp(KeyCode.Mouse1) || !PayCosts(Time.deltaTime))
                            {
                                shieldStateChange = shieldState.shieldDown;
                            }
                        }

                        break;
                    }
            }
        }
        // this check if the player press mouse 1 once (presses it fast and it goes through animator and does disable the shield)
        if (playerHand.GetCurrentAnimatorStateInfo(1).IsName("EnergyStart") ||
            playerHand.GetCurrentAnimatorStateInfo(1).IsName("EnergyHold"))
        {
            DeactivateEnergyShield();
        }
    }

    public void DeactivateEnergyShield()
    {
        if (!Input.GetKey(KeyCode.Mouse1) || !PayCosts(Time.deltaTime))
        {
            energyShield.SetActive(false);
            useShield = false;
            playerHand.SetTrigger("EnergyStopCast");
            audioManager.Stop("Energy Element");
            // go through the list of enemies
            // remove them from the list and 
            for (int i = 0; i < containedEnemies.Count; i++)
            {
                containedEnemies[i].GetComponent<BaseEnemyClass>().damageMultiplier = 1.0f;
                containedEnemies.Remove(containedEnemies[i]);
            }
        }
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        energyShield.SetActive(true);
        useShield = true;
    }
    
    // for the channelling orb
    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    public override void LiftEffect()
    {
        base.LiftEffect();
        DeactivateEnergyShield();
        if(shootingScript.GetRightOrbPos().childCount > 1)
        {
            Destroy(shootingScript.GetRightOrbPos().GetChild(1).gameObject);
        }
    }
    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);
        playerHand.ResetTrigger("EnergyStopCast");
        playerHand.SetTrigger(animationName);
        audioManager.Play("Energy Element");
        Instantiate(activatedVFX, shootingScript.GetRightOrbPos());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (useShield)
        {
            if (other.gameObject.layer == 8 || other.gameObject.layer == 22)
            {

                if (other.gameObject && !containedEnemies.Contains(other.gameObject))
                {
                    containedEnemies.Add(other.gameObject);
                    for (int i = 0; i < containedEnemies.Count; i++)
                    {
                        containedEnemies[i].gameObject.GetComponent<BaseEnemyClass>().damageMultiplier = 0;
                    }
                }
            }
        }

    }
}
