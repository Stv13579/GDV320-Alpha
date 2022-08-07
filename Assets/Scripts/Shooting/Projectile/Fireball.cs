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

        //if enemy, hit them for the damage
        Collider taggedEnemy = null;
        if(other.isTrigger && other.gameObject.layer != 10)
        {
            return;
        }
        if (other.tag == "Shield")
        {
            other.gameObject.GetComponent<EnemyShield>().DamageShield(damage, attackTypes);
            gravity = 0;
            speed = 0;
            Destroy(this.gameObject.GetComponent<Collider>());
            this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
            this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            Debug.Log("Shield");
            Debug.Log(Time.frameCount);
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
        if (other.gameObject.layer  == 8 && active)
        {
            other.gameObject.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            audioManager.Stop("Slime Damage");
            audioManager.Play("Slime Damage");
            hitMarker.transform.GetChild(7).gameObject.SetActive(true);
            Invoke("HitMarkerDsable", 0.2f);
            taggedEnemy = other;
            Debug.Log("Enemy");
            Debug.Log(Time.frameCount);


        }
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Node" && other.gameObject.tag != "Shield" && active)
        {
            Collider[] objectsHit = Physics.OverlapSphere(transform.position, explosionRadii);

            for (int i = 0; i < objectsHit.Length; i++)
            {
                if(objectsHit[i].tag == "Enemy" && objectsHit[i] != taggedEnemy)
                {
                    objectsHit[i].GetComponent<BaseEnemyClass>().TakeDamage(explosionDamage, attackTypes);
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
