using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisTrapElement : BaseElementClass
{
    [SerializeField]
    private GameObject stasisTrap;

    [SerializeField]
    private GameObject indicator;

    [SerializeField]
    private float duration;

    [SerializeField]
    private float currentDamageTicker;

    [SerializeField]
    private float maxDamageTicker;

    [SerializeField]
    private float rayCastRange;

    private RaycastHit hit;

    [SerializeField]
    private LayerMask layerMask;

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //if this element on turn on indicator
        //if()
        //{

        //}
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayCastRange, layerMask))
        {
            indicator.transform.position = hit.point;
        }
        else
        {
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward.normalized * rayCastRange;
            indicator.transform.position = pos;
        }
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        // if the raycast hits then place the trap where the raycast hit
        // else place trap at the end of the raycast range
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayCastRange, layerMask))
        {
            GameObject newStasisTrap = Instantiate(stasisTrap, hit.point + new Vector3(0,0.75f,0), Camera.main.transform.rotation);
            newStasisTrap.GetComponent<StasisTrapProj>().SetVars(damage * (damageMultiplier + elementData.waterDamageMultiplier), duration, currentDamageTicker, maxDamageTicker, attackTypes);
        }
        else
        {
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward.normalized * rayCastRange;
            GameObject newStasisTrap = Instantiate(stasisTrap, pos + new Vector3(0, 0.75f, 0), Camera.main.transform.rotation);
            newStasisTrap.GetComponent<StasisTrapProj>().SetVars(damage * (damageMultiplier + elementData.waterDamageMultiplier), duration, currentDamageTicker, maxDamageTicker, attackTypes);
        }
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName)
    {
        base.StartAnims(animationName);

        playerHand.SetTrigger(animationName);
    }

    protected override void SwitchAnims(string switchAnimationName, string boolTrigger)
    {
        base.SwitchAnims(switchAnimationName, boolTrigger);
    }
}
