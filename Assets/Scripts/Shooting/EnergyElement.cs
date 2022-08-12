using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyElement : BaseElementClass
{
    [SerializeField]
    List<GameObject> containedEnemies = new List<GameObject>();

    public GameObject energyShield;

    [SerializeField]
    // might change to this method depending if current way works
    private enum shieldState
    {
        shieldUp,
        parrying,
        shieldDown
    }
    [SerializeField]
    private shieldState shieldStateChange = shieldState.shieldDown;

    [SerializeField]
    private float timeToParry;
    private bool useShield = false;
    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
            switch (shieldStateChange)
            {
                case shieldState.shieldDown:
                    {
                        if (useShield == true)
                        {
                            if (upgraded == true)
                            {
                                shieldStateChange = shieldState.parrying;
                            }
                            else
                            {
                                shieldStateChange = shieldState.shieldUp;
                            }
                        }
                    break;
                }
                case shieldState.parrying:
                    {
                        timeToParry += Time.deltaTime;
                        if (timeToParry >= 0.25f)
                        {
                            shieldStateChange = shieldState.shieldUp;
                            timeToParry = 0.0f;
                            //parrycode;
                        }
                    break;
                    }
                case shieldState.shieldUp:
                    {
                        if(!PayCosts(Time.deltaTime) || Input.GetKeyDown(KeyCode.E) || Input.GetKeyUp(KeyCode.Mouse1))
                        {
                            DeactivateEnergyShield();
                            shieldStateChange = shieldState.shieldDown;
                        }
                        break;
                    }
            }
    }

    public void DeactivateEnergyShield()
    {
       energyShield.SetActive(false);
       useShield = false;
       playerHand.SetTrigger("EnergyStopCast");
       audioManager.Stop("Energy Element");
       // go through the list of enemies
       // remove them from the list and 
       for (int i = 0; i < containedEnemies.Count; i++)
       {
           containedEnemies[i].GetComponent<BaseEnemyClass>().damageMultiplier = 1.0f;
           containedEnemies.Remove(containedEnemies[i]);
       }
       if (shootingScript.GetRightOrbPos().childCount > 1)
       {
           Destroy(shootingScript.GetRightOrbPos().GetChild(1).gameObject);
       }
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        energyShield.SetActive(true);
        useShield = true;
    }
    
    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    public override void LiftEffect()
    {
        base.LiftEffect();
        DeactivateEnergyShield();
    }
    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);
        playerHand.ResetTrigger("EnergyStopCast");
        playerHand.SetTrigger(animationName);
        audioManager.Play("Energy Element");
        Instantiate(activatedVFX, shootingScript.GetRightOrbPos());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (useShield)
        {
            if (other.gameObject.layer == 8  && other.GetComponent<BaseEnemyClass>() || other.gameObject.layer == 22 && other.GetComponent<BaseEnemyClass>())
            {

                if (other.gameObject && !containedEnemies.Contains(other.gameObject))
                {
                    containedEnemies.Add(other.gameObject);
                    for (int i = 0; i < containedEnemies.Count; i++)
                    {
                        containedEnemies[i].gameObject.GetComponent<BaseEnemyClass>().damageMultiplier = 0;
                    }
                }
            }
        }

    }
}
