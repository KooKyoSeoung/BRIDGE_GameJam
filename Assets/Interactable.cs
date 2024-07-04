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

    private GameObject player;

    private void Awake()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _timeTravelItem = GetComponent<TimeTravelItem>();
    }

    void Start()
    {
        if (interactableType == InteractableType.HeavyMovable)
        {
            _rb2d.bodyType = RigidbodyType2D.Static;
            //player = GameObject.FindGameObjectsWithTag("Player");
        }
    }

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
                _rb2d.bodyType = RigidbodyType2D.Dynamic;
                
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
