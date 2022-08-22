using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : BaseElementSpawnClass
{

    float speed;

    float damage;
    float explosionDamage;

    float gravity;

    AnimationCurve gravCurve;

    float gravityLifetime;
    float startLifetime;

    float explosionRadii;


    [SerializeField]
    private GameObject hitMarker;

    AudioManager audioManager;

    [SerializeField]
    LayerMask enemyDetect;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        hitMarker = GameObject.Find("GameplayUI");
    }

    void Update()
    {
        if(gravityLifetime > 0)
        {
            gravityLifetime -= Time.deltaTime;
        }

        Vector3 movement = transform.forward * speed * Time.deltaTime;
        gravity *= 1.01f;
        movement.y -= gravity /** gravCurve.Evaluate(startLifetime - gravityLifetime)*/ * Time.deltaTime;

        transform.position += movement;
        
        if (this.gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<ParticleSystem>().time >= 2)
        {
            Destroy(this.gameObject);
        }

        if (transform.position.y < -100)
        {
            Destroy(gameObject);
        }

    }

    public void SetVars(float spd, float dmg, float grav, AnimationCurve grCurve, float lifeTime, float explosionRadius, float expDamage, List<BaseEnemyClass.Types> types)
    {
        speed = spd;
        damage = dmg;
        gravity = grav;
        gravCurve = grCurve;
        gravityLifetime = lifeTime;
        explosionRadii = explosionRadius;
        explosionDamage = expDamage;
        attackTypes = types;
    }



    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);

        //if enemy, hit them for the damage
        Collider taggedEnemy = null;
        if(other.isTrigger && other.gameObject.layer != 10)
        {
            return;
        }
        if (other.tag == "Environment")
        {
            gravity = 0;
            speed = 0;
            Destroy(this.gameObject.GetComponent<Collider>());
            this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);


        }
        if (other.gameObject.layer == 8 && active && other.GetComponent<BaseEnemyClass>())
        {
            other.gameObject.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            audioManager.Stop("Slime Damage");
            audioManager.Play("Slime Damage");
            hitMarker.transform.GetChild(7).gameObject.SetActive(true);
            Invoke("HitMarkerDsable", 0.2f);
            taggedEnemy = other;


        }
        if (other.gameObject.layer != 11 && other.gameObject.layer != 20 && other.gameObject.tag != "Node" && active)
        {
            Collider[] objectsHit = Physics.OverlapSphere(transform.position, explosionRadii);

            for (int i = 0; i < objectsHit.Length; i++)
            {
                if(objectsHit[i].tag == "Enemy" && objectsHit[i] != taggedEnemy)
                {
                    RaycastHit hit;
                    if(Physics.Raycast(this.transform.position + (objectsHit[i].transform.position - this.transform.position).normalized * -2, (objectsHit[i].transform.position - this.transform.position).normalized, out hit, 5, enemyDetect))
                    {
                        if((hit.collider.gameObject.GetComponent<EnemyShield>() && !objectsHit[i].GetComponent<EnemyShield>()) || hit.collider.gameObject.layer == 10)
                        {
                            
                        }
                        else
                        {
                            objectsHit[i].GetComponent<BaseEnemyClass>().TakeDamage(explosionDamage, attackTypes);
                        }

                    }
                }
            }
            gravity = 0;
            speed = 0;
            Destroy(this.gameObject.GetComponent<Collider>());
            this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            // Sound FX
            audioManager.Stop("Fireball Impact");
            audioManager.Play("Fireball Impact");
        }


    }

    private void HitMarkerDsable()
    {
        hitMarker.transform.GetChild(7).gameObject.SetActive(false);
    }
}
