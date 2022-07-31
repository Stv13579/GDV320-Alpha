using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealElement : BaseElementClass
{
    [SerializeField]
    private GameObject lifeSteal;

    [SerializeField]
    float damage;
    public float damageMultiplier = 1;

    protected override void Start()
    {
        base.Start();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        if(!Input.GetKey(KeyCode.Mouse0))
        {
            DeactivateLifeSteal();
        }
        if(playerHand.GetCurrentAnimatorStateInfo(0).IsName(""))
        {
            if(Input.GetKeyUp(KeyCode.Mouse0) || !PayCosts(Time.deltaTime))
            {
                DeactivateLifeSteal();
            }
        }
    }
    private void DeactivateLifeSteal()
    {
        lifeSteal.SetActive(false);
        playerHand.SetTrigger("LifeStealStopCast");
        playerHandL.SetTrigger("LifeStealStopCast");
    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        lifeSteal.SetActive(true);
        //mechanic in here
        //sphere cast
        // if enemy is in the spherecast 
        //damage enemy
        //suck health
        // single target
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName)
    {
        base.StartAnims(animationName);

        playerHand.ResetTrigger("LifeStealStopCast");
        playerHandL.ResetTrigger("LifeStealStopCast");

        playerHand.SetTrigger(animationName);
        playerHandL.SetTrigger(animationName);
    }
}
