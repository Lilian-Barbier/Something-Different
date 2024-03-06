using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Door : MonoBehaviour
{
    public bool isOpen = false;
    public float maxDoorAngle = 90.0f;
    public float doorCloseAngle = 0.0f;
    [Range(0.0f, 360.0f)] public float currentDoorAngle = 0.0f;
    public CinemachineVirtualCamera doorCameraAnimation;

    public Transform handlePosition;
    public Transform FrontPlayerPosition;

    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Quaternion.Euler(0, currentDoorAngle, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentDoorAngle < maxDoorAngle)
        {
            transform.rotation = Quaternion.Euler(0, currentDoorAngle, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, maxDoorAngle, 0);
        }

    }
}
