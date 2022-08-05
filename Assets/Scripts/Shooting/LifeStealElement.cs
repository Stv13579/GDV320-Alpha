using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(playerHand.GetCurrentAnimatorStateInfo(0).IsName("LifeStealCastHold"))
        {
            if(Input.GetKeyUp(KeyCode.Mouse0) || !PayCosts(Time.deltaTime))
            {
                DeactivateLifeSteal();
            }
        }
        ActivateLifeSteal();
    }
    private void ActivateLifeSteal()
    {
        if (isShooting == true)
        {
            RaycastHit[] objectHit = Physics.SphereCastAll(Camera.main.transform.position, sphereRadius, Camera.main.transform.forward, sphereRange, hitLayer);
            if(objectHit.Length <= 0)
            {

            }
            else if (objectHit[0].transform.gameObject.layer == environmentLayer)
            {
                // do nothing
            }
            else
            {
                // turn targeting on
                // damage enemy
                enemy = objectHit[0].transform.gameObject;
                isTargeting = true;
            }

            if (isTargeting == true && enemy.GetComponent<BaseEnemyClass>())
            {
                enemy.GetComponent<BaseEnemyClass>().TakeDamage(damage * (damageMultiplier * elementData.waterDamageMultiplier), attackTypes);
                playerClass.ChangeHealth(healValue);
            }
        }
    }
    private void DeactivateLifeSteal()
    {
        isShooting = false;
        lifeSteal.SetActive(false);
        playerHand.SetTrigger("LifeStealStopCast");
    }
    public override void ElementEffect()
    {
        base.ElementEffect();
        lifeSteal.SetActive(true);
        isShooting = true;
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);

        playerHand.ResetTrigger("LifeStealStopCast");

        playerHand.SetTrigger(animationName);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Camera.main.transform.position + Camera.main.transform.forward * sphereRange, sphereRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Camera.main.transform.position + Camera.main.transform.forward * sphereRange, Camera.main.transform.position - Camera.main.transform.forward * sphereRange);
    }
}
