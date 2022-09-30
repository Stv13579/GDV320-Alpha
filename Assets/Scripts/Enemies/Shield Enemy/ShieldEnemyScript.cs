using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyScript : BaseEnemyClass
{
    Vector3 movement;
    [SerializeField]
    float gravity;
    GameObject nearestNode;
    bool attacking = false;
    [SerializeField]
    GameObject shield;
    [SerializeField]
    float attackTime = 0.8f;

    Vector3 previousPosition;

    bool shielding = false;

    public override void Awake()
    {
        base.Awake();

    }
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		StartCoroutine(FindNode());
        previousPosition = transform.position;
	}
    public override void Update()
    {
        base.Update();
        if (!attacking)
        {
            if(shielding)
            {
                if (Vector3.Distance(this.transform.position, player.transform.position) > 8)
                {

                    ShieldMovement(player.transform.position, moveSpeed);
                }
            }
            else
            {
                if (Vector3.Distance(this.transform.position, player.transform.position) < 8)
                {

                    Movement(player.transform.position, moveSpeed);
                }
                else
                {
                    Movement(nearestNode.GetComponent<Node>().bestNextNodePos, moveSpeed);
                }
            }
            
        }



        AssessShielding();

        enemyAnims.SetFloat("MoveSpeed", (previousPosition - transform.position).magnitude);
        previousPosition = transform.position;
    }

    void AssessShielding()
    {

        //Check if the angle between forward of the shield mushroom and the player to shield vector is small, then shield. If not, unshield.
        float angle = Vector3.Angle(transform.forward, player.transform.position - transform.position);

        if(angle < 90)
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
    }

    public override void Movement(Vector3 positionToMoveTo, float speed)
    {
        //this.gameObject.transform.LookAt(positionToMoveTo);
        //this.gameObject.transform.eulerAngles = new Vector3(0, this.gameObject.transform.eulerAngles.y, 0);


        //get the direction of rotation
        float dir = Mathf.Sign(Vector3.Angle(transform.forward, positionToMoveTo - transform.position));

        //rotate towards the disired vector/angle in that direction, modified by a scalar

        transform.Rotate(Vector3.up, dir * Time.deltaTime * rotationSpeed);

        
        //move along the forward vector


        movement = this.transform.forward * speed * Time.deltaTime;
        if (!this.gameObject.GetComponent<CharacterController>().isGrounded)
        {
            movement += new Vector3(0, gravity, 0);
        }
        this.gameObject.GetComponent<CharacterController>().Move(movement);

    }

    public void ShieldMovement(Vector3 positionToMoveTo, float speed)
    {
        movement = this.transform.forward * speed * Time.deltaTime * 0.4f;
        if (!this.gameObject.GetComponent<CharacterController>().isGrounded)
        {
            movement += new Vector3(0, gravity, 0);
        }
        this.gameObject.GetComponent<CharacterController>().Move(movement);
    }

    

    public void Attack()
    {
        //Enable and disable necessary hitboxes
        if(!attacking)
        {
            if(shield)
            {
                StartCoroutine(ShieldBash());
            }
        }
    }

    public void EndAttack()
    {
        //Enable and disable necessary hitboxes
    }

    IEnumerator FindNode()
    {
        while(true)
        {
            float distance = float.MaxValue;
            foreach(Node node in spawner.GetComponent<SAIM>().aliveNodes)
            {
                float newDistance = Vector3.Distance(node.gameObject.transform.position, this.gameObject.transform.position);
                if (newDistance < distance)
                {
                    distance = newDistance;
                    nearestNode = node.gameObject;
                }
            }
            yield return new WaitForSeconds(1);
        }
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
}
