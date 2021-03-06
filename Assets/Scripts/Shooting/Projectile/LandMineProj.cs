using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineProj : MonoBehaviour
{
    private float damage;
    private float explosiveRadius;
    private float lifeTimer;
    List<BaseEnemyClass.Types> attackTypes;
    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        KillProjectile();
    }
    private void KillProjectile()
    {
        if(lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }
    public void SetVars(float dmg, float lifeTime, float explosionRadius, List<BaseEnemyClass.Types> types)
    {
        damage = dmg;
        lifeTimer = lifeTime;
        explosiveRadius = explosionRadius;
        attackTypes = types;

    }
    private void OnTriggerEnter(Collider other)
    {
        // other is an enemy
        // there is a ground, player and the enemy in explosion range

        if(other.gameObject.layer == 8)
        {
            Collider[] objectsHitByExplosion = Physics.OverlapSphere(this.transform.position, explosiveRadius);
            for(int i = 0; i < objectsHitByExplosion.Length; i++)
            {
                if (objectsHitByExplosion[i].gameObject.layer == 8)
                {
                    objectsHitByExplosion[i].GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
                }
            }
            Destroy(gameObject);
        }
    }
}
