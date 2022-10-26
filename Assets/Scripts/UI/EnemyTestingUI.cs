using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTestingUI : MonoBehaviour
{
    bool doneLocking = false;
    bool doneUnlocking = false;
    PlayerClass playerClass;

    public enum EnemyType
    {
        Slime,
        Flying,
        Ranged,
        Shield
    }
    [SerializeField]
    EnemyType eType;

    public enum EnemyElement
    {
        Crystal,
        Fire,
        Water
    }
    [SerializeField]
    EnemyElement eEle;

    [SerializeField]
    int enemiesToSpawn;

    [SerializeField]
    int healthBuffAmount, damageBuffAmount;

    private void Start()
    {
        if(!Application.isPlaying)
        {
            Destroy(this.gameObject);
        }
        playerClass = PlayerClass.GetPlayerClass();
        FindObjectOfType<SAIM>().triggered = true;
    }


    private void Update()
    {
        if(Cursor.lockState == CursorLockMode.Locked && !doneLocking)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
            doneLocking = true;
            doneUnlocking = false;
        }
        else if (Cursor.lockState == CursorLockMode.None && !doneUnlocking)
        {
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(true);
            }       
            doneLocking = false;
            doneUnlocking = true;
        }

        if(Input.GetKeyUp(KeyCode.J))
        {
            ResetAmmo();
        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            ResetHealth();
        }
        if (Input.GetKeyUp(KeyCode.U))
        {
            SpawnEnemies();
        }
        if (Input.GetKeyUp(KeyCode.I))
        {
            ClearEnemies();
        }

    }

    public void ResetAmmo()
    {
        List<PlayerClass.ManaName> manaTypes = new List<PlayerClass.ManaName>();

        for (int i  = 0; i < 6; i++)
        {
            manaTypes.Add((PlayerClass.ManaName)i);
        }

        playerClass.GetComponent<PlayerClass>().ChangeMana(1000, manaTypes);
    }

    public void ResetHealth()
    {
        playerClass.GetComponent<PlayerClass>().ChangeHealth(1000, null);
    }

    public void SpawnEnemies()
    {
        
	    FindObjectOfType<SAIM>().Spawn(enemiesToSpawn, ((int)eType * 3) + (int)eEle);
    }
    
    

    public void ClearEnemies()
    {
        SAIM saim = FindObjectOfType<SAIM>();
        saim.spawnedEnemies.Clear();

        BaseEnemyClass[] beArr = FindObjectsOfType<BaseEnemyClass>();
        for (int i = 0; i < beArr.Length; i++)
        {
            DestroyImmediate(beArr[i].gameObject);
        }
    }

    public void BuffEnemiesHealth()
    {
        SAIM saim = FindObjectOfType<SAIM>();
        
        foreach (BaseEnemyClass ene in saim.spawnedEnemies)
        {
            ene.AddHealth(healthBuffAmount);
        }
    }

    public void BuffEnemiesDamage()
    {
        SAIM saim = FindObjectOfType<SAIM>();

        foreach (BaseEnemyClass ene in saim.spawnedEnemies)
        {
            ene.AddDamage(damageBuffAmount);
        }
    }

}
