using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossScript : BaseEnemyClass //Sebastian
{
	[Header("Boss attacks")]
	float timer = 5.0f;
	[SerializeField]
	Transform projectileSpawnPos, tempProjectileSpawnPos;
	[SerializeField]
	GameObject fireProjectile, fakeFireProjectile, waterProjectile, crystalProjectile;
	[SerializeField]
	GameObject[] homingProjectiles;
	[SerializeField]
	LayerMask groundDetect;
	[SerializeField]
	GameObject bossHealthbar;
	[SerializeField]
	float FireDamage;
	[SerializeField]
	float WaterDamage;
	[SerializeField]
	float CrystalDamage;
	BossHealthbarScript healthbarScript;
	int fireAttack = 5;
	int fireAttackCounter = 0;
	bool waterAttacking = false;
	int homingAttack = 5;
	int homingAttackCounter = 0;
	float hurtTimer = 0.2f;

    public override void Awake()
    {
        base.Awake();
        RaycastHit hit;
        Physics.Raycast(this.gameObject.transform.position, -this.gameObject.transform.up, out hit, Mathf.Infinity, groundDetect);
	    Vector3 emergePos = hit.point - this.transform.GetChild(2).GetChild(3).localPosition * 2 - new Vector3(0, 2, 0);
        this.transform.position = emergePos;
        GameObject healthbar = Instantiate(bossHealthbar);
        healthbarScript = healthbar.GetComponent<BossHealthbarScript>();
	    healthbarScript.AddEnemy(this);
	    healthbarScript.SetName("Dragon Lily");
	    healthbarScript.SetMaxHealth(maxHealth);
    }
    
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		deathTriggers.Add(DestroyHoming);
	}
	
    public override void Update()
	{
		base.Update();
		hurtTimer -= Time.deltaTime;
		this.GetComponent<MeshCollider>().sharedMesh = this.transform.GetChild(2).GetChild(0).GetComponent<SkinnedMeshRenderer>().sharedMesh;
        if(enemyAnims.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            timer -= Time.deltaTime;
        }
	    this.transform.LookAt(player.transform);
	    this.transform.eulerAngles = new Vector3(0, this.transform.eulerAngles.y, 0);
        //Pick a random attack and send it to the animator
        if (timer <= 0.0f)
        {
            int attack = Random.Range(0, 4);
            switch (attack)
            {
                case 0:
                    enemyAnims.SetTrigger("Homing");
                    break;
                case 1:
                    enemyAnims.SetTrigger("Fire");
                    break;
                case 2:
                    enemyAnims.SetTrigger("Crystal");
                    break;
                case 3:
                    enemyAnims.SetTrigger("Water");
                    break;

            }
            timer = Random.Range(2.0f, 4.0f);
        }
    }

    
	public void HomingAttack()
	{
		GameObject homingProj = Instantiate(homingProjectiles[Random.Range(0, homingProjectiles.Length)], projectileSpawnPos.position, Quaternion.identity);
		homingAttackCounter++;
		if(homingAttackCounter >= homingAttack)
		{
			enemyAnims.SetTrigger("StopHoming");
			homingAttackCounter = 0;
		}

	}

    
	public void FireAttack()
	{
		GameObject fireProj = Instantiate(fakeFireProjectile, projectileSpawnPos.position, Quaternion.identity);
		fireProj.transform.forward = transform.up;
		fireProj.GetComponent<RangedBossFakeFireProjectileScript>().SetDamage(FireDamage);
		fireAttackCounter++;
		if(fireAttackCounter >= fireAttack)
		{
			enemyAnims.SetTrigger("StopFire");
			fireAttackCounter = 0;
		}

	}
    //Water attack, fire toSpawn number of water projectiles evenly in a circle around the boss
    public IEnumerator WaterAttack()
	{
		//Will change when animations are in
		while (waterAttacking)
	    {
			GameObject waterProj = Instantiate(waterProjectile, projectileSpawnPos.position, projectileSpawnPos.rotation);
			waterProj.transform.LookAt(player.transform.position);
            waterProj.GetComponent<RangedBossWaterProjectileScript>().SetVars(15, WaterDamage);
            Physics.IgnoreCollision(this.GetComponent<Collider>(), waterProj.GetComponent<Collider>());
			yield return new WaitForSeconds(0.01f);
	    }
		StopCoroutine(WaterAttack());
    }

    //Crystal attack, fires a large crystal projectile towards the player, which can embed in the ground and explode
    public void CrystalAttack()
    {
        GameObject crystalProj = Instantiate(crystalProjectile, projectileSpawnPos.position, Quaternion.identity);
        crystalProj.GetComponent<RangedBossCrystalProjectileScript>().SetVars(0, CrystalDamage);

    }
    //Starts the homing attack, so it can be called by the animator
    public void StartHoming()
    {
	    //StartCoroutine(HomingAttack(5));
    }
    //Starts the fire attack, so it can be called by the animator
    public void StartFire()
    {
	    //StartCoroutine(FireAttack(5));
    }
    //Starts the water attack, so it can be called by the animator
    public void StartWater()
	{
		waterAttacking = true;
        StartCoroutine(WaterAttack());
	}
    
	public void StopWater()
	{
		waterAttacking = false;
	}
	
	protected virtual void DestroyHoming(GameObject temp)
	{
		RangedBossHomingProjectileScript[] homing = FindObjectsOfType<RangedBossHomingProjectileScript>();
		for(int i = homing.Length - 1; i >= 0; i--)
		{
			Destroy(homing[i].gameObject);
		}
	}
	
	public override void Death()
	{
		healthbarScript.RemoveEnemy(this);
		base.Death();
	}
	
	public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1, bool applyTriggers = true)
	{
		if(hurtTimer <= 0)
		{
			base.TakeDamage(damageToTake, attackTypes, extraSpawnScale, applyTriggers);
			hurtTimer = 0.1f;
		}
	}
	
}
