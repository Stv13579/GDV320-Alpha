using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

//base class that all enemies derive from.
public class BaseEnemyClass : MonoBehaviour
{
    [SerializeField]
    protected float currentHealth, maxHealth, baseMaxHealth, damageAmount, baseDamageAmount, groundMoveSpeed, moveSpeed, baseMoveSpeed, rotationSpeed;

    protected StatModifier.FullStat health = new StatModifier.FullStat(0), damage = new StatModifier.FullStat(0), speed = new StatModifier.FullStat(0);

    #region

    protected ProphecyManager prophecyManager;

    #endregion

    //The amount of percentage damage any instance of incoming damage is reduced by
    [SerializeField]
    protected float damageResistance = 1;


    protected bool isDead = false;
    protected GameObject player;

    protected PlayerClass playerClass;

    [SerializeField]
    DropsList drops;

    float startY;

    protected GameObject spawner;

    /// <summary>
    /// Element types for weaknesses and resists
    /// </summary>

    public enum Types
    {
        Fire,
        Crystal,
        Water
    }

    [SerializeField]
    protected List<Types> weaknesses, resistances;

    protected List<GameObject> bounceList = new List<GameObject>();

    //Particle effect when the enemy is destroyed
    [SerializeField]
    protected GameObject deathSpawn;
    //Particle effect when the enemy is hit
    [SerializeField]
    protected GameObject hitSpawn;

    public delegate void DeathTrigger(GameObject temp);

    [HideInInspector]
    protected List<DeathTrigger> deathTriggers = new List<DeathTrigger>();

    public delegate void HitTrigger(BaseEnemyClass ownedEnemy, List<Types> type);

    protected List<HitTrigger> hitTriggers = new List<HitTrigger>();

    protected Vector3 moveDirection;
    protected AudioManager audioManager;
    protected Animator enemyAnims;

    [SerializeField]
    protected string attackAudio;
    [SerializeField]
    protected string deathAudio;
    [SerializeField]
    protected string takeDamageAudio;
    [SerializeField]
    protected string idleAudio;

    [SerializeField]
	GameObject targettingIndicator, witheredVFX, cursedVFX;

    protected Vector3 oldPosition;

    protected GameplayUI uiScript;

    [SerializeField]
    public ParticleSystem healVFX, buffVFX;

	[SerializeField]
	List<Material> enemyMat = new List<Material>();
	
	
	[SerializeField]
	float maxDistance = 10.0f;
	float damageTimer = 0.0f;
	[SerializeField]
	float maxDamageTimer = 30.0f;
	
    public virtual void Awake()
    {
        if(GameObject.Find("ProphecyManager"))
            prophecyManager = GameObject.Find("ProphecyManager").GetComponent<ProphecyManager>();
        startY = transform.position.y;
        player = GameObject.Find("Player");
        playerClass = player.GetComponent<PlayerClass>();
        currentHealth = maxHealth * prophecyManager.prophecyHealthMulti;
        audioManager = FindObjectOfType<AudioManager>();
        oldPosition = new Vector3(-1000, -1000, -1000);
        enemyAnims = GetComponentInChildren<Animator>();
        uiScript = FindObjectOfType<GameplayUI>();
        health.baseValue = baseMaxHealth;
        damage.baseValue = baseDamageAmount;
        speed.baseValue = baseMoveSpeed;
        //
        //if (idleAudio != null && audioManager)
        //{
        //    audioManager.PlaySFX(idleAudio);
        //}

        if(spawner == null)
        {
            spawner = FindObjectOfType<SAIM>().gameObject;
        }


        if(transform.Find("SupportVFXHarness") != null && transform.Find("SupportVFXHarness") != null)
        {
            buffVFX = transform.Find("SupportVFXHarness").GetChild(0).GetComponent<ParticleSystem>();
            healVFX = transform.Find("SupportVFXHarness").GetChild(1).GetComponent<ParticleSystem>();
        }
		
	    if(enemyAnims)
	    {
	    	Material[] mats = enemyAnims.gameObject.transform.GetComponentInChildren<Renderer>().materials;
	    	for(int i = 0; i < mats.Length; i++)
	    	{
	    		enemyMat.Add(mats[i]);
	    	}
	    }

    }

