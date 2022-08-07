using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidElement : BaseElementClass
{
    float timer = 0.0f;
    public float chargeTime = 1.5f;
    public float dashDistance = 10.0f;
    public float dashTime = 0.5f;
    Vector3 targetPos = Vector3.zero;
    bool dashing = false;
    public GameObject capsule;
    public GameObject Indicator;
    
    protected override void Update()
    {
        base.Update();
        //Checking if the mouse button has been released, which cancels the spell if it hasn't been held long enough or casts it if it has
        if (Input.GetKeyUp(KeyCode.Mouse1) && (playerHand.GetCurrentAnimatorStateInfo(1).IsName("VoidHold") || playerHand.GetCurrentAnimatorStateInfo(1).IsName("Void Start Hold")))
        {
            if (timer < chargeTime)
            {
                playerHand.SetTrigger("VoidStopCast");
                audioManager.Stop("Soul Element");
                //Destroy(playerClass.gameObject.GetComponent<Shooting>().GetRightOrbPos().GetChild(1).gameObject);
            }
            else
            {
                playerHand.SetTrigger("VoidCastSuccess");

            }

        }
        if (Input.GetKey(KeyCode.Mouse1) && playerHand.GetCurrentAnimatorStateInfo(1).IsName("VoidHold"))
        {
            timer += Time.deltaTime * (1 / Time.timeScale);
        }
        else
        {
            timer -= Time.deltaTime * 10;
        }
        timer = Mathf.Clamp(timer, 0, chargeTime);
        Time.timeScale = Mathf.Max(1 - timer / chargeTime, 0.1f);
    }

    public override void ElementEffect()
    {
        base.ElementEffect();
        Debug.Log("Effect");
        //Subtract the mana cost
        playerClass.ChangeMana(-manaCost, manaTypes);
        audioManager.Stop("Soul Element");
        //Spherecast to detect all things that can be interacted with along dash route
        RaycastHit[] hits = Physics.SphereCastAll(this.gameObject.transform.position, 0.2f, Camera.main.transform.forward, dashDistance, shootingIgnore);
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
            targetPos = averagePos;
            Vector3 size = this.gameObject.GetComponent<CharacterController>().bounds.size;
            //Move the target position out slightly by the average normal
            targetPos += new Vector3(averageNorm.x * size.x, averageNorm.y * size.y, averageNorm.z * size.z);
        }
        else
        {
            targetPos = this.transform.position + Camera.main.transform.forward * dashDistance;
        }

        StartCoroutine(Dash());

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
        audioManager.Play("Soul Element");

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
            this.gameObject.GetComponent<PlayerMovement>().ableToMove = true;
        }
    }

    IEnumerator Dash()
    {
        playerClass.gameObject.GetComponent<PlayerMovement>().ableToMove = false;
        dashing = true;
        float timer = 0.0f;
        Vector3 startPos = this.transform.position;
        Debug.Log("Routine");
        while(timer < dashTime)
        {
            this.transform.position = Vector3.Lerp(startPos, targetPos, timer / dashTime);
            timer += Time.deltaTime * (1 / Time.timeScale);
            yield return null;
        }
        this.gameObject.GetComponent<PlayerMovement>().ableToMove = true;
        StopCoroutine(Dash());
    }


    public override void Upgrade()
    {
        base.Upgrade();

        useDelay *= 0.5f;
    }

}
