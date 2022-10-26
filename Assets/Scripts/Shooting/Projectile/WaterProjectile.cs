using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterProjectile : BaseElementSpawnClass
{
    float speed;

    float damage;

    float projectileLifetime = 100;

    [SerializeField]
    LayerMask bounceLayers;

    AudioManager audioManager;

    GameObject player;
    void Awake()
    {
	    audioManager = AudioManager.GetAudioManager();
	    player = PlayerClass.GetPlayerClass().gameObject;
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

	    Growing();
    }
	void Growing()
    {
        if (this.gameObject.transform.localScale.x <= 1.0f &&
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

    void OnCollisionEnter(Collision collision)
	{
        if (bounceLayers == (bounceLayers | (1 << collision.collider.gameObject.layer)))
        {
            this.transform.forward = Vector3.Reflect(this.transform.forward, collision.contacts[0].normal);
            //this.transform.eulerAngles = new Vector3(0, this.transform.rotation.y, 0);
            if(audioManager)
            {
                audioManager.StopSFX("Water Element Impact");
                audioManager.PlaySFX("Water Element Impact", player.transform, this.transform);
            }
        }
        if(collision.collider.tag == "Enemy" || collision.collider.gameObject.layer == 8 && collision.collider.GetComponentInParent<BaseEnemyClass>())
        {
	        collision.collider.gameObject.GetComponentInParent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            if (audioManager)
            {
                audioManager.StopSFX("Water Element Impact");
                audioManager.PlaySFX("Water Element Impact", player.transform, this.transform);
            }
        }
	}
    
	// OnTriggerEnter is called when the Collider other enters the trigger.
	protected void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.GetComponent<SporeCloudScript>())
		{
			other.gameObject.GetComponent<SporeCloudScript>().TakeDamage(damage, attackTypes);
		}
	}
}
