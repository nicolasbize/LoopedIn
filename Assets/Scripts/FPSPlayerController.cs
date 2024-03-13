using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSPlayerController : MonoBehaviour {

    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 55.0f;
    public AudioClip[] footstepSounds;

    private float timeBetweenSteps = 0.5f;
    private float timeSinceLastStep = float.NegativeInfinity;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    void Start() {
        characterController = GetComponent<CharacterController>();
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        Player.Instance.OnStateChange += Player_OnStateChange;
    }

    private void Player_OnStateChange(object sender, System.EventArgs e) {
        if (Player.Instance.GetState() == Player.State.Puzzling) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update() {
        if (Player.Instance.CanMove()) {

            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            bool isRunning = Input.GetKey(KeyCode.LeftShift);
            float curSpeedX = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Vertical");
            float curSpeedY = (isRunning ? runningSpeed : walkingSpeed) * Input.GetAxis("Horizontal");
            float movementDirectionY = moveDirection.y;
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            if (Input.GetButton("Jump") && characterController.isGrounded) {
                moveDirection.y = jumpSpeed;
            } else {
                moveDirection.y = movementDirectionY;
            }

            if (!characterController.isGrounded) {
                moveDirection.y -= gravity * Time.deltaTime;
            } else { // disable jump for now
                moveDirection.y = 0f;
            }
            characterController.Move(moveDirection * Time.deltaTime);

            bool isMoving = moveDirection.magnitude > 1f;
            float stepTime = isRunning ? (timeBetweenSteps * 0.7f) : timeBetweenSteps;
            if (isMoving && (Time.timeSinceLevelLoad - timeSinceLastStep > stepTime)) {
                GetComponent<AudioSource>().clip = footstepSounds[Random.Range(0, footstepSounds.Length)];
                GetComponent<AudioSource>().Play();
                timeSinceLastStep = Time.timeSinceLevelLoad;
            }
        }

        if (Player.Instance.CanLookAround()) {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}