    public virtual void Update()
    {
        if(GetComponentInChildren<Animator>() && GetComponentInChildren<Animator>().GetNextAnimatorStateInfo(0).IsName("Death"))
        {

        	
            return;
        }
		
	    if(!deathSpawn)
	    {
		    Destroy(this.gameObject);
	    }
		
        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        if(transform.position.y > 100)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        if (transform.position.y < -100)
        {
            Death();
        }

        maxHealth = StatModifier.UpdateValue(health);
        damageAmount = StatModifier.UpdateValue(damage);
        moveSpeed = StatModifier.UpdateValue(speed);

        if (uiScript)
        {
	        if (uiScript.GetHitMarker().activeSelf == true)
            {
                StartCoroutine(uiScript.HitMarker());
            }
        }

        if(uiScript)
        {
            if(uiScript.GetHitMarkerShield().activeSelf == true)
            {
                StartCoroutine(uiScript.HitMarkerShield());
            }
        }
        
	    damageTimer += Time.deltaTime;
	    //Softlock prevention check
	    if(damageTimer > maxDamageTimer)
	    {
	    	currentHealth = -10;
		    enemyAnims.SetTrigger("Dead");
	    }
    }

    //Movement
    public virtual void Movement(Vector3 positionToMoveTo)
    {
        

        
    }

    public virtual void Movement(Vector3 positionToMoveTo, float speed)
    {



    }


    //Attacking
    public virtual void Attacking()
    {
        if (audioManager)
        {
            audioManager.StopSFX(attackAudio);
            audioManager.PlaySFX(attackAudio);
        }
    }

