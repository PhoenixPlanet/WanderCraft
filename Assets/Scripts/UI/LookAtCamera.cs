using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TH.Core {

public class LookAtCamera : MonoBehaviour
{
    private Transform mainCameraTransform;

    private void Start()
    {
        // Find the main camera using its tag
        GameObject mainCameraObj = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCameraObj != null)
        {
            mainCameraTransform = mainCameraObj.transform;
        }
        else
        {
            Debug.LogWarning("MainCamera not found. Please ensure you have a camera tagged with 'MainCamera'.");
        }
    }

    private void Update()
    {
        if (mainCameraTransform != null)
        {
            // Rotate the Canvas to face the camera
            transform.LookAt(mainCameraTransform);
            
            // Since we want only the canvas to face the camera and not tilt upwards or downwards, 
            // we set the rotation around the X and Z axes to 0.
            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.x = 0;
            rotation.z = 0;

			rotation.y += 180;
            transform.rotation = Quaternion.Euler(rotation);
        }
    }
}

}