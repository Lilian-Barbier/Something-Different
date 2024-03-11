
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public enum InteractionState
    {
        Nothing,
        OpenDoor,
        TakePhoto
    }

    public InteractionState interactionState = InteractionState.Nothing;
    public Controls inputActions;

    //Declaration of delegates methods for events subscription
    public delegate void playerInteract();
    public delegate void playerLookInCamera();
    public delegate void playerUnLookInCamera();

    //Declaration of events
    public event playerInteract playerInteractEvent;
    public event playerLookInCamera playerLookInCameraEvent;
    public event playerUnLookInCamera playerUnLookInCameraEvent;

    public bool IsLookingInCamera = false;
    public bool IsRunning = false;

    void Awake()
    {
        inputActions = new Controls();

        inputActions.Movement.Interact.performed += ctx => playerInteractEvent?.Invoke();
        inputActions.Movement.LookInCamera.started += ctx =>
        {
            IsLookingInCamera = true;
            playerLookInCameraEvent?.Invoke();
        };

        inputActions.Movement.LookInCamera.canceled += ctx =>
        {
            IsLookingInCamera = false;
            playerUnLookInCameraEvent?.Invoke();
        };

        inputActions.Movement.Run.started += ctx => IsRunning = true;
        inputActions.Movement.Run.canceled += ctx => IsRunning = false;

    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public Vector2 GetMovement()
    {
        return inputActions.Movement.Move.ReadValue<Vector2>();
    }

    public Vector2 GetMouseDelta()
    {
        return inputActions.Movement.Look.ReadValue<Vector2>();
    }


}