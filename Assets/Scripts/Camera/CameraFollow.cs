using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset; // The offset from the target object
    public Vector3 chaseOffset; // The offset from the target object
    public bool IsChased = false;
    public float smoothTime = 0.125f;

    private Vector3 velocity = Vector3.zero; // Velocity used by SmoothDamp


    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition;
            if (!IsChased)
                desiredPosition = target.position + offset;
            else
                desiredPosition = target.position + chaseOffset;

            // Smoothly interpolate between the current position and the desired position using SmoothDamp
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);

            // Keep the camera's X position unchanged
            smoothedPosition.x = transform.position.x;

            // Set the position of the camera
            transform.position = smoothedPosition;
        }
    }
}
