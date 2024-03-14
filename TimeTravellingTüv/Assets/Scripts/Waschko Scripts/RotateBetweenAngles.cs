using UnityEngine;

public class RotateBetweenAngles : MonoBehaviour
{
    public float rotationAngle = 65f;
    private readonly float _rotationSpeed = 0.05f;

    private Quaternion _leftRotation;
    private Quaternion _rightRotation;
    private float _time;
    private bool _movingRight = true;

    private void Start()
    {
        // Calculate the left and right rotation angles based on the camera's forward vector
        Vector3 leftDirection = Quaternion.Euler(0f, -rotationAngle, 0f) * transform.forward;
        Vector3 rightDirection = Quaternion.Euler(0f, rotationAngle, 0f) * transform.forward;

        _leftRotation = Quaternion.LookRotation(leftDirection);
        _rightRotation = Quaternion.LookRotation(rightDirection);
    }

    private void Update()
    {
        // Rotate the camera between the left and right rotation angles
        if (_movingRight)
        {
            _time += _rotationSpeed * Time.deltaTime;
            if (_time >= 1f)
            {
                _time = 1f;
                _movingRight = false;
            }
        }
        else
        {
            _time -= _rotationSpeed * Time.deltaTime;
            if (_time <= 0f)
            {
                _time = 0f;
                _movingRight = true;
            }
        }

        transform.rotation = Quaternion.Slerp(_leftRotation, _rightRotation, _time);
    }
}