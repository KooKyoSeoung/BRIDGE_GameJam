using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private GameObject _player;
    private Vector2 _startingOffset;
    private float _startingZPos;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        if (_player == null )
            Debug.LogError("Failed to find a gameobject of tag 'Player'");

        _startingOffset = transform.position - _player.transform.position;
        _startingZPos = transform.position.z;
    }

    void Update()
    {
        Vector2 v2Pos = (Vector2)_player.transform.position + _startingOffset;
        transform.position = new Vector3(v2Pos.x, v2Pos.y, _startingZPos);
    }
}
