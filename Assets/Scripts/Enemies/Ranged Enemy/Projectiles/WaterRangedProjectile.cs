using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRangedProjectile : BaseRangedProjectileScript
{
    protected override void HitEffect()
    {
        player.GetComponent<PlayerMovement>().AddMovementMultiplier(0.5f);
        player.GetComponent<PlayerClass>().ChangeHealth(-damage);
        Destroy(this.gameObject.GetComponent<MeshRenderer>());
        Destroy(this.gameObject.GetComponent<SphereCollider>());
        speed = 0;
        StartCoroutine(UndoSlow());

    }

    IEnumerator UndoSlow()
    {

        yield return new WaitForSeconds(10);

        player.GetComponent<PlayerMovement>().RemoveMovementMultiplier(0.5f);
        Destroy(this.gameObject);
    }
}
