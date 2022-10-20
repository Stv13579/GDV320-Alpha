using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomBossScript : BaseEnemyClass //Sebastian
{
    [Header("Boss attacks")]
    float timer = 5.0f;
    [SerializeField]
	List<GameObject> sporeClouds;
	List<GameObject> spawnedSporeClouds = new List<GameObject>();

    [SerializeField]
    GameObject bossHealthbar;

    bool attacking = false;
    Vector3 movement = Vector3.zero;
    [SerializeField]
	GameObject nearestNode;
	Vector3 bestNodePos = Vector3.zero;
	float contactTimer = 0.0f;
	CharacterController controller;
	Rigidbody rb;
	float damageTimer = 0.0f;

    public override void Awake()
    {
        base.Awake();
        GameObject healthbar = Instantiate(bossHealthbar);
        BossHealthbarScript healthbarScript = healthbar.GetComponent<BossHealthbarScript>();
	    healthbarScript.AddEnemy(this);
	    healthbarScript.SetName("Mushlord");
	    healthbarScript.SetMaxHealth(maxHealth);
    }

    private void Start()
    {
	    StartCoroutine(FindNode());
	    controller = GetComponent<CharacterController>();
	    rb = GetComponent<Rigidbody>();
	    deathTriggers.Add(DestroySporeClouds);

    }
    public override void Update()
    {
        base.Update();
        timer -= Time.deltaTime;
        contactTimer -= Time.deltaTime;
        if (timer <= 0)
        {
            enemyAnims.SetTrigger("Attacking");
            attacking = true;
        }

        if (!attacking)
        {
            if (Vector3.Distance(this.transform.position, player.transform.position) < 15)
            {

                Movement(player.transform.position, moveSpeed);
            }
            else
            {
                Movement(bestNodePos, moveSpeed);
            }
        }
	    damageTimer -= Time.deltaTime;
    }
    
	public override void TakeDamage(float damageToTake, List<Types> attackTypes, float extraSpawnScale = 1, bool applyTriggers = true)
	{
		if(damageTimer <= 0)
		{
			base.TakeDamage(damageToTake, attackTypes, extraSpawnScale, applyTriggers);
		}
		damageTimer = 0.1f;
	}

    public override void Movement(Vector3 positionToMoveTo, float speed)
    {
        this.gameObject.transform.LookAt(positionToMoveTo);
        this.gameObject.transform.eulerAngles = new Vector3(0, this.gameObject.transform.eulerAngles.y, 0);
        movement = this.transform.forward * speed * Time.deltaTime;
        //if (!controller.isGrounded)
        //{
        //    movement += new Vector3(0, gravity, 0);
        //}
        //rb.velocity = movement;
        transform.position += movement;
    }

    IEnumerator FindNode()
    {
        while (true)
        {
            float distance = float.MaxValue;
            foreach (Node node in spawner.GetComponent<SAIM>().aliveNodes)
            {
                float newDistance = Vector3.Distance(node.gameObject.transform.position, this.gameObject.transform.position);
                if (newDistance < distance)
                {
                    distance = newDistance;
	                nearestNode = node.gameObject;
	                bestNodePos = nearestNode.GetComponent<Node>().bestNextNodePos;
                }
            }
            yield return new WaitForSeconds(1);
	    }
    }

    public void SpawnSporeCloud()
    {
	    bool sporing = true;
        while(sporing)
        {
            int randNodeInt = Random.Range(0, spawner.GetComponent<SAIM>().aliveNodes.Count);
            GameObject randNode = spawner.GetComponent<SAIM>().aliveNodes[randNodeInt].gameObject;
            if(Vector3.Distance(randNode.transform.position, player.transform.position) < 20)
            {
	            spawnedSporeClouds.Add(Instantiate(sporeClouds[Random.Range(0, sporeClouds.Count)], randNode.transform.position, Quaternion.identity));
	            sporing = false;
            }
        }
        attacking = false;
	    timer = Random.Range(3f, 5f);
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Player" && contactTimer <= 0)
        {
            playerClass.ChangeHealth(-damageAmount, gameObject);
            contactTimer = 0.3f;
        }
    }
    
	protected virtual void DestroySporeClouds(GameObject temp)
	{
		for(int i = spawnedSporeClouds.Count - 1; i >= 0; i--)
		{
			Destroy(spawnedSporeClouds[i].gameObject);
		}
	}
}
