using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrakioGiveCokeTrigger : MonoBehaviour
{
    private bool _isPlayerReadyCoke = false;
    PlayerTriggerInputController playerTrigger;

    private void Start()
    {
        playerTrigger = FindObjectOfType<PlayerTriggerInputController>();
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (playerTrigger.currentInteractingObject != null && playerTrigger.currentInteractingObject.tag == "Coke" && TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Past)
            {
                _isPlayerReadyCoke = true;
                playerTrigger.canUseInteractionKey = false;
                //F키 인디케이터 및 텍스트 알림 표시 (매프레임 불림 유의)
                UIController.Instance.indicatorTrigger.OnOffIndicator(true, playerTrigger.transform);

            }
            else
            {
                _isPlayerReadyCoke = false;
                playerTrigger.canUseInteractionKey = true;
                //F키 인디케이터 및 텍스트 알림 표시 (매프레임 불림 유의)
                UIController.Instance.indicatorTrigger.OnOffIndicator(false, playerTrigger.transform);
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (_isPlayerReadyCoke)
            {
                _isPlayerReadyCoke = false;
                
                //콜라 아이템 삭제
                playerTrigger.currentInteractingObject.EndInteraction();
                playerTrigger.currentInteractingObject.gameObject.SetActive(false);
                Destroy(playerTrigger.currentInteractingObject.gameObject);
                playerTrigger.currentInteractingObject = null;
                
                //콜라를 브라키오에게 주는 애니메이션 실행
                FindObjectOfType<Brakio>().GetComponent<Animator>().SetTrigger("GiveCoke");
                playerTrigger.canUseInteractionKey = true;
                
                UIController.Instance.indicatorTrigger.OnOffIndicator(false, playerTrigger.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (playerTrigger.currentInteractingObject != null && playerTrigger.currentInteractingObject.tag == "Coke")
            {
                //F키 인디케이터 및 텍스트 알림 숨기기
                UIController.Instance.indicatorTrigger.OnOffIndicator(false, playerTrigger.transform);
                _isPlayerReadyCoke = false;
                playerTrigger.canUseInteractionKey = true;
            }
        }
    }
}
