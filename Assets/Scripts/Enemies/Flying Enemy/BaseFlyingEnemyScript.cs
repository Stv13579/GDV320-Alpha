using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFlyingEnemyScript : BaseEnemyClass //Sebastian
{
    protected GameObject target;
    protected Vector3 targetPos;
    protected float timer = 0.0f;
    [SerializeField]
    float effectTimer, effectRange;
    float effectTimerMulti = 1.0f;
    protected bool effect = false;
    bool moving = false;
    [SerializeField]
    protected LayerMask moveDetect;

    float moveTimer = 0.0f;

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

    private void Update()
    {
	    base.Update();
        {
            //Only start moving every few seconds, that way it's not constantly moving to try and approach its target
            //Move only if far from its target
            Debug.DrawRay(transform.position, (new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized * effectRange, Color.blue);
            Debug.Log(Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.transform.position.x, 0, target.transform.position.z)));
            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(target.transform.position.x, 0, target.transform.position.z))> effectRange)
            {
                Movement();
                enemyAnims.SetTrigger("ExitEffect");
            }
            else
            {
                effect = true;
                enemyAnims.SetTrigger("Effect");
                enemyAnims.ResetTrigger("ExitEffect");

            }

            //Activate the effect every few seconds, with multiplier if we want it
            //timer += Time.deltaTime;
            //if(timer > effectTimer * effectTimerMulti && !effect)
            //{
            //    effect = true;
            //    enemyAnims.SetTrigger("Effect");
            //}
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
        RaycastHit hit;
        if(Physics.SphereCast(this.transform.position, 0.5f, (this.transform.position - targetPos).normalized, out hit, Vector3.Distance(this.transform.position, targetPos), moveDetect))
        {
            FindTarget();
        }
        if(Vector3.Distance(this.transform.position, targetPos) < 5)
        {
            FindTarget();

        }
        if(target)
        {
            if(target.GetComponent<RangedEnemyScript>())
            {
                if(target.GetComponent<RangedEnemyScript>().GetBurrowing())
                {
                    FindTarget();
                }
            }

        }
        SetTargetPos();
        this.transform.position = Vector3.MoveTowards(this.transform.position, targetPos, moveSpeed * Time.deltaTime);
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
                //SetTargetPos();

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
            //SetTargetPos();
        }
        else
        {
            //target = this.gameObject;
            //targetPos = player.transform.position + new Vector3(0, 10, 0);
            //currentHealth = 0;
            //Death();
            target = player;
            //SetTargetPos();
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
}
