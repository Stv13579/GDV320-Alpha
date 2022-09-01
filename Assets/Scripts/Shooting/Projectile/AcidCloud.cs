using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidCloud : BaseElementSpawnClass
{
    private float damage;

    private float cloudSize;

    private float cloudDuration;

    private float damageTicker;

    private float currentDamageTicker;

    [SerializeField] 
    private GameObject acidBurnVFX;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager)
        {
            audioManager.StopSFX("Acid Cloud Shot");
            audioManager.PlaySFX("Acid Cloud Shot");
        }
    }
    private void Update()
    {
        if(transform.localScale.x < cloudSize)
        {
            //Makes the acid cloud grow over time, up to the preset maximum size
            transform.localScale += new Vector3(Time.deltaTime, Time.deltaTime, Time.deltaTime);
        }

        cloudDuration -= Time.deltaTime;
        currentDamageTicker += Time.deltaTime;
        if(cloudDuration <= 1)
        {
            this.transform.parent.GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.transform.parent.GetChild(0).GetChild(0).gameObject.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.transform.parent.GetChild(0).GetChild(1).gameObject.GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
        if (this.transform.parent.GetChild(0).gameObject.GetComponent<ParticleSystem>().particleCount < 100 && cloudDuration < 1)
        {
            Destroy(this.gameObject.GetComponent<Collider>());
        }
        if (this.transform.parent.GetChild(0).gameObject.GetComponent<ParticleSystem>().particleCount < 1 && cloudDuration < 1)
        {
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }

    public void SetVars(float dmg, float size, float duration, List<BaseEnemyClass.Types> types, float tempDamageTicker)
    {
        //Set up the variables according to the element script
        damage = dmg;
        cloudSize = size;
        cloudDuration = duration;
        attackTypes = types;
        damageTicker = tempDamageTicker;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<BaseEnemyClass>())
        {
            if (currentDamageTicker >= damageTicker)
            {
                //If an enemy is inside the cloud, deal damage to it
                other.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
                currentDamageTicker = 0.0f;
            }
            if(other.gameObject.GetComponentInChildren<AcidBurnScript>())
            {
                other.gameObject.GetComponentInChildren<AcidBurnScript>().SetTimer(2.0f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<BaseEnemyClass>())
        {
            //When enemy enters cloud, add vfx
            Instantiate(acidBurnVFX, other.transform);
        }
    }
}
