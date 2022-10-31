using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalGrenadeProj : BaseElementSpawnClass
{
    float speed;
    float damage;
    float timerToExplode;
    float currentTimer;
    float explosionRange;
    float explosionDamage;
    AudioManager audioManager;
    Vector3 originalPosition;
    bool isMoving;
    bool isAttached;
    GameObject enemy;
    Vector3 contactPoint;
    [SerializeField]
    GameObject inAir;
    [SerializeField]
    GameObject attached;
    [SerializeField]
    GameObject explosion;
    bool upgraded = false;
    bool onGround = false;
    bool shieldAttached = false;
    enum grenadestate
    {
        inAir,
        attached,
        explosion,
        destroy
    };
    grenadestate currentState = grenadestate.inAir;
    [SerializeField]
    LayerMask enemyDetect;
    // Start is called before the first frame update
    void Start()
    {
	    audioManager = AudioManager.GetAudioManager();
	    isMoving = true;
        isAttached = false;
        attached.SetActive(false);
        explosion.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;

        if (!isAttached)
        {
            MoveProjectile();
        }
        switch(currentState)
        {
            case grenadestate.inAir:
                {
                    if(!isMoving || isAttached)
                    {
                        currentState = grenadestate.attached;
                    }
                    break;
                }
            case grenadestate.attached:
                {
                    inAir.SetActive(false);
                    attached.SetActive(true);
                    if (currentTimer >= timerToExplode || (!onGround && enemy.GetComponent<BaseEnemyClass>().Dead()))
                    {
                        currentState = grenadestate.explosion;
                    }
                    break;
                }
            case grenadestate.explosion:
                {
                    inAir.SetActive(false);
                    attached.SetActive(false);
                    explosion.SetActive(true);
                    explosion.transform.SetParent(null);
                    
                    if (audioManager)
                    {
                        audioManager.StopSFX("Crystal Grenade Explosion");
                        audioManager.PlaySFX("Crystal Grenade Explosion");
                    }
                    Collider[] objectsHit = Physics.OverlapSphere(transform.position, explosionRange);
                    for (int i = 0; i < objectsHit.Length; i++)
                    {
                        if (objectsHit[i].gameObject.layer == 8 &&
	                        objectsHit[i].GetComponentInParent<BaseEnemyClass>() || objectsHit[i].tag == "Enemy")
                        {
                            RaycastHit hit;
                            if (Physics.Raycast(this.transform.position + (objectsHit[i].transform.position - this.transform.position).normalized * -2, (objectsHit[i].transform.position - this.transform.position).normalized, out hit, 5, enemyDetect))
                            {
                                if ((hit.collider.gameObject.GetComponent<EnemyShield>() && !objectsHit[i].GetComponent<EnemyShield>()))
                                {
                                    objectsHit[i].GetComponentInChildren<EnemyShield>().TakeDamage(explosionDamage, attackTypes);
                                }
                                else if (hit.collider.gameObject.layer == 10)
                                {

                                }
                                else
                                {
	                                objectsHit[i].GetComponentInParent<BaseEnemyClass>().TakeDamage(explosionDamage, attackTypes);
                                }

                            }
                        }
                    }
                    if (!explosion.GetComponent<ParticleSystem>().isPlaying)
                    {
                        explosion.GetComponent<ParticleSystem>().Play();
                    }
	                if(onGround || (enemy && enemy.GetComponent<BaseEnemyClass>().Dead()))
                    {
                        currentState = grenadestate.destroy;
                       
                    }
                    else if(upgraded && !shieldAttached)
                    {

                        currentTimer = 0;
                        currentState = grenadestate.attached;
                    }
                    else
                    {
                        currentState = grenadestate.destroy;
                    }
                    break;
                }
            case grenadestate.destroy:
                {
                    if (currentTimer >= 10)
                    {
                        Destroy(gameObject);
                    }
                    break;
                }
        }

    }
    void MoveProjectile()
    {
        if (isMoving == true)
        {
            Vector3 projMovement = transform.forward * speed * Time.deltaTime;
            transform.position += projMovement;
        }
        else
        {
            transform.position = originalPosition;

        }
    }
    public void SetVars(float spd, float dmg, float timer, float explosionRadius, float expDamage, List<BaseEnemyClass.Types> types, bool upg)
    {
        speed = spd;
        damage = dmg;
        timerToExplode = timer;
        explosionRange = explosionRadius;
        explosionDamage = expDamage;
        attackTypes = types;
        upgraded = upg;
    }

    void OnTriggerEnter(Collider other)
    {
	    if(other.gameObject.layer == 8 && other.GetComponentInParent<BaseEnemyClass>())
        {
            if(other.GetComponentInParent<BaseEnemyClass>().Dead())
            {
                return;
            }
		    this.GetComponentInParent<Rigidbody>().useGravity = false;
            isAttached = true;
		    this.GetComponentInParent<Rigidbody>().isKinematic = true;
            speed = 0;
            other.GetComponentInParent<BaseEnemyClass>().GetDeathTriggers().Add(ResetTrigger);
            other.GetComponentInParent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            
            enemy = other.GetComponentInParent<BaseEnemyClass>().gameObject;
            transform.SetParent(enemy.transform);
            onGround = false;

            if(other.GetComponent<EnemyShield>())
            {
                shieldAttached = true;
                transform.SetParent(enemy.transform.parent);
            }
        }
        else if(other.gameObject.layer == 10)
        {
            isMoving = false;
            this.GetComponent<Rigidbody>().useGravity = false;
            originalPosition = transform.position;
            enemy = other.gameObject;
            onGround = true;
        }
    }

    void ResetTrigger(GameObject enemy)
    {
        currentState = grenadestate.explosion;
        isAttached = false;
        GetComponentInParent<Rigidbody>().useGravity = true;
        GetComponentInParent<Rigidbody>().isKinematic = false;
    }
}
