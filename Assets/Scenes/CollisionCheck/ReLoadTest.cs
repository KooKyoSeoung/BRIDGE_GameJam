using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReLoadTest : MonoBehaviour
{
    [SerializeField] SavePointData savePointData;
    Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        LoadData();
    }

    public void InitData()
    {
        savePointData.Init();
        LoadData();
    }

    public void LoadData()
    {
        transform.position = savePointData.savePoint;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Scene currentScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(currentScene.name);
        }

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(h, v);
    }
}
