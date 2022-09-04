using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeCloudScript : BaseEnemyClass
{
    [SerializeField]
    GameObject fireVFX;
    bool onFire = false;
    float contactTimer = 0.0f;
    public override void Update()
    {
        base.Update();
        contactTimer -= Time.deltaTime;
    }

    public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1)
    {
        if(attackTypes.Contains(Types.Fire) && !onFire)
        {
            StartCoroutine(StartFire());
        }
    }


    IEnumerator StartFire()
    {
        onFire = true;
        Instantiate(fireVFX, this.transform.position, Quaternion.identity, this.transform );
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player && contactTimer <= 0.0f)
        {
            playerClass.ChangeHealth(-damageAmount);
            contactTimer = 0.3f;
        }
    }
}
