using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : BaseElementSpawnClass
{
    float speed;

    float damage;

    float projectileLifetime = 100;

    [SerializeField]
    private GameObject hitMarker;

    [SerializeField]
    private LayerMask bounceLayers;

    AudioManager audioManager;
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        hitMarker = GameObject.Find("GameplayUI");
    }

    void Update()
    {
        if (projectileLifetime > 0)
        {
            projectileLifetime -= Time.deltaTime;
        }
        else
        {
            Destroy(this.gameObject); //Temp, set up to disable particles later
        }

        Vector3 movement = transform.forward * speed * Time.deltaTime;

        transform.position += movement;

        //if (this.gameObject.transform.GetChild(1).GetChild(0).gameObject.GetComponent<ParticleSystem>().time >= 2)
        //{
        //    Destroy(this.gameObject);
        //}

        //if (transform.position.y < -100)
        //{
        //    Destroy(gameObject);
        //}

    }

    public void SetVars(float spd, float dmg, float lifeTime, List<BaseEnemyClass.Types> types)
    {
        speed = spd;
        damage = dmg;
        projectileLifetime = lifeTime;
        attackTypes = types;

    }


    private void OnCollisionEnter(Collision collision)
    {
        if (bounceLayers == (bounceLayers | (1 << collision.collider.gameObject.layer)))
        {
            this.transform.forward = Vector3.Reflect(this.transform.forward, collision.contacts[0].normal);
            //this.transform.eulerAngles = new Vector3(0, this.transform.rotation.y, 0);
        }
        if(collision.collider.tag == "Enemy")
        {
            collision.collider.gameObject.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            Debug.Log(collision.collider.gameObject.name);
        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    //if enemy, hit them for the damage
    //    Collider taggedEnemy = null;

    //    if (other.isTrigger)
    //    {
    //        return;
    //    }

    //    if (other.tag == "Environment")
    //    {
    //        //Destroy(gameObject);
    //        gravity = 0;
    //        speed = 0;
    //        Destroy(this.gameObject.GetComponent<Collider>());
    //        this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
    //        this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
    //        this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
    //        this.gameObject.transform.GetChild(1).gameObject.SetActive(true);

    //    }
    //    if (other.tag == "Enemy")
    //    {
    //        other.gameObject.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
    //        audioManager.Stop("Slime Damage");
    //        audioManager.Play("Slime Damage");
    //        hitMarker.transform.GetChild(7).gameObject.SetActive(true);
    //        Invoke("HitMarkerDsable", 0.2f);
    //        taggedEnemy = other;
    //    }
    //    if (other.gameObject.tag != "Player" && other.gameObject.tag != "Node")
    //    {
    //        Collider[] objectsHit = Physics.OverlapSphere(transform.position, explosionRadii);

    //        for (int i = 0; i < objectsHit.Length; i++)
    //        {
    //            if (objectsHit[i].tag == "Enemy" && objectsHit[i] != taggedEnemy)
    //            {
    //                objectsHit[i].GetComponent<BaseEnemyClass>().TakeDamage(explosionDamage, attackTypes);
    //            }
    //        }
    //        gravity = 0;
    //        speed = 0;
    //        Destroy(this.gameObject.GetComponent<Collider>());
    //        this.gameObject.transform.GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
    //        this.gameObject.transform.GetChild(0).GetChild(0).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
    //        this.gameObject.transform.GetChild(0).GetChild(1).GetComponent<ParticleSystem>().Stop(true, ParticleSystemStopBehavior.StopEmitting);
    //        this.gameObject.transform.GetChild(1).gameObject.SetActive(true);
    //        // Sound FX
    //        audioManager.Stop("Fireball Impact");
    //        audioManager.Play("Fireball Impact");
    //    }


    //}

    private void HitMarkerDsable()
    {
        hitMarker.transform.GetChild(7).gameObject.SetActive(false);
    }
}
