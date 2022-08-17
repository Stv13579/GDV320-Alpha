using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossCrystalProjectileScript : BaseRangedProjectileScript
{
    bool moving = true;
    float timer = 0.0f;
    [HideInInspector]
    public float smallDamage;
    [HideInInspector]
    public float bigDamage;
    public GameObject crystalSpawn;
    private EnergyElement energyScript;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        energyScript = FindObjectOfType<EnergyElement>();
        this.transform.LookAt(GameObject.Find("Player").transform);
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

    protected override void HitEffect(Collider other)
    {
       if(moving && other.GetComponent<PlayerClass>())
        {
            other.GetComponent<PlayerClass>().ChangeHealth(-bigDamage);
            Explode();
        }
        else if (other.gameObject.layer == 10)
        {
            Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), other);
            StartCoroutine(Wait());
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
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(other.gameObject.tag == "Player" && moving && energyScript.GetUseShield() == false)
    //    {
    //        other.GetComponent<PlayerClass>().ChangeHealth(-bigDamage);
    //        Explode();
    //    }
    //    else if (other.gameObject.layer == 10)
    //    {
    //        Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), other);
    //        StartCoroutine(Wait());
    //    }
    //    else if(other.gameObject.tag == "Player" && moving && energyScript.GetUseShield() == true)
    //    {
    //        Explode();
    //    }
    //}

    IEnumerator Wait()
    {
        while(moving)
        {
            yield return new WaitForSeconds(0.1f);
            moving = false;
            this.gameObject.GetComponent<Collider>().isTrigger = false;
        }
    }

    void Explode()
    {
        for(int i = -4; i < 5; i++)
        {
            for(int t = 0; t < 10; t++)
            {
                GameObject crystalProj = Instantiate(crystalSpawn, this.transform.position + Vector3.Cross(new Vector3(0.5f * Mathf.Cos(t * 36), 0, 0.5f * Mathf.Sin(t * 36)), this.transform.up), this.transform.rotation);
                crystalProj.transform.position += this.transform.up * i / 2.0f;
                crystalProj.transform.LookAt(this.transform);
                crystalProj.transform.up = -crystalProj.transform.forward;
                crystalProj.GetComponent<RangedBossSmallCrystalProjectile>().damage = smallDamage;
                crystalProj.GetComponent<RangedBossSmallCrystalProjectile>().speed = 20;

            }
        }
        Destroy(this.gameObject);
    }
}
