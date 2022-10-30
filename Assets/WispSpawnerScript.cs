using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispSpawnerScript : MonoBehaviour
{
	[SerializeField]
	List<GameObject> wisps = new List<GameObject>();
	List<GameObject> validWisps = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
	{
		if(Shooting.GetShooting().GetPrimaryElements()[0] != Shooting.GetShooting().GetBlankElement())
		{
			if(Shooting.GetShooting().GetPrimaryElements()[0] is FireElement || Shooting.GetShooting().GetPrimaryElements()[1] is FireElement)
			{
				validWisps.Add(wisps.Find(obj=>obj.name == "Crystal Wisp Variant"));
			}
			if(Shooting.GetShooting().GetPrimaryElements()[0] is CrystalElement || Shooting.GetShooting().GetPrimaryElements()[1] is CrystalElement)
			{
				validWisps.Add(wisps.Find(obj=>obj.name == "Water Wisp"));
			}
			if(Shooting.GetShooting().GetPrimaryElements()[0] is WaterElement || Shooting.GetShooting().GetPrimaryElements()[1] is WaterElement)
			{
				validWisps.Add(wisps.Find(obj=>obj.name == "Fire Wisp Variant"));
			}
			for(int i = 0; i < 4; i++)
			{
				Instantiate(validWisps[Random.Range(0, validWisps.Count)], this.transform.position, Quaternion.identity);
			}
		}
		else
		{
			for(int i = 0; i < 4; i++)
			{
				Instantiate(wisps[Random.Range(0, wisps.Count)], this.transform.position, Quaternion.identity);
			}
		}

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
