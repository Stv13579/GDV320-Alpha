using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossSmallCrystalProjectile : BaseRangedProjectileScript //Sebastian
{
    protected override void Start()
    {
        base.Start();
    }
    //Damage the player
    protected override void HitEffect(Collider other)
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damage, FindObjectOfType<RangedBossScript>().gameObject);
        Destroy(this.gameObject);
    }

    protected override void Update()
    {
        this.transform.position += this.transform.up * speed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer > 10)
        {
            Destroy(this.gameObject);
        }
    }
}
