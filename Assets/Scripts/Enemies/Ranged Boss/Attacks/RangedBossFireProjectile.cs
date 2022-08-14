using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossFireProjectile : MonoBehaviour
{
    [HideInInspector]
    public GameObject telegraph;
    [HideInInspector]
    public float damage;
    public float radius = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        this.transform.LookAt(telegraph.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Collider[] colliders = Physics.OverlapSphere(this.transform.position, radius);
        bool damaged = false;
        foreach(Collider collider in colliders)
        {
            if(collider.gameObject.GetComponent<PlayerClass>() && !damaged)
            {
                collider.gameObject.GetComponent<PlayerClass>().ChangeHealth(-damage);
                damaged = true;
            }
        }
        Destroy(telegraph);
        Destroy(this.gameObject);
    }
}
