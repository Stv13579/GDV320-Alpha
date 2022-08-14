using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedBossFakeFireProjectileScript : MonoBehaviour
{
    float timer = 0.0f;
    public GameObject fireProjectile;
    public GameObject telegraph;
    public LayerMask enviroMask;
    [HideInInspector]
    public float damage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (timer < 4.0f)
        {
            timer += Time.deltaTime;
            this.transform.position += this.transform.forward * 30 * Time.deltaTime;
        }
        else
        {
            Transform player = GameObject.Find("Player").transform;
            RaycastHit hit;
            Physics.Raycast(player.position, -player.transform.up, out hit, Mathf.Infinity, enviroMask);
            GameObject tele = Instantiate(telegraph, hit.point + new Vector3(0, 0.001f, 0), Quaternion.identity);
            GameObject fire = Instantiate(fireProjectile, hit.point + new Vector3(0, 50, 0), Quaternion.identity);
            RangedBossFireProjectile fireScript = fire.GetComponent<RangedBossFireProjectile>();
            fireScript.telegraph = tele;
            fireScript.damage = damage;
            Destroy(this.gameObject);
        }
    }
}
