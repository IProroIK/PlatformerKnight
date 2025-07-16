using Core.Service;
using Settings;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementController : MonoBehaviour
{
    private static readonly int PlayerState = Animator.StringToHash("playerState");

    [Header("Movement")] [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float accel = 80f;
    [SerializeField] private float decel = 60f;
    [SerializeField] private float airControlMultiplier = 0.5f;

    [Header("Jumping")] [SerializeField] private float jumpForce = 16f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float jumpBufferTime = 0.1f;

    [Header("Wall")] [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(12, 16);
    [SerializeField] private float wallJumpLockTime = 0.2f;

    [Header("Check")] [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Animator")] [SerializeField] private Animator animator;

    private IAppStateService _appStateService;
    private Rigidbody2D _rb;
    private float _moveInput;
    private bool _isFacingRight = true;

    private bool _isGrounded;
    private bool _isTouchingWall;
    private bool _isWallSliding;
    private float _coyoteTimer;
    private float _jumpBufferTimer;
    private float _wallJumpLockTimer;
    private int _wallDir;

    [Inject]
    private void Construct(IAppStateService appStateService)
    {
        _appStateService = appStateService;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _appStateService.AppStateChangedEvent += AppStateChangedEventHandler;
    }

    private void OnDestroy()
    {
        _appStateService.AppStateChangedEvent -= AppStateChangedEventHandler;
    }

    private void AppStateChangedEventHandler()
    {
        if (_appStateService.AppState != Enumerators.AppState.Game)
        {
            _moveInput = 0;
        }
    }

    private void Update()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");

        CheckSurroundings();

        _coyoteTimer -= Time.deltaTime;
        _jumpBufferTimer -= Time.deltaTime;
        _wallJumpLockTimer -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
            _jumpBufferTimer = jumpBufferTime;

        if (_jumpBufferTimer > 0 && (_coyoteTimer > 0 || _isWallSliding))
        {
            if (_isWallSliding)
            {
                _rb.velocity = new Vector2(-_wallDir * wallJumpForce.x, wallJumpForce.y);
                _wallJumpLockTimer = wallJumpLockTime;
                Flip(-_wallDir);
            }
            else
            {
                _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
            }

            _jumpBufferTimer = 0f;
        }
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleWallSliding();
    }

    public void SetMovementInput(float value)
    {
        _moveInput = value;
    }

    public void Jump()
    {
        _jumpBufferTimer = jumpBufferTime;
        animator.SetInteger(PlayerState, 2);
    }

    private void HandleMovement()
    {
        if (_wallJumpLockTimer > 0f)
            return;

        if (Mathf.Abs(_moveInput) != 0)
            animator.SetInteger(PlayerState, (int)Enumerators.PlayerAnimationState.Run);
        else
            animator.SetInteger(PlayerState, (int)Enumerators.PlayerAnimationState.Idle);


        float targetSpeed = _moveInput * moveSpeed;
        float accelRate = _isGrounded ? (Mathf.Abs(targetSpeed) > 0.01f ? accel : decel) : accel * airControlMultiplier;

        float speedDiff = targetSpeed - _rb.velocity.x;
        float movement = speedDiff * accelRate;

        _rb.AddForce(Vector2.right * movement);

        if (_moveInput > 0 && !_isFacingRight)
            Flip(1);
        else if (_moveInput < 0 && _isFacingRight)
            Flip(-1);
    }

    private void HandleWallSliding()
    {
        if (_isTouchingWall && !_isGrounded)
        {
            _isWallSliding = true;

            if (_rb.velocity.y < -wallSlideSpeed)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, -wallSlideSpeed);
            }
        }
        else
        {
            _isWallSliding = false;
        }
    }


    private void CheckSurroundings()
    {
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, groundLayer);
        _isTouchingWall = Physics2D.OverlapCircle(wallCheck.position, checkRadius, groundLayer);

        if (_isGrounded)
            _coyoteTimer = coyoteTime;

        if (_isTouchingWall)
            _wallDir = _isFacingRight ? 1 : -1;
    }

    private void Flip(int dir)
    {
        if (dir == 0) return;

        _isFacingRight = dir > 0;

        float yRotation = _isFacingRight ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
}