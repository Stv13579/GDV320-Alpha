using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class PlayerClass : MonoBehaviour
{
    public float currentHealth;

    public float maxHealth;

    float defenseMultiplier = 1.0f;
    public struct defenseMultiSource
    {
        public float multiplier;
        public string source;
        public defenseMultiSource(float multi, string s)
        {
            multiplier = multi;
            source = s;
        }
    }
    List<defenseMultiSource> defenseMultipliers = new List<defenseMultiSource>();

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

    public Transform fallSpawner;

    /// <summary>
    /// Pushing Away When Hit
    /// </summary>
    public float pushDuration;
    private float pushStrength;
    private float currentPushDuration;
    private Vector3 pushDir;

    [SerializeField]
    private float fallDamage;

    private float fireTimer = 0.0f;
    [SerializeField]
    private float fireDOT;
    [SerializeField]
    private GameObject fireEffect;

    //[SerializeField]
    //private Material fireMaterial;
    void Start()
    {
        currentHealth = maxHealth;
        //currentMana = maxMana;
        for(int i = 0; i < manaTypes.Length; i++)
        {
            manaTypes[i].currentMana = manaTypes[i].maxMana;
        }
        itemUI = GameObject.Find("ItemArray");
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

        if (transform.position.y <= -30)
        {
            transform.position = fallSpawner.position;
            ChangeHealth(-fallDamage);
            Debug.Log("Reset Position");
        }

        if (transform.position.y > 70)
        {
            transform.position = fallSpawner.position;
            //ChangeHealth(-fallDamage);
            Debug.Log("Reset Position");
        }

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
        for (int i = 0; i < heldItems.Count; i++)
        {
            heldItems[i].DeathTriggers();
        }
        itemUI.transform.parent.gameObject.SetActive(false);
        Instantiate(gameOverScreen);
        dead = true;
        this.gameObject.GetComponent<PlayerLook>().ableToMove = false;
        this.gameObject.GetComponent<PlayerLook>().LockCursor();

        this.gameObject.GetComponent<PlayerMovement>().ableToMove = false;
        this.gameObject.GetComponent<Shooting>().ableToShoot = false;


    }

    public void ChangeHealth(float healthAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healthAmount, maxHealth);
        if (currentHealth <= 0 && !dead)
        {
            Death();
        }
    }

    //Get hit and bounce
    public void ChangeHealth(float healthAmount, Vector3 enemyPos, float pushForce)
    {
        currentHealth = Mathf.Min(currentHealth + healthAmount, maxHealth);

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

    public void AddDefenseMultiplier(defenseMultiSource multiplier)
    {
        if(!defenseMultipliers.Contains(multiplier))
        {
            defenseMultipliers.Add(multiplier);
        }
        defenseMultiplier = 1.0f;
        foreach(defenseMultiSource multi in defenseMultipliers)
        {
            defenseMultiplier *= multi.multiplier;
        }
    }

    public void RemoveDefenseMultiplier(defenseMultiSource multiplier)
    {
        if (defenseMultipliers.Contains(multiplier))
        {
            defenseMultipliers.Remove(multiplier);
        }
        defenseMultiplier = 1.0f;
        foreach (defenseMultiSource multi in defenseMultipliers)
        {
            defenseMultiplier *= multi.multiplier;
        }
    }

    public IEnumerator Vulnerable(defenseMultiSource multiplier)
    {
        AddDefenseMultiplier(multiplier);
        yield return new WaitForSeconds(10);
        RemoveDefenseMultiplier(multiplier);
    }

}
