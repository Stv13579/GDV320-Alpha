using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class PlayerClass : MonoBehaviour
{
    [SerializeField]
    float currentHealth, maxHealth, baseMaxHealth, defense, baseDefense;

    StatModifier.FullStat health = new StatModifier.FullStat(0), defenseStat = new StatModifier.FullStat(0);

    [Serializable]
    public enum ManaName
    { 
        Fire,
        Crystal,
        Water,
        Necrotic,
        Energy,
        Void,
        None
    }

    [Serializable]
    public struct ManaType
    {
        public ManaName manaName;
        public float currentMana;
        public float maxMana;
        public float baseMaxMana;
        public StatModifier.FullStat mana;
    }
    [SerializeField]
    public ManaType[] manaTypes;
    public float money;

    //A list of items which are collectible objects which add extra effects to the player
    public List<Item> heldItems = new List<Item>();

    [HideInInspector]
    public GameObject itemUI;

    public GameObject gameOverScreen;
    private bool dead = false;

    /// <summary>
    /// Pushing Away When Hit
    /// </summary>
    public float pushDuration;
    private float pushStrength;
    private float currentPushDuration;
    private Vector3 pushDir;


    private float fireTimer = 0.0f;
    [SerializeField]
    private float fireDOT;
    [SerializeField]
    private GameObject fireEffect;

    [SerializeField]
    private float lowHealthLimit;
    [SerializeField]
    private string lowHealthFastHeartBeat;
    [SerializeField]
    private string lowHealthSlowHeartBeat;

    private AudioManager audioManager;

    //[SerializeField]
    //private Material fireMaterial;
    void Start()
    {
        currentHealth = baseMaxHealth;
        health.baseValue = baseMaxHealth;
        defenseStat.baseValue = baseDefense;
        for(int i = 0; i < manaTypes.Length; i++)
        {
            manaTypes[i].mana = new StatModifier.FullStat(0);
            manaTypes[i].mana.baseValue = manaTypes[i].baseMaxMana;
            manaTypes[i].currentMana = manaTypes[i].baseMaxMana;
        }
        itemUI = GameObject.Find("ItemArray");
        audioManager = FindObjectOfType<AudioManager>();
    }


    public void StartLevel()
    {
        itemUI = GameObject.Find("ItemArray");
    }

    void Update()
    {
        //Don't forget to remove this
        if(Input.GetKeyDown(KeyCode.P))
        {
            BaseEnemyClass[] enemies = FindObjectsOfType<BaseEnemyClass>();
            foreach(BaseEnemyClass enemy in enemies)
            {
                enemy.TakeDamage(1000, new List<BaseEnemyClass.Types>());
            }
        }
        maxHealth = StatModifier.UpdateValue(health);
        defense = StatModifier.UpdateValue(defenseStat);

        LowHealthEffect();

        if(Input.GetKeyDown(KeyCode.O))
        {
            manaTypes[0].currentMana = 10;
        }

        if(fireTimer > 0)
        {
            ChangeHealth(-fireDOT);
            fireTimer -= Time.deltaTime;
        }
        else
        {
            fireEffect.SetActive(false);
        }

        Push();
    }

    public void AddItem(Item newItem)
    {
        //Other functionality.
        newItem.AddEffect(this); 
    }

    void Death()
    {
        if(GameObject.Find("TrinketManager"))
        {
            if (heldItems.Contains(GameObject.Find("TrinketManager").GetComponent<LuckyKnuckleBones>()))
            {
                Item luckyBones = heldItems.Find(LKB => LKB == GameObject.Find("TrinketManager").GetComponent<LuckyKnuckleBones>());
                if(luckyBones.GetComponent<LuckyKnuckleBones>().CanRevive())
                {
                    ChangeHealth(maxHealth);
                    return;
                }    
            }
        }

        if(GameObject.Find("Quest Manager"))
        {
            GameObject.Find("Quest Manager").GetComponent<QuestManager>().DeathUpdate();
        }
        else
        {
            Debug.Log("Player missing quest manager");
        }

        for (int i = 0; i < heldItems.Count; i++)
        {
            heldItems[i].DeathTriggers();
        }
        itemUI.transform.parent.gameObject.SetActive(false);
        Instantiate(gameOverScreen);
        dead = true;
        this.gameObject.GetComponent<PlayerLook>().SetAbleToMove(false);
        this.gameObject.GetComponent<PlayerLook>().ToggleCursor();

        this.gameObject.GetComponent<PlayerMovement>().SetAbleToMove(false);
        this.gameObject.GetComponent<Shooting>().SetAbleToShoot(false);


    }

    public void ChangeHealth(float healthAmount, bool reduceDamage = true)
    {
        //Create a one time defense modifier based on whether the player is recieving damage, or should not apply defense
        float defenseMod = defense;
        if(healthAmount > 0 || !reduceDamage)
        {
            defenseMod = 1;
        }

        if(defenseMod > 1 || defenseMod == 0)
        {
            defenseMod = 1;
        }

        currentHealth = Mathf.Min(currentHealth + (healthAmount * defenseMod), maxHealth);
        if (currentHealth <= 0 && !dead)
        {
            Death();
        }
    }

    //Get hit and bounce
    public void ChangeHealth(float healthAmount, Vector3 enemyPos, float pushForce, bool reduceDamage = true)
    {
        //Create a one time defense modifier based on whether the player is recieving damage, or should not apply defense
        float defenseMod = defense;
        if (healthAmount > 0 || !reduceDamage)
        {
            defenseMod = 1;
        }

        if (defenseMod > 1 || defenseMod == 0)
        {
            defenseMod = 1;
        }

        currentHealth = Mathf.Min(currentHealth + (healthAmount * defenseMod), maxHealth);

        Vector3 bounceVec = transform.position - enemyPos;

        pushDir = bounceVec.normalized;
        pushDir.y = 1;
        pushStrength = pushForce;
        currentPushDuration = 0;

        if (currentHealth <= 0 && !dead)
        {
            Death();
        }
    }

    public void Push()
    {
        if(currentPushDuration < pushDuration)
        {
            currentPushDuration += Time.deltaTime;
            transform.position += pushDir * Time.deltaTime * pushStrength;
        }
    }

    public bool ManaCheck(float manaAmount, List<ManaName> manaNames)
    {
        bool check = true;

        foreach(ManaName mana in manaNames)
        {
            ManaType m = Array.Find(manaTypes, item => item.manaName == mana);
            if(m.currentMana < manaAmount && currentHealth < manaAmount - m.currentMana)
            {
                check = false;
            }
        }

        return check;
    }

    private void LowHealthEffect()
    {
        if(currentHealth <= lowHealthLimit)
        {
            if(audioManager)
            {
                audioManager.PlaySFX(lowHealthFastHeartBeat);
                //audioManager.PlaySFX(lowHealthSlowHeartBeat);
                // play screen effect when we get it
            }
        }
        else
        {
            if (audioManager)
            {
                audioManager.StopSFX(lowHealthFastHeartBeat);
                //audioManager.PlaySFX(lowHealthSlowHeartBeat);
            }

        }

    }
    public void ChangeMana(float manaAmount, List<ManaName> manaNames)
    {
        foreach (ManaName mana in manaNames)
        {
            int i = Array.FindIndex(manaTypes, item => item.manaName == mana);
            manaTypes[i].currentMana = Mathf.Min(manaTypes[i].currentMana + manaAmount, manaTypes[i].maxMana);
            if (manaTypes[i].currentMana < 0)
            {
                ChangeHealth(manaTypes[i].currentMana);
                manaTypes[i].currentMana = 0;
            }
            //manaTypes[i].currentMana = Mathf.Max(manaTypes[i].currentMana, 0);
        }
    }

    public void ChangeMoney(float moneyAmount)
    {
        money = Mathf.Max(money + moneyAmount, 0);
    }

    public void OnFire(float time)
    {
        fireTimer = time;
        if(fireTimer > 0)
        {
            fireEffect.SetActive(true);
            //fireMaterial.SetFloat("_Toggle_EffectIntensity", 0.1f);
        }
        else
        {
            fireEffect.SetActive(false);
            //fireMaterial.SetFloat("_Toggle_EffectIntensity", 0.0f);
        }
    }

    public StatModifier.FullStat GetHealthStat()
    {
        return health;
    }

    public StatModifier.FullStat GetDefenseStat()
    {
        return defenseStat;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}
