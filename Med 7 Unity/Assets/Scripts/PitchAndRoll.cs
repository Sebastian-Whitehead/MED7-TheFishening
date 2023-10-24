using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchAndRoll : MonoBehaviour
{
    [Header("Pitch Limits")]
    public float minPitch = -45f; // Minimum pitch angle in degrees
    public float maxPitch = 45f;  // Maximum pitch angle in degrees

    [Header("Roll Limits")]
    public float minRoll = -45f; // Minimum roll angle in degrees
    public float maxRoll = 45f;  // Maximum roll angle in degrees

    [Header("Rotation Speed")]
    public float pitchSpeed = 30f;
    public float rollSpeed = 30f;

    private void Update()
    {
        // Get input for pitch (up and down) and roll (left and right)
        float pitchInput = Input.GetAxis("Vertical");
        float rollInput = Input.GetAxis("Horizontal");

        // Calculate the rotation angles
        float pitchAngle = pitchInput * pitchSpeed * Time.deltaTime;
        float rollAngle = rollInput * rollSpeed * Time.deltaTime;

        // Apply the rotation with pitch and roll limits
        Vector3 currentRotation = transform.localEulerAngles;
        float newPitch = currentRotation.x - pitchAngle; // Subtract for pitch (up and down)
        float newRoll = currentRotation.z + rollAngle;    // Add for roll (left and right)

        newPitch = Mathf.Clamp(newPitch, minPitch, maxPitch); // Clamp pitch angle within limits
        newRoll = Mathf.Clamp(newRoll, minRoll, maxRoll);    // Clamp roll angle within limits

        // Apply the new pitch and roll angles while preserving yaw
        transform.localEulerAngles = new Vector3(newPitch, currentRotation.y, newRoll);
    }
}

