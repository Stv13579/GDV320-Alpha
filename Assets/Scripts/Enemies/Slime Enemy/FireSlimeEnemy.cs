using System.Collections;
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
	        GameObject tempEnemyTrail = Instantiate(enemyTrail, transform.position + new Vector3(0, 1, 0), Quaternion.LookRotation(Vector3.down, forward));
            tempEnemyTrail.transform.localScale = enemyFireTrailScale;
            tempEnemyTrail.GetComponent<FireSlimeTrail>().SetVars(damageAmount * (prophecyManager.prophecyDamageMulti));
            if (audioManager)
            {
                audioManager.StopSFX("Fire Slime Trail Initial");
                audioManager.PlaySFX("Fire Slime Trail Initial", player.transform, this.transform);
            }
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
	            FireSlimeEnemy newSlime = Instantiate(this.gameObject, this.transform.position + (this.transform.right * ((i * 2) - 1) * 2) + this.transform.up * 4, Quaternion.identity).GetComponent<FireSlimeEnemy>();
	            newSlime.GetComponent<Rigidbody>().AddForce(this.transform.up * 5 + this.transform.forward * 5);
	            newSlime.RestoreHealth(0);
	            StatModifier.AddModifier(newSlime.GetHealthStat().multiplicativeModifiers, new StatModifier.Modifier(1.0f / (generation * 4 + 2), "Split " + generation));
                StatModifier.AddModifier(newSlime.GetDamageStat().multiplicativeModifiers, new StatModifier.Modifier(0.5f, "Split " + generation));
                StatModifier.AddModifier(newSlime.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0.5f, "Split " + generation));
                newSlime.transform.localScale = this.transform.localScale / 2;
                newSlime.generation = generation + 1;
                newSlime.spawner = spawner;
                spawner.GetComponent<SAIM>().spawnedEnemies.Add(newSlime);
	            newSlime.enemyFireTrailScale = enemyFireTrailScale / 2;
	            newSlime.GetComponent<SphereCollider>().radius *= 1.2f * generation + 1;


            }
        }

    }
}
