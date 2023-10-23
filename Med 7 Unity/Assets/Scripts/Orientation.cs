using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orientation : MonoBehaviour
{
    [Header("Rotation Speed")]
    public float rotationSpeed = 30f;

    [Header("Vertical Rotation Limits")]
    public float minVerticalRotation = -30f; // Minimum vertical rotation angle in degrees
    public float maxVerticalRotation = 30f;  // Maximum vertical rotation angle in degrees

    private Transform Head; // Reference to the head bone of the fish

    private void Start()
    {
        // Assuming you have a hierarchy structure for your fish where the head bone is a child of the fish's main transform
        Head = transform.Find("Head");
    }

    private void Update()
    {
        // Get input for vertical rotation (up and down)
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate the rotation angle
        float verticalAngle = verticalInput * rotationSpeed * Time.deltaTime;

        // Apply the rotation with vertical rotation limits
        Quaternion currentRotation = transform.rotation;
        float newVerticalRotation = currentRotation.eulerAngles.x + verticalAngle;

        // Clamp vertical rotation angle within limits
        newVerticalRotation = Mathf.Clamp(newVerticalRotation, minVerticalRotation, maxVerticalRotation);

        // Set the new rotation while preserving horizontal orientation
        transform.rotation = Quaternion.Euler(newVerticalRotation, currentRotation.eulerAngles.y, currentRotation.eulerAngles.z);
    }
}

