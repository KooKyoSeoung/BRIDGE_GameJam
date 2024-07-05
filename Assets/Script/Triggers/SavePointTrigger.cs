using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePointTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SaveManager.Instance.SaveData(transform.position, TimeTravelManager.Instance.CurrentTimeZone);
            gameObject.SetActive(false);
        }
    }
}
