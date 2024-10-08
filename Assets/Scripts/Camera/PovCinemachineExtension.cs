using Cinemachine;
using UnityEngine;

public class PovCinemachineExtension : CinemachineExtension
{

    private InputManager inputManager;

    private Vector3 startingRotation;
    [SerializeField] private float clampAngle;
    [SerializeField] private float verticalSpeed;
    [SerializeField] private float horizontalSpeed;

    protected override void Awake()
    {
        inputManager = InputManager.Instance;
        base.Awake();
        Cursor.visible = false;
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (vcam.Follow)
        {
            if (stage == CinemachineCore.Stage.Aim)
            {
                if (startingRotation == null)
                    startingRotation = transform.localRotation.eulerAngles;

                if(inputManager.interactionState == InputManager.InteractionState.Nothing){
                    Vector2 deltaInput = inputManager.GetMouseDelta();
                    startingRotation.x -= deltaInput.x * horizontalSpeed * Time.deltaTime;
                    startingRotation.y += deltaInput.y * verticalSpeed * Time.deltaTime;
                    startingRotation.y = Mathf.Clamp(startingRotation.y, -clampAngle, clampAngle);
                }
                
                state.RawOrientation = Quaternion.Euler(-startingRotation.y, -startingRotation.x, 0f);
            }
        }
    }

}
