using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyIceSlash : MonoBehaviour
{
    [SerializeField]
    private LayerMask environment;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(((1<<other.gameObject.layer) & environment) != 0)
        {
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
