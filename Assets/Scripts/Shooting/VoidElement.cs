using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidElement : BaseElementClass
{
    [SerializeField]
    float dashDistance = 20.0f;
    float trueDashDistance = 0.0f;
    [SerializeField]
    float dashTime = 0.5f;
    Vector3 targetPos = Vector3.zero;
    [SerializeField]
    GameObject Indicator;
    bool isHolding;

    bool fullScreenOff;
    float toggleEffectIntensity;

    protected override void Start()
    {
        base.Start();
        toggleEffectIntensity = -1.0f;
        fullScreenOff = true;
        activatedVFX.SetActive(false);
    }
    void FullScreenEffect()
    {
        if (gameplayUI)
        {
            gameplayUI.GetVoidFullScreen().material.SetFloat("_Toggle_EffectIntensity", toggleEffectIntensity);
        }
        if (fullScreenOff)
        {
            gameplayUI.GetVoidFullScreen().gameObject.SetActive(true);
            toggleEffectIntensity -= Time.deltaTime * 10;
        }
        else
        {
            gameplayUI.GetVoidFullScreen().gameObject.SetActive(true);
            toggleEffectIntensity = 10.0f;
        }
        if(toggleEffectIntensity <= 0.0f)
        {
            gameplayUI.GetVoidFullScreen().gameObject.SetActive(false);
            toggleEffectIntensity = 0.0f;
        }
    }
    protected override void Update()
    {
        base.Update();
        FullScreenEffect();
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
	                if (Mathf.Abs(hit.distance) < distance + 2 && !hit.collider.gameObject.GetComponent<SporeCloudScript>())
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
                targetPos = averagePos;
                Vector3 size = this.gameObject.GetComponent<CharacterController>().bounds.size;
                //Move the target position out slightly by the average normal
                Indicator.transform.position += new Vector3(averageNorm.x * size.x, averageNorm.y * size.y, averageNorm.z * size.z);
                targetPos += new Vector3(averageNorm.x * size.x, averageNorm.y * size.y, averageNorm.z * size.z);
            }
            else
            {
                Indicator.transform.position = this.transform.position + Camera.main.transform.forward * (trueDashDistance / 2);
                targetPos = this.transform.position + Camera.main.transform.forward * trueDashDistance;
            }
            //targetPos = (this.transform.position + Camera.main.transform.forward * trueDashDistance * 1.5f);
            //targetPos = Indicator.transform.position;
        }
        //Checking if the mouse button has been released at a certain distance, cancels the spell
        if (!Input.GetKey(KeyCode.Mouse1) && playerHand.GetCurrentAnimatorStateInfo(1).IsName("VoidHold") && trueDashDistance < 10 ||
            !Input.GetKey(KeyCode.Mouse1) && playerHand.GetCurrentAnimatorStateInfo(1).IsName("Void Start Hold") && trueDashDistance < 10)
        {
            isHolding = false;
            playerHand.SetTrigger("VoidCastFail");
        }
        else if(!Input.GetKey(KeyCode.Mouse1) && playerHand.GetCurrentAnimatorStateInfo(1).IsName("VoidHold") ||
            !Input.GetKey(KeyCode.Mouse1) && playerHand.GetCurrentAnimatorStateInfo(1).IsName("Void Start Hold"))
        {
            isHolding = false;
            if (audioManager)
            {
                audioManager.PlaySFX(otherShootingSoundFX);
            }
            playerHand.SetTrigger("VoidCastSuccess");
        }
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        //Subtract the mana cost
        playerClass.ChangeMana(-manaCost * (upgraded ? 1 : 0.5f), manaTypes);
        if (shootingScript.GetCatalystElements()[shootingScript.GetRightElementIndex()] == this &&
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
        activatedVFX.SetActive(true);
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

    IEnumerator Dash()
    {
        playerClass.gameObject.GetComponent<PlayerMovement>().SetAbleToMove(false);
        fullScreenOff = false;
        float timer = 0.0f;
        Vector3 startPos = this.transform.position;
        while(timer < dashTime)
        {
            this.transform.position = Vector3.Lerp(startPos, targetPos, timer / dashTime);
            timer += Time.deltaTime * (1 / Time.timeScale);
            yield return null;
        }
        this.gameObject.GetComponent<PlayerMovement>().SetAbleToMove(true);
        fullScreenOff = true;
        StopCoroutine(Dash());
    }

    public override void LiftEffect()
    {
        base.LiftEffect();
        Indicator.SetActive(false);
        activatedVFX.SetActive(false);
    }
    public override void Upgrade()
    {
        base.Upgrade();
    }

}
