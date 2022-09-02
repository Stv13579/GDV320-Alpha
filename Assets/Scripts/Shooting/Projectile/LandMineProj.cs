using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandMineProj : MonoBehaviour
{
    private float damage;
    private float explosiveRadius;
    private float lifeTimer;
    private List<BaseEnemyClass.Types> attackTypes;
    private AudioManager audioManager;
    private bool willExplode;
    private float timerToDestroy;
    [SerializeField]
    private GameObject mine;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private LayerMask enemyDetect;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer -= Time.deltaTime;
        // if true
        // turn off collider
        // show explosion effect
        // turn off mine
        if(willExplode)
        {
            this.GetComponent<Collider>().enabled = false;
            mine.SetActive(false);
            explosion.SetActive(true);
            if (!explosion.GetComponent<ParticleSystem>().isPlaying)
            {
                explosion.GetComponent<ParticleSystem>().Play();
            }
            willExplode = false;
        }
        // this is for the after effect of the animation
        // it is 10 seconds long
        if(!willExplode)
        {
            timerToDestroy += Time.deltaTime;
        }
        KillProjectile();
    }
    // kills the landmine if its been siting in the level for too long and nothing has happened to it or
    // if the animation has finished
    private void KillProjectile()
    {
        if(lifeTimer <= 0  && !willExplode)
        {
            Destroy(gameObject);
        }
        if(timerToDestroy >= 10)
        {
            Destroy(gameObject);
        }
    }
    public void SetVars(float dmg, float lifeTime, float explosionRadius, List<BaseEnemyClass.Types> types)
    {
        damage = dmg;
        lifeTimer = lifeTime;
        explosiveRadius = explosionRadius;
        attackTypes = types;

    }
    
    private void OnTriggerEnter(Collider other)
    {
        // if the other object is an enemy
        if (other.gameObject.layer == 8 && other.GetComponent<BaseEnemyClass>())
        {
            Collider[] objectsHitByExplosion = Physics.OverlapSphere(this.transform.position, explosiveRadius);
            for(int i = 0; i < objectsHitByExplosion.Length; i++)
            {
                if (objectsHitByExplosion[i].gameObject.layer == 8 && 
                    objectsHitByExplosion[i].GetComponent<BaseEnemyClass>())
                {
                    RaycastHit hit;
                    if (Physics.Raycast(this.transform.position + (objectsHitByExplosion[i].transform.position - this.transform.position).normalized * -2, (objectsHitByExplosion[i].transform.position - this.transform.position).normalized, out hit, 5, enemyDetect))
                    {
                        if ((hit.collider.gameObject.GetComponent<EnemyShield>() && !objectsHitByExplosion[i].GetComponent<EnemyShield>()) /*|| hit.collider.gameObject.layer == 10*/)
                        {

                        }
                    }
                    else
                    {
                        objectsHitByExplosion[i].GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
                    }
                }
            }

            if (audioManager)
            {
                audioManager.StopSFX("Land Mine Explosion");
                audioManager.PlaySFX("Land Mine Explosion");
            }
            willExplode = true;
        }
    }
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, explosiveRadius);
    //}
}
