using System.Collections;
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
	bool burrowRoutine = false;
    bool justSpawned = true;
    [SerializeField]
    GameObject projectile;
    [SerializeField]
    float projectileSpeed;

	float destroyTimer = 0.0f;
	[SerializeField]
	GameObject burrowVFX, burrowingVFX;
	GameObject burrowMovementVFX;


    [SerializeField]
    Transform projectileSpawnPos;
    [SerializeField]
    LayerMask groundDetect;
    // Start is called before the first frame update
    public override void Awake()
    {
        base.Awake();
        timer = attackTime;
        //RaycastHit hit;
        //Physics.Raycast(this.gameObject.transform.position, -this.gameObject.transform.up, out hit, Mathf.Infinity, groundDetect);
        //Vector3 emergePos = hit.point - this.transform.GetChild(1).localPosition * 2;
        //this.transform.position = emergePos;
    }


    private void OnEnable()
    {
        RaycastHit hit;
        Physics.Raycast(this.gameObject.transform.position, -this.gameObject.transform.up, out hit, Mathf.Infinity, groundDetect);
        Vector3 emergePos = hit.point - this.transform.GetChild(1).localPosition * 2;
        this.transform.position = emergePos;
        enemyAnims.SetTrigger("Burrow");
        enemyAnims.SetBool("IsBurrow", true);
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Vector3.Angle(transform.forward, player.transform.position - transform.position) > 10)
        {
            float dir = Mathf.Sign(Vector3.SignedAngle(transform.forward, player.transform.position - transform.position, Vector3.up));

            //rotate towards the disired vector/angle in that direction, modified by a scalar

            transform.Rotate(Vector3.up, dir * Time.deltaTime * rotationSpeed * 0.1f);
        }
	    Debug.DrawLine(projectileSpawnPos.position, projectileSpawnPos.position + ((player.transform.position - projectileSpawnPos.position).normalized * 10));
        RaycastHit hit;
        //Makes sure it can see the player
	    if (Physics.Raycast(projectileSpawnPos.position, (player.transform.position - projectileSpawnPos.position).normalized, out hit, 20, viewToPlayer))
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

                    //transform.LookAt(player.transform.position);
                    //Quaternion rot = transform.rotation;
                    //rot.eulerAngles = new Vector3(0, rot.eulerAngles.y, 0);
                    //transform.rotation = rot;

                    //get the direction of rotation
                    
                    

                    //Make sure the player isn't too close or too far
                    if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) < 5 || Vector3.Distance(player.transform.position, this.gameObject.transform.position) > 20)
                    {
	                    enemyAnims.SetTrigger("Burrow");
                        enemyAnims.SetBool("IsBurrow", true);
                        burrowing = true;
	                    Instantiate(burrowVFX, this.transform.position - new Vector3(0, 0.3f, 0), Quaternion.identity);
                    }
                }
               
                
            }
            else
            {
	            destroyTimer += Time.deltaTime;
            	if(destroyTimer > 5)
            	{
	            	if(!burrowing)
	            	{
		            	enemyAnims.SetTrigger("Burrow");
		            	enemyAnims.SetBool("IsBurrow", true);
		            	burrowing = true;
		            	Instantiate(burrowVFX, this.transform.position - new Vector3(0, 0.3f, 0), Quaternion.identity);
	            	}
            	}

                if (destroyTimer > 20)
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
        if (audioManager)
        {
            audioManager.StopSFX(attackAudio);
            audioManager.PlaySFX(attackAudio);
        }
        GameObject newProjectile = Instantiate(projectile, projectileSpawnPos.position, projectileSpawnPos.rotation);
        newProjectile.GetComponent<BaseRangedProjectileScript>().origin = gameObject;
        newProjectile.GetComponent<BaseRangedProjectileScript>().SetVars(projectileSpeed, damageAmount * (prophecyManager.prophecyDamageMulti));
        if (newProjectile.GetComponent<CrystalRangedProjectile>())
        {
            for (int i = 1; i < 3; i++)
            {
                newProjectile = Instantiate(projectile, projectileSpawnPos.position, projectileSpawnPos.rotation);
                newProjectile.GetComponent<BaseRangedProjectileScript>().origin = gameObject;
                newProjectile.transform.RotateAround(projectileSpawnPos.position, projectileSpawnPos.up, -5.0f * i);
                newProjectile.GetComponent<BaseRangedProjectileScript>().SetVars(projectileSpeed , damageAmount * (prophecyManager.prophecyDamageMulti));

            }
            for (int i = 1; i < 3; i++)
            {
                newProjectile = Instantiate(projectile, projectileSpawnPos.position, projectileSpawnPos.rotation);
                newProjectile.GetComponent<BaseRangedProjectileScript>().origin = gameObject;
                newProjectile.transform.RotateAround(projectileSpawnPos.position, projectileSpawnPos.up, 5.0f * i);
                newProjectile.GetComponent<BaseRangedProjectileScript>().SetVars(projectileSpeed, damageAmount * (prophecyManager.prophecyDamageMulti));

            }
        }
    }
	//Starts the enemy burrowing
	public void StartBurrow()
	{
		if(!burrowRoutine)
		{
			StartCoroutine(Burrow());
		}
	}
	//Sets up the burrowing VFX, moves the enemy underground, then starts it emerging
    IEnumerator Burrow()
    {
        Vector3 startPos = this.transform.position;
        Vector3 endPos = this.transform.position + new Vector3(0, -50, 0);
	    timer = 0.0f;
	    burrowMovementVFX = Instantiate(burrowingVFX, this.transform.position, Quaternion.identity);
	    burrowRoutine = true;
	    Node nodeChosen = null;
	    Vector3 emergePos = Vector3.zero;
	    //Finds a SAIM node, checks if it's valid, if so moves the enemy to below it and starts emerging, otherwise finds another node
	    while(nodeChosen == null)
	    {
		    int rand = Random.Range(0, spawner.GetComponent<SAIM>().aliveNodes.Count);
		    nodeChosen = spawner.GetComponent<SAIM>().aliveNodes[rand];
		    RaycastHit hit;
		    Physics.SphereCast(nodeChosen.gameObject.transform.position, 0.5f, -nodeChosen.gameObject.transform.up, out hit, Mathf.Infinity, groundDetect);
		    emergePos = hit.point - this.transform.GetChild(1).localPosition * 2;
		    if (Vector3.Distance(player.transform.position, emergePos) > 12 && Vector3.Distance(player.transform.position, emergePos) < 18)
		    {



		    }
		    else
		    {
			    nodeChosen = null;
		    }
		    yield return null;
	    }
	    
	    burrowMovementVFX.GetComponent<BurrowMovementScript>().SetVars(emergePos, burrowTime * 2);
	    
	    while(timer < burrowTime)
        {
            timer += Time.deltaTime;

            this.transform.position = Vector3.Lerp(startPos, endPos, timer / burrowTime);
            yield return null;
        }
	    this.transform.position = emergePos + new Vector3(0, -50, 0);

	    StartCoroutine(Emerge());
        StopCoroutine(Burrow());

    }
	//Moves the enemy up out of the ground
    IEnumerator Emerge()
    {
        Vector3 startPos = this.transform.position;
        Vector3 endPos = this.transform.position + new Vector3(0, 50, 0);
	    timer = 0.0f;
	    bool spawnedBurrow = false;
        while (timer < burrowTime)
        {
            timer += Time.deltaTime;
	        if(timer > burrowTime - 0.5 && !spawnedBurrow)
	        {
	        	spawnedBurrow = true;
		        Instantiate(burrowVFX, endPos - new Vector3(0, 0.3f, 0), Quaternion.identity);
	        }
            this.transform.position = Vector3.Lerp(startPos, endPos, timer / burrowTime);
            yield return null;
        }
        enemyAnims.SetBool("IsBurrow", false);
	    enemyAnims.SetTrigger("Emerge");

        burrowing = false;
	    timer = 0.0f;
	    burrowRoutine = false;

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

    public override void Death()
    {
        base.Death();
    }

    protected override void ResetEnemy()
    {
        base.ResetEnemy();
        burrowing = false;
        timer = attackTime;
    }
}

