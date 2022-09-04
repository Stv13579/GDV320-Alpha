using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalGrenadeProj : BaseElementSpawnClass
{
    private float speed;
    private float damage;
    private float timerToExplode;
    private float currentTimer;
    private float explosionRange;
    private float explosionDamage;
    AudioManager audioManager;
    private Vector3 originalPosition;
    private bool isMoving;
    private bool isAttached;
    Collider enemy;
    [SerializeField]
    private GameObject inAir;
    [SerializeField]
    private GameObject attached;
    [SerializeField]
    private GameObject explosion;
    private enum grenadestate
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
        audioManager = FindObjectOfType<AudioManager>();
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
                    if (currentTimer >= timerToExplode)
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
                            objectsHit[i].GetComponent<BaseEnemyClass>())
                        {
                            RaycastHit hit;
                            if (Physics.Raycast(this.transform.position + (objectsHit[i].transform.position - this.transform.position).normalized * -2, (objectsHit[i].transform.position - this.transform.position).normalized, out hit, 5, enemyDetect))
                            {
                                if ((hit.collider.gameObject.GetComponent<EnemyShield>() && !objectsHit[i].GetComponent<EnemyShield>()) || hit.collider.gameObject.layer == 10)
                                {

                                }
                                else
                                {
                                    objectsHit[i].GetComponent<BaseEnemyClass>().TakeDamage(explosionDamage, attackTypes);
                                }

                            }
                        }
                    }
                    if (!explosion.GetComponent<ParticleSystem>().isPlaying)
                    {
                        explosion.GetComponent<ParticleSystem>().Play();
                    }
                    currentState = grenadestate.destroy;
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
    public void SetVars(float spd, float dmg, float timer, float explosionRadius, float expDamage, List<BaseEnemyClass.Types> types)
    {
        speed = spd;
        damage = dmg;
        timerToExplode = timer;
        explosionRange = explosionRadius;
        explosionDamage = expDamage;
        attackTypes = types;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8 && other.GetComponent<BaseEnemyClass>())
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            isAttached = true;
            this.gameObject.transform.SetParent(other.transform);
            this.GetComponent<Rigidbody>().isKinematic = true;
            speed = 0;
            other.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            other = enemy;
        }
        else if(other.gameObject.layer == 10)
        {
            isMoving = false;
            this.GetComponent<Rigidbody>().useGravity = false;
            originalPosition = transform.position;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, explosionRange);
    //}
}
