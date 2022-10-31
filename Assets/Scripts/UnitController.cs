using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
    [Header("Movement Params")]
    public float speed;
    public float rotationSpeed;

    private Vector3 movementDirection;

    private CharacterController characterController;
    private float ySpeed;

    void Start() {
        characterController = GetComponent<CharacterController>();
        movementDirection = Vector3.forward;
        if (!characterController.isGrounded) {
            ySpeed = -2000f;
        }
    }

    void Update() {
        if (Input.GetMouseButtonDown(1)) {
            if (CameraController.RaycastMouse(out var hit)) {
                hit.point -= hit.point.y * Vector3.up;
                movementDirection = hit.point - transform.position;
                movementDirection.Normalize();
            }
        }

        AdjustYSpeed();
        var velocity = movementDirection * speed;
        velocity.y += ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        var dstRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, dstRotation, rotationSpeed * Time.deltaTime);
    }

    void AdjustYSpeed() {
        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (characterController.isGrounded) {
            ySpeed = -1f;
        }
    }

}
