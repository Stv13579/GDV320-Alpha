﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyScript : BaseEnemyClass
{
    Vector3 movement;
    [SerializeField]
    float gravity;
    public bool attacking = false;
    [SerializeField]
    GameObject shield;
    [SerializeField]
    float attackTime = 0.8f;

    [SerializeField]
    public GameObject capHitter;

    Vector3 previousPosition;

    bool shielding = false;

    [SerializeField]
    LayerMask checkToSee;

    bool guardBroken = false;

    [SerializeField]
    float brokenShieldTimer;

    float guardTimer;

    [SerializeField]
    int moveDelayFrames;
    int currentDelayFrames;

    float timeSinceLastMove;

    public override void Awake()
    {
        base.Awake();

    }
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
        previousPosition = transform.position;
	}
    public override void Update()
    {
        base.Update();

        if(guardBroken)
        {
            guardTimer -= Time.deltaTime;

            if(guardTimer < 0)
            {
                guardBroken = false;
                enemyAnims.SetTrigger("Shield Regen");
                SetShield(true);
            }

            return;
        }

        RaycastHit hitInfo;
        if (shielding)
        {


            //Debug.DrawRay(transform.position, player.transform.position - transform.position);
            //ShieldRotation(player.transform.position, moveSpeed);
            if (!Physics.Raycast(transform.position, player.transform.position - transform.position, out hitInfo, Vector3.Distance(this.transform.position, player.transform.position), checkToSee))
            {


                ShieldRotation(player.transform.position, moveSpeed);
            }
            else
            {
                ShieldRotation(transform.position + moveDirection.normalized, moveSpeed);
                //ShieldMovement(player.transform.position, moveSpeed);
            }
            //ShieldRotation(nearestNode.GetComponent<Node>().bestNextNodePos, moveSpeed);

            ShieldMovement(player.transform.position, moveSpeed);

            if (Vector3.Distance(this.transform.position, player.transform.position) < 3 && !attacking)
            {
                Attack();
            }
        }
        else
        {

            //if (!Physics.Raycast(transform.position, player.transform.position - transform.position, out hitInfo, Vector3.Distance(this.transform.position, player.transform.position), checkToSee))
            //{


            //    Movement(player.transform.position, moveSpeed);
            //}
            //else
            //{
            //    Movement(nearestNode.GetComponent<Node>().bestNextNodePos, moveSpeed);
            //}

            //if (currentDelayFrames > moveDelayFrames)
            //{
            //    Movement(nearestNode.GetComponent<Node>().bestNextNodePos, moveSpeed);
            //    timeSinceLastMove = 0;
            //}
            //else
            //{
            //    timeSinceLastMove += Time.deltaTime;
            //    currentDelayFrames++;
            //}

            Movement(transform.position + moveDirection.normalized, moveSpeed);
        }

        AssessShielding();

        enemyAnims.SetFloat("MoveSpeed", (previousPosition - transform.position).magnitude);
        previousPosition = transform.position;
    }

    void AssessShielding()
    {

        //Check if the angle between forward of the shield mushroom and the player to shield vector is small, then shield. If not, unshield.
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);
        if(angle < 75)
        {
            
            enemyAnims.SetTrigger("Shield Up");
            
        }
        else
        {
            enemyAnims.SetTrigger("Shield Down");
        }
    }

    public void SetShield(bool state)
    {
        shielding = state;
        shield.SetActive(state);
        shield.transform.GetChild(0).gameObject.SetActive(state);
    }

    public override void Movement(Vector3 positionToMoveTo, float speed)
    {
        //this.gameObject.transform.LookAt(positionToMoveTo);
        //this.gameObject.transform.eulerAngles = new Vector3(0, this.gameObject.transform.eulerAngles.y, 0);
        float angleBetwixt = Vector3.Angle(transform.forward, positionToMoveTo - transform.position);

        //get the direction of rotation
        float dir = Mathf.Sign(Vector3.SignedAngle(transform.forward, positionToMoveTo - transform.position, Vector3.up));

        //rotate towards the disired vector/angle in that direction, modified by a scalar

        transform.Rotate(Vector3.up, dir * Time.deltaTime * rotationSpeed);

        if( angleBetwixt < 10 && angleBetwixt > -10)
        {
            transform.LookAt(positionToMoveTo);
            transform.eulerAngles = new Vector3(0, this.gameObject.transform.eulerAngles.y, 0);
        }
        //move along the forward vector


        movement = transform.forward * speed * (Time.deltaTime + timeSinceLastMove);
        if (!gameObject.GetComponent<CharacterController>().isGrounded)
        {
            movement += new Vector3(0, gravity, 0);
            //transform.position += new Vector3(0, gravity, 0);
        }
        gameObject.GetComponent<CharacterController>().Move(movement);

    }

    //Rotate much slower
    public void ShieldMovement(Vector3 positionToMoveTo, float speed)
    {
        //this.gameObject.transform.LookAt(positionToMoveTo);
        //this.gameObject.transform.eulerAngles = new Vector3(0, this.gameObject.transform.eulerAngles.y, 0);


        
        movement = transform.forward * speed * (Time.deltaTime + timeSinceLastMove);
        if (!gameObject.GetComponent<CharacterController>().isGrounded)
        {
            movement += new Vector3(0, gravity, 0);
            //transform.position += new Vector3(0, gravity, 0);
        }

        
        gameObject.GetComponent<CharacterController>().Move(movement);

    }

    public void ShieldRotation(Vector3 positionToMoveTo, float speed)
    {
        //get the direction of rotation
        float dir = Mathf.Sign(Vector3.SignedAngle(transform.forward, positionToMoveTo - transform.position, Vector3.up));

        //rotate towards the disired vector/angle in that direction, modified by a scalar

        transform.Rotate(Vector3.up, dir * Time.deltaTime * rotationSpeed * 0.1f);

        //if (Vector3.Angle(transform.forward, positionToMoveTo - transform.position) < 10 && Vector3.Angle(transform.forward, positionToMoveTo - transform.position) > -10)
        //{
        //    transform.LookAt(positionToMoveTo);
        //    transform.eulerAngles = new Vector3(0, this.gameObject.transform.eulerAngles.y, 0);
        //}



    }


    public void Attack()
    {

        attacking = true;
        enemyAnims.SetTrigger("Attacking");
        capHitter.SetActive(true);
        SetShield(false);
    }

    public void CapDamage()
    {
        player.GetComponent<PlayerClass>().ChangeHealth(-damageAmount, gameObject);
        capHitter.SetActive(false);
        
    }

    public void EndAttack()
    {
        //Enable and disable necessary hitboxes
    }

    IEnumerator ShieldBash()
    {
        Vector3 startPos = shield.transform.localPosition;
        Vector3 endPos = startPos + new Vector3(0, 0, 5);
        float timer = 0.0f;
        attacking = true;
        shield.GetComponent<EnemyShield>().StartAttack();
        while(timer < attackTime)
        {
            if(shield)
            {
                shield.transform.localPosition = Vector3.Lerp(startPos, endPos, timer / attackTime);
                timer += Time.deltaTime;
            }
            else
            {
                StopCoroutine(ShieldBash());
            }
            yield return null;
        }
        while (timer > 0)
        {
            if(shield)
            {
                shield.transform.localPosition = Vector3.Lerp(startPos, endPos, timer / attackTime);
                timer -= Time.deltaTime;
            }
            else
            {
                StopCoroutine(ShieldBash());
            }
            yield return null;
        }
        shield.GetComponent<EnemyShield>().EndAttack();

        attacking = false;

    }

    public void GuardBreak()
    {
        guardBroken = true;
	    enemyAnims.SetTrigger("Shield Broken");
	    foreach(Material mat in enemyMat)
	    {
	    	mat.SetFloat("IsStunned", 1);
	    }
        guardTimer = brokenShieldTimer;
        SetShield(false);
    }

}
