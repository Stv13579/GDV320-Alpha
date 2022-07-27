using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyElement : BaseElementClass
{
    [SerializeField]
    private GameObject chargeVFX;

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
    private BoxCollider shieldCollider;

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
                    if(Input.GetKey(KeyCode.Mouse1) && useShield == true)
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
                        if (playerHand.GetCurrentAnimatorStateInfo(0).IsName("Hold"))
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

    public void DeactivateEnergyShield()
    {
        energyShield.SetActive(false);
        shieldCollider.enabled = false;
        useShield = false;
        playerHand.SetTrigger("StopEnergy");
        audioManager.Stop("Energy Element");
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        energyShield.SetActive(true);
        shieldCollider.enabled = true;
        useShield = true;
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName)
    {
        base.StartAnims(animationName);

        playerHand.ResetTrigger("StopEnergy");
        playerHand.SetTrigger(animationName);

        audioManager.Play("Energy Element");
        Instantiate(chargeVFX, playerClass.gameObject.GetComponent<Shooting>().GetRightOrbPos());

    }

    protected override bool PayCosts(float modifier = 1)
    {
        //Override of paycosts so that mana is only subtracted at then end, in case the cast is cancelled
        if (playerClass.ManaCheck(manaCost * modifier, manaTypes))
        {
            playerClass.ChangeMana(-manaCost * modifier, manaTypes);
            return true;
        }
        else
        {
            playerHand.SetTrigger("NoMana");
            // this section needs fixing will fix later
            Destroy(playerClass.gameObject.GetComponent<Shooting>().GetRightOrbPos().GetChild(1).gameObject);
            return false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            other.gameObject.GetComponent<BaseEnemyClass>().damageAmount = 0;
        }
    }
}
