using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(TimeTravelItem))]
public class Interactable : MonoBehaviour
{
    public InteractableType interactableType;
    private Rigidbody2D _rb2d;
    private TimeTravelItem _timeTravelItem;
    private Transform _originalParent;
    private SpriteRenderer _spriteRenderer;

    private GameObject player;
    private PlayerControl playerControl;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float rayDistance = 5.0f;
    private RaycastHit2D hit;
    private float _originalGravity;

    public bool IsHeavyItemDrop { get; private set; }
    public bool IsRopeJumped { get; set; }

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _timeTravelItem = GetComponent<TimeTravelItem>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (interactableType == InteractableType.HeavyMovable)
        {
            _rb2d.bodyType = RigidbodyType2D.Static;
            _rb2d.mass = 1.0f;
            _rb2d.drag = 0.0f;
            _rb2d.gravityScale = 5.0f;
            player = GameObject.Find("Player");
            playerControl = player.GetComponent<PlayerControl>();
        }
        else if(interactableType == InteractableType.Rope)
        {
            player = GameObject.Find("Player");
            playerControl = player.GetComponent<PlayerControl>();
            IsRopeJumped = false;
        }

        //아웃라인 이니셜라이즈
        if (_spriteRenderer != null)
            _spriteRenderer.material.SetFloat("_OutlinePixelWidth", 0);
    }

    void Update()
    {
        if (interactableType == InteractableType.HeavyMovable)
        {
            hit = Physics2D.RaycastAll(transform.position, Vector2.down, rayDistance, groundMask)
                .FirstOrDefault(x => x.collider.isTrigger == false);
            
            //Debug.DrawLine(hit.point, hit.point + hit.normal, Color.red);

            if (Vector3.Distance(player.transform.position, transform.position) > 2.0f && !IsHeavyItemDrop)
            {
                HeavyInteractionEnd();
                IsHeavyItemDrop = true;
            }

            if (IsHeavyItemDrop && hit)
                _rb2d.bodyType = RigidbodyType2D.Static;
        }
    }

    public void StartInteraction()
    {
        switch (interactableType)
        {
            case InteractableType.LightCarriable:
                _rb2d.bodyType = RigidbodyType2D.Kinematic;
                _originalParent = transform.parent;
                transform.SetParent(FindObjectOfType<PlayerControl>().transform);
                transform.localPosition = Vector3.zero;
                break;
            case InteractableType.HeavyMovable:
                print("START!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                if (Mathf.Abs(hit.point.y - playerControl.PlayerStandPosY) < 0.2f)
                {
                    IsHeavyItemDrop = false;
                    _rb2d.bodyType = RigidbodyType2D.Dynamic;
                    playerControl.HeldObject = this.gameObject;
                    playerControl.PlayerAnimationEnd();
                    if (playerControl.IsSubscribing)
                        playerControl.PlayerUnSubscribe();
                }
                break;
            case InteractableType.QuickInteraction:
                break;
            case InteractableType.Rope:
                IsRopeJumped = false;
                playerControl.IsClimbing = true;
                _originalGravity = playerControl.GetComponent<Rigidbody2D>().gravityScale;
                playerControl.GetComponent<Rigidbody2D>().gravityScale = 0.0f;
                playerControl.GripObject = this.gameObject;
                playerControl.RopePos = new Vector2(transform.position.x, transform.GetChild(0).position.y);
                playerControl.PlayerAnimationEnd();
                if (transform.childCount > 1)
                    playerControl.RopeBottomPos = transform.GetChild(1).position.y;
                break;
            default:
                break;
        }
    }

    public void EndInteraction()
    {
        switch (interactableType)
        {
            case InteractableType.LightCarriable:
                _rb2d.bodyType = RigidbodyType2D.Dynamic;
                transform.SetParent(_originalParent);
                break;
            case InteractableType.HeavyMovable:
                print("END????????????????????????????");
                HeavyInteractionEnd();
                _rb2d.bodyType = RigidbodyType2D.Static;
                break;
            case InteractableType.QuickInteraction:
                break;
            case InteractableType.Rope:
                RopeInteractionEnd();
                break;
            default:
                break;
        }
    }

    public void HeavyInteractionEnd()
    {
        //_rb2d.bodyType = RigidbodyType2D.Static;
        playerControl.HeldAnimationEnd();
        playerControl.HeldObject = null;
        if (!playerControl.IsSubscribing)
            playerControl.PlayerSubscribe();
    }

    public void RopeInteractionEnd()
    {
        playerControl.RopeAnimationEnd();
        playerControl.IsClimbing = false;
        playerControl.GetComponent<Rigidbody2D>().gravityScale = _originalGravity;
        playerControl.GripObject = null;
        playerControl.RopeBottomPos = 0.0f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, (Vector2) transform.position + Vector2.down * rayDistance);
    }
}

public enum InteractableType
{
    LightCarriable,
    HeavyMovable,
    QuickInteraction,
    Rope,
}
