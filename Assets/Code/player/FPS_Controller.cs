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
    [SerializeField, Range(-90f, 90f)] private float maxPitch = 90f;
    [SerializeField, Range(-90f, 90f)] private float minPitch = -90f;
    [SerializeField] private Transform mPitchController;

    public CharacterController controller;

    private float _mYaw;
    private float _mPitch;
    private Vector2 _inputMove;
    private Vector2 _inputLook;
    private Vector3 _velocity;
    private Vector3 _moveVector;


    private Vector2 _prevLookInput;
    private Vector2 _lookVelocity;


    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Vector3 startRot = transform.rotation.eulerAngles;
        _mYaw = startRot.y;
        _mPitch = mPitchController.localRotation.eulerAngles.x;
    }
    void OnEnable()
    {
        PortalEvents.OnPlayerTeleported += HandleTeleportEvent;
    }

    void OnDisable()
    {
        PortalEvents.OnPlayerTeleported -= HandleTeleportEvent;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }
    private void HandleRotation()
    {
        if (_inputLook.sqrMagnitude < 0.0001f) { _lookVelocity = Vector2.zero; return; }

        float lookX = _inputLook.x * lookSensitivity * rotationSpeed * Time.deltaTime;
        float lookY = _inputLook.y * lookSensitivity * rotationSpeed * Time.deltaTime;

        _lookVelocity = (_inputLook - _prevLookInput) / Time.deltaTime;
        _prevLookInput=_inputLook;

        _mYaw += lookX;
        _mPitch += invertPitch ? lookY : -lookY;

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
        else
        {
            _moveVector= Vector3.zero;
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

    private void HandleTeleportEvent(Portal fromPortal, Portal toPortal, GameObject player)
    {
        
        controller.enabled = false;

        Transform portalA = fromPortal.transform;
        Transform portalB = toPortal.transform;

        //calculate position
        Vector3 localPos = portalA.InverseTransformPoint(transform.position);
        //localPos.z -= 0.03f;
        localPos.z = -localPos.z;
        localPos.x = -localPos.x;
        Vector3 finalPos = portalB.TransformPoint(localPos);

        //calculate dir
        Vector3 localDir = portalA.InverseTransformDirection(transform.forward);
        localDir.z = -localDir.z;
        localDir.x = -localDir.x;
        Vector3 finalDir = portalB.TransformDirection(localDir);

        Quaternion camRot = toPortal.reflectionCamera.transform.rotation;

        Quaternion bodyYaw = Quaternion.Euler(0f, camRot.eulerAngles.y, 0f);
        transform.SetPositionAndRotation(finalPos, bodyYaw);

        float camPitch = NormalizeAngle(camRot.eulerAngles.x);
        mPitchController.localRotation = Quaternion.Euler(camPitch, 0f, 0f);

        // Actualizar valores internos del controlador de cámara
        _mYaw = NormalizeAngle(transform.eulerAngles.y);
        _mPitch = NormalizeAngle(mPitchController.localEulerAngles.x);

        //_velocity = Vector3.zero;

        controller.enabled = true;

    }
    public Vector3 GetVelocity()
    {
        Vector3 baseVel = _moveVector * moveSpeed;
        Vector3 camYawImpulse = transform.right * _lookVelocity.x * 0.01f;
        Vector3 camPitchImpulse = mPitchController.up * (-_lookVelocity.y) * 0.01f;

        return baseVel + camYawImpulse + camPitchImpulse;

}
    private float NormalizeAngle(float angle)
    {
        angle %= 360f;
        if (angle > 180f) angle -= 360f;
        return angle;
    }



}
