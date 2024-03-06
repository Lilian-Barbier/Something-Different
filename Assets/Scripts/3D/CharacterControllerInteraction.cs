using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class CharacterControllerInteraction : MonoBehaviour
{
    private InputManager inputManager;
    private Transform cameraTransform;

    [SerializeField] private float interactionDistance = 2.0f;

    // Arm Rig constraints 
    private TwoBoneIKConstraint rightArmRig;
    private Transform armTargetPoint;
    private Vector3 newArmTargetPoint;

    //Door Opening Variables
    private Door currentDoor;
    [SerializeField] private float doorOpeningSpeed = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        inputManager = InputManager.Instance;
        inputManager.playerInteractEvent += Interact;
        cameraTransform = Camera.main.transform;
        rightArmRig = GetComponentInChildren<TwoBoneIKConstraint>();
        armTargetPoint = rightArmRig.transform.GetChild(0);

        rightArmRig.weight = 0;
    }

    private void Interact()
    {
        if (inputManager.interactionState == InputManager.InteractionState.OpenDoor)
        {
            FinishInteraction();
        }
        else
        {
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactionDistance, Color.red, 2);

            // Does the ray intersect any objects 
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactionDistance))
            {
                if (hit.transform.CompareTag("Door"))
                {
                    currentDoor = hit.transform.GetComponent<Door>();
                    inputManager.interactionState = InputManager.InteractionState.OpenDoor;
                    newArmTargetPoint = currentDoor.handlePosition.position;
                    rightArmRig.weight = 1;
                    transform.position = currentDoor.FrontPlayerPosition.position;
                    currentDoor.doorCameraAnimation.Priority = 20;
                }
            }
        }
    }

    private void FinishInteraction()
    {
        rightArmRig.weight = 0;

        //Vérifier si possible de faire plus propre, le CharacterController empéche de changer la position directement
        var charController = GetComponent<CharacterController>();
        charController.enabled = false;
        transform.position = new Vector3(cameraTransform.position.x, transform.position.y, cameraTransform.position.z);
        charController.enabled = true;

        currentDoor.doorCameraAnimation.Priority = 0;
        inputManager.interactionState = InputManager.InteractionState.Nothing;
    }

    void Update()
    {
        if (newArmTargetPoint != null)
        {
            newArmTargetPoint = currentDoor.handlePosition.position;
            armTargetPoint.position = newArmTargetPoint;
        }

        if (inputManager.interactionState == InputManager.InteractionState.OpenDoor)
        {
            //voir pour passer plutot par un évenement pour éviter les moment ou on garde la touche appuyer
            if (inputManager.GetMovement() != Vector2.zero)
                FinishInteraction();

            //Get inverse mouse delta to open the door
            float angleDelta = -inputManager.GetMouseDelta().x * doorOpeningSpeed * Time.deltaTime;
            transform.position = currentDoor.FrontPlayerPosition.position;

            //update camera dolly position to match the door opening
            currentDoor.doorCameraAnimation.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = currentDoor.currentDoorAngle / currentDoor.maxDoorAngle;

            if (currentDoor.currentDoorAngle + angleDelta < currentDoor.maxDoorAngle && currentDoor.currentDoorAngle + angleDelta > currentDoor.doorCloseAngle)
                currentDoor.currentDoorAngle += angleDelta;

            //If the door is open to 85% of maxAngle, we open it completely and stop the interaction
            if (currentDoor.currentDoorAngle > currentDoor.maxDoorAngle * 0.85f && !currentDoor.isOpen)
            {
                currentDoor.currentDoorAngle = currentDoor.maxDoorAngle;
                currentDoor.isOpen = true;
                FinishInteraction();
            }
            //If the door is closed to 5% of maxAngle, we close it completely and stop the interaction
            else if (currentDoor.currentDoorAngle < currentDoor.maxDoorAngle * 0.1f && currentDoor.isOpen)
            {
                currentDoor.currentDoorAngle = currentDoor.doorCloseAngle;
                currentDoor.isOpen = false;
                FinishInteraction();
            }

        }
    }

}
