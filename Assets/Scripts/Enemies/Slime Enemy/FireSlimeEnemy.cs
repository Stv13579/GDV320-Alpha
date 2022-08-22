﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlimeEnemy : WaterSlimeEnemy
{
    [SerializeField]
    GameObject enemyTrail;
    [SerializeField]
    private LayerMask trailLayerMask;
    [SerializeField]
    private Vector3 enemyFireTrailScale;
    [SerializeField]
    private DecalRendererManager decalManager;
    [SerializeField]
    private float spawnTimer;
    [SerializeField]
    private float spawnTimerLength = 1.0f;
    public override void Awake()
    {
        base.Awake();
        decalManager = FindObjectOfType<DecalRendererManager>();
    }
    new private void Update()
    {
        base.Update();
        spawnTimer -= Time.deltaTime;
    }
    public override void Attacking()
    {
        base.Attacking();
    }
    //Spawn a patch of fire beneath the slime
    public void FireSlimeAttack()
    {
        if (spawnTimer <= 0.0f)
        { 
            float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
            Vector3 forward = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
            // creates a plane which is the trail of the fire slime
            GameObject tempEnemyTrail = Instantiate(enemyTrail, transform.position, Quaternion.LookRotation(Vector3.down, forward));
            tempEnemyTrail.transform.localScale = enemyFireTrailScale;
            tempEnemyTrail.GetComponent<FireSlimeTrail>().SetVars(damageAmount * (damageMultiplier + prophecyManager.prophecyDamageMulti));
            audioManager.Stop("Fire Slime Trail Initial");
            audioManager.Play("Fire Slime Trail Initial", player.transform, this.transform);
            spawnTimer = spawnTimerLength;
        }
    }
    //Move around the player to try and trap them in fire
    public override void Movement(Vector3 positionToMoveTo)
    {
        float off = Random.Range(0.0f, 6.0f);
        posOffset = new Vector3(Mathf.Cos(off), 0, Mathf.Sin(off)) * 8;
        base.Movement(positionToMoveTo);
    }

    public override void OnCollisionEnter(Collision collision)
    {
        base.OnCollisionEnter(collision);
        if (collision.gameObject.layer == 10)
        {
            FireSlimeAttack();
        }
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
        if (generation < 2)
        {
            for (int i = 0; i < 2; i++)
            {
                FireSlimeEnemy newSlime = Instantiate(this.gameObject, this.transform.position + (this.transform.right * ((i * 2) - 1) * 2), Quaternion.identity).GetComponent<FireSlimeEnemy>();
                newSlime.maxHealth = maxHealth / 2;
                newSlime.damageAmount = damageAmount / 2;
                newSlime.transform.localScale = this.transform.localScale / 2;
                newSlime.moveSpeed = moveSpeed / 2;
                newSlime.generation = generation + 1;
                newSlime.spawner = spawner;
                spawner.GetComponent<SAIM>().spawnedEnemies.Add(newSlime);
                newSlime.enemyFireTrailScale = enemyFireTrailScale / 2;
                newSlime.GetMoveMultis().Clear();

            }
        }

    }
}
