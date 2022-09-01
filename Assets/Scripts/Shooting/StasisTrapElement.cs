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
    private float maxDuration;

    [SerializeField]
    private float currentDamageTicker;

    [SerializeField]
    private float maxDamageTicker;

    [SerializeField]
    private float rayCastRange;

    private RaycastHit hit;

    [SerializeField]
    private LayerMask layerMask;

    private List<GameObject> stasisTrapList = new List<GameObject>();

    private int maxStasisTraps = 1;
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //if this element is turned on turn on indicator
        if (shootingScript.GetComboElements()[shootingScript.GetLeftElementIndex()].comboElements[shootingScript.GetRightElementIndex()] == this
            && shootingScript.GetInComboMode() == true)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
        // updating the indicator position
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

    // gets called in the animation event triggers
    public override void ElementEffect()
    {
        base.ElementEffect();
        // if the raycast hits then place the trap where the raycast hit
        // else place trap at the end of the raycast range
        if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayCastRange, layerMask))
        {
            if(stasisTrapList.Count >= maxStasisTraps)
            {
                Destroy(stasisTrapList[0]);
                stasisTrapList.RemoveAt(0);
            }
            GameObject newStasisTrap = Instantiate(stasisTrap, hit.point + new Vector3(0,0.75f,0), Camera.main.transform.rotation);
            newStasisTrap.GetComponent<StasisTrapProj>().SetVars(damage * (damageMultiplier + elementData.waterDamageMultiplier), maxDuration, currentDamageTicker, maxDamageTicker, attackTypes);
            stasisTrapList.Add(newStasisTrap);
        }
        else
        {
            if (stasisTrapList.Count >= maxStasisTraps)
            {
                Destroy(stasisTrapList[0]);
                stasisTrapList.RemoveAt(0);
            }
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward.normalized * rayCastRange;
            GameObject newStasisTrap = Instantiate(stasisTrap, pos + new Vector3(0, 0.75f, 0), Camera.main.transform.rotation);
            newStasisTrap.GetComponent<StasisTrapProj>().SetVars(damage * (damageMultiplier + elementData.waterDamageMultiplier), maxDuration, currentDamageTicker, maxDamageTicker, attackTypes);
            stasisTrapList.Add(newStasisTrap);
        }
    }

    // gets called in the animation event triggers
    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    // gets called before the element effect and activate VFX
    // gets called in the activate elements functions
    // when player press the right mouse button
    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);

        playerHand.SetTrigger(animationName);
    }

    public override void Upgrade()
    {
        base.Upgrade();

        maxStasisTraps += 1;
    }
}
