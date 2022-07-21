using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothTransformFollow : MonoBehaviour
{
    [Header("Attached Components")]
    [SerializeField] private Transform m_TargetTransform;

    [Header("Movement Settings")]
    [SerializeField] private float m_MoveSpeed = 8.0f;
    [SerializeField] private float m_RotateSpeed = 8.0f;

    public void SetTransform (Transform _target)
    {
        m_TargetTransform = _target;
    }

    private void OnValidate()
    {
        if (m_TargetTransform)
        {
            transform.position = m_TargetTransform.position;
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, m_TargetTransform.position, m_MoveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, m_TargetTransform.rotation, m_RotateSpeed * Time.deltaTime);
    }
}
