using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;

    private AudioSource audioSource;

    private Vector2 inputVector;
    private bool inAir = false;

    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update() {
        inputVector = InputManager.Instance.GetInputVectorNormalized();
        if (inputVector.magnitude > 0) {
            if (!audioSource.isPlaying) {
                audioSource.clip = footstepClips[Random.Range(0, footstepClips.Length)];
                audioSource.Play();
            }
        }

        if (Input.GetButtonDown("Jump")) {
            audioSource.clip = jumpClip;
            audioSource.Play();
            inAir = true;
        }

        if (Player.IsGrounded() && inAir) {
            audioSource.clip = landClip;
            audioSource.Play();
            inAir = false;
        }
    }
}
