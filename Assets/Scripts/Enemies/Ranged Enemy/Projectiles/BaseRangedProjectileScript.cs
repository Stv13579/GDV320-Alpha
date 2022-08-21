using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRangedProjectileScript : MonoBehaviour //Sebastian
{
    protected float speed;
    protected float damage;
    protected GameObject player;
    protected float timer = 0;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime;
        timer += Time.deltaTime;
        if(timer > 10)
        {
            Destroy(this.gameObject);
        }
    }
    //What the projectile does when it hits  a player
    protected virtual void HitEffect(Collider other)
    {

    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player && player.GetComponent<EnergyElement>().GetUseShield() == false)
        {
            HitEffect(other);
        }
        else if (other.gameObject.layer == 10 || other.gameObject.layer == 16)
        {
            Destroy(this.gameObject);
        }


    }
    //Set the variables of the projectile
    public void SetVars(float projectileSpeed, float projectileDamage)
    {
        speed = projectileSpeed;
        damage = projectileDamage;
    }
}
