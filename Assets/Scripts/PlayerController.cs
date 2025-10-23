using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int IsJumping = Animator.StringToHash("isJumping");
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 15f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    private Animator _animator;
    private bool _isGrounded;
    private Rigidbody2D _rb;
    private PlayerInputActions _inputActions;
    private GameManager _gameManager;
    private bool _isJumping; // Thêm để quản lý trạng thái nhảy
    private AudioManager _audioManager;
    private void Awake()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _audioManager = FindObjectOfType<AudioManager>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        _inputActions = new PlayerInputActions();
        _inputActions.Player.Jump.performed += _ => HandleJump();
        _inputActions.Player.Move.Enable();
        _inputActions.Player.Jump.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Player.Move.Disable();
        _inputActions.Player.Jump.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(_gameManager.IsGameOver()||_gameManager.IsGameWin()) return;
        UpdateGrounded(); // Cập nhật _isGrounded liên tục
        float moveInput = _inputActions.Player.Move.ReadValue<Vector2>().x;
        HandleMovement(moveInput);
        UpdateAnimation(moveInput);
    }

    private void HandleMovement(float moveInput)
    {
        _rb.linearVelocity = new Vector2(moveInput * moveSpeed, _rb.linearVelocity.y);
        if (Mathf.Abs(moveInput) > 0.1f)
        {
            transform.localScale = new Vector3(Mathf.Sign(moveInput), 1, 1);
        }
        // Debug để kiểm tra giá trị đầu vào
    }


    private void HandleJump()
    {
        if(_gameManager.IsGameOver()||_gameManager.IsGameWin()||!_isGrounded) return;
        _audioManager.PlayJumpSound();
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, jumpForce);
        _isJumping = true; // Đặt trạng thái nhảy
        _animator.SetBool(IsJumping, _isJumping); // Kích hoạt animation ngay
    }

    private void UpdateGrounded()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (_isGrounded && _rb.linearVelocity.y <= 0) // Chạm đất và không rơi
        {
            _isJumping = false;
            _animator.SetBool(IsJumping, _isJumping); // Tắt animation khi chạm đất
        }
    }

    private void UpdateAnimation(float moveInput)
    {
        bool isRunning = Mathf.Abs(moveInput) > 0.1f; // Dựa vào input thay vì velocity
        _animator.SetBool(IsRunning, isRunning);
    }

    private void OnDrawGizmos()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
}