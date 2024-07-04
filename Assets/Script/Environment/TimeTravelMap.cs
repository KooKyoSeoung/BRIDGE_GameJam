using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TimeTravelMap : MonoBehaviour
{
    [SerializeField] TimeZoneType currentTime;
    [SerializeField] GameObject tilemap;
    [SerializeField] GameObject prop;

    TilemapRenderer tileRenderer;
    TilemapCollider2D tileCollider;
    SpriteRenderer[] propSprites;
    Collider2D[] propColliders;

    #region Unity Life Cycle
    private void Awake()
    {
        if (tilemap != null)
        {
            tileRenderer = tilemap.GetComponent<TilemapRenderer>();
            tileCollider = tilemap.GetComponent<TilemapCollider2D>();
        }
        if (prop != null)
        {
            propSprites = prop.GetComponentsInChildren<SpriteRenderer>();
            propColliders = prop.GetComponentsInChildren<Collider2D>();
        }
    }

    // 지울지말지 고민
    private void Start()
    {
        TimeTravelManager.Instance.ChangeTimeZoneMap();
    }

    #endregion


    public void ApplyTimeZone()
    {
        if(TimeTravelManager.Instance.CurrentTimeZone== currentTime)
        {
            // Prop
            int propCnt = propSprites.Length;
            for(int idx= 0; idx<propCnt; idx++)
            {
                propSprites[idx].enabled = true;
                propColliders[idx].isTrigger = false;
            }
            // TileMap
            tileRenderer.enabled = true;
            tileCollider.isTrigger = false;
            tileCollider.usedByComposite = true;
        }
        else
        {
            // Prop
            int propCnt = propSprites.Length;
            for (int idx = 0; idx < propCnt; idx++)
            {
                propSprites[idx].enabled = false;
                propColliders[idx].isTrigger = true;
            }
            // TileMap
            tileRenderer.enabled = false;
            tileCollider.usedByComposite = false;
            tileCollider.isTrigger = true;
        }
    }
}
