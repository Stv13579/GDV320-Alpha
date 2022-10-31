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
        if(PlayerPrefs.GetFloat("Sensitivity") <= 0.0f)
        {
            this.GetComponent<PlayerLook>().SetSensitivity(2.0f);
        }
        else
        {
            this.GetComponent<PlayerLook>().SetSensitivity(PlayerPrefs.GetFloat("Sensitivity"));
        }
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
        playerClass.ChangeMana(-manaCost * (upgraded ? 1 : 0.5f), manaTypes);
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