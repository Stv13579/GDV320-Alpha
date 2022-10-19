using System.Collections;
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
        gameplayUI = FindObjectOfType<GameplayUI>();
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
        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
	    if(other.gameObject == player && contactTimer <= 0.0f && !onFire)
        {
            gameplayUI.GetInToxicFullScreen().gameObject.SetActive(true);
            gameplayUI.GetInToxicFullScreen().material.SetFloat("_Toggle_EffectIntensity", 10.0f);
            playerClass.ChangeHealth(-damageAmount, gameObject);
            contactTimer = 0.3f;
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
