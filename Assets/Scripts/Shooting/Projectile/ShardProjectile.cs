using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardProjectile : BaseElementSpawnClass
{
    private float speed;

    private float damage;

    private int pierceAmount;

    [SerializeField]
    private GameObject impactSpawn;

    private void Update()
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            other.gameObject.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
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
