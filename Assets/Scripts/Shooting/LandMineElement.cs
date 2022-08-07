using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineElement : BaseElementClass
{
    [SerializeField]
    private GameObject landMineProjectile;

    [SerializeField]
    private GameObject indicator;

    [SerializeField]
    private float explosiveRadius;

    [SerializeField]
    private float lifeTimer;

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
        if (shootingScript.GetLeftElementIndex() == leftIndex &&
            shootingScript.GetRightElementIndex() == rightIndex &&
            shootingScript.GetInComboMode() == true)
        {
            indicator.SetActive(true);
        }
        else
        {
            indicator.SetActive(false);
        }
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
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, rayCastRange, layerMask))
        {
            GameObject newLandMine = Instantiate(landMineProjectile, hit.point + new Vector3(0, 0.75f, 0), Quaternion.identity);
            newLandMine.GetComponent<LandMineProj>().SetVars(damage * (damageMultiplier + elementData.fireDamageMultiplier), lifeTimer, explosiveRadius, attackTypes);
        }
        else
        {
            Vector3 pos = Camera.main.transform.position + Camera.main.transform.forward.normalized * rayCastRange;
            GameObject newLandMine = Instantiate(landMineProjectile, pos + new Vector3(0, 0.75f, 0), Quaternion.identity);
            newLandMine.GetComponent<LandMineProj>().SetVars(damage * (damageMultiplier + elementData.fireDamageMultiplier), lifeTimer, explosiveRadius, attackTypes);
        }
    }
    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }
    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);

        playerHand.SetTrigger(animationName);
    }
}