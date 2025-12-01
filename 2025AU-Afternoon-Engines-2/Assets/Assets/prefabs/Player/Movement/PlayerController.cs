using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump = true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Stamina")]
    public Image staminaBar;
    public float stamina;
    public float maxStamina;
    public float runCost;
    public float chargeRate;
    private Coroutine recharge;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState movementState;

    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        air
    }

    private InputSystems controls;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private bool jumpPressed;
    private bool sprintPressed;
    private bool crouchPressed;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        startYScale = transform.localScale.y;
    }

    private void Awake()
    {
        controls = new InputSystems();

        controls = new InputSystems();

        // movement
        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;

        // looking
        controls.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        controls.Player.Look.canceled += ctx => lookInput = Vector2.zero;

        // jumping
        controls.Player.Jump.performed += ctx => jumpPressed = true;

        // sprint
        controls.Player.Sprint.performed += ctx => sprintPressed = true;
        controls.Player.Sprint.canceled += ctx => sprintPressed = false;

        // crouch
        controls.Player.Crouch.performed += ctx => crouchPressed = true;
        controls.Player.Crouch.canceled += ctx => crouchPressed = false;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // handle drag
        if (grounded)
            rb.linearDamping = groundDrag;
        else
            rb.linearDamping = 0;
    }

    private IEnumerator rechargeStamina()
    {
        yield return new WaitForSeconds(1f);

        while (stamina < maxStamina)
        {
            stamina += chargeRate / 10f;
            if (stamina > maxStamina) stamina = maxStamina;
            staminaBar.fillAmount = stamina / maxStamina;
            yield return new WaitForSeconds(.1f);
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = moveInput.x;
        verticalInput = moveInput.y;

        // when jump key is pressed
        if (jumpPressed && readyToJump && grounded)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
        jumpPressed = false;

        // crouch
        if (crouchPressed)
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 10f, ForceMode.Impulse);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // mode - crouching
        if (Input.GetKey(crouchKey))
        {
            movementState = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // mode - sprinting
        else if (sprintPressed && stamina > 0)
        {
            movementState = MovementState.sprinting;
            moveSpeed = sprintSpeed;

            if (moveInput != Vector2.zero)
            {
                stamina -= runCost * Time.deltaTime;
                if (stamina < 0)
                {
                    stamina = 0;
                    movementState = MovementState.walking;
                    moveSpeed = walkSpeed;
                }
            }
            staminaBar.fillAmount = stamina / maxStamina;

            if (recharge != null) StopCoroutine(recharge);
            recharge = StartCoroutine(rechargeStamina());
        }
        // mode - walking
        else if (grounded)
        {
            movementState = MovementState.walking;
            moveSpeed = walkSpeed;
        }
        // mode - air
        else
        {
            movementState = MovementState.air;
        }
    }
    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * PerkChecker.MovementPerkSpeed * 10f, ForceMode.Force);

        // if on ground
        if (grounded)
            rb.AddForce(Vector3.down * 10f, ForceMode.Force);
        // if in air
        else
            rb.AddForce(Vector3.down * 10f * airMultiplier, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        // reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }
}
