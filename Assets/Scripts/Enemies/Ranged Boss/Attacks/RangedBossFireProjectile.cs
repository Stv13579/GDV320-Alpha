using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossFireProjectile : BaseRangedProjectileScript //Sebastian
{
    GameObject telegraph;
    [SerializeField]
	float radius = 2.0f;
    
	[SerializeField]
	GameObject impactEffect;
	bool hit = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        this.transform.LookAt(telegraph.transform);
    }
    //If the player is within radius of the hit point, damage them
    protected override void HitEffect(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);
        bool damaged = false;
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<PlayerClass>() && !damaged && player.GetComponent<EnergyElement>().GetUseShield() == false)
            {
                collider.gameObject.GetComponent<PlayerClass>().ChangeHealth(-damage);
                damaged = true;
            }
        }
	    Destroy(telegraph);
	    foreach (ParticleSystem particles in GetComponentsInChildren<ParticleSystem>())
	    {
	    	particles.Stop();
	    }
	    
	    impactEffect.active = true;
	    hit = true;

	    
    }
    
	protected override void Update()
	{
		if(hit)
		{
			if(!impactEffect.GetComponent<ParticleSystem>().isPlaying)
			{
				Destroy(this.gameObject);
			}
			
		}
	}

    protected override void OnTriggerEnter(Collider other)
	{
		if(!hit)
		{        
			HitEffect(other);
		}
		
    }

    public void SetTelegraph(GameObject tele)
    {
        telegraph = tele;
    }
}
