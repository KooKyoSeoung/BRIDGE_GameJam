using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    [SerializeField] bool isSave = false;
    [SerializeField] Vector2 saveVec;
    public Vector2 SaveVec{ get { return saveVec; } }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isSave)
        {
            isSave = true;
            SaveManager.Instance.AddSavePoint(this);
        }
    }
}
