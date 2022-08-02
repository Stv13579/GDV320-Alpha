using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemyScript : BaseEnemyClass
{
    Vector3 movement;
    public float gravity;
    GameObject nearestNode;
    bool attacking = false;

    private void Start()
    {
        base.Start();
        StartCoroutine(FindNode());
        spawner = GameObject.Find("SAIMPosition");
    }
    private void Update()
    {
        if (!attacking)
        {
            if(Vector3.Distance(this.transform.position, player.transform.position) < 2)
            {
                enemyAnims.SetTrigger("Attacking");
                attacking = true;
            }
            if (Vector3.Distance(this.transform.position, player.transform.position) < 5)
            {

                Movement(player.transform.position, moveSpeed * moveSpeedMulti);
            }
            else
            {
                Movement(nearestNode.GetComponent<Node>().bestNextNodePos, moveSpeed * moveSpeedMulti);
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
    }

    public void EndAttack()
    {
        //Enable and disable necessary hitboxes
        attacking = false;
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
}
