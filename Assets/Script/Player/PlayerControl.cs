using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private bool isWalkingSoundPlaying = false;
    private bool isLandingSoundPlaying = false;
    private float coyoteCounter;

    private Rigidbody2D playerRigidbody;
    private BoxCollider2D playerCollider;
    private Animator playerAnimator;

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
    public float RopeBottomPos { get; set; }

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerAnimator = GetComponent<Animator>();

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

        if (isGround && playerAnimator.GetBool("isRun"))
        {
            if (!isWalkingSoundPlaying)
            {
                StartCoroutine("PlayWalkSound");
            }
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
            playerAnimator.SetBool("isClimb", true);
            playerRigidbody.gravityScale = 0.0f;
            gameObject.transform.position = new Vector2(RopePos.x, gameObject.transform.position.y);
            float verticalInput = Input.GetAxis("Vertical");

            if (verticalInput == 0.0f)
                playerAnimator.speed = 0.0f;
            else
                playerAnimator.speed = 1.0f;

            if (RopeBottomPos != 0.0f)
            {
                if (transform.position.y <= RopeBottomPos)
                {
                    if (verticalInput < 0)
                        playerRigidbody.velocity = Vector2.zero;
                }
            }

            if (transform.position.y >= RopePos.y)
            {
                if (verticalInput > 0)
                    playerRigidbody.velocity = Vector2.zero;
                else
                    playerRigidbody.velocity = new Vector2(0.0f, verticalInput * climbSpeed);
            }
            else if (RopeBottomPos != 0.0f && transform.position.y <= RopeBottomPos)
            {
                if (verticalInput < 0)
                {
                    playerRigidbody.velocity = Vector2.zero;
                }
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
            playerAnimator.SetBool("isPush", true);
            Vector2 force = new Vector2(1, 0) * (Input.GetAxis("Horizontal") * pushPullForce);
            moveHorizontal = force.x;
            if (force == Vector2.zero)
                playerAnimator.speed = 0.0f;
            else
            {
                playerAnimator.speed = 1.0f;
                if (HeldObject.transform.position.x - transform.position.x > 0 && force.x > 0)
                {
                    playerAnimator.SetBool("isPush", true);
                    playerAnimator.SetBool("isPull", false);
                }
                else if (HeldObject.transform.position.x - transform.position.x < 0 && force.x < 0)
                {
                    playerAnimator.SetBool("isPush", true);
                    playerAnimator.SetBool("isPull", false);
                }
                else
                {
                    playerAnimator.SetBool("isPull", true);
                    playerAnimator.SetBool("isPush", false);
                }
            }
            Vector2 velocityYOnly = new Vector2(0.0f, HeldObject.GetComponent<Rigidbody2D>().velocity.y);
            Vector2 playerVelocityYOnly = new Vector2(0.0f, playerRigidbody.velocity.y);
            HeldObject.GetComponent<Rigidbody2D>().velocity = force + velocityYOnly;
            playerRigidbody.velocity = force + velocityYOnly;
        }

        if (moveHorizontal == 0.0f)
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        else
            playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        #region Animating Move,Jump
        if (Mathf.Abs(playerRigidbody.velocity.x) <= 0.1f)
        {
            playerAnimator.SetBool("isRun", false);
        }
        else
        {
            playerAnimator.SetBool("isRun", true);
        }

        if (Mathf.RoundToInt(playerRigidbody.velocity.normalized.y) == 0)
        {
            playerAnimator.SetBool("isUp", false);
            playerAnimator.SetBool("isDown", false);
        }

        if (!isGround)
        {
            if (playerRigidbody.velocity.normalized.y > 0)
            {
                playerAnimator.SetBool("isUp", true);
                playerAnimator.SetBool("isDown", false);
            }
            else if (playerRigidbody.velocity.normalized.y < 0)
            {
                playerAnimator.SetBool("isUp", false);
                playerAnimator.SetBool("isDown", true);
            }
            else
            {
                playerAnimator.SetBool("isUp", false);
                playerAnimator.SetBool("isDown", false);
            }
        }
        #endregion
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

    public void RopeAnimationEnd()
    {
        playerAnimator.speed = 1.0f;
        playerAnimator.SetBool("isClimb", false);
        playerAnimator.SetBool("isUp", true);
    }

    public void PlayerAnimationEnd()
    {
        playerAnimator.SetBool("isRun", false);
        playerAnimator.SetBool("isUp", false);
        playerAnimator.SetBool("isDown", false);
    }

    public void HeldAnimationEnd()
    {
        playerAnimator.speed = 1.0f;
        playerAnimator.SetBool("isPush", false);
        playerAnimator.SetBool("isPull", false);
    }

    private IEnumerator PlayWalkSound()
    {
        int v = Random.Range(1, 6);
        isWalkingSoundPlaying = true;
        Managers.Sound.PlaySFX("FootStep0" + v.ToString());
        yield return new WaitForSeconds(0.6f);
        isWalkingSoundPlaying = false;
    }

    private void OnPlayerJump()
    {
        if (Input.GetKeyDown(jumpKey) && coyoteCounter > 0 && !IsClimbing)
        {
            playerRigidbody.velocity = Vector2.zero;
            Managers.Sound.PlaySFX("Jump");
            playerRigidbody.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
        }
    }

    private void OnPlayerMove()
    {
        moveHorizontal = Input.GetAxis("Horizontal");

        if (moveHorizontal < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (moveHorizontal > 0)
            transform.localScale = new Vector3(1, 1, 1);

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
        groundRay = Physics2D.RaycastAll(slopeCheckPosition.position, Vector2.down, slopeRayDistance, groundMask)
            .FirstOrDefault(x=>x.collider.isTrigger == false);

        if (groundRay)
        {
            /*if (groundRay.collider.tag == "Ground")
            {
                isGround = true;
            }*/

            if (IsClimbing && playerRigidbody.velocity.y < 0 && RopeBottomPos == 0.0f)
            {
                //IsClimbing = false;
                //playerRigidbody.gravityScale = originGravity;
                GripObject.GetComponent<Interactable>().IsRopeJumped = true;
                GripObject.GetComponent<Interactable>().EndInteraction();
                //playerCollider.isTrigger = false;
            }

            if (!isLandingSoundPlaying)
            {
                isLandingSoundPlaying = true;
                Managers.Sound.PlaySFX("Land");
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
            isLandingSoundPlaying = false;
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
        var grounded = Physics2D.OverlapBoxAll((Vector2) transform.position + groundCheckOffset, groundCheckSize, 0f, LayerMask.GetMask("Ground"))
            .Any(x=>x.isTrigger == false);
        var onHeavyItem = Physics2D.OverlapBoxAll((Vector2) transform.position + groundCheckOffset, groundCheckSize, 0f, LayerMask.GetMask("HeavyItem"))
            .Any(x=>x.isTrigger == false);

        
        if (grounded || onHeavyItem)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            //Managers.Sound.PlaySFX("Land");
        }
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
