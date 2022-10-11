using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script_Godrays : MonoBehaviour
{
    [SerializeField]
    Vector3 GodRayRot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.eulerAngles != GodRayRot)
        {
            transform.eulerAngles = GodRayRot;
        }
    }
}
