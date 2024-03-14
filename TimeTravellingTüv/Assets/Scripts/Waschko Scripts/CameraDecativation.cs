using UnityEngine;

public class CameraDecativation : MonoBehaviour
{
    private Camera _camera;
    private void Start()
    {
        _camera = GetComponent<Camera>();
        CameraSensor.Instance.OnPlayerDetected += OnPlayerDetected;
        
        //initially disable the camera
        _camera.enabled = false;
    }

    private void OnPlayerDetected(bool detected)
    {
        _camera.enabled = detected;
    }
    private void OnDestroy()
    {
        CameraSensor.Instance.OnPlayerDetected -= OnPlayerDetected;
    }
}
