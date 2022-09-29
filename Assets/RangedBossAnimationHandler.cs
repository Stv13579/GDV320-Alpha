using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossAnimationHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	public void FireAttack()
	{
		this.transform.parent.gameObject.GetComponent<RangedBossScript>().FireAttack();
	}
	
	public void StartWater()
	{
		this.transform.parent.gameObject.GetComponent<RangedBossScript>().StartWater();
	}
	
	public void StopWater()
	{
		this.transform.parent.gameObject.GetComponent<RangedBossScript>().StopWater();
	}
	
	public void HomingAttack()
	{
		this.transform.parent.gameObject.GetComponent<RangedBossScript>().HomingAttack();

	}
	
	public void CrystalAttack()
	{
		this.transform.parent.gameObject.GetComponent<RangedBossScript>().CrystalAttack();

	}
}
