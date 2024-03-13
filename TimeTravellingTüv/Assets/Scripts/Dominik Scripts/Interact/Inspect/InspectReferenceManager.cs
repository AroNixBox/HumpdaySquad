using System;
using UnityEngine;
using UnityEngine.Serialization;

public class InspectReferenceManager : MonoBehaviour
{
    public static InspectReferenceManager Instance { get; private set; }
    [field: SerializeField] public GameObject InspectReferenceCube { get; private set; }
    private const float MouseRotSpeed = 500f;
    private const float ControllerRotSpeed = 1f;
    private const float ControllerDivisor = 1f;
    private const float MouseDivisor = 10f;
    private const float WaitTimeUntilNextInputDeviceCheck = 5f;
    private float _currentWaitTime;

    private float _rotMod;
    private float _divisor;

    public event Action<float, float> OnModifiersChangedEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void LateUpdate()
    {
        _currentWaitTime -= Time.deltaTime;
        if (!(_currentWaitTime <= 0)) return;
        
        _currentWaitTime = WaitTimeUntilNextInputDeviceCheck;
        UpdateRotModBasedOnInputDevice();
    }
    private void UpdateRotModBasedOnInputDevice()
    {
        _rotMod = IsControllerConnected() ? ControllerRotSpeed : MouseRotSpeed;
        _divisor = IsControllerConnected() ? ControllerDivisor : MouseDivisor;
        OnModifiersChangedEvent?.Invoke(_rotMod, _divisor);
    }

    private bool IsControllerConnected()
    {
        var joysticks = Input.GetJoystickNames();
        return joysticks.Length > 0 && !string.IsNullOrEmpty(joysticks[0]);
    }
}
