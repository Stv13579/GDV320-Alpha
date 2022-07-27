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

    float force = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        // collision with the enemy
        if(collision.gameObject.layer == 8)
        {
            Vector3 dir = collision.contacts[0].point - transform.position;

            dir = -dir.normalized;

            collision.gameObject.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
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
