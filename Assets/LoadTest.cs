using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTest : MonoBehaviour
{
	float rot = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
	{
		rot += Time.deltaTime * 100;
	    this.transform.eulerAngles = new Vector3(0, 0, rot);
    }
}
