using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    [SerializeField] private CharacterController _controller;
    [SerializeField] private float _speed = 5;

    public bool IsRunning { get; private set; }
    
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private float gravity = -20f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.3f;
    [SerializeField] private LayerMask groundMask;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2f;

    private bool _isEnableController = true;
    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation;
    private bool isGrounded;

    public float XRotation
    {
        get => xRotation;
        set => xRotation = value;
    }
    
    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (_isEnableController == false) return;
        
        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        ApplyGravity();
        HandleCameraLook();
    }

    public void OnDisableController()
    {
        _isEnableController = false;
    }

    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Сбрасываем вертикальную скорость при приземлении
        if (isGrounded && velocity.y < 0f)
            velocity.y = -2f;
    }

    private void HandleMovement()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical   = Input.GetAxisRaw("Vertical");

        // Движение относительно направления взгляда
        Vector3 direction = transform.right * horizontal + transform.forward * vertical;

        // Нормализуем только если есть реальный ввод (избегаем деления на 0)
        if (direction.magnitude > 1f)
            direction.Normalize();

        controller.Move(direction * moveSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // Формула из физики: v = sqrt(h * -2 * g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleCameraLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Вращаем камеру по вертикали (с ограничением угла)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Вращаем персонажа по горизонтали
        transform.Rotate(Vector3.up * mouseX);
    }

    // Отображение groundCheck в редакторе
    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
    }
}