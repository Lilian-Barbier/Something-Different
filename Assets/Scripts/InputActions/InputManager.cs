
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public Controls inputActions;

    //Declaration of delegates methods for events subscription
    public delegate void playerInteract();

    //Declaration of events
    public event playerInteract playerInteractEvent;

    void Awake()
    {
        inputActions = new Controls();
        inputActions.Movement.Interact.performed += ctx => playerInteractEvent?.Invoke();
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