using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;

    [SerializeField] private float _camSpeed = 1f;
    [SerializeField] private float _camRotationAmount = 1f;
    [SerializeField] private bool cursorVisible = true;

    public Vector3 newZoom;
    public Vector3 rotateStartPosition;

    [SerializeField] private float pointDistanceOffset = 10f; // Offset for circling around Vector3.zero

    void Start()
    {
        newZoom = cameraTransform.localPosition;
    }

    void Update()
    {
        HandleMouseInput();
        HandleMovementInput();
    }

    void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(2))
        {
            rotateStartPosition = Input.mousePosition;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        if (Input.GetMouseButton(2))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = cursorVisible;

            // Rotate the camera around Vector3.zero
            cameraTransform.RotateAround(Vector3.zero, Vector3.up, Input.GetAxis("Mouse X") * _camRotationAmount);
        }
    }

    void HandleMovementInput()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            // Rotate the camera around Vector3.zero
            cameraTransform.RotateAround(Vector3.zero, Vector3.up, -_camRotationAmount);
        }

        if (Input.GetKey(KeyCode.E))
        {
            // Rotate the camera around Vector3.zero
            cameraTransform.RotateAround(Vector3.zero, Vector3.up, _camRotationAmount);
        }

        // Calculate the new position based on the pointDistanceOffset and rotation
        Vector3 offset = cameraTransform.localRotation * Vector3.forward * pointDistanceOffset;
        Vector3 newPosition = Vector3.zero + offset;

        // Update camera position
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * _camSpeed);
    }
}