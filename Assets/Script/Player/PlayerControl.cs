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
    [SerializeField] private Vector2 groundCheckOffset;
    [SerializeField] private Vector2 groundCheckSize;
    [SerializeField] private float coyoteTime = .15f;
    private float moveHorizontal = 0.0f;
    private float originGravity;
    private bool isGround = true;
    private float coyoteCounter;

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

    private float pushPullForce = 2.0f;
    public GameObject HeldObject { get; set; }
    public GameObject GripObject { get; set; }
    public float PlayerStandPosY { get { return groundRay.point.y; } }
    public bool IsSubscribing { get; private set; }
    public bool IsClimbing { get; set; }
    public Vector2 RopePos { get; set; }

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

        PlayerSubscribe();
    }

    void Update()
    {
        GroundedCheck();
        ProcessCoyoteTime();
        SlopeCheck();

        if (InputManager.isNeedInit)
        {
            InputManager.isNeedInit = false;
            //moveHorizontal = 0.0f;
        }

        /*
        if (canClimb)
        {
            if (Input.GetKeyDown(interactionKey))
                isClimbing = true;
        }*/

        #region Climb
        if (IsClimbing)
        {
            playerRigidbody.gravityScale = 0.0f;
            gameObject.transform.position = new Vector2(RopePos.x, gameObject.transform.position.y);
            float verticalInput = Input.GetAxis("Vertical");

            if (transform.position.y >= RopePos.y)
            {
                if (verticalInput > 0)
                    playerRigidbody.velocity = Vector2.zero;
                else
                    playerRigidbody.velocity = new Vector2(0.0f, verticalInput * climbSpeed);
            }
            else
                playerRigidbody.velocity = new Vector2(0.0f, verticalInput * climbSpeed);

            if (Input.GetKeyDown(moveLeftKey))
                ClimbJump(-1);

            if (Input.GetKeyDown(moveRightKey))
                ClimbJump(1);
        }
        #endregion

        if (HeldObject != null)
        {
            Vector2 force = new Vector2(1, 0) * (Input.GetAxis("Horizontal") * pushPullForce);
            Vector2 velocityYOnly = new Vector2(0.0f, HeldObject.GetComponent<Rigidbody2D>().velocity.y);
            HeldObject.GetComponent<Rigidbody2D>().velocity = force + velocityYOnly;
            playerRigidbody.velocity = force;
        }

        if (moveHorizontal == 0.0f)
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void OnDisable()
    {
        PlayerUnSubscribe();
    }

    #region PlayerActionActive
    public void PlayerSubscribe()
    {
        IsSubscribing = true;
        Managers.Input.keyAction += OnPlayerMove;
        Managers.Input.keyAction += OnPlayerJump;
    }

    public void PlayerUnSubscribe()
    {
        IsSubscribing = false;
        Managers.Input.keyAction -= OnPlayerMove;
        Managers.Input.keyAction -= OnPlayerJump;
    }
    #endregion

    private void OnPlayerJump()
    {
        if (Input.GetKeyDown(jumpKey) && coyoteCounter > 0 && !IsClimbing)
        {
            playerRigidbody.velocity = Vector2.zero;
            playerRigidbody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }
    }

    private void OnPlayerMove()
    {
        moveHorizontal = Input.GetAxis("Horizontal");

        if (moveHorizontal < 0)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            transform.rotation = Quaternion.Euler(0, 0, 0);

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
            /*if (groundRay.collider.tag == "Ground")
            {
                isGround = true;
            }*/

            if (IsClimbing && playerRigidbody.velocity.y < 0)
            {
                IsClimbing = false;
                playerRigidbody.gravityScale = originGravity;
                //playerCollider.isTrigger = false;
            }

            slopePerp = Vector2.Perpendicular(groundRay.normal).normalized;
            slopeAngle = Vector2.Angle(groundRay.normal, Vector2.up);

            if (slopeAngle != 0)
                isSlope = true;
            else
                isSlope = false;

            Debug.DrawLine(groundRay.point, groundRay.point + groundRay.normal, Color.red);
        }
        /*else
            isGround = false;*/
    }

    private void ClimbJump(int _direction)
    {
        GripObject.GetComponent<Interactable>().IsRopeJumped = true;
        GripObject.GetComponent<Interactable>().EndInteraction();
        moveHorizontal = _direction;
        playerRigidbody.AddForce(new Vector2(moveHorizontal * 15, 10.0f), ForceMode2D.Impulse);
    }

    private void GroundedCheck()
    {
        var ground = Physics2D.OverlapBox((Vector2) transform.position + groundCheckOffset, groundCheckSize, 0f, LayerMask.GetMask("Ground"));
        var heavyItem = Physics2D.OverlapBox((Vector2) transform.position + groundCheckOffset, groundCheckSize, 0f, LayerMask.GetMask("HeavyItem"));

        if (ground != null || heavyItem != null)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
    }

    private void ProcessCoyoteTime()
    {
        if (isGround) coyoteCounter = coyoteTime;
        else coyoteCounter -= Time.deltaTime;
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
        {
            canClimb = true;
            climbCenterX = collision.transform.position.x;
            boxSizeY = collision.transform.GetChild(0).position.y;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Rope"))
        {
            canClimb = false;
            playerRigidbody.gravityScale = originGravity;
            isClimbing = false;
        }
    }*/

    private void OnDrawGizmosSelected()
    {
        //Draw gizmos for ground check area
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube((Vector2)transform.position + groundCheckOffset, groundCheckSize);
    }
}
