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
            tilemap.AddComponent<TimeTravelColliderChecker>();
        }
        if (prop != null)
        {
            propSprites = prop.GetComponentsInChildren<SpriteRenderer>();
            propColliders = prop.GetComponentsInChildren<Collider2D>();
        }
    }
    #endregion

    public void ApplyAllTimeZone()
    {
        if(TimeTravelManager.Instance.CurrentTimeZone== currentTime)
        {
            // Prop
            //Sprite
            int sprCnt = propSprites.Length;
            for(int sprIdx= 0; sprIdx < sprCnt; sprIdx++)
            {
                propSprites[sprIdx].enabled = true;
            }
            //Collider
            int collCnt = propColliders.Length;
            for (int collIdx = 0; collIdx < collCnt; collIdx++)
            { 
                propColliders[collIdx].isTrigger = false;
            }

            // TileMap
            tileRenderer.enabled = true;
            tileCollider.isTrigger = false;
            tileCollider.usedByComposite = true;
        }
        else
        {
            //Prop
            //Sprite
            int sprCnt = propSprites.Length;
            for (int sprIdx = 0; sprIdx < sprCnt; sprIdx++)
            {
                propSprites[sprIdx].enabled = false;
            }
            //Collider
            int collCnt = propColliders.Length;
            for (int collIdx = 0; collIdx < collCnt; collIdx++)
            {
                propColliders[collIdx].isTrigger = true;
            }

            // TileMap
            tileRenderer.enabled = false;
            tileCollider.usedByComposite = false;
            tileCollider.isTrigger = true;
        }
    }
}
