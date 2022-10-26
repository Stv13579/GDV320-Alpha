using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyElement : BaseElementClass
{
    [SerializeField]
    protected List<GameObject> containedEnemies = new List<GameObject>();

    [SerializeField]
    protected GameObject energyShield;

    [SerializeField]
    // might change to this method depending if current way works
    enum shieldState
    {
        shieldUp,
        parrying,
        shieldDown
    }

    [SerializeField]
    shieldState shieldStateChange = shieldState.shieldDown;

    [SerializeField]
    protected float timeToParry;
    protected bool useShield = false;

    protected float materialChanger;
    [SerializeField]
    protected float materialTimer = 2.0f;
    protected bool beenHit;

    [SerializeField]
    protected float MovementSpeed;

    [SerializeField]
    protected float chargeTimer;

    [SerializeField]
    protected float currentTimer;

    [SerializeField]
    protected float upgradedChargeTimer;
    public bool GetUseShield() { return useShield; }
    public void SetBeenHit(bool tempBeenHit) { beenHit = tempBeenHit; }
    public List<GameObject> GetContainedEnemies() { return containedEnemies; }
    public GameObject GetEnergyShield() { return energyShield; }
    public void SetMaterialChanger(float tempMaterial) { materialChanger = tempMaterial; }
    public float GetMaterialChanger() { return materialChanger; }
    protected override void Start()
    {
        base.Start();
        activatedVFX.SetActive(false);
    }
    protected override void Update()
    {
        base.Update();
        if (!Input.GetKey(KeyCode.Mouse1) && playerHand.GetCurrentAnimatorStateInfo(3).IsName("EnergyHold"))
        {
            LiftEffect();
        }
        HitShield();
        if(upgraded)
        {
            chargeTimer = upgradedChargeTimer;
        }
        //// states for the energy Shield
        //    switch (shieldStateChange)
        //    {
        //    // if shield is not being used
        //        case shieldState.shieldDown:
        //            {
        //            // checks if the player has pressed left click
        //                if (useShield == true)
        //                {
        //                // checks in the shield is upgrade
        //                    if (upgraded == true)
        //                    {
        //                        shieldStateChange = shieldState.parrying;
        //                    }
        //                    else
        //                    {
        //                        shieldStateChange = shieldState.shieldUp;
        //                    }
        //                }
        //            break;
        //            }
        //        // for beta
        //        // basically player will be able to parry in a certain time
        //        case shieldState.parrying:
        //            {
        //                timeToParry += Time.deltaTime;
        //                if (timeToParry >= 0.25f)
        //                {
        //                    shieldStateChange = shieldState.shieldUp;
        //                    timeToParry = 0.0f;
        //                    //parrycode;
        //                    GetComponent<PlayerClass>().ChangeMana(50, manaTypes);
        //                }
        //            break;
        //            }
        //        // if shields is up
        //        // does all the checks if to deactivate shield and go back to down state
        //        case shieldState.shieldUp:
        //            {
        //                HitShield();

        //                if (!PayCosts(manaCost * Time.deltaTime) || !Input.GetKey(KeyCode.Mouse1))
        //                { 
        //                    DeactivateEnergyShield();
        //                    shieldStateChange = shieldState.shieldDown;
        //                }
        //                break;
        //            }
        //}
    }
    IEnumerator Charge()
    {
        currentTimer = 0;
        while (currentTimer < chargeTimer)
        {
            this.GetComponent<PlayerMovement>().SetInputs(false);
            this.GetComponent<PlayerMovement>().SetZ(1);
            this.GetComponent<PlayerMovement>().GetCharacterController().Move(this.GetComponent<PlayerMovement>().GetVelocity() * MovementSpeed * Time.deltaTime);
            this.GetComponent<PlayerLook>().SetSensitivity(0.5f);
            Physics.IgnoreLayerCollision(11, 8, true);
            currentTimer += Time.deltaTime;
            yield return null;
        }
        this.GetComponent<PlayerMovement>().SetZ(0);
        this.GetComponent<PlayerMovement>().SetInputs(true);
        this.GetComponent<PlayerLook>().SetSensitivity(PlayerPrefs.GetFloat("Sensitivity"));
        Physics.IgnoreLayerCollision(11, 8, false);
        DeactivateEnergyShield();
    }
    void HitShield()
    {
        if (beenHit)
        {
            materialTimer += Time.deltaTime;
        }
        if (materialTimer >= 2.0f)
        {
            beenHit = false;
            materialChanger -= Time.deltaTime;
        }
        if (materialChanger <= 0.0f)
        {
            materialChanger = 0.0f;
            materialTimer = 0.0f;
        }
        energyShield.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_ShieldDamage", materialChanger);
    }
    // function to deactivate shield
    void DeactivateEnergyShield()
    {
        //shieldStateChange = shieldState.shieldDown;
        energyShield.SetActive(false);
        useShield = false;
        playerHand.SetTrigger("EnergyStopCast");

        if (audioManager)
        {
            audioManager.StopSFX(shootingSoundFX);
        }
        // go through the list of enemies
        // remove them from the list and 
        for (int i = 0; i < containedEnemies.Count; i++)
        {
            if (containedEnemies[i])
            {
                BaseEnemyClass enemy = containedEnemies[i].gameObject.GetComponentInParent<BaseEnemyClass>();
                StatModifier.RemoveModifier(enemy.GetDamageStat().multiplicativeModifiers, new StatModifier.Modifier(0.0f, "Shield"));
                StatModifier.UpdateValue(enemy.GetDamageStat());
            }
            containedEnemies.Remove(containedEnemies[i]);
        }
        activatedVFX.SetActive(false);
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        energyShield.SetActive(true);
        useShield = true;
        activatedVFX.SetActive(true);
        StartCoroutine(Charge());
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    public override void LiftEffect()
    {
        base.LiftEffect();
        DeactivateEnergyShield();
        currentTimer = chargeTimer;
        StopCoroutine(Charge());
    }
    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);
        playerHand.ResetTrigger("EnergyStopCast");
        playerHand.SetTrigger(animationName);
        playerClass.ChangeMana(-manaCost * (upgraded ? 1 : 0.5f), manaTypes);
        if (audioManager)
        {
            audioManager.PlaySFX(shootingSoundFX);
            audioManager.PlaySFX(otherShootingSoundFX);
        }
    }

    public override void Upgrade()
    {
        base.Upgrade();
    }
}