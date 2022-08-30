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

    void Start()
    {
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

        growing();
    }
    private void growing()
    {
        if(this.gameObject.transform.localScale.x <= 1.0f &&
           this.gameObject.transform.localScale.y <= 1.0f &&
           this.gameObject.transform.localScale.z <= 1.0f)
        {
            this.gameObject.transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
        }   
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

    private void HitMarkerDsable()
    {
        hitMarker.transform.GetChild(7).gameObject.SetActive(false);
    }
}
