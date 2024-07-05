using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointTrigger : MonoBehaviour
{
    [SerializeField] Vector2 saveVec;
    [SerializeField] TimeZoneType currentTimeZone;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveManager.Instance.SaveData(saveVec, currentTimeZone);
            Destroy(gameObject);
        }
    }
}
