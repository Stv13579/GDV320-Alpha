using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LifeStealElement : BaseElementClass
{
    [SerializeField]
    private GameObject lifeSteal;

    [SerializeField]
    private float sphereRadius;

    [SerializeField]
    private float sphereRange;

    // might use later
    [SerializeField]
    private float healValue;

    bool isTargeting;
    bool isShooting;

    [SerializeField]
    private LayerMask hitLayer;

    [SerializeField]
    private LayerMask environmentLayer;

    private GameObject enemy;

    //[SerializeField]
    //private Material lifestealFullScreenEffect;

    private float lifestealeffectvalue;
    protected override void Start()
    {
        base.Start();
        lifestealeffectvalue = 0;
        //lifestealFullScreenEffect.SetFloat("_Toggle_EffectIntensity", lifestealeffectvalue);
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        ActivateLifeSteal();
        if (!isShooting || (Input.GetKeyDown(KeyCode.E) || !Input.GetKey(KeyCode.Mouse0)
            || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.F)))
        {
            DeactivateLifeSteal();
        }
    }
    private void ActivateLifeSteal()
    {
        if (isShooting == true)
        {
            LifeStealFullScreenEffect(0);
            RaycastHit[] objectHit = Physics.SphereCastAll(Camera.main.transform.position, sphereRadius, Camera.main.transform.forward, sphereRange, hitLayer);
            if(objectHit.Length <= 0)
            {
                isTargeting = false;
                lifeSteal.SetActive(false);
                return;
            }
            // needs fixing
            else if (objectHit[0].transform.gameObject.layer == environmentLayer)
            {
                isTargeting = false;
                lifeSteal.SetActive(false);
            }
            // if objectHit is in the enemy layer
            // suck health from him
            else if (objectHit[0].transform.gameObject.layer == 8 && objectHit[0].transform.GetComponent<BaseEnemyClass>())
            {
                // turn targeting on
                // damage enemy
                enemy = objectHit[0].transform.gameObject;
                isTargeting = true;
            }
            if (isTargeting == true && enemy != null)
            {
                //LifeStealFullScreenEffect(0.1f);
                playerClass.ChangeMana(-Time.deltaTime, manaTypes);
                lifeSteal.SetActive(true);
                enemy.GetComponent<BaseEnemyClass>().TakeDamage(damage * (damageMultiplier * elementData.waterDamageMultiplier), attackTypes);
                playerClass.ChangeHealth(healValue);
            }
        }
    }
    private void DeactivateLifeSteal()
    {
        isTargeting = false;
        isShooting = false;
        lifeSteal.SetActive(false);
        playerHand.SetTrigger("LifeStealStopCast");
        LifeStealFullScreenEffect(0);
    }
    private void LifeStealFullScreenEffect(float value)
    {
        lifestealeffectvalue = value;
        //lifestealFullScreenEffect.SetFloat("_Toggle_EffectIntensity", lifestealeffectvalue);
    }
    public override void ElementEffect()
    {
        base.ElementEffect();
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }
    public override void LiftEffect()
    {
        base.LiftEffect();
        DeactivateLifeSteal();
    }
    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);
        playerHand.ResetTrigger("LifeStealStopCast");
        playerHand.SetTrigger(animationName);
        isShooting = true;
    }
    protected override bool PayCosts(float modifier = 1)
    {
        //Override of paycosts so that mana is only subtracted at then end, in case the cast is cancelled
        if (playerClass.ManaCheck(manaCost * modifier, manaTypes))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
