using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : MonoBehaviour
{
    enum ViewPosition
    {
        Blaze,
        Silvain
    }

    [SerializeField]
    ViewPosition vP;

    [SerializeField]
    List<Transform> positions;

    private void Start()
    {
        UpdatePosition();
    }

    public void UpdatePosition()
    {
        transform.SetPositionAndRotation(positions[(int)vP].position, positions[(int)vP].rotation);
    }
}
