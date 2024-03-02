using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using StarterAssets;


public class Inspect : MonoBehaviour, IInteraction
{
    private Vector3 Position;
    private Quaternion Rotation;
    private Vector3 Scale;

    private bool Inspecting;
    private bool Returning;
    private bool StartInspecting;
    private bool StopInspecting;

    [Tooltip("(This Value will be set automatically in-script) " +
        "Value for rotating object along local x-axis")]
    [SerializeField] private float RotX;
    [Tooltip("(This Value will be set automatically in-script) " +
        "Value for rotating object along local y-axis")]
    [SerializeField] private float RotY;
    [Tooltip("(Value for the rotation-speed of the object")]
    [SerializeField] private float RotSpeed = 2f;

    [Tooltip("(This Value will be set automatically in-script) " +
        "Value for the amount of zooming happening")]
    [SerializeField] private float Zoom;
    [Tooltip("(This Value will be set automatically in-script) " +
        "Value for the maximum size the object can become")]
    [SerializeField] private Vector3 MaxSize;
    [Tooltip("(This Value will be set automatically in-script) " +
        "Value for the minimum size the object can become")]
    [SerializeField] private Vector3 MinSize;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    private FirstPersonController CC;
    private Interact _interact;

    [Tooltip("Size the object will inherit when inspecting (needed for large objects (needs to be manually added")]
    public Vector3 InspectSize;
    void Start()
    {
        Position = transform.position;
        Rotation = transform.rotation;
        Scale = transform.localScale;

        MaxSize = InspectSize * 5f;
        MinSize = InspectSize / 5f;
    }

    void Update()
    {
        if (Inspecting)
        {
            RotX = -Input.GetAxis("Mouse X");
            RotY = Input.GetAxis("Mouse Y");
            Zoom = Input.GetAxis("Mouse ScrollWheel") * transform.localScale.x;

            CC = Player.GetComponent<FirstPersonController>();
            CC.enabled = false;
            _interact = Camera.GetComponent<Interact>();
            _interact.IsInteracting = true;

            if (transform.position != Camera.transform.position + Camera.transform.forward * 1.5f)
            {
                transform.position = Vector3.Lerp(transform.position, Camera.transform.position
                + Camera.transform.forward * 1.5f, 0.1f);
            }

            if (StartInspecting) // Change rotation and scale of object to inspect values at start of inspecting
            {
                if (transform.localScale != InspectSize)
                {
                    transform.localScale = Vector3.Lerp(transform.localScale, InspectSize, 0.5f);
                }

                if (transform.position == Camera.transform.position + Camera.transform.forward * 1.5f ||
                    transform.localScale == InspectSize || transform.rotation == Camera.transform.rotation)
                {
                    StartInspecting = false;
                }
            }

            if (Input.GetMouseButton(0)) // Holding LMB and moving the mouse will rotate the object (along local-axis)
            {
                transform.rotation = Quaternion.AngleAxis(RotX * RotSpeed, transform.up) *
                Quaternion.AngleAxis(RotY * RotSpeed, transform.right) *
                transform.rotation;
            }
            if (Input.GetKeyDown(KeyCode.R)) // This will set the rotation and scale of the object to default inspect
            {
                transform.rotation = Camera.transform.rotation;
                transform.localScale = InspectSize;
            }
            transform.localScale = new Vector3(Mathf.Clamp(transform.localScale.x + Zoom, MinSize.x, MaxSize.x), // Zoom in and out of an object
            Mathf.Clamp(transform.localScale.y + Zoom, MinSize.y, MaxSize.y), Mathf.Clamp(transform.localScale.z + Zoom, MinSize.z, MaxSize.z));
        }

        if (StopInspecting)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
            {
                ReturnToDefault();
                StopInspecting = false;
                StartCoroutine(EnableInteract());
            }
        }
        if (Returning) // Set Transform of object to state before inspect
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, Position, 0.5f) 
            , Quaternion.Lerp(transform.rotation, Rotation, 0.5f));
            transform.localScale = Vector3.Lerp(transform.localScale, Scale, 0.5f);

            if (transform.position == Position || transform.rotation == Rotation
                || transform.localScale == Scale)
            {
                Returning = false;
            }
        }
    }

    public void Interacter()
    {
        StartInspecting = true;
        Inspecting = true;
        transform.rotation = Quaternion.Lerp(transform.rotation, Camera.transform.rotation, 0.5f);
        StartCoroutine(EnableBool());
    }

    private IEnumerator EnableBool()
    {
        yield return new WaitForSeconds(0.2f);
        StopInspecting = true;
    }

    private IEnumerator EnableInteract() 
    {
        yield return new WaitForSeconds(0.1f);
        _interact.IsInteracting = false;
    }

    void ReturnToDefault()
    {
        Returning = true;
        Inspecting = false;
        CC.enabled = true;
    }
}
