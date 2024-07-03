using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private KeyCode moveLeftKey;
    private KeyCode moveRightKey;
    private KeyCode jumpKey;
    private KeyCode ropeUpKey;
    private KeyCode ropeDownKey;
    private KeyCode interactionKey;

    [SerializeField] private float playerSpeed = 6.0f;
    [SerializeField] private float jumpPower;
    [SerializeField] private float climbSpeed = 5.0f;
    private float moveHorizontal = 0.0f;
    private float moveVertical = 0.0f;
    private float originGravity;
    private bool isGround = true;

    private Rigidbody2D playerRigidbody;
    private BoxCollider2D playerCollider;

    private RaycastHit2D groundRay;
    [SerializeField] private Transform slopeCheckPosition;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float slopeRayDistance;
    private float slopeAngle;
    private Vector2 slopePerp;
    private bool isSlope;
    private bool canClimb;
    private bool isClimbing;
    private bool isClimbSetOnce;

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();

        moveLeftKey = KeyCode.LeftArrow;
        moveRightKey = KeyCode.RightArrow;
        jumpKey = KeyCode.UpArrow;
        ropeUpKey = KeyCode.UpArrow;
        ropeDownKey = KeyCode.DownArrow;
        interactionKey = KeyCode.F;

        originGravity = playerRigidbody.gravityScale;

        Managers.Input.keyAction += OnPlayerMove;
        Managers.Input.keyAction += OnPlayerJump;
    }

    void Update()
    {
        SlopeCheck();

        if (InputManager.isNeedInit)
        {
            InputManager.isNeedInit = false;
            moveHorizontal = 0.0f;
        }

        if (canClimb)
        {
            if (Input.GetKeyDown(interactionKey))
                isClimbing = true;
        }

        if (isClimbing)
        {
            playerRigidbody.gravityScale = 0.0f;
            playerCollider.isTrigger = true;
            float verticalInput = Input.GetAxis("Vertical");
            playerRigidbody.velocity = new Vector2(0.0f, verticalInput * climbSpeed);
        }

        if (moveHorizontal == 0.0f)
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnDisable()
    {
        Managers.Input.keyAction -= OnPlayerMove;
        Managers.Input.keyAction -= OnPlayerJump;
    }

    private void OnPlayerJump()
    {
        if (Input.GetKeyDown(jumpKey) && isGround)
        {
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }
    }

    private void OnPlayerMove()
    {
        moveHorizontal = Input.GetAxis("Horizontal");

        if (moveHorizontal < 0)
            transform.rotation = Quaternion.Euler(0, 0, 0);
        else
            transform.rotation = Quaternion.Euler(0, 180, 0);

        Vector2 movement = new Vector2(moveHorizontal, 0.0f);

        if (movement.magnitude > 1)
            movement.Normalize();

        movement *= playerSpeed;

        Vector2 velocityYOnly = new Vector2(0.0f, playerRigidbody.velocity.y);

        if (isSlope)
            movement *= slopePerp * -1f;

        playerRigidbody.velocity = movement + velocityYOnly;
    }

    private void SlopeCheck()
    {
        groundRay = Physics2D.Raycast(slopeCheckPosition.position, Vector2.down, slopeRayDistance, groundMask);

        if (groundRay)
        {
            if (groundRay.collider.tag == "Ground")
            {
                isGround = true;
            }

            slopePerp = Vector2.Perpendicular(groundRay.normal).normalized;
            slopeAngle = Vector2.Angle(groundRay.normal, Vector2.up);

            if (slopeAngle != 0)
                isSlope = true;
            else
                isSlope = false;

            Debug.DrawLine(groundRay.point, groundRay.point + groundRay.normal, Color.red);
        }
        else
            isGround = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
        {
            canClimb = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
        {
            canClimb = false;
            playerCollider.isTrigger = false;
            playerRigidbody.gravityScale = originGravity;
            isClimbing = false;
        }
    }
}
