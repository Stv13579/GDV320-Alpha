using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSlimeEnemy : WaterSlimeEnemy
{
    public GameObject enemyTrail;
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
    public override void Start()
    {
        base.Start();
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

    public void FireSlimeAttack()
    {
        if (spawnTimer <= 0.0f)
        { 
            float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
            Vector3 forward = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
            // creates a plane which is the trail of the fire slime
            GameObject tempEnemyTrail = Instantiate(enemyTrail, transform.position, Quaternion.LookRotation(Vector3.down, forward));
            tempEnemyTrail.transform.localScale = enemyFireTrailScale;
            tempEnemyTrail.GetComponent<FireSlimeTrail>().SetVars(damageAmount);
            audioManager.Stop("Fire Slime Trail Initial");
            audioManager.Play("Fire Slime Trail Initial", player.transform, this.transform);
            spawnTimer = spawnTimerLength;
        }
    }
    public override void Movement(Vector3 positionToMoveTo)
    {
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

    protected override void Split()
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
            }
        }

    }
}
