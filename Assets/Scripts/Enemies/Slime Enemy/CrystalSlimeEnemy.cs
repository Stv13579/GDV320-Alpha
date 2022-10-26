﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSlimeEnemy : WaterSlimeEnemy
{
    [SerializeField]
    private float shootTimer = 0.0f;
    [SerializeField]
    private float shootTimerLength = 2.0f;
    [SerializeField]
    private GameObject enemyProjectile;
    [SerializeField]
    private Vector3 enemyProjectileScale;
    // Update is called once per frame
    new private void Update()
    {
        base.Update();
        CrystalSlimeAttack();
        shootTimer += Time.deltaTime;
    }

    public override void Attacking()
    {
        base.Attacking();
    }
    // Fire a circle of crystals at the player
    public void CrystalSlimeAttack()
    {
        if (shootTimer >= shootTimerLength)
        {
            // instanciates 5 projectiles above itself
            for (int i = 0; i < 5; i++)
            {
	            GameObject tempEnemyProjectile = EnemyAttackPool.GetAttackPool().GetReadiedAttack(enemyProjectile);
	            tempEnemyProjectile.SetActive(true);
	            tempEnemyProjectile.transform.position = transform.position + new Vector3(0.0f, 3.0f, 0.0f);
	            tempEnemyProjectile.transform.rotation = Quaternion.identity;
                tempEnemyProjectile.GetComponent<CrystalSlimeProjectile>().enemy = gameObject;
                // ignores physics for the with the crystal slime and the enemy crystal slime projectiles 
                Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), tempEnemyProjectile.GetComponent<Collider>());
                // setting scale of enemy projectile based on enemy size
                tempEnemyProjectile.transform.localScale = enemyProjectileScale;
                // setter to set variables from CrystalSlimeProject
                tempEnemyProjectile.GetComponent<CrystalSlimeProjectile>().SetVars(damageAmount * (prophecyManager.prophecyDamageMulti));
                //setting the rotations of the projectiles so that it spawns in like a circle
                tempEnemyProjectile.transform.eulerAngles = new Vector3(tempEnemyProjectile.transform.eulerAngles.x, tempEnemyProjectile.transform.eulerAngles.y + (360.0f / 5.0f * i), tempEnemyProjectile.transform.eulerAngles.z);
	            tempEnemyProjectile.GetComponent<CrystalSlimeProjectile>().Shoot();
                if (audioManager)
                {
                    // play SFX
                    audioManager.StopSFX("Crystal Slime Projectile");
                    audioManager.PlaySFX("Crystal Slime Projectile", player.transform, this.transform);
                }
                //Play animation
                enemyAnims.SetTrigger("Shoot");
            }
            shootTimer = 0.0f;
        }
    }
    //Target a position slightly away from the player so it can shoot the player from a distance
    //public override void Movement(Vector3 positionToMoveTo)
    //{
    //    posOffset = (this.transform.position - player.transform.position).normalized * 10;
    //    posOffset.y = 0;
    //    base.Movement(positionToMoveTo);
    //}
    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
    }
    public override void OnCollisionStay(Collision collision)
    {
        base.OnCollisionStay(collision);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }
    //When the slime dies, spawn two new smaller weaker ones
    protected override void Split(GameObject temp)
    {
        if (generation < 1)
        {
            for (int i = 0; i < 4; i++)
            {
	            CrystalSlimeEnemy newSlime = spawner.GetComponent<SAIM>().SetSpawn(gameObject, this.transform.position + (this.transform.right * ((i * 2) - 1) * 2) + this.transform.up * 4).GetComponent<CrystalSlimeEnemy>();
	            newSlime.GetComponent<Rigidbody>().AddForce(this.transform.up * 5 + this.transform.forward * 5);
	            newSlime.RestoreHealth(0);
	            StatModifier.AddModifier(newSlime.GetHealthStat().multiplicativeModifiers, new StatModifier.Modifier(1.0f / (generation * 4 + 2), "Split " + generation));
                StatModifier.AddModifier(newSlime.GetDamageStat().multiplicativeModifiers, new StatModifier.Modifier(0.5f, "Split " + generation));
                StatModifier.AddModifier(newSlime.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0.5f, "Split " + generation));
                newSlime.transform.localScale = this.transform.localScale / 2;
                newSlime.generation = generation + 1;
                newSlime.spawner = spawner;
                spawner.GetComponent<SAIM>().spawnedEnemies.Add(newSlime);

	            newSlime.GetComponent<SphereCollider>().radius = 1.8f + (1.8f * 0.2f * generation);
	            newSlime.enemyProjectileScale = enemyProjectileScale / 1.5f;

            }
        }

    }
}
