﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyScript : BaseEnemyClass //Sebastian
{
    float timer = 0;
    [SerializeField]
    float attackTime;
    float attackTimeMulti = 1.0f;
    [SerializeField]
    LayerMask viewToPlayer;
    [SerializeField]
    float burrowTime;
    bool burrowing = false;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    float projectileSpeed;

    float destroyTimer = 0.0f;


    [SerializeField]
    Transform projectileSpawnPos;
    [SerializeField]
    LayerMask groundDetect;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        timer = attackTime;
        RaycastHit hit;
        Physics.Raycast(this.gameObject.transform.position, -this.gameObject.transform.up, out hit, Mathf.Infinity, groundDetect);
        Vector3 emergePos = hit.point - this.transform.GetChild(1).localPosition * 2;
        this.transform.position = emergePos;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
 

        RaycastHit hit;
        //Makes sure it can see the player
        if (Physics.Raycast(projectileSpawnPos.position, player.transform.position - projectileSpawnPos.position, out hit, Mathf.Infinity, viewToPlayer))
        {

            if (hit.collider.gameObject.tag == "Player")
            {
                destroyTimer = 0.0f;
                if(!burrowing)
                {
                    timer += Time.deltaTime;
                    if (timer >= attackTime * attackTimeMulti)
                    {
                        timer = 0;
                        enemyAnims.SetTrigger("Attacking");
                    }
                    transform.LookAt(player.transform.position);
                    Quaternion rot = transform.rotation;
                    rot.eulerAngles = new Vector3(0, rot.eulerAngles.y, 0);
                    transform.rotation = rot;
                    //Make sure the player isn't too close or too far
                    if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) < 10 || Vector3.Distance(player.transform.position, this.gameObject.transform.position) > 20)
                    {
                        enemyAnims.SetTrigger("Burrow");
                        StartCoroutine(Burrow());
                    }
                }
                
            }
            else
            {
                destroyTimer += Time.deltaTime;
                if(destroyTimer > 20)
                {
                    currentHealth = 0;
                    Death();
                }
            }
        }





    }
    //Spawns the enemies given projectile, and in the case it's a crystal projectile spawns multiple
    public void Attack()
    {
        projectileSpawnPos.transform.LookAt(player.transform);
        audioManager.StopSFX(attackAudio);
        audioManager.PlaySFX(attackAudio);
        GameObject newProjectile = Instantiate(projectile, projectileSpawnPos.position, projectileSpawnPos.rotation);
        newProjectile.GetComponent<BaseRangedProjectileScript>().SetVars(projectileSpeed, damageAmount * (prophecyManager.prophecyDamageMulti));
        if (newProjectile.GetComponent<CrystalRangedProjectile>())
        {
            for (int i = 1; i < 3; i++)
            {
                newProjectile = Instantiate(projectile, projectileSpawnPos.position, projectileSpawnPos.rotation);
                newProjectile.transform.RotateAround(projectileSpawnPos.position, projectileSpawnPos.up, -5.0f * i);
                newProjectile.GetComponent<BaseRangedProjectileScript>().SetVars(projectileSpeed , damageAmount * (prophecyManager.prophecyDamageMulti));

            }
            for (int i = 1; i < 3; i++)
            {
                newProjectile = Instantiate(projectile, projectileSpawnPos.position, projectileSpawnPos.rotation);
                newProjectile.transform.RotateAround(projectileSpawnPos.position, projectileSpawnPos.up, 5.0f * i);
                newProjectile.GetComponent<BaseRangedProjectileScript>().SetVars(projectileSpeed, damageAmount * (prophecyManager.prophecyDamageMulti));

            }
        }
    }
    //Starts the enemy burrowing
    IEnumerator Burrow()
    {
        Vector3 startPos = this.transform.position;
        Vector3 endPos = this.transform.position + new Vector3(0, -50, 0);
        burrowing = true;
        timer = 0.0f;
        while(timer < burrowTime)
        {
            timer += Time.deltaTime;

            this.transform.position = Vector3.Lerp(startPos, endPos, timer / burrowTime);
            yield return null;
        }

        Node nodeChosen = null;
        //Finds a SAIM node, checks if it's valid, if so moves the enemy to below it and starts emerging, otherwise finds another node
        while(nodeChosen == null)
        {
            int rand = Random.Range(0, spawner.GetComponent<SAIM>().aliveNodes.Count);
            nodeChosen = spawner.GetComponent<SAIM>().aliveNodes[rand];
            RaycastHit hit;
            Physics.SphereCast(nodeChosen.gameObject.transform.position, 0.5f, -nodeChosen.gameObject.transform.up, out hit, Mathf.Infinity, groundDetect);
            Vector3 emergePos = hit.point - this.transform.GetChild(1).localPosition * 2;
            if (Vector3.Distance(player.transform.position, emergePos) > 10 && Vector3.Distance(player.transform.position, emergePos) < 20)
            {

                this.transform.position = emergePos + new Vector3(0, -50, 0);


                StartCoroutine(Emerge());
            }
            else
            {
                nodeChosen = null;
            }
            yield return null;
        }
        StopCoroutine(Burrow());

    }
    //Starts the enemy emerging from the ground
    IEnumerator Emerge()
    {
        Vector3 startPos = this.transform.position;
        Vector3 endPos = this.transform.position + new Vector3(0, 50, 0);
        timer = 0.0f;
        while (timer < burrowTime)
        {
            timer += Time.deltaTime;

            this.transform.position = Vector3.Lerp(startPos, endPos, timer / burrowTime);
            yield return null;
        }
        burrowing = false;
        timer = 0.0f;
        StopCoroutine(Emerge());
    }

    public bool GetBurrowing()
    {
        return burrowing;
    }

    public void SetAttackMulti(float multi)
    {
        attackTimeMulti = multi;
    }

    public float GetAttackMulti()
    {
        return attackTimeMulti;
    }

}

