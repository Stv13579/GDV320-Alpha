using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossCrystalProjectileScript : BaseRangedProjectileScript //Sebastian
{
    bool moving = true;
    [SerializeField]
    GameObject crystalSpawn;
    private EnergyElement energyScript;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        energyScript = FindObjectOfType<EnergyElement>();
	    this.transform.LookAt(PlayerClass.GetPlayerClass().transform);
        this.transform.up = this.transform.forward;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(moving)
        {
            this.transform.position += this.transform.up * 10 * Time.deltaTime;
        }
        else
        {
            timer += Time.deltaTime;
            if (timer > 5.0f)
            {
                Explode();
            }
        }
    }
    //If it hits the player, do lots of damage and explode, otherwise sart waiting
    protected override void HitEffect(Collider other)
    {
       if(moving && other.GetComponent<PlayerClass>())
        {
            other.GetComponent<PlayerClass>().ChangeHealth(-damage * 5, FindObjectOfType<RangedBossScript>().gameObject);
            Explode();
        }
        else if (other.gameObject.layer == 10)
        {
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), other);
	        Explode();
        }
        else if(moving && player.GetComponent<EnergyElement>().GetUseShield() == true)
        {
            Explode();
        }
    }

    protected override void OnTriggerEnter(Collider other)
    {
        HitEffect(other);
    }

    //Embed in he ground slightly before stopping
    IEnumerator Wait()
    {
        while(moving)
        {
            yield return new WaitForSeconds(0.1f);
            moving = false;
            this.gameObject.GetComponent<Collider>().isTrigger = false;
        }
    }
    //Laucnh a bunch of small crystals out from the big crystal
    void Explode()
    {
        for(int i = -4; i < 5; i++)
        {
            for(int t = 0; t < 10; t++)
            {
	            GameObject crystalProj = Instantiate(crystalSpawn, this.transform.position + Vector3.Cross(new Vector3(1.5f * Mathf.Cos(t * 0.2f * Mathf.PI), 0, 1.5f * Mathf.Sin(t * 0.2f * Mathf.PI)), this.transform.up), this.transform.rotation);
                crystalProj.transform.position += this.transform.up * i / 2.0f;
                crystalProj.transform.LookAt(this.transform);
                crystalProj.transform.up = -crystalProj.transform.forward;
                crystalProj.GetComponent<RangedBossSmallCrystalProjectile>().SetVars(20, damage);

            }
        }
        Destroy(this.gameObject);
    }
}
