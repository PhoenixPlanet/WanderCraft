﻿
using System;
using UnityEngine;
using DG.Tweening;

namespace TH.Core
{
    public class CameraController : MonoBehaviour
    {
        public Transform cameraTransform;
        public GameObject waterGroup;
        [SerializeField] private float rotationSpeed = 10f; // Speed of camera rotation
        [SerializeField] private float zoomSpeed = 5f; // Speed of camera zoom
        [SerializeField] private float minZoomDistance = 1f; // Minimum distance for zoom
        [SerializeField] private float maxZoomDistance = 20f; // Maximum distance for zoom
        [SerializeField] private float yMoveSpeed = 3f;
        [SerializeField] private float centerBlockCount = 0;
        private bool isRotating = false;
        private float currentZoomDistance = 10f; // Initial zoom distance
        [SerializeField] float startingYpos = 3.1f; 
        [SerializeField] private float lerpSpeed = 5f;
        //[SerializeField] private float yHeightOffset = 5f;

        void Start()
        {
            transform.position = new Vector3(transform.position.x * 2, startingYpos * 2, transform.position.z * 2);
        }

        void Update()
        {
            HandleKeyboardInput();
            LerpCameraYPosition();
            //ClampWithWaterHeight();
        }

        /*
        private void ClampWithWaterHeight()
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, waterGroup.transform.position.y + 3f, 1000));
        }
        */
        void HandleKeyboardInput()
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            //float verticalInput = Input.GetAxis("Vertical");
            float verticalInput = Input.GetAxis("Mouse ScrollWheel");

            RotateCameraByInput(horizontalInput);
            ZoomCamera(verticalInput);
        }


        void RotateCameraByInput(float horizontalInput)
        {
            if (Input.GetButtonDown("Horizontal") && !isRotating)
            {
                Quaternion targetRotation;
                isRotating = true; 
                float targetRotationY = transform.eulerAngles.y + (horizontalInput > 0 ? -90 : 90);
                targetRotation = Quaternion.Euler(25, targetRotationY, 0);
                print(rotationSpeed);
                transform.DORotateQuaternion(targetRotation, rotationSpeed).SetEase(Ease.InCubic).OnComplete(() => isRotating = false);
            }
        }

        void ZoomCamera(float verticalInput)
        {
            currentZoomDistance -= verticalInput * zoomSpeed * Time.deltaTime;
            currentZoomDistance = Mathf.Clamp(currentZoomDistance, minZoomDistance, maxZoomDistance);
            cameraTransform.GetComponent<Camera>().orthographicSize = currentZoomDistance;
        }

        void LerpCameraYPosition()
        {
            switch (GridManager.Instance.State)
            {
                case GridManager.BuildingState.Normal:
                    changeYPos();
                    break;
                case GridManager.BuildingState.Building:
                    //lockOnYpos(GridManager.Instance.SelectedBuildingLevel);
                    changeYPos();
                    break;
            }
           
        }

        private void changeYPos()
        {
            if(Input.GetKey(KeyCode.W))
            {
                cameraTransform.DOMoveY(cameraTransform.position.y + yMoveSpeed * Time.deltaTime, 0.1f);
            } else if (Input.GetKey(KeyCode.S))
            {
                cameraTransform.DOMoveY(cameraTransform.position.y - yMoveSpeed * Time.deltaTime, 0.1f);
            }
        }
        /*
        private void lockOnYpos(int centerBlockCount)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, yHeightOffset + centerBlockCount * CenterBlock.HEIGHT, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
        }
        */

    }
}