    public virtual void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1, bool applyTriggers = true)
    {
        hitSpawn.GetComponent<ParticleSystem>().Clear();
        hitSpawn.GetComponent<ParticleSystem>().Play();

        //GameObject hitSpn = Instantiate(hitSpawn, transform.position, Quaternion.identity);
        //Vector3 scale = hitSpn.transform.lossyScale * extraSpawnScale;
        //hitSpn.transform.localScale = scale;
        
        if(applyTriggers)
        {
            foreach (Item item in playerClass.GetHeldItems())
            {
                item.OnHitTriggers(this, attackTypes);
            }

            //Call all hit triggers; effects which trigger whenever the enemy is hit
            foreach (HitTrigger hTrigs in hitTriggers)
            {
                hTrigs(this, attackTypes);
            }
        }
        
        float multiplier = 1;
        foreach (Types type in attackTypes)
        {
            foreach (Types weak in weaknesses)
            {
                if (weak == type)
                {
                    multiplier *= 2 * prophecyManager.prophecyWeaknessMulti;
                }
            }
            foreach (Types resist in resistances)
            {
                if (resist == type)
                {
                    multiplier *= 0.5f * prophecyManager.prophecyResistMulti;
                }
            }
        }

        currentHealth -= (damageToTake * multiplier) * damageResistance;

        if(enemyAnims)
        {
            enemyAnims.SetTrigger("TakeDamage");
        }
        if (uiScript)
        {
            StopCoroutine(uiScript.HitMarker());
            StartCoroutine(uiScript.HitMarker());
        }
        if (audioManager)
        {
            audioManager.StopSFX(takeDamageAudio);
            audioManager.PlaySFX(takeDamageAudio, player.transform, this.transform);
        }

        foreach (Material mat in enemyMat)
        {
            mat.SetFloat("_Toggle_EnemyHPEmissive", Mathf.Clamp(currentHealth / maxHealth, 0, 1));
        }

        //Instead of calling death here, make an animation trigger instead
        if (currentHealth <= 0)
        {
            if (enemyAnims)
            {
                enemyAnims.SetTrigger("Dead");
            }
        }
        
	    damageTimer = 0;
        //Death();
    }

    //Checks if the enemy has died and applies relevant behaviour, such as triggering any on death effects, before destroying it
    public virtual void Death()
    {
        if(isDead)
        {
            return;
        }
        if(currentHealth <= 0)
        {
            isDead = true;
            //Normally do death animation/vfx, might even fade alpha w/e before deleting.


            //Destroy for now
            if(spawner)
            {
                spawner.GetComponent<SAIM>().spawnedEnemies.Remove(this);
            }


            int dropType = Random.Range(0, 3);

            switch (dropType)
            {
                case 0:
	                Drop(drops.GetCurrencyList(), drops.GetMinCurrencySpawn(), drops.GetMaxCurrencySpawn());
                    break;
                case 1:
	                Drop(drops.GetMinAmmoSpawn(), drops.GetMaxAmmoSpawn());
                    break;
                case 2:
	                Drop(drops.GetHealthList(), drops.GetMinHealthSpawn(), drops.GetMaxHealthSpawn());
                    break;
                default:
                    break;
            }

            //Death triggers

            foreach (DeathTrigger dTrigs in deathTriggers)
            {
                dTrigs(gameObject);
            }
			
	        deathSpawn.GetComponent<ParticleSystem>().Play();
	        enemyAnims.gameObject.SetActive(false);
	        //Instantiate(deathSpawn, transform.position, Quaternion.identity);

            if (audioManager)
            {
                //if(idleAudio != null)
                //{
                //    audioManager.StopSFX(idleAudio);
                //}
                audioManager.StopSFX(deathAudio);
                audioManager.PlaySFX(deathAudio, player.transform, this.transform);
            }
	        //Destroy(gameObject);
        }
    }

    void Drop(List<DropListEntry> dropType, int minSpawn, int maxSpawn)
    {
        //Spawn drops
        for (int i = 0; i < Random.Range(minSpawn, maxSpawn); i++)
        {
            GameObject drop = Instantiate(drops.GetDrop(dropType), this.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            
        }
    }

    //Overload for dropping ammo
    void Drop(int minSpawn, int maxSpawn)
    {
        //Spawn drops
        for (int i = 0; i < Random.Range(minSpawn, maxSpawn); i++)
        {
            GameObject drop = Instantiate(drops.GetAmmoDrop(), this.transform.position + new Vector3(0, 2, 0), Quaternion.identity);

        }
    }

    public void Targetted(bool targetted, Color colour)
    {
        if(targettingIndicator.activeSelf == false && targetted == true)
        {
            targettingIndicator.SetActive(targetted);
        }
        
        if(targettingIndicator.activeSelf == true && targetted == false)
        {
            targettingIndicator.SetActive(targetted);
        }
        if(this.transform.GetChild(1).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>())
        {
            this.transform.GetChild(1).GetChild(1).gameObject.GetComponent<SkinnedMeshRenderer>().material.SetColor("_Outline_Colour", colour);

        }
    }

	public virtual void RestoreHealth(float amount)
    {
	    currentHealth = Mathf.Clamp(currentHealth, currentHealth += amount, maxHealth);
        foreach (Material mat in enemyMat)
        {
            mat.SetFloat("_Toggle_EnemyHPEmissive", Mathf.Clamp(currentHealth / maxHealth, 0, 1));
        }
    }


    public float GetHealth()
    {
        return currentHealth;
    }
    public float GetDamageResistance()
    {
        return damageResistance;
    }

    public void SetDamageResistance(float tempDamageResistance)
    {
        damageResistance = tempDamageResistance;
    }

    public void SetSpawner(GameObject spawn)
    {
        spawner = spawn;
    }

    public List<GameObject> GetBounceList()
    {
        return bounceList;
    }

    public Vector3 GetOldPosition()
    {
        return oldPosition;
    }

    public void SetOldPosition(Vector3 pos)
    {
        oldPosition = pos;
    }

    public void SetMoveDirection(Vector3 dir)
    {
        moveDirection = dir;
    }

    public List<DeathTrigger> GetDeathTriggers()
    {
        return deathTriggers;
    }
    public List<HitTrigger> GetHitTriggers()
    {
        return hitTriggers;
    }

    public StatModifier.FullStat GetHealthStat()
    {
        return health;
    }

    public StatModifier.FullStat GetDamageStat()
    {
        return damage;
    }

    public StatModifier.FullStat GetSpeedStat()
    {
        return speed;
    }

    //For testing purposes only, in conjunction with enemy testing UI
    public void AddHealth(int amount)
    {
        currentHealth += amount;
        maxHealth += amount;
    }

    public void AddDamage(int amount)
    {
        damageAmount += amount;
        baseDamageAmount += amount;
    }
    
	public void SetCursed()
	{
		cursedVFX.SetActive(true);
	}
	
	public void SetWithered()
	{
		witheredVFX.SetActive(true);
	}
}
