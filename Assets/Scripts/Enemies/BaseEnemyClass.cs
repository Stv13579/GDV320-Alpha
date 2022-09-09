using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

//base class that all enemies derive from.
public class BaseEnemyClass : MonoBehaviour
{
    [SerializeField]
    protected float currentHealth, maxHealth, baseMaxHealth, damageAmount, baseDamageAmount, moveSpeed, baseMoveSpeed;

    protected StatModifier.FullStat health = new StatModifier.FullStat(0), damage = new StatModifier.FullStat(0), speed = new StatModifier.FullStat(0);

    #region

    protected ProphecyManager prophecyManager;

    #endregion

    //The amount of flat damage any instance of incoming damage is reduced by
    [SerializeField]
    protected float damageThreshold;

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
    GameObject targettingIndicator;

    protected Vector3 oldPosition;

    GameplayUI uiScript;
    public virtual void Awake()
    {
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
        if (idleAudio != null && audioManager)
        {
            audioManager.PlaySFX(idleAudio);
        }
    }

    public virtual void Update()
    {
        if(transform.position.y > 100)
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }

        maxHealth = StatModifier.UpdateValue(health);
        damageAmount = StatModifier.UpdateValue(damage);
        moveSpeed = StatModifier.UpdateValue(speed);

        if(uiScript)
        {
            if(uiScript.GetHitMarker().activeInHierarchy == true)
            {
                StopCoroutine(uiScript.HitMarker());
            }
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

    public virtual void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1)
    {
        hitSpawn.GetComponent<ParticleSystem>().Clear();
        hitSpawn.GetComponent<ParticleSystem>().Play();

        //GameObject hitSpn = Instantiate(hitSpawn, transform.position, Quaternion.identity);
        //Vector3 scale = hitSpn.transform.lossyScale * extraSpawnScale;
        //hitSpn.transform.localScale = scale;

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
        currentHealth -= (damageToTake * multiplier) * damageResistance - damageThreshold;

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
        Death();
    }

    //Death
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
                    Drop(drops.currencyList, drops.minCurrencySpawn, drops.maxCurrencySpawn);
                    break;
                case 1:
                    Drop(drops.ammoList, drops.minAmmoSpawn, drops.maxAmmoSpawn);
                    break;
                case 2:
                    Drop(drops.healthList, drops.minHealthSpawn, drops.maxHealthSpawn);
                    break;
                default:
                    break;
            }

            //Death triggers

            foreach (DeathTrigger dTrigs in deathTriggers)
            {
                dTrigs(gameObject);
            }

            Instantiate(deathSpawn, transform.position, Quaternion.identity);

            if (audioManager)
            {
                if(idleAudio != null)
                {
                    audioManager.StopSFX(idleAudio);
                }
                audioManager.StopSFX(deathAudio);
                audioManager.PlaySFX(deathAudio, player.transform, this.transform);
            }
            Destroy(gameObject);
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

    public void Targetted(bool targetted, Color colour)
    {
        if(targettingIndicator.active == false && targetted == true)
        {
            targettingIndicator.SetActive(targetted);
        }
        
        if(targettingIndicator.active == true && targetted == false)
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
}
