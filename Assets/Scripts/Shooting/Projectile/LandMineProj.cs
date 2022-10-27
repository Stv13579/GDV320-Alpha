using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineProj : MonoBehaviour
{
    float damage;
    float explosiveRadius;
    float lifeTimer;
    float activateTimer;
    float currentActivateTimer;
    
    List<BaseEnemyClass.Types> attackTypes;
    AudioManager audioManager;
    float timerToDestroy;
    [SerializeField]
    GameObject mine;
    [SerializeField]
    GameObject explosion;
    [SerializeField]
    LayerMask enemyDetect;
    enum landMineState
    {
        placed,
        explode,
        destroyed
    };
    landMineState currentState = landMineState.placed;

    // Start is called before the first frame update
    void Start()
    {
	    audioManager = AudioManager.GetAudioManager();
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case landMineState.placed:
                {
                    currentActivateTimer += Time.deltaTime;
                    if (currentActivateTimer >= activateTimer)
                    {
                        this.GetComponent<Collider>().enabled = true;
                        currentActivateTimer = activateTimer;
                    }
                }
                break;
            case landMineState.explode:
                {
                    // turn off collider
                    // show explosion effect
                    // turn off mine
                    this.GetComponent<Collider>().enabled = false;
                    mine.SetActive(false);
                    explosion.SetActive(true);
                    if (!explosion.GetComponent<ParticleSystem>().isPlaying)
                    {
                        explosion.GetComponent<ParticleSystem>().Play();
                    }
                    currentState = landMineState.destroyed;
                }
                break;
            case landMineState.destroyed:
                {
                    // this is for the after effect of the animation
                    // it is 10 seconds long
                    timerToDestroy += Time.deltaTime;
                    if (timerToDestroy >= 10)
                    {
                        Destroy(gameObject);
                    }
                }
                break;

        }
        lifeTimer -= Time.deltaTime;
        KillProjectile();
    }
    // kills the landmine if its been siting in the level for too long and nothing has happened to it or
    // if the animation has finished
    void KillProjectile()
    {
        if(lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
        if(timerToDestroy >= 10)
        {
            Destroy(gameObject);
        }
    }
    public void SetVars(float dmg, float lifeTime, float explosionRadius,float activateTime, List<BaseEnemyClass.Types> types)
    {
        damage = dmg;
        lifeTimer = lifeTime;
        explosiveRadius = explosionRadius;
        attackTypes = types;
        activateTimer = activateTime;
    }
    
    void OnTriggerEnter(Collider other)
    {
        // if the other object is an enemy
        if (other.gameObject.layer == 8 && other.GetComponent<BaseEnemyClass>() || other.tag == "Enemy")
        {
	        Collider[] objectsHitByExplosion = Physics.OverlapSphere(this.transform.position, explosiveRadius, enemyDetect, QueryTriggerInteraction.Collide);
            for(int i = 0; i < objectsHitByExplosion.Length; i++)
            {
                if (objectsHitByExplosion[i].gameObject.layer == 8 && 
	                objectsHitByExplosion[i].GetComponentInParent<BaseEnemyClass>())
                {
                    //RaycastHit hit;
                    //if (Physics.Raycast(this.transform.position + (objectsHitByExplosion[i].transform.position - this.transform.position).normalized * -2, (objectsHitByExplosion[i].transform.position - this.transform.position).normalized, out hit, 5, enemyDetect))
                    //{
                        //if ((hit.collider.gameObject.GetComponent<EnemyShield>() && !objectsHitByExplosion[i].GetComponent<EnemyShield>()))
                        //{

                        //}
                        //else
                        //{
                            objectsHitByExplosion[i].GetComponentInParent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
                        //}
                    //}
                }
            }

            if (audioManager)
            {
                audioManager.StopSFX("Land Mine Explosion");
                audioManager.PlaySFX("Land Mine Explosion");
            }
            currentState = landMineState.explode;
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, explosiveRadius);
    //}
}
