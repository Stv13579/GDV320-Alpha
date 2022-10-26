using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProphecyManager : MonoBehaviour
{
	static ProphecyManager currentProphecyManager;
    /// <summary>
    /// A list of the attached prophecy components.
    /// When adding a prophecy to the prophecy manager, remember to add it to this list as well.
    /// </summary>
    [SerializeField]
    public List<Prophecy> allProphecies;
    [HideInInspector]
    public float prophecyDamageMulti = 1.0f, prophecyEffectMulti = 1.0f, prophecyResistMulti = 1.0f, prophecyWeaknessMulti = 1.0f, prophecyHealthMulti = 1.0f;

    public bool wealth = false, famine = false;
   
    public List<Multiplier> damageMultipliers = new List<Multiplier>();
    public List<Multiplier> effectMultipliers = new List<Multiplier>();
    public List<Multiplier> resistMultipliers = new List<Multiplier>();
    public List<Multiplier> weaknessMultipliers = new List<Multiplier>();
    public List<Multiplier> healthMultipliers = new List<Multiplier>();

    private void Start()
    {
	    DontDestroyOnLoad(gameObject);
	    currentProphecyManager = this;
    }
    
	public static ProphecyManager GetProphecyManager()
	{
		return currentProphecyManager;
	}
	

}
