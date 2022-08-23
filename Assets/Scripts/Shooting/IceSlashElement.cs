using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSlashElement : BaseElementClass
{
    [SerializeField]
    private GameObject iceSlash;

    [SerializeField]
    private float projectileSpeed;

    [SerializeField]
    private float lifeTimer;

    [SerializeField]
    private GameObject iceSlashShootVFX;

    // gets called in the animation event triggers
    public override void ElementEffect()
    {
        base.ElementEffect();
        PlayVFX();
        GameObject iceSlashTemp = Instantiate(iceSlash, shootingTranform.position, Camera.main.transform.rotation);
        iceSlashTemp.GetComponent<IceSlashProj>().SetVars(projectileSpeed, damage * (damageMultiplier + elementData.waterDamageMultiplier), lifeTimer, attackTypes);
    }

    // gets called in the animation event triggers
    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }
    private void PlayVFX()
    {
        if (randomAnimationToPlay == 0)
        {
            //right hand
            if (!iceSlashShootVFX.transform.GetChild(1).GetComponent<ParticleSystem>().isPlaying)
            {
                iceSlashShootVFX.transform.GetChild(1).GetComponent<ParticleSystem>().Play();
            }
        }
        else
        {
            //left hand
            if (!iceSlashShootVFX.transform.GetChild(0).GetComponent<ParticleSystem>().isPlaying)
            {
                iceSlashShootVFX.transform.GetChild(0).GetComponent<ParticleSystem>().Play();
            }
        }
    }

    // gets called before the element effect and activate VFX
    // gets called in the activate elements functions
    // when player press the right mouse button
    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName, animationNameAlt);
        randomAnimationToPlay = Random.Range(0, 2);
        if (randomAnimationToPlay == 0)
        {
            playerHand.SetTrigger(animationName);
        }
        else
        {
            playerHand.SetTrigger(animationNameAlt);
        }
    }
}
