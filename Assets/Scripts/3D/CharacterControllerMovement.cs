using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterControllerMovement : MonoBehaviour
{
     
    private Controls inputActions;
    private CharacterController characterController;
    private Vector3 direction;

    [SerializeField] private float speed = 5f;


    void Awake()
    {
        inputActions = new Controls();

        // Input callback
        inputActions.Movement.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        inputActions.Movement.Move.canceled += ctx => direction = Vector3.zero;

        characterController = GetComponent<CharacterController>();
    }


    void OnEnable()
    {
        inputActions.Enable();
    }

    void OnDisable()
    {
        inputActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        characterController.Move(direction * speed * Time.deltaTime);
        
    }

    private void Move(Vector2 vector2)
    {
        direction = new Vector3(vector2.x, 0, vector2.y);
    }
}
