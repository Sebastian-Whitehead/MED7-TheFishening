using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitchRollYaw : MonoBehaviour
{
    [Header("Rotation Speed")]
    public float pitchSpeed = 30f;
    public float rollSpeed = 30f;
    public float yawSpeed = 30f;

    [Header("Rotation Limits")]
    public Vector2 pitchLimits = new Vector2(-45f, 45f); // Specify min and max pitch angles in degrees
    public Vector2 rollLimits = new Vector2(-45f, 45f);  // Specify min and max roll angles in degrees
    public Vector2 yawLimits = new Vector2(-90f, 90f);   // Specify min and max yaw angles in degrees

    private void Update()
    {
        // Get input for pitch, roll, and yaw
        float pitchInput = Input.GetAxis("Vertical");
        float rollInput = Input.GetAxis("Horizontal");
        float yawInput = Input.GetAxis("Rotate");

        // Calculate the rotation angles
        float pitchAngle = pitchInput * pitchSpeed * Time.deltaTime;
        float rollAngle = rollInput * rollSpeed * Time.deltaTime;
        float yawAngle = yawInput * yawSpeed * Time.deltaTime;

        // Apply the rotations with limits
        Vector3 newRotation = transform.localEulerAngles + new Vector3(pitchAngle, yawAngle, rollAngle);
        newRotation.x = Mathf.Clamp(newRotation.x, pitchLimits.x, pitchLimits.y);
        newRotation.y = Mathf.Clamp(newRotation.y, yawLimits.x, yawLimits.y);
        newRotation.z = Mathf.Clamp(newRotation.z, rollLimits.x, rollLimits.y);

        transform.localEulerAngles = newRotation;
    }
}

