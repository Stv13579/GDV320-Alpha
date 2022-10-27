using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFlyingEnemyScript : BaseEnemyClass //Sebastian
{
    protected GameObject target;
    protected Vector3 targetPos;
    protected float timer = 0.0f;
    [SerializeField]
    protected float effectTimer, effectRange;
    float effectTimerMulti = 1.0f;
    protected bool effect = false;
    bool hitHeight = false;
    [SerializeField]
    float bobHeight;
    float bobbedAmount = 0;
    [SerializeField]
    protected LayerMask moveDetect;

    [SerializeField]
    float hoverHeight;

    [SerializeField]
    protected ParticleSystem buffingVFX;

    public override void Awake()
    {
        base.Awake();
        effectTimerMulti = 1.0f;
    }

    private void Start()
    {
        //Doing this in start means that if the flying enemy is first in the run order, it will kill itself since no other enemies are alive yet.
        //This is what kills the flying enemies on spawn for seemingly no reason.
        FindTarget();
    }

	public override void Update()
    {
	    base.Update();
        {
            //Only start moving every few seconds, that way it's not constantly moving to try and approach its target
            //Move only if far from its target
            
            if (target && Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.transform.position.x, 0, target.transform.position.z))> effectRange)
            {
                Movement();
                enemyAnims.SetTrigger("ExitEffect");
            }
            else
            {
                effect = true;
                enemyAnims.SetTrigger("Effect");
                enemyAnims.ResetTrigger("ExitEffect");
                Bob();
            }

            Quaternion startRot = this.transform.rotation;
            this.transform.LookAt(targetPos);
            this.transform.eulerAngles += new Vector3(0, 90, 0);
            this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
            this.transform.rotation = Quaternion.RotateTowards(startRot, this.transform.rotation, 90.0f * Time.deltaTime);

        }
    }

    //Makes sure the target position is valid, if so move towards it, if not find a new target
    void Movement()
    {
        //Check if the path to its destination is clear, if not pick a new destination

        FindTarget();
        SetTargetPos();
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, moveSpeed * Time.deltaTime);
    }

    //Locate the player for the crystal enemy, pick an enemy for the other variants
    protected virtual void FindTarget()
    {
        //List<BaseEnemyClass> validEnemies = new List<BaseEnemyClass>();
        foreach (BaseEnemyClass enemy in spawner.GetComponent<SAIM>().spawnedEnemies)
        {
            //Get a list of all non-flying enemies that the enemy can reach

            if (!enemy.gameObject.GetComponent<BaseFlyingEnemyScript>())
            {
                if (enemy.gameObject != target)
                {
                    target = enemy.gameObject;
                }
                //SetTargetPos();

                RaycastHit hit;
                if (!Physics.Raycast(this.transform.position, (this.transform.position - target.transform.position).normalized, out hit, Vector3.Distance(this.transform.position, target.transform.position), moveDetect))
                {
                    //validEnemies.Add(enemy);
                    break;
                }
            }
        }
    }

    void SetTargetPos()
    {
        targetPos = target.transform.position;
        //ray cast down from target pos to find the floor, place it there (on y) then move it up
        RaycastHit targetHit;
        Physics.Raycast(targetPos + new Vector3(0, 1000, 0), Vector3.down, out targetHit, 10000, moveDetect);
        targetPos = targetHit.point + new Vector3(0, hoverHeight, 0);
    }

	public virtual void Effect()
    {
        effect = false;
        timer = 0.0f;
        enemyAnims.ResetTrigger("Effect");

        buffingVFX.Play();

        if (audioManager)
        {
            audioManager.StopSFX(attackAudio);
            audioManager.PlaySFX(attackAudio);
        }
    }

    public float GetEffectMulti()
    {
        return effectTimerMulti;
    }

    public void SetEffectMulti(float multi)
    {
        effectTimerMulti = multi;
    }

    //Move up and down
    void Bob()
    {
        if(hitHeight)
        {
            if(bobbedAmount <= 0)
            {
                hitHeight = false;
            }
            else
            {
                Vector3 pos = transform.position;
                pos.y -= Time.deltaTime * 3;
                transform.position = pos;
                bobbedAmount -= Time.deltaTime;
            }
        }
        else
        {
            if(bobbedAmount >= bobHeight)
            {
                hitHeight = true;
            }
            else
            {
                Vector3 pos = transform.position;
                pos.y += Time.deltaTime * 3;
                transform.position = pos;
                bobbedAmount += Time.deltaTime;
            }
        }
    }
}
