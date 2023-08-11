using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    [SerializeField] private Transform head;
    [SerializeField] private Transform groundCheck;

    [SerializeField] private PlayerVisual playerVisual;

    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float walkSpeed = 5;
    [SerializeField] private float crouchSpeed = 2;
    [SerializeField] private float sprintSpeed = 8;
    [SerializeField] private float jumpSpeed = 5;

    [SerializeField] private float mouseSensitivity = 90;
    [SerializeField] private float jumpHeight = 15;
    [SerializeField] private float gravity = 3;

    [SerializeField] private Vector3 standCenterPosition;
    [SerializeField] private Vector3 crouchCenterPosition;
    [SerializeField] private Vector3 crouchHeadPosition;
    [SerializeField] private Vector3 sprintHeadPosition;
    [SerializeField] private Vector3 standHeadPosition;

    private CharacterController characterController;

    public static bool isGrounded = false;
    private bool isSprinting = false;
    private bool isCrouching = false;
    private RaycastHit groundHit;
    private Vector3 moveVector = Vector3.zero;
    private Vector3 jumpVector = Vector3.zero;

    private Vector3 jumpVelocity;

    private Vector2 inputVector;

    [SerializeField] private float groundCheckRadius = 0.15f;
    [SerializeField] private float groundCheckHeight = 0.85f;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void FixedUpdate() {
        HandleCollider();
        HandleMouse();
        HandleMovement();
    }

    private void HandleMouse() {
        Vector2 mouseVector = InputManager.Instance.GetMouseVectorNormalized();
        Vector3 rotation = transform.eulerAngles;
        rotation.y = Mathf.Lerp(rotation.y, rotation.y + mouseVector.x * mouseSensitivity * Time.deltaTime, 0.1f);
        transform.eulerAngles = rotation;


        //head rotation

        float headRotation = head.transform.eulerAngles.x;
        headRotation = Mathf.Lerp(headRotation, headRotation + mouseVector.y * mouseSensitivity * Time.deltaTime * -1, 0.1f); // inverted

        // manually clmaping because mathf.clamp does not work
        if (headRotation > 80 && headRotation < 180) {
            headRotation = 80f;
        } else if (headRotation > 180 && headRotation < 280) {
            headRotation = 290f;
        }

        head.transform.eulerAngles = new Vector3(headRotation, head.eulerAngles.y, head.eulerAngles.z);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(groundCheck.position + new Vector3(0, -groundCheckHeight, 0), groundCheckRadius);
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.name == "Enemy") {
            if (Random.Range(0, 2) == 1) {
                Debug.Log("hurt");
            }
        }
    }

    private void HandleMovement() {
        isGrounded = Physics.SphereCast(groundCheck.position, groundCheckRadius, Vector3.down, out groundHit, groundCheckHeight);
        isSprinting = Input.GetButton("Sprint");
        isCrouching = InputManager.Instance.IsCrouching();

        inputVector = InputManager.Instance.GetInputVectorNormalized();
        moveVector = new Vector3(inputVector.x , 0, inputVector.y);

        if (moveVector.y < -20) {
            jumpVector.y = -20f;
        }

        if (isGrounded && jumpVector.y < 0) {
            jumpVector.y = 0f;
        }

        if (isGrounded && Input.GetButtonDown("Jump")) {
            //jump code here
            // characterController.Move(transform.TransformDirection(Vector3.up) * gravity * jumpHeight * Time.deltaTime);
            jumpVector.y += Mathf.Sqrt(jumpHeight * gravity) * 0.2f;
        }

        jumpVector.y += -gravity * Time.deltaTime;

        if (isCrouching) {
            moveSpeed = crouchSpeed;
            head.localPosition = Vector3.Lerp(head.localPosition, crouchHeadPosition, 0.1f);
        } else if (isSprinting) {
            moveSpeed = sprintSpeed;
            head.localPosition = Vector3.Lerp(head.localPosition, sprintHeadPosition, 0.1f);
        } else {
            moveSpeed = walkSpeed;
            head.localPosition = Vector3.Lerp(head.localPosition, standHeadPosition, 0.1f);
        }

        characterController.Move(transform.TransformDirection(moveVector)*moveSpeed*Time.deltaTime);
        characterController.Move(transform.TransformDirection(jumpVector)*jumpSpeed*Time.deltaTime);
    }

    public static bool IsGrounded() {
        return isGrounded;
    }

    private void ApplyGravity() {
        characterController.Move(transform.TransformDirection(Vector3.down) * gravity * Time.deltaTime);
    }

    private void HandleCollider() {
        isCrouching = InputManager.Instance.IsCrouching();

        if (isCrouching) {
            characterController.center = crouchCenterPosition;
            characterController.radius = 0.25f;
            characterController.height = 1.2f;
        } else {
            characterController.center = standCenterPosition;
            characterController.radius = 0.2f;
            characterController.height = 1.76f;
        }
    }
}