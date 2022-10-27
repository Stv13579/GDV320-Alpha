using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseDropScript : MonoBehaviour //Sebastian
{
    Rigidbody rb;
    protected Transform player;

    protected AudioManager audioManager;
    bool moving;
    bool roomEnd = false;

    [SerializeField]
    float pickupRange;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
	    player = PlayerClass.GetPlayerClass().transform;

        //Add an inital force so the ammo shoots out
        rb.AddForce((this.transform.up * 500 + new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)) * 50));
        //Ignore collisions between ammo objects
        Physics.IgnoreLayerCollision(4, 4);
        Physics.IgnoreLayerCollision(4, 8);
	    audioManager = AudioManager.GetAudioManager();
    }

    // Update is called once per frame
    void Update()
    {
        //If the player moves in range, disable he rigidbody and switch the collider to a trigger
        if (((player.position - transform.position).magnitude < pickupRange && !moving) || roomEnd)
        {
            rb.isKinematic = true;
            this.gameObject.GetComponent<Collider>().isTrigger = true;
            moving = true;
        }
        else if ((player.position - transform.position).magnitude > pickupRange && moving && !roomEnd)
        {
            rb.isKinematic = false;
            this.gameObject.GetComponent<Collider>().isTrigger = false;
            moving = false;
        }

        if (moving)
        {
            //Move towards the player
            transform.position = Vector3.MoveTowards(transform.position, player.position, 5 * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PickupEffect();
            Destroy(this.gameObject);
        }
    }
    //Does whatever the drop should do 
    protected virtual void PickupEffect()
    {
        if (audioManager)
        {
            audioManager.StopSFX("Currency Pickup");
            audioManager.PlaySFX("Currency Pickup");
        }
    }

    public void SetRoomEnd(bool end)
    {
        roomEnd = end;
    }
}
