﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeCloudScript : BaseEnemyClass
{
    [SerializeField]
    GameObject fireVFX;
    bool onFire = false;
    float contactTimer = 0.0f;
    GameplayUI gameplayUI;
    private void Start()
    {
	    gameplayUI = GameplayUI.GetGameplayUI();
    }
    public override void Update()
    {
        contactTimer -= Time.deltaTime;
    }

    public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1, bool applyTriggers = true)
	{
		Debug.Log(attackTypes[0]);
	    if(attackTypes.Contains(weaknesses[0]) && !onFire)
        {
            StartCoroutine(StartFire());
        }
    }
	

    IEnumerator StartFire()
    {
        onFire = true;
        Instantiate(fireVFX, this.transform.position, Quaternion.identity, this.transform);
        yield return new WaitForSeconds(5);
	    gameplayUI.GetInToxicFullScreen().gameObject.SetActive(false);
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
        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
	    if(other.gameObject == player && contactTimer <= 0.0f && !onFire)
        {
            gameplayUI.GetInToxicFullScreen().gameObject.SetActive(true);
            gameplayUI.GetInToxicFullScreen().material.SetFloat("_Toggle_EffectIntensity", 10.0f);
            playerClass.ChangeHealth(-damageAmount, gameObject);
            contactTimer = 0.5f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == player)
        {
            gameplayUI.GetInToxicFullScreen().gameObject.SetActive(false);
            gameplayUI.GetInToxicFullScreen().material.SetFloat("_Toggle_EffectIntensity", 0.0f);
        }
    }
}
