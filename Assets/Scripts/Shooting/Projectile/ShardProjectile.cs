using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardProjectile : BaseElementSpawnClass
{
    float speed;
	float damage;
	[SerializeField]
    int pierceAmount;
    [SerializeField]
    GameObject impactSpawn;

    void Update()
    {
        Vector3 movement = transform.up * speed * Time.deltaTime;
        transform.position += movement;
    }

    public void SetVars(float spd, float dmg, List<BaseEnemyClass.Types> types)
    {
        speed = spd;
        damage = dmg;
        attackTypes = types;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 || other.tag == "Enemy")
        {
	        other.gameObject.GetComponentInParent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
        }

        if (other.gameObject.tag != "Player")
        {
            Instantiate(impactSpawn, transform.position, Quaternion.identity);
            if (pierceAmount > 0)
            {
                pierceAmount--;
            }
            else
            {
                Destroy(gameObject);
            }            
        }


    }
}
