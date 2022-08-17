using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossFireProjectile : BaseRangedProjectileScript
{
    [HideInInspector]
    public GameObject telegraph;
    [HideInInspector]
    public float radius = 2.0f;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        this.transform.LookAt(telegraph.transform);
    }

    protected override void HitEffect(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);
        bool damaged = false;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<PlayerClass>() && !damaged && player.GetComponent<EnergyElement>().GetUseShield() == false)
            {
                collider.gameObject.GetComponent<PlayerClass>().ChangeHealth(-damage);
                damaged = true;
            }
        }
        Destroy(telegraph);
        Destroy(this.gameObject);
    }

    protected override void OnTriggerEnter(Collider other)
    {
        HitEffect(other);
    }
}
