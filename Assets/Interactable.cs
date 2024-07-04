using System.Collections;
using System.Collections.Generic;
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
        }

        //아웃라인 이니셜라이즈
        if (_spriteRenderer != null)
            _spriteRenderer.material.SetFloat("_OutlinePixelWidth", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
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
                break;
            case InteractableType.QuickInteraction:
                break;
            case InteractableType.Rope:
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
                break;
            case InteractableType.QuickInteraction:
                break;
            case InteractableType.Rope:
                break;
            default:
                break;
        }
    }
}

public enum InteractableType
{
    LightCarriable,
    HeavyMovable,
    QuickInteraction,
    Rope,
}
