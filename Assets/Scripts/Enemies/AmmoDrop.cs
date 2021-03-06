using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoDrop : MonoBehaviour
{
    Rigidbody rb;
    Transform player;
    public List<PlayerClass.ManaName> manaTypes;
    AudioManager audioManager;
    bool moving;

    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;

        //Add an inital force so the ammo shoots out
        rb.AddForce((this.transform.up * 500 + new Vector3(Random.Range(-1.0f, 1.0f), 0.0f, Random.Range(-1.0f, 1.0f)) * 50));
        //Ignore collisions between ammo objects
        Physics.IgnoreLayerCollision(4, 4);
        Physics.IgnoreLayerCollision(4, 8);
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        //If the player moves in range, disable he rigidbody and switch the collider to a trigger
        if ((player.position - transform.position).magnitude < 5 && !moving)
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
            this.gameObject.GetComponent<Collider>().isTrigger = true;
            moving = true;
        }
        else if ((player.position - transform.position).magnitude > 10 && moving)
        {
            this.gameObject.GetComponent<Rigidbody>().isKinematic = true;
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
        if(other.gameObject.tag == "Player")
        {
            player.gameObject.GetComponent<PlayerClass>().ChangeMana(10, manaTypes);
            Destroy(this.gameObject);
        }
    }
}
