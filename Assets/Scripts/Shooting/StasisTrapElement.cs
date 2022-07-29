using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisTrapElement : BaseElementClass
{
    [SerializeField]
    private GameObject StasisTrap;

    [SerializeField]
    private float damage;
    public float damageMultiplier = 1;

    [SerializeField]
    private float duration;

    [SerializeField]
    private float currentDamageTicker;

    [SerializeField]
    private float maxDamageTicker;

    [SerializeField]
    private float rayCastRange;

    [SerializeField]
    private LayerMask layerMask;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward, Color.blue);
    }

    public override void ElementEffect()
    {
        RaycastHit hit;
        base.ElementEffect();
        // if the raycast hits then place the trap where the raycast hit
        // else place trap at the end of the raycast range
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayCastRange, layerMask))
        {
            GameObject newStasisTrap = Instantiate(StasisTrap, hit.point, Camera.main.transform.rotation);
            newStasisTrap.GetComponent<StasisTrapProj>().SetVars(damage * damageMultiplier, duration, currentDamageTicker, maxDamageTicker, attackTypes);
        }
        else
        {
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward.normalized * rayCastRange;
            GameObject newStasisTrap = Instantiate(StasisTrap, pos, Camera.main.transform.rotation);
            newStasisTrap.GetComponent<StasisTrapProj>().SetVars(damage * damageMultiplier, duration, currentDamageTicker, maxDamageTicker, attackTypes);
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
        playerHandL.SetTrigger(animationName);
    }
}
