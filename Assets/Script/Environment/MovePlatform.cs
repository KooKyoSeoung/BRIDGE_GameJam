using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField, Range(2f, 10f)] float reachTime;
    [SerializeField, Range(0f, 10f)] float stopTime;
    int currentPoint = 0;
    int pointCnt = 0;
    [SerializeField] Vector2[] movePos;
    [SerializeField] float soundValue = 0.3f;
    SFXPlayer sfxPlayer;
    //WaitForSeconds delayTime;

    private void Awake()
    {
        transform.position = movePos[currentPoint];
        pointCnt = movePos.Length;
        sfxPlayer = GetComponent<SFXPlayer>();
    }

    private void Start()
    {
        //delayTime = new WaitForSeconds(stopTime);
        StartCoroutine(MovePlatformCor());
    }
        
    public IEnumerator MovePlatformCor()
    {
        sfxPlayer.PlayAudioClip(0);
        float timer = 0f;
        Vector2 startPos = transform.position;
        currentPoint += 1;
        if (currentPoint >= pointCnt)
            currentPoint = 0;
        Vector2 destPos = movePos[currentPoint];

        while (timer<=reachTime)
        { 
            timer += Time.deltaTime;
            transform.position = Vector2.Lerp(startPos, destPos, timer / reachTime);
            yield return null;
        }
        //yield return delayTime;
        sfxPlayer.PlayAudioClip(2);
        yield return new WaitForSeconds(stopTime);
        StartCoroutine(MovePlatformCor());
    }

    #region ReStart or Stop
    public void StartMove()
    {
        StartCoroutine(MovePlatformCor());
    }

    public void StopMove()
    {
        StopCoroutine(MovePlatformCor());
    }
    #endregion

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.parent = null;
        }
    }
}
