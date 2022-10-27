using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIndicator : MonoBehaviour
{
    Transform player;
    private void Start()
    {
	    player = PlayerClass.GetPlayerClass().gameObject.transform.GetChild(1);
    }
    private void Update()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        rot.z = - (player.transform.rotation.eulerAngles.y);
            transform.rotation = Quaternion.Euler(rot);
    }
}
