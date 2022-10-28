using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
	GameObject enemyToSpawn;
	[SerializeField]
	Animator animator;
	[SerializeField]
	ParticleSystem particles;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
	public void SetEnemy(GameObject enemy)
	{
		enemyToSpawn = enemy;
	}
	
	public void StartSpawn()
	{
		StartCoroutine(Spawn());
	}
	
	IEnumerator Spawn()
	{
		yield return new WaitForSeconds(0.5f);
		animator.SetTrigger("Spawn");
		yield return new WaitForSeconds(0.2f);
		particles.Play();
		while(particles.time < 3)
		{
			yield return null;
		}
		enemyToSpawn.transform.position = this.transform.position;
		enemyToSpawn.SetActive(true);
		animator.gameObject.GetComponent<MeshRenderer>().enabled = false;
		animator.ResetTrigger("Spawn");
		animator.SetTrigger("Finish Spawn");
		while(particles.isPlaying)
		{
			yield return null;
		}
		animator.gameObject.GetComponent<MeshRenderer>().enabled = true;
		this.gameObject.SetActive(false);
	}
}
