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

    public LayerMask viewToPlayer;

    public LayerMask nodeMask;

    [HideInInspector]
    public int generation = 0;

    protected float jumpTimer = 0.0f;

    Vector3 pos = Vector3.zero;
    protected Vector3 posOffset = Vector3.zero;

    public override void Start()
    {
        base.Start();
        deathTriggers.Add(Split);
        jumpTimer = Random.Range(1.5f, 3.0f);
    }

    // damages the player
    // takes alway the players health
    public override void Attacking()
    {
        base.Attacking();
        playerClass.ChangeHealth(-damageAmount * (damageMultiplier + prophecyManager.prophecyDamageMulti), transform.position, pushForce);
    }

    public override void Movement(Vector3 positionToMoveTo)
    {
        base.Movement(moveDirection);

        //RaycastHit hit;

        //Debug.DrawRay(transform.position + (Vector3.up * 10), Vector3.up /*player.transform.position - transform.position*/, Color.blue);
        //if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, Mathf.Infinity, viewToPlayer))
        //{
        //    if (hit.collider.gameObject.tag == "Player")
        //    {
        //        Vector3 moveVec = (player.transform.position - transform.position).normalized * moveSpeed * moveSpeedMulti * Time.deltaTime;
        //        moveVec.y = 0;
        //        moveVec.y -= 1 * Time.deltaTime;
        //        transform.position += moveVec;
        //    }
        //    else
        //    {
        //        Vector3 moveVec = (moveDirection - transform.position).normalized * moveSpeed * moveSpeedMulti * Time.deltaTime;
        //        moveVec.y = 0;
        //        moveVec.y -= 1 * Time.deltaTime;
        //        transform.position += moveVec;
        //    }


        //}
        //else
        //{
        //    Vector3 moveVec = (moveDirection - transform.position).normalized * moveSpeed * moveSpeedMulti * Time.deltaTime;
        //    moveVec.y = 0;
        //    moveVec.y -= 1 * Time.deltaTime;
        //    transform.position += moveVec;
        //}


        jumpTimer -= Time.deltaTime;
        if(jumpTimer <= 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, 10, viewToPlayer) && spawner)
            {
                float distance = float.MaxValue;
                Vector3 nearestNode = Vector3.zero;
                foreach(Node node in spawner.GetComponent<SAIM>().aliveNodes)
                {
                    float newDistance = Vector3.SqrMagnitude(node.gameObject.transform.position - this.transform.position);
                    if(newDistance < distance)
                    {
                        distance = newDistance;
                        nearestNode = node.bestNextNodePos;
                    }
                }
                transform.LookAt(nearestNode);
                Quaternion rot = transform.rotation;
                rot.eulerAngles = new Vector3(0, rot.eulerAngles.y + 135, 0);
                transform.rotation = rot;
                pos = nearestNode;
            }
            else
            {
                transform.LookAt(player.transform.position);
                Quaternion rot = transform.rotation;
                rot.eulerAngles = new Vector3(0, rot.eulerAngles.y + 135, 0);
                transform.rotation = rot;

                pos = player.transform.position + posOffset;
            }
            Vector3 moveForce = (pos - this.transform.position).normalized * moveSpeed * moveSpeedMulti;
            moveForce += new Vector3(0, jumpForce, 0);
            GetComponent<Rigidbody>().AddForce(moveForce);
            jumpTimer = Random.Range(1.5f, 3.0f);
        }
        else
        {
            if(Vector3.SqrMagnitude(this.transform.position - pos) > 10)
            {
                Vector3 dir = (pos - this.transform.position).normalized;
                Vector3 move = dir * ((moveSpeed * moveSpeedMulti) / 50) * Time.deltaTime;
                this.transform.position += move;
            }

        }
        
    }

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
                Vector3 moveVec = (player.transform.position - transform.position).normalized * speed * moveSpeedMulti * Time.deltaTime;
                moveVec.y = 0;
                moveVec.y -= 1 * Time.deltaTime;
                transform.position += moveVec;
            }
            else
            {
                Vector3 moveVec = (moveDirection - transform.position).normalized * speed * moveSpeedMulti * Time.deltaTime;
                moveVec.y = 0;
                moveVec.y -= 1 * Time.deltaTime;
                transform.position += moveVec;
            }


        }
        else
        {
            Vector3 moveVec = (moveDirection - transform.position).normalized * speed * moveSpeedMulti * Time.deltaTime;
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

    protected virtual void Split()
    {
        if(generation < 2)
        {
            for (int i = 0; i < 2; i++)
            {
                WaterSlimeEnemy newSlime = Instantiate(this.gameObject, this.transform.position + (this.transform.right * ((i * 2) - 1) * 2), Quaternion.identity).GetComponent<WaterSlimeEnemy>();
                newSlime.maxHealth = maxHealth / 2;
                newSlime.damageAmount = damageAmount / 2;
                newSlime.transform.localScale = this.transform.localScale / 2;
                newSlime.moveSpeed = moveSpeed / 2;
                newSlime.generation = generation + 1;
                newSlime.spawner = spawner;
                spawner.GetComponent<SAIM>().spawnedEnemies.Add(newSlime);
            }
        }

    }
}
