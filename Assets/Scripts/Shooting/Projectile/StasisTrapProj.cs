using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisTrapProj : MonoBehaviour
{
    private float damage;
    private float duration;
    private float maxDamageTicker;
    private float currentDamageTicker;
    List<BaseEnemyClass.Types> attackTypes;
    List<GameObject> containedEnemies = new List<GameObject>();
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        duration -= Time.deltaTime;
        currentDamageTicker += Time.deltaTime;
        KillProjectile();
    }

    void KillProjectile()
    {
        if(duration <= 0)
        {
            for(int i = 0; i < containedEnemies.Count; i++)
            {
                if(containedEnemies[i])
                {
                    containedEnemies[i].GetComponent<BaseEnemyClass>().RemoveMovementMultiplier(0);
                    containedEnemies.Remove(containedEnemies[i]);
                }
            }
            Destroy(gameObject);
        }
    }
    public void SetVars(float dmg, float dur, float ct, float mdt, List<BaseEnemyClass.Types> types)
    {
        damage = dmg;
        duration = dur;
        attackTypes = types;
        currentDamageTicker = ct;
        maxDamageTicker = mdt;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8 && !containedEnemies.Contains(other.gameObject))
        {
            containedEnemies.Add(other.gameObject);
            for (int i = 0; i < containedEnemies.Count; i++)
            {
                containedEnemies[i].gameObject.GetComponent<BaseEnemyClass>().AddMovementMultiplier(0);
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (currentDamageTicker > maxDamageTicker)
            {
                currentDamageTicker = 0;
                other.gameObject.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            }
        }
    }
}
