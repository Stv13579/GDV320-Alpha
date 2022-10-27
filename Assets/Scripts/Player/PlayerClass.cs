using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class PlayerClass : MonoBehaviour
{
	static PlayerClass currentPlayerClass;
	static GameObject player;
    [SerializeField]
    float currentHealth;
    [SerializeField]
    float maxHealth;
    [SerializeField]
    float baseMaxHealth;
    [SerializeField]
    float defense;
    [SerializeField]
    float baseDefense;

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
    ManaType[] manaTypes;
    [SerializeField]
	static float money;

    //A list of items which are collectible objects which add extra effects to the player
    [SerializeField]
    List<Item> heldItems = new List<Item>();

    [HideInInspector]
    GameObject itemUI;

    [SerializeField]
    GameObject gameOverScreen;
    bool dead = false;

    /// <summary>
    /// Pushing Away When Hit
    /// </summary>
    float pushDuration;
    float pushStrength;
    float currentPushDuration;
    Vector3 pushDir;


    float fireTimer = 0.0f;
    [SerializeField]
    float fireDOT;
    [SerializeField]
    GameObject fireEffect;

    [SerializeField]
    float lowHealthLimit;
    [SerializeField]
    string lowHealthFastHeartBeat;
    [SerializeField]
    string lowHealthSlowHeartBeat;

    AudioManager audioManager;

    GameplayUI gameplayUI;

    float lowHealthValue;

    bool slowed;
    float slowedtimer = 10.0f;

    public ManaType[] GetManaTypeArray() { return manaTypes; }
    public List<Item> GetHeldItems() { return heldItems; }
    public GameObject GetItemUI() { return itemUI; }
    public GameObject GetGameOverScreen() { return gameOverScreen; }
    public void SetGameOverScreen(GameObject tempGameOverScreen) { gameOverScreen = tempGameOverScreen; }
    public float GetMoney() { return money; }
    public void SubtractMoney(float cost) { money -= cost; }
    public void SetSlowed(bool tempslowed) { slowed = tempslowed; }
    void Start()
    {
	    DontDestroyOnLoad(gameObject);
        currentHealth = baseMaxHealth;
        health.baseValue = baseMaxHealth;
        defenseStat.baseValue = baseDefense;
        for(int i = 0; i < manaTypes.Length; i++)
        {
            manaTypes[i].mana = new StatModifier.FullStat(0);
            manaTypes[i].mana.baseValue = manaTypes[i].baseMaxMana;
            manaTypes[i].currentMana = manaTypes[i].baseMaxMana;
        }
	    gameplayUI = GameplayUI.GetGameplayUI();

	    itemUI = gameplayUI.transform.GetChild(2).gameObject;
	    audioManager = AudioManager.GetAudioManager();
        lowHealthValue = 0.0f;
    }
    
	// Awake is called when the script instance is being loaded.
	protected void Awake()
	{
		currentPlayerClass = this;
		player = this.gameObject;
	}

    public void StartLevel()
    {
	    itemUI = gameplayUI.transform.GetChild(2).gameObject;
    }

    void Update()
    {

        maxHealth = StatModifier.UpdateValue(health);
        defense = StatModifier.UpdateValue(defenseStat);

        LowHealthEffect();
        FullScreenSlowed();

        if(Input.GetKeyDown(KeyCode.O))
        {
            manaTypes[0].currentMana = 10;
        }

        if(fireTimer > 0)
        {
            ChangeHealth(-fireDOT, null);
            fireTimer -= Time.deltaTime;
        }
        else
        {
            fireEffect.SetActive(false);
            gameplayUI.GetBurnFullScreen().gameObject.SetActive(false);
            gameplayUI.GetBurnFullScreen().material.SetFloat("_Toggle_EffectIntensity", 0.0f);
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
	    if(TrinketManager.GetTrinketManager())
        {
		    if (heldItems.Contains(TrinketManager.GetTrinketManager().GetComponent<LuckyKnuckleBones>()))
            {
                Item luckyBones = heldItems.Find(LKB => LKB == TrinketManager.GetTrinketManager().GetComponent<LuckyKnuckleBones>());
                if(luckyBones.GetComponent<LuckyKnuckleBones>().CanRevive())
                {
                    ChangeHealth(maxHealth, null);
                    return;
                }    
            }
        }

	    if(QuestManager.GetQuestManager())
        {
            QuestManager.GetQuestManager().GetComponent<QuestManager>().DeathUpdate();
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

    public void ChangeHealth(float healthAmount, GameObject source, bool reduceDamage = true)
    {
        //Create a one time defense modifier based on whether the player is recieving damage, or should not apply defense
        float defenseMod = defense;
        if(healthAmount > 0 || !reduceDamage)
        {
            defenseMod = 1;
        }

        if(defenseMod == 0)
        {
            defenseMod = 1;
        }
        currentHealth = Mathf.Min(currentHealth + (healthAmount * defenseMod), maxHealth);


        if (healthAmount < 0)
        {
            if (gameplayUI)
            {
                //StopCoroutine(gameplayUI.DamageIndicator());
                StartCoroutine(gameplayUI.DamageIndicator(source));
            }
        }

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
        if (gameplayUI)
        {
            gameplayUI.GetLowHealthFullScreen().material.SetFloat("_Toggle_EffectIntensity", lowHealthValue);
        }
        if (currentHealth <= lowHealthLimit)
        {
            if(audioManager)
            {
                audioManager.PlaySFX(lowHealthFastHeartBeat);
                //audioManager.PlaySFX(lowHealthSlowHeartBeat);
            }
            gameplayUI.GetLowHealthFullScreen().gameObject.SetActive(true);
            lowHealthValue += Time.deltaTime * 10.0f;
            if (lowHealthValue >= 10.0f)
            {
                lowHealthValue = 10.0f;
            }
        }
        else
        {
            if (audioManager)
            {
                audioManager.StopSFX(lowHealthFastHeartBeat);
                //audioManager.PlaySFX(lowHealthSlowHeartBeat);
            }
            lowHealthValue -= Time.deltaTime * 10.0f;
            if (lowHealthValue <= 0.0f)
            {
                lowHealthValue = 0.0f;
                gameplayUI.GetLowHealthFullScreen().gameObject.SetActive(false);
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
                ChangeHealth(manaTypes[i].currentMana, null);
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
            if(gameplayUI)
            {
                gameplayUI.GetBurnFullScreen().gameObject.SetActive(true);
                gameplayUI.GetBurnFullScreen().material.SetFloat("_Toggle_EffectIntensity", 10.0f);
            }
        }
        else
        {
            fireEffect.SetActive(false);
            if (gameplayUI)
            {
                gameplayUI.GetBurnFullScreen().gameObject.SetActive(false);
                gameplayUI.GetBurnFullScreen().material.SetFloat("_Toggle_EffectIntensity", 0.0f);
            }
        }
    }

    void FullScreenSlowed()
    {
        if(slowed)
        {
            gameplayUI.GetSlowedFullScreen().gameObject.SetActive(true);
            gameplayUI.GetSlowedFullScreen().material.SetFloat("_Toggle_EffectIntensity", 10.0f);
            slowedtimer -= Time.deltaTime;
            if (slowedtimer <= 0.0f)
            {
                slowed = false;
                slowedtimer = 5.0f;
            }
        }
        if(!slowed)
        {
            gameplayUI.GetSlowedFullScreen().gameObject.SetActive(false);
            gameplayUI.GetSlowedFullScreen().material.SetFloat("_Toggle_EffectIntensity", 0.0f);
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
    
	public static PlayerClass GetPlayerClass()
	{
		return currentPlayerClass;
	}
	
	public static GameObject GetPlayer()
	{
		return player;
	}
}
