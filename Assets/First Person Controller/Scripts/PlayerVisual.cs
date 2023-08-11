using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerVisual : MonoBehaviour {

    [SerializeField] private Player player;

    private Animator animator;

    private Vector2 inputVector;

    private bool isCrouching = false;
    private bool inAir = false;

    private void Start() {
        animator = GetComponent<Animator>();
    }

    private void Update() {
        isCrouching = InputManager.Instance.IsCrouching();
        inputVector = InputManager.Instance.GetInputVectorNormalized();


        if (Player.IsGrounded() && inAir) {
            inAir = false;
            animator.SetBool("isJumping", false);
        }

        if (inputVector.y != 0 || inputVector.x != 0) {
            animator.SetBool("isWalking", true);
        } else {
            animator.SetBool("isWalking", false);
        }

        if (Input.GetButtonDown("Jump")) {
            animator.SetBool("isJumping", true);
            inAir = true;
        }

        if (Input.GetButton("Sprint")) {
            animator.SetBool("isSprinting", true);
        } else {
            animator.SetBool("isSprinting", false);
        }

        animator.SetBool("isCrouching", isCrouching);

    }

}
