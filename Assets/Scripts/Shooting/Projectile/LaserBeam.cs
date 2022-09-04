using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    private float damage;
    private List<BaseEnemyClass.Types> attackTypes;

    private List<GameObject> containedEnemies = new List<GameObject>();

    private float hitDelay;

    private float currentHitDelay;

    [SerializeField]
    private GameObject laserBeamEndParticle;
    [SerializeField]
    private GameObject laserBeamEffectParticle;

    private bool isHittingObj;

    [SerializeField]
    private LayerMask layerMask;

    private float initalLaserScale = 20.0f;

    public bool GetIsHittingObj() { return isHittingObj; }
    public void SetIsHittingObj(bool tempIsHittingObj) { isHittingObj = tempIsHittingObj; }

    private void Start()
    {
        isHittingObj = false;
    }

    // Update is called once per frame
    private void Update()
    {
        // if hitting an object show the particle effect of laserbeam hitting
        if(isHittingObj == true)
        {
            laserBeamEndParticle.SetActive(true);
        }
        else
        {
            laserBeamEndParticle.SetActive(false);
        }

        RaycastHit hit;
        // physics raycast to check if the laser is hitting the ground or enemies
        // this check is to change the size of the laser beam
        if (Physics.Raycast(laserBeamEffectParticle.transform.position, laserBeamEffectParticle.transform.forward, out hit, initalLaserScale, layerMask))
        {
            laserBeamEndParticle.transform.position = hit.point;
            laserBeamEffectParticle.GetComponentInChildren<LineRenderer>().SetPosition(1, new Vector3(0, 0, hit.distance));
            this.gameObject.transform.localScale = new Vector3(0, hit.distance, 0);
            isHittingObj = true;
        }
        else
        {
            laserBeamEffectParticle.GetComponentInChildren<LineRenderer>().SetPosition(1, new Vector3(0, 0, initalLaserScale));
            this.gameObject.transform.localScale = new Vector3(0, initalLaserScale, 0);
            isHittingObj = false;
        }

        // might need later to add some juice
        currentHitDelay += Time.deltaTime;

        if(currentHitDelay > hitDelay)
        {
            currentHitDelay = 0;
            foreach (GameObject enemy in containedEnemies.ToArray())
            {
                if(enemy)
                {

                    enemy.GetComponent<BaseEnemyClass>().TakeDamage(damage, attackTypes);
                }
                else
                {
                    containedEnemies.Remove(enemy);
                }
            }
        }
    }
    // setter
    public void SetVars(float dmg, List<BaseEnemyClass.Types> types, float tempHitDelay)
    {
        damage = dmg;
        attackTypes = types;
        containedEnemies.Clear();
        hitDelay = tempHitDelay;
    }
    private void OnTriggerStay(Collider other)
    {
        //if enemy, hit them for the damage
        if(other.tag == "Enemy" || other.tag == "Environment")
        {
            isHittingObj = true;
        }

        if (other.tag == "Enemy" && !containedEnemies.Contains(other.gameObject))
        { 
            containedEnemies.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemy" || other.tag == "Environment")
        {
            isHittingObj = false;
        }
        if (containedEnemies.Contains(other.gameObject))
        {
            containedEnemies.Remove(other.gameObject);
        }
    }
}
