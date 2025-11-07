using UnityEngine;
using UnityEngine.InputSystem;

public class FPS_Controller : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Camera Rotation")]
    [SerializeField] private float lookSensitivity = 1.5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private bool invertPitch = false;
    [SerializeField, Range(-80f, 80f)] private float maxPitch = 80f;
    [SerializeField, Range(-80f, 80f)] private float minPitch = -80f;
    [SerializeField] private Transform mPitchController;

    private CharacterController controller;

    private float _mYaw;
    private float _mPitch;
    private Vector2 _inputMove;
    private Vector2 _inputLook;
    private Vector3 _velocity;
    private Vector3 _moveVector;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 startRot = transform.rotation.eulerAngles;
        _mYaw = startRot.y;
        _mPitch = mPitchController.localRotation.eulerAngles.x;
    }

    void Update()
    {
        HandleRotation();
        HandleMovement();
    }

    private void HandleRotation()
    {
        if (_inputLook.sqrMagnitude < 0.0001f) return;

        float lookX = _inputLook.x * lookSensitivity * rotationSpeed * Time.deltaTime;
        float lookY = _inputLook.y * lookSensitivity * rotationSpeed * Time.deltaTime;

        _mYaw += lookX;
        _mPitch += (invertPitch ? lookY : -lookY);

        _mPitch = Mathf.Clamp(_mPitch, minPitch, maxPitch);

        transform.rotation = Quaternion.Euler(0f, _mYaw, 0f);
        mPitchController.localRotation = Quaternion.Euler(_mPitch, 0f, 0f);
    }

    private void HandleMovement()
    {
        if (_inputMove.sqrMagnitude > 0.001f)
        {
            _moveVector.x = _inputMove.x;
            _moveVector.z = _inputMove.y;
            _moveVector.Normalize();

            _moveVector = transform.TransformDirection(_moveVector);
            controller.Move(_moveVector * moveSpeed * Time.deltaTime);
        }

        if (controller.isGrounded)
        {
            if (_velocity.y < 0f) _velocity.y = -2f;
        }

        _velocity.y += gravity * Time.deltaTime;
        controller.Move(_velocity * Time.deltaTime);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _inputMove = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _inputLook = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && controller.isGrounded)
        {
            _velocity.y = jumpForce;
        }
    }
}
