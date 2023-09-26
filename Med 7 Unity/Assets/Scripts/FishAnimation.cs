using System.Collections;
using UnityEngine;

public class FishAnimation : MonoBehaviour
{
    public GameObject leadingPoint; // The GameObject that leads the rotation
    public float baseRotationSpeed = 1f; // Base speed of rotation
    public float maxSwingAngle = 15f; // Maximum swing angle
    public float accelerationSmoothingFactor = 0.05f; // Factor for smoothing the acceleration
    public Vector3 rotationAdjustment = new Vector3(0, -90, 90); // Adjustment for the rotation
    public float slerpSpeedFactor = 1f; // Speed factor for the Slerp function

    private Vector3 _previousPosition; // Previous position of the leading point
    private Vector3 _previousVelocity; // Previous velocity of the leading point
    private Vector3 _smoothedAcceleration; // Smoothed acceleration of the leading point

    private float _rotationTime;
    private bool _isSwingAngleIncreasing = true;
    private float _previousAngle = 0f;

    void Start()
    {
        _previousPosition = leadingPoint.transform.position;
        StartCoroutine(Rotate());
    }

    IEnumerator Rotate()
    {
        while (true)
        {
            CalculateVelocityAndAcceleration();
            SmoothAcceleration();
            float speed = CalculateSpeed();
            RotateLeadingPoint(speed);

            if (_rotationTime >= 1f)
            {
                _rotationTime = 0f;
                _isSwingAngleIncreasing = !_isSwingAngleIncreasing;
            }

            yield return null;
        }
    }

    void CalculateVelocityAndAcceleration()
    {
        // Calculate the velocity and acceleration based on the leading point's movement
        Vector3 velocity = (leadingPoint.transform.position - _previousPosition) / Time.deltaTime;
        Vector3 acceleration = (velocity - _previousVelocity) / Time.deltaTime;

        _previousPosition = leadingPoint.transform.position;
        _previousVelocity = velocity;
    }

    void SmoothAcceleration()
    {
        // Smooth the acceleration using Lerp function
        _smoothedAcceleration = Vector3.Lerp(_smoothedAcceleration, _previousVelocity, accelerationSmoothingFactor);
    }

    float CalculateSpeed()
    {
        // Calculate the speed based on the leading point's smoothed acceleration, ensuring it's never negative
        return baseRotationSpeed * Mathf.Max(0, _smoothedAcceleration.magnitude);
    }

    void RotateLeadingPoint(float speed)
    {
        // Calculate the new rotation angle
        _rotationTime += Time.deltaTime * speed;
        float newAngle = _isSwingAngleIncreasing ? Mathf.Lerp(-maxSwingAngle, maxSwingAngle, _rotationTime) : Mathf.Lerp(maxSwingAngle, -maxSwingAngle, _rotationTime);

        float angleDifference = newAngle - _previousAngle;
        _previousAngle = newAngle;

        // Apply the relative rotation to the leading point and set its forward direction to the velocity
        if (leadingPoint != null)
        {
            leadingPoint.transform.Rotate(angleDifference, 0, 0);

            if (_previousVelocity != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(_previousVelocity) * Quaternion.Euler(rotationAdjustment);
                leadingPoint.transform.rotation = Quaternion.Slerp(leadingPoint.transform.rotation, targetRotation, Time.deltaTime * speed * slerpSpeedFactor);
            }
        }
    }
}
