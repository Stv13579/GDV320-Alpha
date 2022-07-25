using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    private float shieldHealthTicker;

    [SerializeField]
    private EnergyElement energyElement;

    [SerializeField]
    private GameObject Player;

    AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), Player.GetComponent<Collider>());
    }

    private void OnCollisionEnter(Collision collision)
    {
        // collision with the enemy
        if(collision.gameObject.layer == 8)
        {
            
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        // collision with the enemy
        if (collision.gameObject.layer == 8)
        {

        }
    }
}
