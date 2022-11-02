using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossHomingProjectileScript : BaseEnemyClass //Sebastian
{
    public override void Update()
    {
        //Move towards the player
        this.transform.LookAt(player.transform);
        this.transform.position += this.transform.forward * moveSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        //if it hits the player, damage them
        if (other.gameObject.GetComponent<PlayerClass>())
        {
            other.gameObject.GetComponent<PlayerClass>().ChangeHealth(-damageAmount, FindObjectOfType<RangedBossScript>().gameObject);
            Destroy(this.gameObject);
        }
        else if (other.gameObject.layer == 10)
        {
            Destroy(this.gameObject);
        }
    }
	
	public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1, bool applyTriggers = true)
	{
		base.TakeDamage(damageToTake, attackTypes, extraSpawnScale, applyTriggers);
		if(currentHealth <= 0)
		{
			Death();
		}
		
	}
	
    public override void Death()
	{
		StartCoroutine(Destruction());
		int dropType = Random.Range(0, 3);

		switch (dropType)
		{
		case 0:
		case 1:
			Drop(drops.GetMinAmmoSpawn(), drops.GetMaxAmmoSpawn());
			break;
		case 2:
			Drop(drops.GetHealthList(), drops.GetMinHealthSpawn(), drops.GetMaxHealthSpawn());
			break;
		default:
			break;
		}
	}
    
	IEnumerator Destruction()
	{
		GetComponent<Collider>().enabled = false;
		GetComponent<Renderer>().enabled = false;
		
		for(int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
		yield return new WaitForSeconds(2);
		Destroy(this.gameObject);
	}
}
