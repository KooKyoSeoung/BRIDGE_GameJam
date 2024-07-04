using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brakio : MonoBehaviour
{
    [SerializeField] private Transform _colliderHeadUp;
    [SerializeField] private Transform _colliderHeadDown;

    [SerializeField] private Vector2 _pushColliderSize;
    [SerializeField] private Vector2 _pushColliderOffset;

    private void Start()
    {
        _colliderHeadUp.gameObject.SetActive(true);
        _colliderHeadDown.gameObject.SetActive(false);
    }

    public void GiveCoke()
    {
        GetComponent<Animator>().SetTrigger("GiveCoke");
    }

    private IEnumerator PushCollider()
    {
        const float PUSH_TIME = 1f;
        var box = _colliderHeadUp.GetComponent<BoxCollider2D>();
        var counter = 0f;
        var sizeDelta = (_pushColliderSize - box.size) / PUSH_TIME * Time.deltaTime;
        var offsetDelta = (_pushColliderOffset - box.offset) / PUSH_TIME * Time.deltaTime;

        while (counter < PUSH_TIME)
        {
            yield return null;
            counter += Time.deltaTime;
            box.size += sizeDelta;
            box.offset += offsetDelta;
        }

        box.size = _pushColliderSize;
        box.offset = _pushColliderOffset;
    }

    //끼임 방지를 위해 콜라가 놓인 시점부터 콜라이더를 키우면서 플레이어를 밀기.
    public void AnimEvent_PushCollider()
    {
        StartCoroutine(PushCollider());
    }

    public void AnimEvent_StartHeadDown()
    {
        _colliderHeadUp.gameObject.SetActive(false);
        _colliderHeadDown.gameObject.SetActive(true);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube((Vector2)transform.position + _pushColliderOffset, _pushColliderSize * transform.localScale);
    }
}
