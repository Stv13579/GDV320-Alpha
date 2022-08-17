using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossScript : BaseEnemyClass
{
    [Header("Boss attacks")]
    float timer = 5.0f;
    public Transform projectileSpawnPos;
    public GameObject fireProjectile;
    public GameObject fakeFireProjectile;
    public GameObject waterProjectile;
    public GameObject crystalProjectile;
    public GameObject[] homingProjectiles;
    public Transform tempProjectileSpawnPos;

    public LayerMask groundDetect;

    public override void Start()
    {
        base.Start();
        RaycastHit hit;
        Physics.Raycast(this.gameObject.transform.position, -this.gameObject.transform.up, out hit, Mathf.Infinity, groundDetect);
        Vector3 emergePos = hit.point - this.transform.GetChild(1).localPosition * 2;
        this.transform.position = emergePos;
    }
    public override void Update()
    {
        if(enemyAnims.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            timer -= Time.deltaTime;
        }
        if (timer <= 0.0f)
        {
            int attack = Random.Range(0, 4);
            switch (attack)
            {
                case 0:
                    enemyAnims.SetTrigger("Homing");
                    break;
                case 1:
                    enemyAnims.SetTrigger("Fire");
                    break;
                case 2:
                    enemyAnims.SetTrigger("Crystal");
                    break;
                case 3:
                    enemyAnims.SetTrigger("Water");
                    break;

            }
            timer = Random.Range(2.0f, 4.0f);
        }


        if(Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(FireAttack(5));
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            StartCoroutine(WaterAttack(60));
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            CrystalAttack();
        }
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartCoroutine(HomingAttack(5));
        }
    }
    //Homing attack, randomly instantiates a homing attack variant toSpawn times
    public IEnumerator HomingAttack(int toSpawn)
    {
        int i = 0;
        while (i < toSpawn)
        {
            GameObject homingProj = Instantiate(homingProjectiles[Random.Range(0, homingProjectiles.Length)], projectileSpawnPos.position, Quaternion.identity);
            i++;
            yield return new WaitForSeconds(0.5f);
        }
    }
    //Fire attack, instantiates toSpawn number of fake fireballs, shoots them in the air, which then spawn real fireballs over the player
    public IEnumerator FireAttack(int toSpawn)
    {
        int i = 0;
        while(i < toSpawn)
        {
            GameObject fireProj = Instantiate(fakeFireProjectile, projectileSpawnPos.position, Quaternion.identity);
            fireProj.transform.forward = transform.up;
            fireProj.GetComponent<RangedBossFakeFireProjectileScript>().damage = damageAmount * damageMultiplier;
            i++;
            yield return new WaitForSeconds(0.2f);
        }
    }
    //Water attack, fire toSpawn number of water projectiles evenly in a circle around the boss
    public IEnumerator WaterAttack(int toSpawn)
    {
        //Will change when animations are in
        float angle = 360 / toSpawn;
        int i = 0;
        while (i < toSpawn)
        {
            GameObject waterProj = Instantiate(waterProjectile, tempProjectileSpawnPos.position, Quaternion.identity);
            waterProj.GetComponent<RangedBossWaterProjectileScript>().damage = damageAmount * damageMultiplier;
            waterProj.GetComponent<RangedBossWaterProjectileScript>().speed = 15;
            waterProj.transform.eulerAngles = new Vector3(0, angle * i, 0);
            Physics.IgnoreCollision(this.GetComponent<Collider>(), waterProj.GetComponent<Collider>());
            i++;
            yield return new WaitForSeconds(0.1f);
        }
    }
    //Crystal attack, fires a large crystal projectile towards the player, which can embed in the ground and explode
    public void CrystalAttack()
    {
        GameObject crystalProj = Instantiate(crystalProjectile, projectileSpawnPos.position, Quaternion.identity);
        crystalProj.GetComponent<RangedBossCrystalProjectileScript>().smallDamage = damageAmount * damageMultiplier;
        crystalProj.GetComponent<RangedBossCrystalProjectileScript>().bigDamage = damageAmount * 2 * damageMultiplier;

    }
    //Starts the homing attack, so it can be called by the animator
    public void StartHoming()
    {
        StartCoroutine(HomingAttack(5));
    }
    //Starts the fire attack, so it can be called by the animator
    public void StartFire()
    {
        StartCoroutine(FireAttack(5));
    }
    //Starts the water attack, so it can be called by the animator
    public void StartWater()
    {
        StartCoroutine(WaterAttack(60));
    }
}
