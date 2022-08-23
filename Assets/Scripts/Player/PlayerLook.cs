﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this is a class that is attached to the camera
/// this class allows the player the look around like a player in the scene
/// there are also features to this class such as tilting left and right
/// </summary>
public class PlayerLook : MonoBehaviour
{
    [SerializeField] 
    private Camera currentCamera;

    // current rotation of camera
    [SerializeField]
    private float spin = 0.0f;
    [SerializeField]
    private float tilt = 0.0f;
    [SerializeField]
    private float roll = 0.0f;

    private float targetRoll = 0.0f;
    [SerializeField]
    private float rollSpeed = 4.0f;
    [SerializeField]
    private float maxRoll = 2.5f;

    [SerializeField] 
    private Vector2 tiltExtents = new Vector2(-85.0f, 85.0f);
    private float bumpTilt = 0.0f;

    [SerializeField] 
    private Vector2 spinExtents = new Vector2(0.0f, 0.0f);

    [SerializeField] 
    private float sensitivity = 2.0f;

    private bool cursorLocked = false;

    private bool ableToMove = true;

    private float rollNumber = 0.5f;

    // getter and setters
    public Camera GetCamera() { return currentCamera; }
    public float GetSpin() { return spin; }
    public void SetSpin(float tempSpin) { spin = tempSpin; }
    public float GetSensitivity() { return sensitivity; }
    public void SetSensitivity(float tempSensitivity) { sensitivity = tempSensitivity; }
    public void SetRoll(float normalizedRoll) { targetRoll = -normalizedRoll * maxRoll; }
    public void SetBumpTilt(float tempBumpTilt) { bumpTilt = tempBumpTilt; }
    public void SetAbleToMove(bool tempAbleToMove) { ableToMove = tempAbleToMove; }

    // function is called when script is loaded or values have change on inspector
    private void OnValidate()
    {
        currentCamera.transform.localEulerAngles = new Vector3(tilt, spin, 0);
    }

    // Start is called before the first frame update
    private void Start()
    {
        ToggleCursor();
    }

    // Update is called once per frame
    private void Update()
    {
        if (ableToMove)
        {
            MoveCamera();
        }
        HandleEditorInputs();
    }

    // toggle to lock the cursor
    public void ToggleCursor()
    {
        cursorLocked = !cursorLocked;
        // locks cursor in the middle of screen or unlocks cursor
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        // cursor is not visable or cursor is visable
        Cursor.visible = !cursorLocked;
    }

    public void ForceLockCursor()
    {
        cursorLocked = true;
        // locks cursor in the middle of screen or unlocks cursor
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        // cursor is not visable or cursor is visable
        Cursor.visible = !cursorLocked;
    }

    public void ForceUnlockCursor()
    {
        cursorLocked = false;
        // locks cursor in the middle of screen or unlocks cursor
        Cursor.lockState = cursorLocked ? CursorLockMode.Locked : CursorLockMode.None;
        // cursor is not visable or cursor is visable
        Cursor.visible = !cursorLocked;
    }

    // moves the players head around so that they can look around
    private void MoveCamera()
    {
        // if cursor is locked
        if (cursorLocked)
        {
            // getting input for the x and y axis for the mouse
            float mouseX = Input.GetAxisRaw("Mouse X");
            float mouseY = Input.GetAxisRaw("Mouse Y");

            // moves the players look when moving the mouse
            spin += mouseX * sensitivity;
            tilt -= mouseY * sensitivity;

            // stops the player from snapping their neck
            tilt = Mathf.Clamp(tilt, tiltExtents.x, tiltExtents.y);

            // 
            roll = Mathf.Lerp(roll, targetRoll, rollSpeed * Time.deltaTime);

            RollInput();

            // rotation on the x axis for the mouse (rotating head to look from side to side)
            // rotation on the y axis for the mouse (rotating head so looking up and down)
            currentCamera.transform.localEulerAngles = new Vector3(tilt + bumpTilt, spin, roll);
        }
    }

    // function for player tilting 
    // checks for input and does tilting
    // TO DO:
    private void RollInput()
    {
        if (Input.GetKey(KeyCode.A))
        {
            SetRoll(-rollNumber);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            SetRoll(rollNumber);
        }
        else
        {
            SetRoll(0.0f);
        }
    }

    // function that checks for input to toggle cursor on or off
    // for debugging purposes
    private void HandleEditorInputs()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleCursor();
        }
#endif
    }
}

