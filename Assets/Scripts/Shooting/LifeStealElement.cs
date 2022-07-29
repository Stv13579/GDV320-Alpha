using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealElement : BaseElementClass
{
    [SerializeField]
    private GameObject LifeSteal;

    [SerializeField]
    float damage;
    public float damageMultiplier = 1;


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
