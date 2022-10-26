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

    StasisTrapElement stasisTrap;
    void Start()
    {
        duration = 0.0f;
	    audioManager = AudioManager.GetAudioManager();
	    stasisTrap = FindObjectOfType<StasisTrapElement>();
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
                    for (int i = 0; i < stasisTrap.GetContainedEnemies().Count; i++)
                    {
                        if (stasisTrap.GetContainedEnemies()[i].activeInHierarchy)
                        {
                            BaseEnemyClass enemy = stasisTrap.GetContainedEnemies()[i].GetComponentInParent<BaseEnemyClass>();
                            StatModifier.RemoveModifier(enemy.GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0, "Stasis"));
                            StatModifier.UpdateValue(enemy.GetSpeedStat());
                            stasisTrap.GetContainedEnemies().Remove(stasisTrap.GetContainedEnemies()[i]);
                        }
                    }
                    if (duration >= maxDuration)
                    {
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

        for(int i = 0; i < stasisTrap.GetContainedEnemies().Count; i++)
        {
            if(stasisTrap.GetContainedEnemies()[i].GetComponentInParent<BaseEnemyClass>().GetHealth() <= 0)
            {
                StatModifier.RemoveModifier(stasisTrap.GetContainedEnemies()[i].GetComponentInParent<BaseEnemyClass>().GetSpeedStat().multiplicativeModifiers, new StatModifier.Modifier(0, "Stasis"));
                StatModifier.UpdateValue(stasisTrap.GetContainedEnemies()[i].GetComponentInParent<BaseEnemyClass>().GetSpeedStat());
                stasisTrap.GetContainedEnemies().Remove(stasisTrap.GetContainedEnemies()[i]);
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
        if(other.gameObject.layer == 8 && !stasisTrap.GetContainedEnemies().Contains(other.gameObject) && !other.GetComponent<EnemyShield>() ||
           other.GetComponentInParent<BaseEnemyClass>() && !stasisTrap.GetContainedEnemies().Contains(other.gameObject) && !other.GetComponent<EnemyShield>() ||
           other.tag == "Enemy" && !stasisTrap.GetContainedEnemies().Contains(other.gameObject) && !other.GetComponent<EnemyShield>())
        {
            stasisTrap.GetContainedEnemies().Add(other.gameObject);
            for (int i = 0; i < stasisTrap.GetContainedEnemies().Count; i++)
            {
                if (stasisTrap.GetContainedEnemies()[i].activeInHierarchy)
                {
                    BaseEnemyClass enemy = stasisTrap.GetContainedEnemies()[i].GetComponentInParent<BaseEnemyClass>();
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
	    if (other.gameObject.layer == 8 && other.GetComponentInParent<BaseEnemyClass>() ||
           other.tag == "Enemy")
        {
            if (currentDamageTicker > maxDamageTicker)
            {
                currentDamageTicker = 0;
	            other.gameObject.GetComponentInParent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            }
        }
    }
}
