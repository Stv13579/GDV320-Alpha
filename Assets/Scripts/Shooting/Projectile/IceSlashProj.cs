using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSlashProj : BaseElementSpawnClass
{
    float speed;
    float damage;
    float startLifeTimer;

    // Update is called once per frame
    void Update()
    {
        startLifeTimer -= Time.deltaTime;
        MoveIceSlash();
        KillProjectile();
    }
    // moves the ice slash
    void MoveIceSlash()
    {
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        transform.position += movement;
    }

    void KillProjectile()
    {
        if(startLifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void SetVars(float spd, float dmg, float stTimer, List<BaseEnemyClass.Types> types)
    {
        speed = spd;
        damage = dmg;
        startLifeTimer = stTimer;
        attackTypes = types;
    }

    void OnTriggerEnter(Collider other)
    {
        // goes through enemies and damages them aswell
	    if (other.gameObject.layer == 8 && other.gameObject.GetComponentInParent<BaseEnemyClass>() && other.isTrigger == false || other.tag == "Enemy" && other.isTrigger == false)
        {
		    other.gameObject.GetComponentInParent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
        }
    }
}
