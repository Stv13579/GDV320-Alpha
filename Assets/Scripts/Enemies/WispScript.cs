using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WispScript : BaseEnemyClass
{
	float moveTimer = 0;
	Vector3 targetPos;
	Vector3 startPos;
	bool destroyed = false;
	Types element;
	Renderer renderer;
	public override void Awake()
	{
		startPos = this.transform.position;
		targetPos = this.transform.position;
		renderer = GetComponentInChildren<Renderer>();
	}
	
	// Start is called on the frame when a script is enabled just before any of the Update methods is called the first time.
	protected void Start()
	{
		Physics.IgnoreCollision(GetComponentInChildren<Collider>(), PlayerClass.GetPlayer().GetComponent<Collider>());

	}
	
	public override void Update()
	{
		moveTimer -= Time.deltaTime;
		if(moveTimer <= 0)
		{
			float x = startPos.x + Random.Range(-3.0f, 3.0f);
			float y = startPos.y + Random.Range(-1.0f, 3.0f);
			float z = startPos.z + Random.Range(-3.0f, 3.0f);

			targetPos = new Vector3(x, y, z);
			moveTimer = Random.Range(2, 4);
		}
		Movement(targetPos);
		this.transform.GetChild(0).localPosition = new Vector3(0, Mathf.Sin(Time.time), 0);
	}
	
	public override void Movement(Vector3 positionToMoveTo)
	{
		this.transform.position = Vector3.MoveTowards(this.transform.position, positionToMoveTo, 0.1f);
	}
	
	public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1, bool applyTriggers = true)
	{
		if(attackTypes[0] == weaknesses[0])
		{
			//Special effect
			Drop(drops.GetHealthList(), drops.GetMinHealthSpawn(), drops.GetMaxHealthSpawn());
			Drop(drops.GetMinAmmoSpawn(), drops.GetMaxAmmoSpawn());

			destroyed = true;
			StartCoroutine(FadeOut());
		}
		else if(attackTypes[0] == resistances[0])
		{
			StartCoroutine(FadeOut());
		}
		else
		{
			StartCoroutine(FadeOut());
		}

	}
	
	IEnumerator FadeOut()
	{
		float a = 1;
		GetComponentInChildren<Collider>().enabled = false;
		Material mat = renderer.material;
		while(a > 0)
		{
			a -= Time.deltaTime / 2;
			Color color = new Color(mat.color.r, mat.color.g, mat.color.b, a);
			renderer.material.color = color;
			yield return null;
		}
		yield return new WaitForSeconds(5);
		if(!destroyed)
		{
			StartCoroutine(FadeIn());
		}
		else
		{
			Destroy(this.gameObject);
		}
		

		
	}
	
	IEnumerator FadeIn()
	{
		float a = 0;
		Material mat = renderer.material;
		while(a < 1)
		{
			a += Time.deltaTime / 2;
			Color color = new Color(mat.color.r, mat.color.g, mat.color.b, a);
			renderer.material.color = color;
			yield return null;
		}
		GetComponentInChildren<Collider>().enabled = true;

	}
	
	public Types GetElement()
	{
		return element;
	}
	
}
