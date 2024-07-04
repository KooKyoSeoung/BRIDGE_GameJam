using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TimeTravelMap : MonoBehaviour
{
    [SerializeField, Tooltip("소품들의 부모")] GameObject propParent;
    [SerializeField, Tooltip("타일맵")] GameObject tilemap;
    SpriteRenderer[] sprs;
    TilemapRenderer tmr;

    private void Awake()
    {
        sprs = propParent.GetComponentsInChildren<SpriteRenderer>();
        tmr = tilemap.GetComponent<TilemapRenderer>();
    }

    private void Start()
    {
        tilemap.SetActive(true);
        TimeTravelManager.Instance.ChangeTimeZoneMap();
    }

    public void ChangeOrderInLayer(int _layer)
    {
        // TileMap
        if (tmr == null)
            tmr = tilemap.GetComponent<TilemapRenderer>();
        tmr.sortingOrder = _layer;

        // Prop
        if (sprs == null)
            sprs = propParent.GetComponentsInChildren<SpriteRenderer>();
        int sprsCnt = sprs.Length;
        for(int idx = 0; idx< sprsCnt; idx++)
        {
            sprs[idx].sortingOrder = _layer;
        }
    }
}
