﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalProj : BaseElementSpawnClass
{
    float speed;
    float damage;
    AnimationCurve damageCurve;
    float startLifeTimer;
    bool ismoving;
    float damageLimit;
    [SerializeField]
    GameObject particleEffect;
    float damageDecreaser;

    void Start()
    {
        ismoving = true;
    }
    // Update is called once per frame
    void Update()
    {
        // the max drop off the damage is 0.5f
        if(damage <= 0.5f)
        {
            damage = damageLimit;
        }
        // decrease the life of the crystal once its been shot out
        if(startLifeTimer > 0)
        {
            startLifeTimer -= Time.deltaTime;
        }
        // decrease the damage of the crystals every frame
        damage -= damageCurve.Evaluate(startLifeTimer) * damageDecreaser * Time.deltaTime;
        // moves the projectiles
        MoveCrystalProjectile();
        KillProjectile();
    }
    // move crystal projectile forwards
    void MoveCrystalProjectile()
    {
        if (ismoving == true)
        {
            Vector3 projMovement = transform.forward * speed * Time.deltaTime;
            transform.position += projMovement;
        }
    }
    //setter to set the varibles
    public void SetVars(float spd, float dmg, AnimationCurve dmgCurve, float stLifeTimer, List<BaseEnemyClass.Types> types, float tempDamageLimit, float tempDamageDecreaser)
    {
        speed = spd;
        damage = dmg;
        damageCurve = dmgCurve;
        startLifeTimer = stLifeTimer;
        attackTypes = types;
        damageLimit = tempDamageLimit;
        damageDecreaser = tempDamageDecreaser;
    }
    // if the life timer for the projectiles is 0
    // destroy the projectiles
    void KillProjectile()
    {
        if (startLifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // if bullet hits the environment
        // stops it from moving
        // gets embedded in the environment
        if (other.gameObject.layer == 10)
        {
            Destroy(gameObject);
        }
        //if enemy, hit them for the damage
        // destroy projectile after
        if (other.gameObject.layer == 8 && other.gameObject.GetComponent<BaseEnemyClass>())
        {
            other.gameObject.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            Destroy(gameObject);
        }
    }
}
