using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTransformFollow : MonoBehaviour
{
    [Header("Attached Components")]
    [SerializeField] private Transform targetTransform;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float rotateSpeed = 8.0f;

    private void OnValidate()
    {
        if (targetTransform)
        {
            transform.position = targetTransform.position;
        }
    }

    //the hands will follow the camera as the player moves the mouse
    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, rotateSpeed * Time.deltaTime);
    }
}
