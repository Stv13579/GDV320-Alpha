using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSlimeProjectile : MonoBehaviour
{
    Rigidbody rb;
    private float projectileDamage;
    GameObject player;
    [SerializeField]
    private float followTimer;
    [SerializeField]
    private float followTimerLength;
    [SerializeField]
    private float lifeTimer;
    [SerializeField]
    private float lifeTimerLength;
    private AudioManager audioManager;
    [SerializeField]
    private float upForce;
    [SerializeField]
    private float forwardForce;
    [SerializeField]
    private bool isMoving;

    float rotTimer = 0.0f;
    [SerializeField]
    private float rotTimerMax = 1.0f;

    private Vector3 originPos;

    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        // setting the timers at start
        // shoots the projectiles up and out 
	    player = PlayerMovement.GetPlayerMovement().gameObject;//GameObject.Find("Player");
	    audioManager = AudioManager.GetAudioManager();
    }
    
	public void Shoot()
	{
		isMoving = true;
		this.gameObject.GetComponent<Rigidbody>().AddForce(this.transform.up * upForce + this.transform.forward * forwardForce);
	}

    // Update is called once per frame
    void Update()
    {
        followTimer -= Time.deltaTime;
        lifeTimer -= Time.deltaTime;
        // when the crystals shoot out rotate teh crystals so that it faces the player
        if (rotTimer <= rotTimerMax)
        {
            rotTimer += Time.deltaTime;
            transform.Rotate(transform.right, Time.deltaTime * 200);
        }
        if (isMoving == true)
        {
            //if follow timer is greater then 0 then follow the player
	        if (followTimer >= 0)
            {
                this.transform.position = Vector3.MoveTowards(this.transform.position, player.transform.position, 10 * Time.deltaTime);
            }
        }
        else
        {
            transform.position = originPos;
            this.GetComponent<SphereCollider>().enabled = false;
        }
        // if life timer is less then 0 then destroy enemy slime crystal projectile and reset timer
	    if (lifeTimer <= 0)
        {
            Color color = this.gameObject.GetComponent<MeshRenderer>().material.GetColor("_BaseColour");
            color = new Color(color.r, color.g, color.b, color.a - Time.deltaTime);
            if (color.a <= 0.0f)
            {
            	color.a = 1;
            	rotTimer = 0.0f;
            	followTimer = followTimerLength;
            	lifeTimer = lifeTimerLength;
            	this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            	this.gameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColour", color);
            	this.gameObject.SetActive(false);
	            //Destroy(this.gameObject);
            }
            else
            {
                this.gameObject.GetComponent<MeshRenderer>().material.SetColor("_BaseColour", color);
            }
        }
    }

    // damages the player if get in contact with the projectile
    // or destroy object if it touchs the ground
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.gameObject.GetComponentInParent<PlayerClass>().ChangeHealth(-projectileDamage, enemy);
            Destroy(this.gameObject);

            if (audioManager)
            {
                audioManager.StopSFX("Player Damage");
                audioManager.PlaySFX("Player Damage", player.transform, this.transform);
            }
        }
        if (other.gameObject.layer == 10 || other.gameObject.layer == 16)
        {
            isMoving = false;
            originPos = transform.position;
        }
    }
    // setter
    public void SetVars(float damage)
    {
        projectileDamage = damage;
    }
}
