using System;
using System.Collections;
using System.Collections.Generic;
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
        rightArmRig =  GetComponentInChildren<TwoBoneIKConstraint>();
        armTargetPoint = rightArmRig.transform.GetChild(0);

        rightArmRig.weight = 0;
    }

    private void Interact()
    {
        if(inputManager.interactionState == InputManager.InteractionState.OpenDoor)
        {
            FinishInteraction();
        }
        else{
            Debug.DrawRay(cameraTransform.position, cameraTransform.forward * interactionDistance, Color.red, 2);

            // Does the ray intersect any objects 
            if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out RaycastHit hit, interactionDistance))
            {
                if(hit.transform.CompareTag("Door")){
                    currentDoor = hit.transform.GetComponent<Door>();
                    inputManager.interactionState = InputManager.InteractionState.OpenDoor;
                    //todo : a modifier
                    newArmTargetPoint = hit.transform.GetChild(0).position;
                    rightArmRig.weight = 1;
                }
            }
        }
    }

    private void FinishInteraction(){
        rightArmRig.weight = 0;
        inputManager.interactionState = InputManager.InteractionState.Nothing;
    }

    void Update(){

        if(newArmTargetPoint != null){
            armTargetPoint.position = Vector3.Lerp(armTargetPoint.position, newArmTargetPoint, Time.deltaTime * 5);
        }

        if(inputManager.interactionState == InputManager.InteractionState.OpenDoor){
            //Get inverse mouse delta to open the door
            float angleDelta = -inputManager.GetMouseDelta().x * doorOpeningSpeed * Time.deltaTime;

            if(currentDoor.currentDoorAngle + angleDelta < currentDoor.maxDoorAngle && currentDoor.currentDoorAngle + angleDelta > currentDoor.doorCloseAngle)
                currentDoor.currentDoorAngle += angleDelta;

            //If the door is open to 85% of maxAngle, we open it completely and stop the interaction
            if(currentDoor.currentDoorAngle > currentDoor.maxDoorAngle * 0.85f){
                currentDoor.currentDoorAngle = currentDoor.maxDoorAngle;
                FinishInteraction();
            }
                
        }
    }

}
