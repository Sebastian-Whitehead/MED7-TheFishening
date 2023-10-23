using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchControl : MonoBehaviour
{
    [Header("Pitch Limits")]
    public float minPitch = -45f; // Minimum pitch angle in degrees
    public float maxPitch = 45f;  // Maximum pitch angle in degrees

    [Header("Rotation Speed")]
    public float pitchSpeed = 30f;

    private void Update()
    {
        // Get input for pitch (up and down)
        float pitchInput = Input.GetAxis("Vertical");

        // Calculate the rotation angle
        float pitchAngle = pitchInput * pitchSpeed * Time.deltaTime;

        // Apply the rotation with pitch limits
        Vector3 currentRotation = transform.localEulerAngles;
        float newPitch = currentRotation.x - pitchAngle; // Subtract for pitch (up and down)
        newPitch = Mathf.Clamp(newPitch, minPitch, maxPitch); // Clamp pitch angle within limits

        // Apply the new pitch angle while preserving yaw and roll
        transform.localEulerAngles = new Vector3(newPitch, currentRotation.y, currentRotation.z);
    }
}

