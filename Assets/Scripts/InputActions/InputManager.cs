
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    public Controls inputActions;

    void Awake()
    {
        inputActions = new Controls();
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