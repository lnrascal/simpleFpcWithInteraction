using UnityEngine;
using UnityEngine.InputSystem;

public class FPController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpForce = 1.6f;

    [Header("Mouse")]
    public float mouseSensitivity = 100f;
    public Transform cameraPivot;

    private CharacterController _controller;
    private PlayerInput _input;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private float _yVelocity;
    private float _xRotation;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _input = new PlayerInput();
    }
    
    void OnEnable()
    {
        _input.Enable();

        _input.Player.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
        _input.Player.Move.canceled += _ => _moveInput = Vector2.zero;

        _input.Player.Look.performed += ctx => _lookInput = ctx.ReadValue<Vector2>();
        _input.Player.Look.canceled += _ => _lookInput = Vector2.zero;

        _input.Player.Jump.performed += _ => Jump();
        
        Cursor.lockState = CursorLockMode.Locked;          
        Cursor.visible = false;      
    }

    void OnDisable()
    {
        _input.Disable();
        
        Cursor.lockState = CursorLockMode.None;          
        Cursor.visible = true;        
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMovement()
    {
        if (_controller.isGrounded && _yVelocity < 0)
            _yVelocity = -2f;

        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;

        _controller.Move(move * (moveSpeed * Time.deltaTime));

        _yVelocity += gravity * Time.deltaTime;
        _controller.Move(Vector3.up * (_yVelocity * Time.deltaTime));
    }

    void Jump()
    {
        if (_controller.isGrounded)
        {
            _yVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    void HandleMouseLook()
    {
        float mouseX = _lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = _lookInput.y * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);

        cameraPivot.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
    
    public void DisablePlayerControl()               
    {                                         
        this.enabled = false;         
    }                                         
                                              
    public void EnablePlayerControl()                
    {                                         
        this.enabled = true;          
    }                                         
}