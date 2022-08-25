using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
#if UNITY_EDITOR
[ExecuteAlways]
#endif
public class SmoothTransformFollow : MonoBehaviour
{
    [Header("Attached Components")]
    [SerializeField] private Transform targetTransform;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8.0f;
    [SerializeField] private float rotateSpeed = 8.0f;

    // function is called when script is loaded or values have change on inspector
    private void OnValidate()
    {
        if (targetTransform)
        {
            SetToTarget();
        }
    }

    // the hands will follow the camera as the player moves the mouse
    private void Update()
    {
#if UNITY_EDITOR
        if(!Application.isPlaying)
        {
            SetToTarget();
        }
#endif
        transform.position = Vector3.Lerp(transform.position, targetTransform.position, moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetTransform.rotation, rotateSpeed * Time.deltaTime);
    }

    private void SetToTarget()
    {
        transform.position = targetTransform.position;
        transform.rotation = targetTransform.rotation;
    }
}
