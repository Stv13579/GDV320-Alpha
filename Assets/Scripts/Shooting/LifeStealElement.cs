using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LifeStealElement : BaseElementClass
{
    [SerializeField]
    private GameObject lifeStealSuccess;

    [SerializeField]
    private GameObject lifeStealfail;

    [SerializeField]
    private float sphereRadius;

    [SerializeField]
    private float sphereRange;

    // might use later
    [SerializeField]
    private float healValue;

    [SerializeField]
    private float damageAndHealthTicker;
    private float currentDamageAndHealthTicker;

    private bool isTargeting;
    private bool isShooting;

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
        if (!isShooting)
        {
            DeactivateLifeSteal();
        }
    }
    private void ActivateLifeSteal()
    {
        if (isShooting == true)
        {
            currentDamageAndHealthTicker += Time.deltaTime;
            LifeStealFullScreenEffect(0);
            RaycastHit[] objectHit = Physics.SphereCastAll(Camera.main.transform.position, sphereRadius, Camera.main.transform.forward, sphereRange, hitLayer);
            if(objectHit.Length <= 0)
            {
                isTargeting = false;
                lifeStealSuccess.SetActive(false);
                lifeStealfail.SetActive(true);
                return;
            }
            // needs fixing
            else if (objectHit[0].transform.gameObject.layer == environmentLayer)
            {
                isTargeting = false;
                lifeStealSuccess.SetActive(false);
                lifeStealfail.SetActive(true);
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
                if (audioManager)
                {
                    audioManager.PlaySFX(shootingSoundFX);
                }
                lifeStealSuccess.SetActive(true);
                lifeStealfail.SetActive(false);
                if (currentDamageAndHealthTicker >= damageAndHealthTicker)
                {
                    playerClass.ChangeMana(-manaCost * Time.deltaTime, manaTypes);
                    enemy.GetComponent<BaseEnemyClass>().TakeDamage(damage * (damageMultiplier + elementData.waterDamageMultiplier), attackTypes);
                }
                playerClass.ChangeHealth(healValue);
            }
        }
    }
    private void DeactivateLifeSteal()
    {
        if (audioManager)
        {
            audioManager.StopSFX(shootingSoundFX);
        }
        isTargeting = false;
        isShooting = false;
        lifeStealSuccess.SetActive(false);
        lifeStealfail.SetActive(false);
        playerHand.SetTrigger("LifeStealStopCast");
        LifeStealFullScreenEffect(0);
    }
    private void LifeStealFullScreenEffect(float value)
    {
        lifestealeffectvalue = value;
        //lifestealFullScreenEffect.SetFloat("_Toggle_EffectIntensity", lifestealeffectvalue);
    }

    // gets called in the animation event triggers
    public override void ElementEffect()
    {
        base.ElementEffect();
    }

    // gets called in the animation event triggers
    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    // function to stop the effect of the element
    public override void LiftEffect()
    {
        base.LiftEffect();
        DeactivateLifeSteal();
    }

    // gets called before the element effect and activate VFX
    // gets called in the activate elements functions
    // when player press the right mouse button
    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
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
