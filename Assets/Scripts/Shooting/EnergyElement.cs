using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyElement : BaseElementClass
{
    [SerializeField]
    List<GameObject> containedEnemies = new List<GameObject>();

    [SerializeField]
    GameObject energyShield;

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
    float timeToParry;
    bool useShield = false;

    float materialChanger;
    [SerializeField]
    float materialTimer = 2.0f;
    bool beenHit;
    public bool GetUseShield() { return useShield; }
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
        // states for the energy Shield
            switch (shieldStateChange)
            {
            // if shield is not being used
                case shieldState.shieldDown:
                    {
                    // checks if the player has pressed left click
                        if (useShield == true)
                        {
                        // checks in the shield is upgrade
                            if (upgraded == true)
                            {
                                shieldStateChange = shieldState.parrying;
                            }
                            else
                            {
                                shieldStateChange = shieldState.shieldUp;
                            }
                        }
                    break;
                    }
                // for beta
                // basically player will be able to parry in a certain time
                case shieldState.parrying:
                    {
                        timeToParry += Time.deltaTime;
                        if (timeToParry >= 0.25f)
                        {
                            shieldStateChange = shieldState.shieldUp;
                            timeToParry = 0.0f;
                            //parrycode;
                        }
                    break;
                    }
                // if shields is up
                // does all the checks if to deactivate shield and go back to down state
                case shieldState.shieldUp:
                    {
                        HitShield();

                        if (!PayCosts(manaCost * Time.deltaTime) || !Input.GetKey(KeyCode.Mouse1))
                        { 
                            DeactivateEnergyShield();
                            shieldStateChange = shieldState.shieldDown;
                        }
                        break;
                    }
            }
    }
    void HitShield()
    {
        if(beenHit)
        {
            materialTimer += Time.deltaTime;
        }
        if(materialTimer >= 2.0f)
        {
            beenHit = false;
            materialChanger -= Time.deltaTime;
        }
        if(materialChanger <= 0.0f)
        {
            materialChanger = 0.0f;
            materialTimer = 0.0f;
        }
        energyShield.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_ShieldDamage", materialChanger);
    }
    // function to deactivate shield
    void DeactivateEnergyShield()
    {
        shieldStateChange = shieldState.shieldDown;
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
            if(containedEnemies[i])
            {
                BaseEnemyClass enemy = containedEnemies[i].gameObject.GetComponent<BaseEnemyClass>();
                StatModifier.RemoveModifier(enemy.GetDamageStat().multiplicativeModifiers, new StatModifier.Modifier(0.0f, "Shield"));
            }
            containedEnemies.Remove(containedEnemies[i]);
       }
       if (shootingScript.GetRightOrbPos().childCount > 1)
       {
           Destroy(shootingScript.GetRightOrbPos().GetChild(1).gameObject);
       }
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        energyShield.SetActive(true);
        useShield = true;
        Instantiate(activatedVFX, shootingScript.GetRightOrbPos());
    }
    
    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    public override void LiftEffect()
    {
        base.LiftEffect();
        DeactivateEnergyShield();
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

    private void OnTriggerEnter(Collider other)
    {
        if (useShield)
        {
            if (other.gameObject.layer == 8  && other.GetComponent<BaseEnemyClass>() || 
                other.gameObject.tag == "Enemy" && other.GetComponent<BaseEnemyClass>())
            {
                if (!containedEnemies.Contains(other.gameObject))
                {
                    containedEnemies.Add(other.gameObject);
                    BaseEnemyClass enemy = other.gameObject.GetComponent<BaseEnemyClass>();
                    StatModifier.AddModifier(enemy.GetDamageStat().multiplicativeModifiers, new StatModifier.Modifier(0.0f, "Shield"));
                }
            }
            if (other.gameObject.layer == 22 && other.GetComponent<BaseRangedProjectileScript>())
            {
                Destroy(other.gameObject);
            }
            if (other.gameObject.layer == 22 && other.GetComponent<BaseRangedProjectileScript>() ||
                other.gameObject.layer == 8 && other.GetComponent<BaseEnemyClass>() ||
                 other.gameObject.tag == "Enemy" && other.GetComponent<BaseEnemyClass>())
            {
                materialChanger = 1.0f;
                energyShield.transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_ShieldDamage", materialChanger);
                beenHit = true;
            }
        }

    }
}
