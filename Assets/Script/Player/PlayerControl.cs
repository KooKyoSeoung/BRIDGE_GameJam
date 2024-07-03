using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private KeyCode moveLeftKey;
    private KeyCode moveRightKey;
    private KeyCode jumpKey;

    [SerializeField] private float playerSpeed = 6.0f;
    [SerializeField] private float jumpPower;
    private float moveHorizontal = 0.0f;
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

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();

        moveLeftKey = KeyCode.LeftArrow;
        moveRightKey = KeyCode.RightArrow;
        jumpKey = KeyCode.UpArrow;

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
        moveHorizontal = 0.0f;

        if (Input.GetKey(moveLeftKey))
        {
            moveHorizontal = -1.0f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
            PlayerMovement();
        }

        if (Input.GetKey(moveRightKey))
        {
            moveHorizontal = 1.0f;
            transform.rotation = Quaternion.Euler(0, 180, 0);
            PlayerMovement();
        }
    }

    private void PlayerMovement()
    {
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
}
