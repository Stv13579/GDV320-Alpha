using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
	GameObject enemyToSpawn;
	SAIM spawnSAIM;
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
    
	public void SetEnemy(GameObject enemy, SAIM spawner)
	{
		enemyToSpawn = enemy;
		spawnSAIM = spawner;
	}
	
	public void StartSpawn()
	{
		animator.SetTrigger("Spawn");
	}
	
	IEnumerator Spawn()
	{
		yield return new WaitForSeconds(0.5f);
		animator.SetTrigger("Spawn");
		yield return new WaitForSeconds(0.2f);
		particles.Play();
		while(particles.isPlaying)
		{
			yield return null;
		}
		enemyToSpawn.transform.position = this.transform.position;
		enemyToSpawn.SetActive(true);
		animator.SetTrigger("Finish Spawn");
		
	}
}
