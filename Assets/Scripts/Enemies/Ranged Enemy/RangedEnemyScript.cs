using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyScript : BaseEnemyClass
{
    float timer = 0;
    public float attackTime;
    [HideInInspector]
    public float attackTimeMulti;
    public LayerMask viewToPlayer;
    public float burrowTime;
    bool burrowing = false;

    public GameObject projectile;
    public float projectileDamage;
    public float projectileSpeed;
    [HideInInspector]
    public float projectileSpeedMulti;



    public Transform projectileSpawnPos;
    public LayerMask groundDetect;
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        timer = attackTime;
        RaycastHit hit;
        Physics.Raycast(this.gameObject.transform.position, -this.gameObject.transform.up, out hit, Mathf.Infinity, groundDetect);
        Vector3 emergePos = hit.point - this.transform.GetChild(0).GetChild(1).localPosition * 2;
        this.transform.position = emergePos;
    }

    // Update is called once per frame
    void Update()
    {


        RaycastHit hit;
        if (Physics.Raycast(transform.position, player.transform.position - transform.position, out hit, Mathf.Infinity, viewToPlayer))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                if(!burrowing)
                {
                    timer += Time.deltaTime;
                    transform.LookAt(player.transform.position);
                    Quaternion rot = transform.rotation;
                    rot.eulerAngles = new Vector3(0, rot.eulerAngles.y, 0);
                    transform.rotation = rot;
                    if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) < 10 || Vector3.Distance(player.transform.position, this.gameObject.transform.position) > 100)
                    {
                        enemyAnims.SetTrigger("Burrow");
                        StartCoroutine(Burrow());
                    }
                }
                
            }
            else
            {
                //Idle anim
            }
        }


        if(timer >= attackTime * attackTimeMulti)
        {
            timer = 0;
            enemyAnims.SetTrigger("Attacking");
        }


    }

    public void Attack()
    {
        projectileSpawnPos.transform.LookAt(player.transform);
        GameObject newProjectile = Instantiate(projectile, projectileSpawnPos.position, projectileSpawnPos.rotation);
        newProjectile.GetComponent<BaseRangedProjectileScript>().SetVars(projectileSpeed * projectileSpeedMulti, projectileDamage);
        if (newProjectile.GetComponent<CrystalRangedProjectile>())
        {
            for (int i = 1; i < 3; i++)
            {
                newProjectile = Instantiate(projectile, projectileSpawnPos.position, projectileSpawnPos.rotation);
                newProjectile.transform.RotateAround(projectileSpawnPos.position, projectileSpawnPos.up, -5.0f * i);
                newProjectile.GetComponent<BaseRangedProjectileScript>().SetVars(projectileSpeed * projectileSpeedMulti, projectileDamage);

            }
            for (int i = 1; i < 3; i++)
            {
                newProjectile = Instantiate(projectile, projectileSpawnPos.position, projectileSpawnPos.rotation);
                newProjectile.transform.RotateAround(projectileSpawnPos.position, projectileSpawnPos.up, 5.0f * i);
                newProjectile.GetComponent<BaseRangedProjectileScript>().SetVars(projectileSpeed * projectileSpeedMulti, projectileDamage);

            }
        }
    }

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

        while(nodeChosen == null)
        {
            int rand = Random.Range(0, spawner.GetComponent<SAIM>().aliveNodes.Count);
            Debug.Log(rand);
            nodeChosen = spawner.GetComponent<SAIM>().aliveNodes[rand];
            RaycastHit hit;
            Physics.Raycast(nodeChosen.gameObject.transform.position, -nodeChosen.gameObject.transform.up, out hit, Mathf.Infinity, groundDetect);
            Vector3 emergePos = hit.point - this.transform.GetChild(0).GetChild(1).localPosition * 2;
            if (Vector3.Distance(player.transform.position, emergePos) > 10 && Vector3.Distance(player.transform.position, emergePos) < 100)
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
}

