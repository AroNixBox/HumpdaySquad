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

    [SerializeField] private float RotX;
    [SerializeField] private float RotY;
    [SerializeField] private float RotSpeed = 2f;

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject Camera;
    private FirstPersonController CC;
    public Vector3 InspectSize;
    void Start()
    {
        Position = transform.position;
        Rotation = transform.rotation;
        Scale = transform.localScale;
    }

    void Update()
    {
        RotX = -Input.GetAxis("Mouse X");
        RotY = Input.GetAxis("Mouse Y");

        if (Inspecting)
        {
            CC = Player.GetComponent<FirstPersonController>();
            CC.enabled = false;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnToDefault();
            }
            if (Input.GetMouseButton(0))
            {
                transform.rotation = Quaternion.AngleAxis(RotX * RotSpeed, transform.up) *
                Quaternion.AngleAxis(RotY * RotSpeed, transform.right) *
                transform.rotation;
            }
            transform.position = Vector3.Lerp(transform.position,Camera.transform.position 
            + Camera.transform.forward * 1.5f, 0.1f);
            transform.localScale = Vector3.Lerp(transform.localScale, InspectSize, 0.5f);
        }

        if (Returning)
        {
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, Position, 0.5f)
            , Quaternion.Lerp(transform.rotation, Rotation, 0.5f));
            transform.localScale = Vector3.Lerp(transform.localScale, Scale, 0.5f);

            if (transform.position == Position && transform.rotation == Rotation
                && transform.localScale == Scale)
            {
                Returning = false;
            }
        }
    }

    public void Interacter()
    {
        Inspecting = true;
    }

    void ReturnToDefault()
    {
        Returning = true;
        Inspecting = false;
        CC.enabled = true;
    }
}
