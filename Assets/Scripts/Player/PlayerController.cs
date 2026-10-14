using UnityEngine;

public enum CameraMode
{
    FirstPerson,
    ThirdPerson
}

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private CharacterController controller;
    [SerializeField] private float moveSpeed = 5.5f;
    [SerializeField] private float sprintSpeed = 8.5f;
    [SerializeField] private float jumpStrength = 5f;
    [SerializeField] private float gravity = -18f;
    [SerializeField] private float turnSmoothTime = 0.08f;

    [Header("Camera")]
    [SerializeField] private Transform firstPersonPivot;
    [SerializeField] private Transform thirdPersonPivot;

    private Vector3 velocity;
    private float turnSmoothVelocity;
    private Vector2 inputVector;

    public CameraMode cameraMode { get; private set; } = CameraMode.ThirdPerson;

    private void Awake()
    {
        if (controller == null)
        {
            controller = GetComponent<CharacterController>();
        }
    }

    public void SetMovementInput(Vector2 axis)
    {
        inputVector = axis;
    }

    public void SetCameraMode(CameraMode mode)
    {
        cameraMode = mode;
    }

    public void TriggerJump()
    {
        if (controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpStrength * -2f * gravity);
        }
    }

    public bool IsSprinting()
    {
        return inputVector.sqrMagnitude > 0.01f && Input.GetKey(KeyCode.LeftShift);
    }

    private void Update()
    {
        Vector3 movement = Vector3.zero;

        if (inputVector.sqrMagnitude > 0.01f)
        {
            Vector3 desired = new Vector3(inputVector.x, 0f, inputVector.y);
            desired = transform.TransformDirection(desired);
            desired.y = 0f;

            if (desired.magnitude > 1f)
            {
                desired.Normalize();
            }

            float currentSpeed = IsSprinting() ? sprintSpeed : moveSpeed;
            movement = desired * currentSpeed;

            float targetAngle = Mathf.Atan2(desired.x, desired.z) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        if (controller.isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(movement * Time.deltaTime);
        controller.Move(new Vector3(0f, velocity.y, 0f) * Time.deltaTime);
    }

    public Transform GetCurrentViewAnchor()
    {
        return cameraMode == CameraMode.FirstPerson ? firstPersonPivot : thirdPersonPivot;
    }
}
