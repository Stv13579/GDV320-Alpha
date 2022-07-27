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

            timer += Time.deltaTime;
            if(timer > effectTimer * effectTimerMulti)
            {
                effect = true;
                enemyAnims.SetTrigger("Effect");
            }
        }



    }


    void Movement()
    {
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
            target = enemies[i].gameObject;
            targetPos = target.transform.position + new Vector3(0, 10, 0);

            if (!target.GetComponent<BaseFlyingEnemyScript>())
            {
                RaycastHit hit;
                if (!Physics.SphereCast(this.transform.position, 0.5f, (this.transform.position - targetPos).normalized, out hit, Vector3.Distance(this.transform.position, targetPos), moveDetect))
                {
                    foundTarget = true;
                }
            }
        }
    }

    protected virtual void Effect()
    {
        effect = false;
        timer = 0.0f;
    }
}
