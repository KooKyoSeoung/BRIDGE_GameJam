using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ColliderPlayer : MonoBehaviour
{
    [SerializeField] bool canTimeTravel = true;
    [SerializeField] bool isPast = false;
    [SerializeField] float moveSpeed = 3;
    SpriteRenderer spr;

    Rigidbody2D rb;
    [SerializeField] CircleCollider2D playerColl;



    [SerializeField] TilemapCollider2D pastTileMapCollider;
    [SerializeField] TilemapCollider2D presentTileMapCollider;

    bool isWarning = false;
    [SerializeField] GameObject warnTextObject;

    // 가져가야 하는 코드
    #region Set TileMap State
    /// <summary>
    /// On / Off : Current Time Zone
    /// </summary>
    public void ChangeTileMapState()
    {
        if (TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Past)
        {
            OnTileMap(pastTileMapCollider);
            OffTileMap(presentTileMapCollider);
        }
        else if (TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Present)
        {
            OnTileMap(presentTileMapCollider);
            OffTileMap(pastTileMapCollider);
        }
    }

    public void OnTileMap(TilemapCollider2D _tileCollider)
    {
        _tileCollider.isTrigger = false;
        _tileCollider.usedByComposite = true;
        _tileCollider.GetComponent<TilemapRenderer>().enabled = true;
    }

    public void OffTileMap(TilemapCollider2D _tileCollider)
    {
        _tileCollider.usedByComposite = false;
        _tileCollider.isTrigger = true;
        _tileCollider.GetComponent<TilemapRenderer>().enabled = false;
    }
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

  

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(moveSpeed * horizontal, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (canTimeTravel)
                TimeChange();
            else
                WarnTimeTravel();
        }
    }

    public void TimeChange()
    {
        isPast = !isPast;
        if (isPast)
        {
            OnTileMap(pastTileMapCollider);
            OffTileMap(presentTileMapCollider);
        }
        else
        {
            OnTileMap(presentTileMapCollider);
            OffTileMap(pastTileMapCollider);
        }
    }



    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            canTimeTravel = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            canTimeTravel = true;
        }
    }

    public void WarnTimeTravel()
    {
        if (isWarning)
            return;
        StartCoroutine(WarnTextCor());
    }

    public IEnumerator WarnTextCor()
    {
        isWarning = true;
        warnTextObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        warnTextObject.SetActive(false);
        isWarning = false;
    }
}
