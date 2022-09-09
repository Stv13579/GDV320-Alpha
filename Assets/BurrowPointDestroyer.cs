using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowPointDestroyer : MonoBehaviour
{
	float lifeTimer = 0;
	[SerializeField]
	float lifeTimerLength;
    // Start is called before the first frame update
    void Start()
    {
	    
    }

    // Update is called once per frame
    void Update()
	{
		lifeTimer += Time.deltaTime;
		if (lifeTimer >= lifeTimerLength)
		{
			StartDisappearAnimation();
	    }

	}
    
	public void StartAppearAnimation()
	{
		this.GetComponent<Animator>().SetTrigger("Appear");
	}
	
	public void StartDisappearAnimation()
	{
		this.GetComponent<Animator>().SetTrigger("Disappear");
	}
	
	public void Destroy()
	{
		Destroy(this.transform.parent.gameObject);

	}
}
