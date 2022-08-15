using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossHomingProjectileScript : BaseEnemyClass
{
    public override void Update()
    {
        this.transform.LookAt(player.transform);
        this.transform.position += this.transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerClass>())
        {
            other.gameObject.GetComponent<PlayerClass>().ChangeHealth(-damageAmount);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.layer == 10)
        {
            Destroy(this.gameObject);
        }
    }

    public override void Death()
    {
        Destroy(this.gameObject);
    }
}
