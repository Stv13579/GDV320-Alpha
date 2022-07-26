using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRangedProjectileScript : MonoBehaviour
{
    [SerializeField]
    protected float speed;
    [SerializeField]
    protected float damage;
    protected GameObject player;
    // Start is called before the first frame update
    protected void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime;
    }

    protected virtual void HitEffect()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            HitEffect();
        }
        else if (other.gameObject.layer == 8 || other.gameObject.layer == 10 || other.gameObject.layer == 16)
        {
            Destroy(this.gameObject);
        }


    }
}
