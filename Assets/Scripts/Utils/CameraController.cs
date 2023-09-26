﻿
using System;
using UnityEngine;
using DG.Tweening;

namespace TH.Core
{
    public class CameraController : MonoBehaviour
    {
        public Transform cameraTransform;

        [SerializeField] private float rotationSpeed = 10f; // Speed of camera rotation
        [SerializeField] private float zoomSpeed = 5f; // Speed of camera zoom
        [SerializeField] private float minZoomDistance = 1f; // Minimum distance for zoom
        [SerializeField] private float maxZoomDistance = 20f; // Maximum distance for zoom
        [SerializeField] private float yHeightOffset = 5f;
        [SerializeField] private float yMoveSpeed = 3f;
        [SerializeField] private float centerBlockCount = 0;

        private float currentZoomDistance = 10f; // Initial zoom distance

        private float _originX;

        private Vector3 rotateStartPosition;
        private Quaternion originalRotation;

        [SerializeField] private float lerpSpeed = 5f;
        void Start()
        {
            originalRotation = transform.rotation;
        }

        void Update()
        {
            HandleKeyboardInput();
            LerpCameraYPosition();
        }

        void HandleKeyboardInput()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            RotateCameraByInput(horizontalInput);
            ZoomCamera(verticalInput);
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
            switch (GridManager.Instance.State)
            {
                case GridManager.BuildingState.Normal:
                    changeYPos();
                    break;
                case GridManager.BuildingState.Building:
                    lockOnYpos(GridManager.Instance.SelectedBuildingLevel);
                    break;
            }
           
        }

        private void changeYPos()
        {
            if(Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                transform.DOMoveY(transform.position.y + yMoveSpeed, 0.1f);
            } else if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                transform.DOMoveY(transform.position.y - yMoveSpeed, 0.1f);
            }
        }

        private void lockOnYpos(int centerBlockCount)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, yHeightOffset + centerBlockCount * 0.6f, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        }


    }
}