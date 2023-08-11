using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour {
    [SerializeField] private bool enable = true;

    [SerializeField, Range(0, 0.1f)] private float amplitude = 0.015f;
    [SerializeField, Range(0, 30)] private float frequency = 10.1f;

    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _cameraHolder;

    private float toggleSpeed = 0.5f;
    private Vector3 startPos;
    private CharacterController characterController;

    private void Awake() {
        characterController = GetComponent<CharacterController>();
        startPos = _camera.localPosition;
    }

    private void Update() {
        if (!enable) return;
        CheckMotion();
        ResetPosition();
        _camera.LookAt(FocusTarget());
    }

    private void PlayMotion(Vector3 motion) {
        _camera.localPosition += motion;
    }

    private Vector3 FootStepMotion() {
        Vector3 pos = Vector3.zero;
        pos.y += Mathf.Sin(Time.time * frequency) * amplitude;
        pos.x += Mathf.Sin(Time.time * frequency / 2) * amplitude / 2;
        return pos;
    }

    private void CheckMotion() {
        Vector2 inputVector = InputManager.Instance.GetInputVectorNormalized();
        float speed = new Vector3(inputVector.x, 0, inputVector.y).magnitude;
        if (speed < toggleSpeed) return;
        if (!characterController.isGrounded) return;


        PlayMotion(FootStepMotion());
    }

    private void ResetPosition() {
        if (_camera.localPosition == startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, startPos, 1*Time.deltaTime);
    }

    private Vector3 FocusTarget() {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.01f;
        return pos;
    }

}
