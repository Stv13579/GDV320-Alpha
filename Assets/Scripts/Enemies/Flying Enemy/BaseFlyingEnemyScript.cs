using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFlyingEnemyScript : BaseEnemyClass
{
    protected GameObject target;
    protected Vector3 targetPos;
    protected float timer = 0.0f;
    public float effectTimer;
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
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
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



        List<BaseEnemyClass> validEnemies = new List<BaseEnemyClass>();
        foreach (BaseEnemyClass enemy in enemies)
        {
            //Get a list of all non-flying enemies that the enemy can reach

            if (!enemy.gameObject.GetComponent<BaseFlyingEnemyScript>())
            {
                if (enemy.gameObject != target)
                {
                    target = enemy.gameObject;
                }
                targetPos = target.transform.position + new Vector3(0, 10, 0);
                RaycastHit hit;
                if (!Physics.SphereCast(this.transform.position, 0.5f, (this.transform.position - targetPos).normalized, out hit, Vector3.Distance(this.transform.position, targetPos), moveDetect))
                {
                    validEnemies.Add(enemy);
                }
            }
        }
        if(validEnemies.Count > 0)
        {
            int i = Random.Range(0, validEnemies.Count);
            target = validEnemies[i].gameObject;
            targetPos = target.transform.position + new Vector3(0, 10, 0);
        }
        else
        {
            target = this.gameObject;
            targetPos = player.transform.position + new Vector3(0, 10, 0);
        }
    }

    protected virtual void Effect()
    {
        effect = false;
        timer = 0.0f;
    }
}
