using UnityEditor.Animations;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float gravityValue = -9.81f;

    private Transform cameraTransform;
    private Animator animatorController;

    [SerializeField] private float playerSpeed = 2.0f;
    [SerializeField] private float jumpHeight = 1.0f;


    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        cameraTransform = Camera.main.transform;
        animatorController = GetComponentInChildren<Animator>();
    }
    
    void Update()
    {
        if(groundedPlayer != controller.isGrounded && controller.isGrounded)
            animatorController.SetBool("IsJumping", false);

        groundedPlayer = controller.isGrounded;
        
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        transform.rotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        Vector2 movement = InputManager.Instance.GetMovement();

        if(movement.magnitude > 0)
            animatorController.SetBool("IsRunning", true);
        else
            animatorController.SetBool("IsRunning", false);

        Vector3 move = new(movement.x, 0, movement.y);
        move = transform.forward * move.z + cameraTransform.right * move.x;
        move.y = 0;

        controller.Move(playerSpeed * Time.deltaTime * move);
        
        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animatorController.SetBool("IsJumping", true);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}

