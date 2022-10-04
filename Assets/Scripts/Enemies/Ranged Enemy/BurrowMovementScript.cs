using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowMovementScript : MonoBehaviour
{
	float moveTime;
	float timer = 0;
	Vector3 targetPos;
	[SerializeField]
	LayerMask enviroLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	public void SetVars(Vector3 pos, float time)
	{
		targetPos = pos;
		moveTime = time;
		StartCoroutine(Move());
	}
	
	IEnumerator Move()
	{
		Vector3 startPos = this.transform.position;
		while (timer <= moveTime)
		{
			timer += Time.deltaTime;
			if(GetComponent<ParticleSystem>().isPaused)
			{
				GetComponent<ParticleSystem>().Play();

			}

			Vector3 pos = Vector3.Lerp(startPos, targetPos, timer / moveTime);
			RaycastHit hit;
			if(Physics.Raycast(this.transform.position + new Vector3(0, 2, 0), -this.transform.up, out hit, Mathf.Infinity, enviroLayer))
			{
				pos.y = hit.point.y - 0.05f;
			}
			if(Mathf.Abs(this.transform.position.y - pos.y) > 1)
			{
				Debug.Log(Mathf.Abs(this.transform.position.y - pos.y));
				GetComponent<ParticleSystem>().Pause();
			}
			this.transform.position = pos;
			yield return null;
		}
		Destroy(this.gameObject);
	}
}
