using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceSlashProj : BaseElementSpawnClass
{
    private float speed;
    private float damage;

    private float startLifeTimer;
    private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(startLifeTimer > 0)
        {
            startLifeTimer -= Time.deltaTime;
            //this.gameObject.GetComponent<Transform>().localScale += new Vector3(0.1f, 0, 0.1f);
        }
        MoveIceSlash();
        KillProjectile();
    }
    private void MoveIceSlash()
    {
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        transform.position += movement;
    }

    private void KillProjectile()
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

    private void OnTriggerEnter(Collider other)
    {
        // goes through enemies and damages them aswell
        if (other.gameObject.layer == 8)
        {
            other.gameObject.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
        }
        if (other.tag == "Shield")
        {
            other.gameObject.GetComponent<EnemyShield>().DamageShield(damage, attackTypes);
        }
        // hits environment and destroys itself
        if (other.gameObject.layer == 10)
        {
            if (startLifeTimer < 4.97f)
            {
                Destroy(gameObject);
            }
        }
    }
}
