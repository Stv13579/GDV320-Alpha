using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StasisTrapProj : MonoBehaviour
{
    float damage;
    float duration;
    float maxDuration;
    float maxDamageTicker;
    float currentDamageTicker;
    List<BaseEnemyClass.Types> attackTypes;
    List<GameObject> containedEnemies = new List<GameObject>();
    AudioManager audioManager;

    [SerializeField]
    GameObject visualBubble;

    [SerializeField]
    GameObject aftermathVFX;

    enum StasisTrapProjState
    {
        idle,
        active,
        aftermath,
        destroy
    }
    StasisTrapProjState currentstate = StasisTrapProjState.idle;
    // Start is called before the first frame update
    void Start()
    {
        duration = 0.0f;
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        duration += Time.deltaTime;
        currentDamageTicker += Time.deltaTime;
        // switch case for the VFXs
        switch (currentstate)
        {
            // after spawn animation
            // turn on actual StasisTrap
            case StasisTrapProjState.idle:
                {
                    if (duration >= 0.45f)
                    {
                        currentstate = StasisTrapProjState.active;
                    }
                    break;
                }
            // if duration of StasisTrap is greater then certain amount of time
            // play aftermath
            case StasisTrapProjState.active:
                {
                    visualBubble.SetActive(true);
                    if (duration >= maxDuration - 0.75f)
                    {
                        currentstate = StasisTrapProjState.aftermath;
                    }
                    break;
                }
            // play explosions effect
            // turn off collider
            // then destory if duration is greater then certain time
            case StasisTrapProjState.aftermath:
                {
                    visualBubble.SetActive(false);
                    aftermathVFX.SetActive(true);
                    if (!aftermathVFX.GetComponent<ParticleSystem>().isPlaying)
                    {
                        aftermathVFX.GetComponent<ParticleSystem>().Play();
                    }
                    this.GetComponent<Collider>().enabled = false;

                    if (audioManager)
                    {
                        audioManager.StopSFX("Stasis Trap Explosion");
                        audioManager.PlaySFX("Stasis Trap Explosion");
                    }
                    if (duration >= maxDuration)
                    {
                        for (int i = 0; i < containedEnemies.Count; i++)
                        {
                            if (containedEnemies[i])
                            {
	                            BaseEnemyClass enemy = containedEnemies[i].GetComponentInParent<BaseEnemyClass>();
                                StatModifier.RemoveModifier(enemy.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0, "Stasis"));
                                StatModifier.UpdateValue(enemy.GetSpeedStat());
                                containedEnemies.Remove(containedEnemies[i]);
                            }
                        }
                        currentstate = StasisTrapProjState.destroy;
                    }
                    break;
                }
            case StasisTrapProjState.destroy:
                {
                    Destroy(gameObject);
                    break;
                }
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

    // if enemy enters trigger collider and is not in the list
    // add to list and turn movement multi to 0
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8 && !containedEnemies.Contains(other.gameObject) && other.GetComponent<BaseEnemyClass>())
        {
            containedEnemies.Add(other.gameObject);
            for (int i = 0; i < containedEnemies.Count; i++)
            {
                if (containedEnemies[i])
                {
                    BaseEnemyClass enemy = containedEnemies[i].GetComponentInParent<BaseEnemyClass>();
                    StatModifier.AddModifier(enemy.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0, "Stasis"));
                    StatModifier.UpdateValue(enemy.GetSpeedStat());
                }
            }
        }
    }
    // if enemy is still in trigger collider
    // do damage to the enemy
    void OnTriggerStay(Collider other)
    {
	    if (other.gameObject.layer == 8 && other.GetComponentInParent<BaseEnemyClass>())
        {
            if (currentDamageTicker > maxDamageTicker)
            {
                currentDamageTicker = 0;
	            other.gameObject.GetComponentInParent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            }
        }
    }
}
