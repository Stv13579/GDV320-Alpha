using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSlimeEnemy : BaseEnemyClass
{
    float damageTicker = 0.0f;

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
    
	float hurtTimer = 0.0f;

    public override void Awake()
    {
        base.Awake();
        deathTriggers.Add(Split);
        jumpTimer = Random.Range(1.5f, 3.0f);
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

        playerClass.ChangeHealth(-damageAmount * (prophecyManager.prophecyDamageMulti), transform.position, pushForce);
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
            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 10, viewToPlayer) && spawner)
            {
                float distance = float.MaxValue;
                Vector3 nearestNode = Vector3.zero;
                foreach (Node node in spawner.GetComponent<SAIM>().aliveNodes)
                {
                    float newDistance = Vector3.SqrMagnitude(node.gameObject.transform.position - this.transform.position);
                    if (newDistance < distance)
                    {
                        distance = newDistance;
                        nearestNode = node.bestNextNodePos;
                    }
                }
                transform.LookAt(nearestNode);
                Quaternion rot = transform.rotation;
                rot.eulerAngles = new Vector3(0, rot.eulerAngles.y + 135, 0);
                transform.rotation = rot;
                pos = moveDirection;
            }
            else
            {
                transform.LookAt(player.transform.position);
                Quaternion rot = transform.rotation;
                rot.eulerAngles = new Vector3(0, rot.eulerAngles.y + 135, 0);
                transform.rotation = rot;

                pos = player.transform.position + posOffset;
            }
            Vector3 moveForce = (pos - this.transform.position).normalized * moveSpeed;
            moveForce += new Vector3(0, jumpForce, 0);
            GetComponent<Rigidbody>().AddForce(moveForce);
            jumpTimer = Random.Range(1.5f, 3.0f);
        }
        else
        {
            //if(Vector3.SqrMagnitude(this.transform.position - pos) > 10)
            //{
            //    Vector3 dir = (pos - this.transform.position).normalized;
            //    Vector3 move = dir * ((moveSpeed) / 50) * Time.deltaTime;
            //    this.transform.position += move;
            //}
            Movement(positionToMoveTo, moveSpeed / groundMoveSpeed);
        }
        
    }
    //Unused
    public override void Movement(Vector3 positionToMoveTo, float speed)
    {
        base.Movement(moveDirection);

        RaycastHit hit;

        //If they can see the player, go for it, otherwise pathfind
        Debug.DrawRay(transform.position + (Vector3.up * 10), Vector3.up /*player.transform.position - transform.position*/, Color.blue);
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, Mathf.Infinity, viewToPlayer))
        {
            if(hit.collider.gameObject.tag == "Player")
            {
                Vector3 moveVec = (player.transform.position - transform.position).normalized * speed * Time.deltaTime;
                moveVec.y = 0;
                moveVec.y -= 1 * Time.deltaTime;
                transform.position += moveVec;
            }
            else
            {
                Vector3 moveVec = (moveDirection - transform.position).normalized * speed * Time.deltaTime;
                moveVec.y = 0;
                moveVec.y -= 1 * Time.deltaTime;
                transform.position += moveVec;
            }


        }
        else
        {
            Vector3 moveVec = (moveDirection - transform.position).normalized * speed * Time.deltaTime;
            moveVec.y = 0;
            moveVec.y -= 1 * Time.deltaTime;
            transform.position += moveVec;
        }




        // slime is always looking at the player
        transform.LookAt(player.transform.position);
        Quaternion rot = transform.rotation;
        rot.eulerAngles = new Vector3(0, rot.eulerAngles.y + 135, 0);
        transform.rotation = rot;

    }

    protected virtual void Update()
    {
	    base.Update();
        
        Movement(player.transform.position);
	    damageTicker -= Time.deltaTime;
	    if(hurtTimer > 0)
	    {
	    	hurtTimer -= Time.deltaTime;
	    	if(hurtTimer <= 0)
	    	{
		    	this.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Renderer>().material.SetFloat("_IsBeingDamaged", 0);

	    	}
	    	
	    }
	    
    }
    
    // when the slime collides with the ground player audio for slime bounce
    // and add force to the slime so that it jumps
    public virtual void OnCollisionEnter(Collision collision)
    {
        //if (GetComponent<Rigidbody>().velocity.y < 10 && collision.gameObject.layer == 10)
        //{
        //    audioManager.Stop("Slime Bounce");
        //    audioManager.Play("Slime Bounce", player.transform, this.transform);
        //    GetComponent<Rigidbody>().AddForce(0, jumpForce, 0);
        //}
        // if colliding with player attack enemy reset damage ticker
        // we reset it so that the player doesn't take double damage
        if (collision.gameObject.tag == "Player")
        {
            Attacking();
	        damageTicker = 1.0f;
	        GetComponent<Rigidbody>().velocity *= -1;
        }
    }
    public virtual void OnCollisionStay(Collision collision)
    {
        //if (GetComponent<Rigidbody>().velocity.y < 10 && collision.gameObject.layer == 10)
        //{
        //    audioManager.Stop("Slime Bounce");
        //    audioManager.Play("Slime Bounce", player.transform, this.transform);
        //    GetComponent<Rigidbody>().AddForce(0, jumpForce, 0);
        //}

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
        if(generation < 2)
        {
            for (int i = 0; i < 2; i++)
            {
                GameObject newSlime = Instantiate(this.gameObject, this.transform.position + (this.transform.right * ((i * 2) - 1) * 2) + this.transform.up * 2, Quaternion.identity);
                newSlime.GetComponent<WaterSlimeEnemy>().maxHealth = maxHealth / 2;
                newSlime.GetComponent<WaterSlimeEnemy>().baseMaxHealth = baseMaxHealth / 2;
                newSlime.GetComponent<WaterSlimeEnemy>().health.baseValue = maxHealth / 2;
                newSlime.GetComponent<WaterSlimeEnemy>().damageAmount = damageAmount / 2;
                newSlime.GetComponent<WaterSlimeEnemy>().transform.localScale = this.transform.localScale / 2;
                newSlime.GetComponent<WaterSlimeEnemy>().moveSpeed = moveSpeed / 2;
                newSlime.GetComponent<WaterSlimeEnemy>().generation = generation + 1;
                newSlime.GetComponent<WaterSlimeEnemy>().spawner = spawner;
                spawner.GetComponent<SAIM>().spawnedEnemies.Add(newSlime.GetComponent<WaterSlimeEnemy>());
            }
        }

    }
    
	public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1, bool applyTriggers = true)
	{
		base.TakeDamage(damageToTake, attackTypes, extraSpawnScale, applyTriggers);
		this.transform.GetChild(1).GetChild(1).gameObject.GetComponent<Renderer>().material.SetFloat("_IsBeingDamaged", 1);
		hurtTimer = 0.2f;
	}
	
}
