using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform cameraTransform;

    [SerializeField] private float rotationSpeed = 10f; // Speed of camera rotation
    [SerializeField] private float zoomSpeed = 5f; // Speed of camera zoom
    [SerializeField] private float minZoomDistance = 1f; // Minimum distance for zoom
    [SerializeField] private float maxZoomDistance = 20f; // Maximum distance for zoom

    [SerializeField] private float centerBlockCount = 0;

    private float currentZoomDistance = 10f; // Initial zoom distance

    //MouseMovement
    private Vector3 rotateStartPosition;
    private Quaternion originalRotation;

    // Lerp settings
    [SerializeField] private float lerpSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        originalRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //HandleMouseInput();
        HandleKeyboardInput();
        LerpCameraYPosition(); // Call the method to lerp the camera's Y position
    }
    /*
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
            RotateCamera(Input.mousePosition);
        }
    }
    */
    void HandleKeyboardInput()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        RotateCameraByInput(horizontalInput);
        ZoomCamera(verticalInput);
    }

    void RotateCamera(Vector3 currentPosition)
    {
        Vector3 difference = currentPosition - rotateStartPosition;
        float rotationX = -difference.y / Screen.height * rotationSpeed;
        float rotationY = difference.x / Screen.width * rotationSpeed;

        transform.rotation = originalRotation * Quaternion.Euler(rotationX, rotationY, 0);
    }

    void RotateCameraByInput(float horizontalInput)
    {
        float rotationY = horizontalInput * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, rotationY, 0);
    }

    void ZoomCamera(float verticalInput)
    {
        currentZoomDistance -= verticalInput * zoomSpeed;
        currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);
        cameraTransform.localPosition = new Vector3(0, 0, -currentZoomDistance);
    }

    void LerpCameraYPosition()
    {
        // Calculate the target position
        Vector3 targetPosition = new Vector3(transform.position.x, centerBlockCount * 0.5f, transform.position.z);
        // Lerp the camera's position
        transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
    }
}
