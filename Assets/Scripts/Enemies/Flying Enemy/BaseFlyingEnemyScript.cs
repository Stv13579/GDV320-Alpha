using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFlyingEnemyScript : BaseEnemyClass
{
    protected GameObject target;
    protected Vector3 targetPos;
    protected float timer = 0.0f;
    public float effectTimer;
    [HideInInspector]
    public float effectTimerMulti;
    protected bool effect = false;
    bool moving = false;

    public LayerMask moveDetect;

    float moveTimer = 0.0f;

    private void Start()
    {
        base.Start();
        FindTarget();
        effectTimerMulti = 1.0f;
    }

    private void Update()
    {
        if(!effect)
        {
            //Only start moving every few seconds, that way it's not constantly moving to try and approach its target
            if (moveTimer > 5)
            {
                if (Vector3.Distance(this.transform.position, targetPos) > 5)
                {
                    Movement();
                }
                else
                {
                    moveTimer = 0;
                    enemyAnims.SetTrigger("Stop Move");
                    enemyAnims.ResetTrigger("Move");


                }
            }
            moveTimer += Time.deltaTime;

            //Activate the effect every few seconds, with multiplier if we want it
            timer += Time.deltaTime;
            if(timer > effectTimer * effectTimerMulti)
            {
                effect = true;
                enemyAnims.SetTrigger("Effect");
            }

            targetPos = target.transform.position + new Vector3(0, 10, 0);
            this.transform.LookAt(targetPos);
            this.transform.eulerAngles += new Vector3(0, 90, 0);
        }




    }


    void Movement()
    {
        //Check if the path to its destination is clear, if not pick a new destination
        enemyAnims.SetTrigger("Move");
        RaycastHit hit;
        if(Physics.SphereCast(this.transform.position, 0.5f, (this.transform.position - targetPos).normalized, out hit, Vector3.Distance(this.transform.position, targetPos), moveDetect))
        {
            FindTarget();
        }
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, moveSpeed * moveSpeedMulti * Time.deltaTime);
    }



    //Locate the player for the crystal enemy, pick an enemy for the other variants
    protected virtual void FindTarget()
    {
        BaseEnemyClass[] enemies = FindObjectsOfType<BaseEnemyClass>();
        bool foundTarget = false;
        while (!foundTarget)
        {
            int i = Random.Range(0, enemies.Length);
            bool onlyFlying = true;
            //Check that there aren't only flying enemies left
            foreach(BaseEnemyClass enemy in enemies)
            {
                if (!enemy.gameObject.GetComponent<BaseFlyingEnemyScript>())
                {
                    onlyFlying = false;
                }
            }
            //If there aren't only flying enemies left, get all the enemies in the scene and pick a new one
            if(!onlyFlying)
            {
                if (enemies[i].gameObject != target)
                {
                    target = enemies[i].gameObject;
                }
                targetPos = target.transform.position + new Vector3(0, 10, 0);
                //Make sure the chosen enemy isn't a flying enemy, and that the path to them is clear
                if (!target.GetComponent<BaseFlyingEnemyScript>())
                {
                    RaycastHit hit;
                    if (!Physics.SphereCast(this.transform.position, 0.5f, (this.transform.position - targetPos).normalized, out hit, Vector3.Distance(this.transform.position, targetPos), moveDetect))
                    {
                        foundTarget = true;
                    }
                }
            }
            else
            {
                //Stay in idle I guess
            }

        }
    }

    protected virtual void Effect()
    {
        effect = false;
        timer = 0.0f;
    }
}
