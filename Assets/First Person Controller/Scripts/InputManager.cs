using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;
    private bool isCrouching = false;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update() {
        if (Input.GetButtonDown("Crouch")) {
            isCrouching = !isCrouching;
        }
    }

    public Vector2 GetInputVectorNormalized() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 inputVector = new Vector2(horizontal, vertical);
        return inputVector.normalized;
    }

    public Vector2 GetMouseVectorNormalized() {
        float horizontal = Input.GetAxisRaw("Mouse X");
        float vertical = Input.GetAxisRaw("Mouse Y");
        Vector2 mouseVector = new Vector2(horizontal, vertical);
        return mouseVector.normalized;
    }

    public bool IsCrouching() {
        return isCrouching;
    }
}
