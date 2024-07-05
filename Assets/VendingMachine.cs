using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private AudioClip breakSfx;
    public AudioClip dragSfx;
    [SerializeField] GameObject cokeItemPrefab;
    [SerializeField] private float breakableHeight = 3f;

    private bool _isBroken;
    private float _startingHeight;

    private void Start()
    {
        _startingHeight = transform.position.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            var fallDistance = Mathf.Abs(transform.position.y - _startingHeight);
            if (fallDistance >= breakableHeight)
            {
                Break();
            }
        }
    }

    private void Break()
    {
        if (_isBroken) return;
        _isBroken = true;

        //Change Sprite
        GetComponent<SpriteRenderer>().sprite = brokenSprite;

        //Turn uninteractable
        GetComponent<Interactable>().enabled = false;

        //Remove collider
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
        GetComponent<BoxCollider2D>().enabled = false;

        //Instantiate coke
        var cokeGO = Instantiate(cokeItemPrefab, transform.position, Quaternion.identity);

        //Coke item animation.
        cokeGO.transform.DOJump((Vector2) transform.position + Vector2.left * 2f, 5f, 1, .6f);
    }
}
