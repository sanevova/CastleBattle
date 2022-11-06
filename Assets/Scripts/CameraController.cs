using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private Vector3 gripStartMousePosition;
    private bool shouldGripCamera = false;

    void Start() {
    }

    void Update() {
        CameraGripMovement();
    }

    void CameraGripMovement() {
        // camera grip
        if (Input.GetMouseButtonDown(2)) {

            if (RaycastMouse(out RaycastHit hit)) {
                gripStartMousePosition = hit.point;
                shouldGripCamera = true;
            }
        }
        if (Input.GetMouseButtonUp(2)) {
            shouldGripCamera = false;
        }
        if (shouldGripCamera) {
            if (RaycastMouse(out RaycastHit hit)) {
                // adjust for `y` difference
                var xzDifference = gripStartMousePosition - hit.point;
                xzDifference.y = 0;
                transform.position += xzDifference;
            }
        }
    }

    public static bool RaycastMouse(out RaycastHit hit) {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return Physics.Raycast(ray, out hit);
    }
}
