using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProphecyManager : MonoBehaviour
{
    /// <summary>
    /// A list of the attached prophecy components.
    /// When adding a prophecy to the prophecy manager, remember to add it to this list as well.
    /// </summary>
    [SerializeField]
    public List<Prophecy> allProphecies;
    public float prophecyDamageMulti = 1.0f;
    public float prophecyEffectMulti = 1.0f;
    public bool wealth = false;
    
   
    public List<Multiplier> damageMultipliers = new List<Multiplier>();
    public List<Multiplier> effectMultipliers = new List<Multiplier>();

    private void Start()
    {

    }



}
