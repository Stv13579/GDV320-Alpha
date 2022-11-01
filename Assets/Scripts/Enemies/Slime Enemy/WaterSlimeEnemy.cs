using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlimeEnemy : BaseEnemyClass
{
    protected float damageTicker = 0.0f;

    [SerializeField]
    protected float jumpForce;

    [SerializeField]
    protected float pushForce;

    [SerializeField]
    protected float attackJumpForce;

    [SerializeField]
    LayerMask viewToPlayer;

    [SerializeField]
    LayerMask nodeMask;

    protected int generation = 0;

    protected float jumpTimer = 0.0f;

    Vector3 pos = Vector3.zero;
	protected Vector3 posOffset = Vector3.zero;
    
	protected float hurtTimer = 0.0f;

    public override void Awake()
    {
        base.Awake();
        deathTriggers.Add(Split);
        jumpTimer = Random.Range(1f, 2f);
    }

    // damages the player
    // takes alway the players health
    public override void Attacking()
    {
        base.Attacking();
        //Add some force in the opposite direction
        Vector3 knockAway = player.transform.position - transform.position;
        knockAway.y = 10;
        GetComponent<Rigidbody>().AddForce(-knockAway * attackJumpForce);

        playerClass.ChangeHealth(-damageAmount * (prophecyManager.prophecyDamageMulti), gameObject);
    }
    //Moves towards the player if the slime can see them, othewise follow the flowfield to them
    public override void Movement(Vector3 positionToMoveTo)
    {
        base.Movement(moveDirection);

        jumpTimer -= Time.deltaTime;
        if(jumpTimer <= 0)
        {

            if (audioManager)
            {
                audioManager.StopSFX("Slime Bounce");
                audioManager.PlaySFX("Slime Bounce", player.transform, this.transform);
            }
            // slime is always looking at the player
            transform.LookAt(player.transform.position);
            Quaternion rot = transform.rotation;
            rot.eulerAngles = new Vector3(0, rot.eulerAngles.y + 135, 0);
            transform.rotation = rot;

            pos = moveDirection.normalized;
            Vector3 moveForce = positionToMoveTo.normalized * moveSpeed;
            if ((player.transform.position - transform.position).magnitude < 3)
            {
                moveForce = (player.transform.position - transform.position).normalized * moveSpeed * Time.deltaTime;
            }
            moveForce += new Vector3(0, jumpForce, jumpForce / 2);
	        GetComponent<Rigidbody>().AddForce(moveForce);
	        StartCoroutine(JumpCollision());
            jumpTimer = Random.Range(1f, 2f);
        }
        else
        {
            Movement(positionToMoveTo, moveSpeed / groundMoveSpeed);
        }
        
    }

    public override void Movement(Vector3 positionToMoveTo, float speed)
    {
        base.Movement(moveDirection);
        RaycastHit hit;

        Vector3 moveVec = positionToMoveTo.normalized * speed * Time.deltaTime;
        moveVec.y = 0;
        moveVec.y -= 1 * Time.deltaTime;

	    if((player.transform.position - transform.position).magnitude < 3)
        {
            moveVec = (player.transform.position - transform.position).normalized * speed * Time.deltaTime;
            moveVec.y = 0;
            moveVec.y -= 1 * Time.deltaTime;
        }

        transform.position += moveVec;

        // slime is always looking at the player
        transform.LookAt(player.transform.position);
        Quaternion rot = transform.rotation;
        rot.eulerAngles = new Vector3(0, rot.eulerAngles.y + 135, 0);
        transform.rotation = rot;

    }

	public override void Update()
    {
	    base.Update();

        
	    
    }

    public void FixedUpdate()
    {
        Movement(moveDirection);
        damageTicker -= Time.deltaTime;

    }

    // when the slime collides with the ground player audio for slime bounce
    // and add force to the slime so that it jumps
    public virtual void OnCollisionEnter(Collision collision)
    {
        // if colliding with player attack enemy reset damage ticker
        // we reset it so that the player doesn't take double damage
        if (collision.gameObject.tag == "Player")
        {
            Attacking();
	        damageTicker = 1.0f;
	        this.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }
    }
    public virtual void OnCollisionStay(Collision collision)
    {

        // checks if colliding with player and damage ticker is less then 0
        // player should take damage every one second after if they are still colliding with enemy normal slime
        if (collision.gameObject.tag == "Player" && damageTicker <= 0.0f)
        {
            Attacking();
            damageTicker = 1.0f;
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        // if colliding with player attack enemy reset damage ticker
        // we reset it so that the player doesn't take double damage
        if (other.gameObject.tag == "Player")
        {
            Attacking();
            damageTicker = 1.0f;
        }
    }

    public virtual void OnTriggerStay(Collider other)
    {
        // checks if colliding with player and damage ticker is less then 0
        // player should take damage every one second after if they are still colliding with enemy normal slime
        if (other.gameObject.tag == "Player" && damageTicker <= 0.0f)
        {
            Attacking();
            damageTicker = 1.0f;
        }
    }
    //When the slime dies, spawn two new smaller weaker ones
    protected virtual void Split(GameObject temp)
    {
        if(generation < 1)
        {
            for (int i = 0; i < 4; i++)
            {
	            GameObject newSlime = spawner.GetComponent<SAIM>().SetSpawn(gameObject, this.transform.position + (this.transform.right * ((i * 2) - 1) * 2) + this.transform.up * 4 + (new Vector3(Random.value, 0, Random.value) * 5));
	            newSlime.GetComponent<Rigidbody>().AddForce(this.transform.up * 5 + new Vector3(Random.value, 0, Random.value) * 5);
	            newSlime.GetComponent<WaterSlimeEnemy>().RestoreHealth(0);
	            StatModifier.AddModifier(newSlime.GetComponent<WaterSlimeEnemy>().GetHealthStat().multiplicativeModifiers, new StatModifier.Modifier(1.0f / (generation * 4 + 2), "Split " + generation));
	            StatModifier.AddModifier(newSlime.GetComponent<WaterSlimeEnemy>().GetDamageStat().multiplicativeModifiers, new StatModifier.Modifier(0.5f, "Split " + generation));
	            StatModifier.AddModifier(newSlime.GetComponent<WaterSlimeEnemy>().GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0.5f, "Split " + generation));
                newSlime.GetComponent<WaterSlimeEnemy>().transform.localScale = this.transform.localScale / 2;
                newSlime.GetComponent<WaterSlimeEnemy>().generation = generation + 1;
                newSlime.GetComponent<WaterSlimeEnemy>().spawner = spawner;
	            spawner.GetComponent<SAIM>().spawnedEnemies.Add(newSlime.GetComponent<WaterSlimeEnemy>());
	            newSlime.GetComponent<SphereCollider>().radius = 1.8f + (1.8f * 0.2f * generation);
            }
        }

    }
    
	public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1, bool applyTriggers = true)
	{
		base.TakeDamage(damageToTake, attackTypes, extraSpawnScale, applyTriggers);

	}

    protected override void ResetEnemy()
    {
        base.ResetEnemy();

        transform.localScale = new Vector3(0.6f, 0.6f, 0.6f); /*this.transform.localScale * (generation == 0 ? 0.5f : generation * 2);*/
	    GetComponent<SphereCollider>().radius = 1.8f;
	    StopAllCoroutines();
	    GetComponent<SphereCollider>().enabled = true;
        deathTriggers.Add(Split);
        generation = 0;
    }
    
	IEnumerator JumpCollision()
	{
		GetComponent<SphereCollider>().enabled = false;
		yield return new WaitForSeconds(0.1f);
		GetComponent<SphereCollider>().enabled = true;
		StopCoroutine(JumpCollision());

	}

}
