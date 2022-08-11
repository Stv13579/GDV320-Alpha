using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisTrapProj : MonoBehaviour
{
    private float damage;
    private float duration;
    private float maxDuration;
    private float maxDamageTicker;
    private float currentDamageTicker;
    private List<BaseEnemyClass.Types> attackTypes;
    private List<GameObject> containedEnemies = new List<GameObject>();
    private AudioManager audioManager;

    [SerializeField]
    private GameObject visualBubble;

    [SerializeField]
    private GameObject aftermathVFX;

    // Start is called before the first frame update
    void Start()
    {
        duration = 0.0f;
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(duration >= 0.5f)
        {
            visualBubble.SetActive(true);
        }
        duration += Time.deltaTime;
        currentDamageTicker += Time.deltaTime;
        if(duration >= maxDuration - 0.75f)
        {
            visualBubble.SetActive(false);
            aftermathVFX.SetActive(true);
            if (!aftermathVFX.GetComponent<ParticleSystem>().isPlaying)
            {
                aftermathVFX.GetComponent<ParticleSystem>().Play();
            }
        }
        KillProjectile();
    }

    void KillProjectile()
    {
        if(duration >= maxDuration)
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
        maxDuration = dur;
        attackTypes = types;
        currentDamageTicker = ct;
        maxDamageTicker = mdt;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8 && !containedEnemies.Contains(other.gameObject) && other.GetComponent<BaseEnemyClass>())
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
        if (other.gameObject.layer == 8 && other.GetComponent<BaseEnemyClass>())
        {
            if (currentDamageTicker > maxDamageTicker)
            {
                currentDamageTicker = 0;
                other.gameObject.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            }
        }
    }
}
