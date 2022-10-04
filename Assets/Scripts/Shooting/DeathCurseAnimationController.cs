using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathCurseAnimationController : MonoBehaviour
{
	[SerializeField]
    ParticleSystem SplatDamage;
    public void VFXDeathCurseParticleSplat()
    {
        SplatDamage.Play();
    }
}
