using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidElement : BaseElementClass
{
    [SerializeField]
    private float dashDistance = 10.0f;
    private float trueDashDistance = 0.0f;
    [SerializeField]
    private float dashTime = 0.5f;
    private Vector3 targetPos = Vector3.zero;
    private bool dashing = false;
    [SerializeField]
    private GameObject Indicator;
    private bool isHolding;
    //[SerializeField]
    //private Material voidMaterial;

    protected override void Start()
    {
        base.Start();
        //voidMaterial.SetFloat("_Toggle_EffectIntensity", 0.0f);
    }
    protected override void Update()
    {
        base.Update();
        //Checking if the mouse button has been released, which cancels the spell if it hasn't been held long enough or casts it if it has
        if (!Input.GetKey(KeyCode.Mouse1) && (playerHand.GetCurrentAnimatorStateInfo(1).IsName("VoidHold") || 
            playerHand.GetCurrentAnimatorStateInfo(1).IsName("Void Start Hold")))
        {
            isHolding = false;
            audioManager.PlaySFX(otherShootingSoundFX);
            playerHand.SetTrigger("VoidCastSuccess");
        }
        if (isHolding)
        {
            RaycastHit hit1;
            if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit1, dashDistance, shootingIgnore))
            {
                trueDashDistance = Vector3.Distance(hit1.point, this.transform.position);
            }
            else
            {
                trueDashDistance = dashDistance;
            }
            RaycastHit[] hits = Physics.SphereCastAll(this.gameObject.transform.position, 0.2f, Camera.main.transform.forward, trueDashDistance, shootingIgnore);
            if (hits.Length > 0)
            {
                Vector3 averagePos = Vector3.zero;
                Vector3 averageNorm = Vector3.zero;
                int count = 0;
                float distance = Mathf.Abs(hits[0].distance);
                //Sum up the normals and positions of objects within a small range of the first object hit with the spherecast
                foreach (RaycastHit hit in hits)
                {
                    if (Mathf.Abs(hit.distance) < distance + 2)
                    {
                        averagePos += hit.point;
                        averageNorm += hit.normal;
                        count += 1;
                    }

                }
                //Average out the positions and normals
                averagePos /= count;
                averageNorm.Normalize();
                //Set the target pos to the average position
                Indicator.transform.position = averagePos;
                Vector3 size = this.gameObject.GetComponent<CharacterController>().bounds.size;
                //Move the target position out slightly by the average normal
                Indicator.transform.position += new Vector3(averageNorm.x * size.x, averageNorm.y * size.y, averageNorm.z * size.z);
            }
            else
            {
                Indicator.transform.position = this.transform.position + Camera.main.transform.forward * trueDashDistance;
            }
            targetPos = Indicator.transform.position;
        }
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        //Subtract the mana cost
        playerClass.ChangeMana(-manaCost, manaTypes);
        if (shootingScript.catalystElements[shootingScript.GetRightElementIndex()] == this &&
            shootingScript.GetInComboMode() == false)
        {
            StartCoroutine(Dash());
        }
    }

    public override void ActivateVFX()
    {
        base.ActivateVFX();
    }

    protected override void StartAnims(string animationName, string animationNameAlt = null)
    {
        base.StartAnims(animationName);
        playerHand.SetTrigger(animationName);
        playerHand.ResetTrigger("VoidStopCast");
        Indicator.SetActive(true);
        isHolding = true;
        Instantiate(activatedVFX, shootingScript.GetRightOrbPos());
    }
    protected override bool PayCosts(float modifier = 1)
    {
        //Override of paycosts so that mana is only subtracted at then end, in case the cast is cancelled
        if (playerClass.ManaCheck(manaCost * modifier, manaTypes))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(dashing)
        {
            StopCoroutine(Dash());
            dashing = false;
            this.gameObject.GetComponent<PlayerMovement>().SetAbleToMove(true);
        }
    }

    IEnumerator Dash()
    {
        playerClass.gameObject.GetComponent<PlayerMovement>().SetAbleToMove(false);
        //voidMaterial.SetFloat("_Toggle_EffectIntensity", 0.1f);
        dashing = true;
        float timer = 0.0f;
        Vector3 startPos = this.transform.position;
        while(timer < dashTime)
        {
            this.transform.position = Vector3.Lerp(startPos, targetPos, timer / dashTime);
            timer += Time.deltaTime * (1 / Time.timeScale);
            yield return null;
        }
        this.gameObject.GetComponent<PlayerMovement>().SetAbleToMove(true);
        //voidMaterial.SetFloat("_Toggle_EffectIntensity", 0.0f);
        StopCoroutine(Dash());
    }

    public override void LiftEffect()
    {
        base.LiftEffect();
        Indicator.SetActive(false);
        if (shootingScript.GetRightOrbPos().childCount > 1)
        {
            Destroy(shootingScript.GetRightOrbPos().GetChild(1).gameObject);
        }
    }
    public override void Upgrade()
    {
        base.Upgrade();
    }

}
