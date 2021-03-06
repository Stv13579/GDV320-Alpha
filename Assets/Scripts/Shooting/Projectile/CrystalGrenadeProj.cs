using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalGrenadeProj : MonoBehaviour
{
    private float speed;
    private float damage;
    private float timerToExplode;
    private float currentTimer;
    private float gravity;
    private float explosionRange;
    private float explosionDamage;
    List<BaseEnemyClass.Types> attackTypes;
    AudioManager audioManager;
    private Vector3 originalPosition;
    private bool isMoving;
    private bool isAttached;
    Collider enemy;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        isMoving = true;
        isAttached = false;
    }

    // Update is called once per frame
    void Update()
    {
        currentTimer += Time.deltaTime;
        if (!isAttached)
        {
            MoveProjectile();
        }
        if (currentTimer >= timerToExplode)
        {
            Collider[] objectsHit = Physics.OverlapSphere(transform.position, explosionRange);
            for (int i = 0; i < objectsHit.Length; i++)
            {
                if (objectsHit[i].gameObject.layer == 8)
                {
                    objectsHit[i].GetComponent<BaseEnemyClass>().TakeDamage(explosionDamage, attackTypes);
                }
            }
            Destroy(gameObject);
        }
    }
    void MoveProjectile()
    {
        if (isMoving == true)
        {
            Vector3 projMovement = transform.forward * speed * Time.deltaTime;
            transform.position += projMovement;
        }
        else
        {
            transform.position = originalPosition;
        }
    }
    public void SetVars(float spd, float dmg, float timer, float grav, float explosionRadius, float expDamage, List<BaseEnemyClass.Types> types)
    {
        speed = spd;
        damage = dmg;
        gravity = grav;
        timerToExplode = timer;
        explosionRange = explosionRadius;
        explosionDamage = expDamage;
        attackTypes = types;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 8)
        {
            this.GetComponent<Rigidbody>().useGravity = false;
            isAttached = true;
            this.gameObject.transform.SetParent(other.transform);
            this.GetComponent<Rigidbody>().isKinematic = true;
            speed = 0;
            gravity = 0;
            other.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
            other = enemy;
        }
        if(other.gameObject.layer == 10)
        {
            originalPosition = transform.position;
            isMoving = false;
        }
    }
}
