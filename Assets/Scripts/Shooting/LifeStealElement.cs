using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealElement : BaseElementClass
{
    [SerializeField]
    private GameObject lifeSteal;

    [SerializeField]
    private float damage;
    public float damageMultiplier = 1;

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

        RaycastHit hit;
        RaycastHit[] objectHit = Physics.SphereCastAll(Camera.main.transform.position, sphereRadius, Camera.main.transform.forward, sphereRange, hitLayer);
        if (Physics.SphereCast(Camera.main.transform.position, sphereRadius, Camera.main.transform.forward, out hit, sphereRange, hitLayer))
        {
            for (int i = 0; i < objectHit.Length; i++)
            {
                if (hit.distance < objectHit[i].distance)
                {
                    objectHit[i].distance = hit.distance;

                }
                if(objectHit[i].transform.gameObject.layer == environmentLayer && isShooting == true)
                {
                    isTargeting = false;
                    enemy = null;
                }
                if (objectHit[i].transform.gameObject.layer == 8 && isShooting == true)
                {
                    enemy = objectHit[i].transform.gameObject;
                    isTargeting = true;
                }
            }
        }
        else
        {
            DeactivateLifeSteal();
            enemy = null;
            return;
        }
        if (isTargeting == true && enemy != null)
        {
            enemy.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            playerClass.ChangeHealth(Time.deltaTime);
        }
    }
    private void DeactivateLifeSteal()
    {
        isShooting = false;
        lifeSteal.SetActive(false);
        playerHand.SetTrigger("LifeStealStopCast");
        playerHandL.SetTrigger("LifeStealStopCast");
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

    protected override void StartAnims(string animationName)
    {
        base.StartAnims(animationName);

        playerHand.ResetTrigger("LifeStealStopCast");
        playerHandL.ResetTrigger("LifeStealStopCast");

        playerHand.SetTrigger(animationName);
        playerHandL.SetTrigger(animationName);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(Camera.main.transform.position + Camera.main.transform.forward * sphereRange, sphereRadius);
    }
}
