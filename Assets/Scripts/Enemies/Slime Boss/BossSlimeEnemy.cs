using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSlimeEnemy : WaterSlimeEnemy
{
    enum Type
    {
        normal,
        fire,
        crystal
    }
    Type currentType = Type.normal;

    //Properties for the material editig

    [SerializeField]
    Renderer rend;

    BossSpawn bossSpawner;

    /// <summary>
    /// Weak point objects
    /// </summary>
    [SerializeField]
    GameObject firePoint, normalPoint, crystalPoint;

    /// <summary>
    /// The duration of the lerp between mats
    /// </summary>
    [SerializeField]
    float matLerpMax;

    float currentMatLerp;

    float crystalLerpValue;
    float fireLerpValue;

    //Properties for type transitions
    /// <summary>
    /// The time it takes before a type switch occurs
    /// </summary>
    [SerializeField]
    float changeTime;
    float currentChangeTime;

    [SerializeField]
    float timeBetweenAttacks;
    float currentAttackTime;

    /// <summary>
    /// Fire Type Properties
    /// </summary>
    [SerializeField]
    float fireChargeDuration;
    public float currentChargeDuration;
    [SerializeField]
    float fireChargeSpeed;
    Vector3 chargeVec;
    [SerializeField]
    private LayerMask trailLayerMask;
    [SerializeField]
    private Vector3 fireTrailScale;
    [SerializeField]
	GameObject enemyTrail;
	[SerializeField]
	GameObject chargeVFX;

    /// <summary>
    /// Normal Type Properties
    /// </summary>
    [SerializeField]
    float normalSlimeJumpForce;
    bool startAttack;
    bool endAttack;
    float cachedMoveSpeed;
    [SerializeField]
    float airSpeed;
    float previousY;
    [SerializeField]
    GameObject slamEffect;

    /// <summary>
    /// Crystal Type Properties
    /// </summary>
    [SerializeField]
    GameObject crystalProjectiles;
    [SerializeField]
    Vector3 projScale;

    /// <summary>
    /// Fire Trail Stuff
    /// </summary>
    [SerializeField]
    DecalRendererManager decalManager;
    float spawnTimer;

    /// <summary>
    /// Hits
    /// </summary>
    [SerializeField]
    GameObject crystalHit, crystalDeath, fireHit, fireDeath, normalHit, normalDeath;
    [SerializeField]
    GameObject healthBar;
    [HideInInspector]
    protected BossHealthbarScript bossHealthBar;
    bool split = false;

    public override void Awake()
    {
        base.Awake();
        moveDirection = player.transform.position;
        currentChangeTime = changeTime;
        currentAttackTime = timeBetweenAttacks;
        currentChargeDuration = fireChargeDuration;
        decalManager = FindObjectOfType<DecalRendererManager>();
        currentMatLerp = 0;
        //spawner = GameObject.Find("BossSpawner").GetComponent<BossSpawn>();
        if(!GameObject.Find("Boss Healthbar(Clone)"))
        {
            bossHealthBar = Instantiate(healthBar).GetComponent<BossHealthbarScript>();
	        bossHealthBar.AddEnemy(this);
	        bossHealthBar.SetName("King Slime");
	        bossHealthBar.SetMaxHealth(maxHealth);

        }
        deathTriggers.Remove(Split);

    }

	public override void Update()
    {

	    base.Update();

        if(!ExecuteAttack())
        {
            moveDirection = player.transform.position;
            base.Update();

            chargeVec = (moveDirection - transform.position).normalized;

            SwitchType();
        }


        //If the boss is on fire mode, make the fire trail
        if(currentType == Type.fire)
        {
            spawnTimer -= Time.deltaTime;

            
        }
        
        
        UpdateMaterials();
    }

    
    //Execute the attack based on the type it currently is
    private bool ExecuteAttack()
    {
        if(currentAttackTime > 0)
        {
            currentAttackTime -= Time.deltaTime;
            return false;
        }

        switch (currentType)
        {
            case Type.crystal:
                //Slowly send crystals out which bombard the arena, giving some telegraph to their landing zones
                CrystalAttack();
                break;
            case Type.fire:
                //Periodically charge in a straight line, setting the ground on fire.
                FireAttack();
                break;
            case Type.normal:
                //Periodically jump up high and slam down.
                NormalAttack();
                break;
            default:
                break;
        }

        //Reset attack time when ready
        //currentAttackTime = timeBetweenAttacks;
        return true;

    }

    //Choose a type at random, and switch up weak point, mats and enum
    private void SwitchType()
    {
        if(currentChangeTime < changeTime)
        {
            currentChangeTime += Time.deltaTime;

            return;
        }

        currentChangeTime = 0;

        //Set material lerping properties
        currentMatLerp = 0;

        int choice = Random.Range(0, 2);

        switch (currentType)
        {
            case Type.crystal:

                if (choice == 0)
                {
                    SwitchToFire();
                }
                else
                {
                    SwitchToNormal();
                }

                break;
            case Type.fire:

                if (choice == 0)
                {
                    SwitchToCrystal();
                }
                else
                {
                    SwitchToNormal();
                }

                break;
            case Type.normal:

                if (choice == 0)
                {
                    SwitchToFire();
                }
                else
                {
                    SwitchToCrystal();
                }

                break;
            default:

                break;
        }
    }

    //Jump in the air, then slam down on the player
    private void NormalAttack()
    {
        if(!startAttack)
        {
            GetComponent<Rigidbody>().AddForce(0, normalSlimeJumpForce, 0);
            startAttack = true;
            cachedMoveSpeed = moveSpeed;
            previousY = transform.position.y;
        }
        else
        {
            if(transform.position.y > previousY)
            {
                moveSpeed = cachedMoveSpeed * airSpeed;
            }
            else
            {
                moveSpeed = cachedMoveSpeed;
            }

            previousY = transform.position.y;

            moveDirection = player.transform.position;
            base.Update();
        }

        if(endAttack)
        {
            startAttack = false;
            endAttack = false;
            currentAttackTime = timeBetweenAttacks;
            moveSpeed = cachedMoveSpeed;
            Instantiate(slamEffect, this.transform.position, slamEffect.transform.rotation);
        }
    }

    //Crystal attack very similar to the crystal slime
    private void CrystalAttack()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject tempEnemyProjectile = Instantiate(crystalProjectiles, transform.position + new Vector3(0.0f, 3.0f, 0.0f), Quaternion.identity);
            // ignores physics for the with the crystal slime and the enemy crystal slime projectiles 
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), tempEnemyProjectile.GetComponent<Collider>());
            // setting scale of enemy projectile based on enemy size
            tempEnemyProjectile.transform.localScale = projScale;
            // setter to set variables from CrystalSlimeProject
            tempEnemyProjectile.GetComponent<CrystalSlimeProjectile>().SetVars(damageAmount);
            //setting the rotations of the projectiles so that it spawns in like a circle
            tempEnemyProjectile.transform.eulerAngles = new Vector3(tempEnemyProjectile.transform.eulerAngles.x, tempEnemyProjectile.transform.eulerAngles.y + (360.0f / 5.0f * i), tempEnemyProjectile.transform.eulerAngles.z);

            if (audioManager)
            {
                // play SFX
                audioManager.StopSFX("Crystal Slime Projectile");
                audioManager.PlaySFX("Crystal Slime Projectile", player.transform, this.transform);
            }
            enemyAnims.SetTrigger("Shoot");
        }

        currentAttackTime = timeBetweenAttacks;
    }
    //Charge the player, leaving fire behind it
    private void FireAttack()
    {
        if (currentChargeDuration > 0)
        {
            Vector3 moveVec = chargeVec.normalized * moveSpeed * fireChargeSpeed * Time.deltaTime;
            moveVec.y = 0;
            moveVec.y -= 1 * Time.deltaTime;
            transform.position += moveVec;
	        currentChargeDuration -= Time.deltaTime;
	        chargeVFX.SetActive(true);
        }
        else
        {
            currentChargeDuration = fireChargeDuration;
	        currentAttackTime = timeBetweenAttacks;
	        chargeVFX.SetActive(false);

        }
    }


    //Change slime type to crystal
    private void SwitchToCrystal()
    {

        currentType = Type.crystal;
        crystalPoint.SetActive(true);
        firePoint.SetActive(false);
        normalPoint.SetActive(false);
        hitSpawn = crystalHit;
        deathSpawn = crystalDeath;
    }
    //Change slime type to fire

    private void SwitchToFire()
    {

        currentType = Type.fire;
        crystalPoint.SetActive(false);
        firePoint.SetActive(true);
        normalPoint.SetActive(false);
        hitSpawn = fireHit;
        deathSpawn = fireDeath;
    }
    //Change slime type to water

    private void SwitchToNormal()
    {

        currentType = Type.normal;
        crystalPoint.SetActive(false);
        firePoint.SetActive(false);
        normalPoint.SetActive(true);
        hitSpawn = normalHit;
        deathSpawn = normalDeath;
    }
    //Update the maerials to match the slimes current type
    private void UpdateMaterials()
    {
        if(currentMatLerp < matLerpMax)
        {
            currentMatLerp += Time.deltaTime;
        }
        else
        {
            currentMatLerp = matLerpMax;
        }    

        switch(currentType)
        {
            case Type.crystal:

                crystalLerpValue -= Time.deltaTime;
                fireLerpValue += Time.deltaTime;

                break;
            case Type.fire:

                crystalLerpValue -= Time.deltaTime;
                fireLerpValue -= Time.deltaTime;

                
                break;
            case Type.normal:

                crystalLerpValue += Time.deltaTime;
                fireLerpValue += Time.deltaTime;

                break;
        }

        crystalLerpValue = Mathf.Clamp(crystalLerpValue, -1, 1);
        fireLerpValue = Mathf.Clamp(fireLerpValue, -1, 1);


        rend.material.SetFloat("_FireTextureLerp", fireLerpValue);
        rend.material.SetFloat("_CrystalTextureLerp", crystalLerpValue);
        
        
    }

    public override void OnCollisionEnter(Collision collision)
    {

        if (currentType == Type.normal && startAttack && (collision.gameObject.layer == 10 || collision.gameObject.tag == "Player" || collision.gameObject.layer == 18) )
        {
            endAttack = true;
        }

        if(currentType == Type.fire)
        {
            if (spawnTimer <= 0.0f)
            {
                float angle = Random.Range(0.0f, Mathf.PI * 2.0f);
                Vector3 forward = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
                // creates a plane which is the trail of the fire slime
                Vector3 trailPos = transform.position;
                trailPos.y -= 0.5f;

                GameObject tempEnemyTrail = Instantiate(enemyTrail, trailPos, Quaternion.LookRotation(Vector3.down, forward));
                tempEnemyTrail.transform.localScale = fireTrailScale;
                tempEnemyTrail.GetComponent<FireSlimeTrail>().SetVars(damageAmount);

                if (audioManager)
                {
                    audioManager.StopSFX("Fire Slime Trail Initial");
                    audioManager.PlaySFX("Fire Slime Trail Initial", player.transform, this.transform);
                }
                spawnTimer = 1.0f;
            }
        }

        

        base.OnCollisionEnter(collision);
    }

    public override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    public override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other);
    }

    public override void Death()
    {
        //if(currentHealth <= 0)
        //{
        //    spawner.bossDead = true;
        //}
        base.Death();

        
    }
    //If the slime gets to hald health, spit into two new slimes
    public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1, bool applyTriggers = true)
    {
        base.TakeDamage(damageToTake, attackTypes, 2, applyTriggers);
        if(currentHealth <= maxHealth / 2 && !split)
        {
            Split(gameObject);
        }
    }
    
    public void PushAway()
    {
        //GetComponent<Rigidbody>().AddForce( -(player.transform.position - transform.position).normalized * pushForce);
        GetComponent<Rigidbody>().AddForce((player.transform.position - transform.position).normalized.x * pushForce, 5 * pushForce, (player.transform.position - transform.position).normalized.z * pushForce);
    }
    //Spawns two smaller and weaker slimes and kills the original
    protected override void Split(GameObject temp)
    {
        if (generation < 3)
        {
            for (int i = 0; i < 2; i++)
            {
	            BossSlimeEnemy newSlime = Instantiate(this.gameObject, this.transform.position + (this.transform.right * ((i * 2) - 1) * 2), Quaternion.identity).GetComponent<BossSlimeEnemy>();
	            newSlime.RestoreHealth(0);
                StatModifier.AddModifier(newSlime.GetHealthStat().multiplicativeModifiers, new StatModifier.Modifier(0.25f, "Split " + generation));
                StatModifier.AddModifier(newSlime.GetDamageStat().multiplicativeModifiers, new StatModifier.Modifier(0.5f, "Split " + generation));
                StatModifier.AddModifier(newSlime.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0.5f, "Split " + generation));
                newSlime.transform.localScale = this.transform.localScale / 2;
                newSlime.generation = generation + 1;
	            bossHealthBar.AddEnemy(newSlime);
                newSlime.bossHealthBar = bossHealthBar;
            }
	        bossHealthBar.RemoveEnemy(this);
            split = true;
            Destroy(this.gameObject);
        }
    }


}
