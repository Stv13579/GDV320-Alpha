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

    public override void Awake()
    {
        base.Awake();
        StartCoroutine(FindNode());
    }
    public override void Update()
    {
        base.Update();
        if (!attacking)
        {
            if(Vector3.Distance(this.transform.position, player.transform.position) < 5 && shield)
            {
                //enemyAnims.SetTrigger("Attacking");
                Attack();
            }
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


    public override void Movement(Vector3 positionToMoveTo, float speed)
    {
        this.gameObject.transform.LookAt(positionToMoveTo);
        this.gameObject.transform.eulerAngles = new Vector3(0, this.gameObject.transform.eulerAngles.y, 0);
        movement = this.transform.forward * speed * Time.deltaTime;
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
