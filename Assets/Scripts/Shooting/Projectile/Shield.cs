using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField]
    private EnergyElement energyElement;

    [SerializeField]
    private GameObject Player;

    private AudioManager audioManager;

    [SerializeField]
    private float force = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    private void OnTriggerStay(Collider other)
    {
        // collision with the enemy
        if(other.gameObject.layer == 8)
        {
            Vector3 dir = other.transform.position - this.transform.position;
            dir.y = 0;
            dir = dir.normalized;

            other.gameObject.GetComponent<Rigidbody>().AddForce(dir * force, ForceMode.Impulse);
        }
    }
}
