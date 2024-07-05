using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTreeTrigger : MonoBehaviour
{
    private bool _isPlayerReadyTree = false;
    PlayerTriggerInputController playerTrigger;
    private bool _didWarnEnding = false;

    private void Start()
    {
        playerTrigger = FindObjectOfType<PlayerTriggerInputController>();
    }


    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (playerTrigger.currentInteractingObject != null && playerTrigger.currentInteractingObject.tag == "SpecialTree")
            {
                _isPlayerReadyTree = true;
                playerTrigger.canUseInteractionKey = false;
                //F키 인디케이터 및 텍스트 알림 표시 (매프레임 불림 유의)
                UIController.Instance.indicatorTrigger.OnOffIndicator(true, playerTrigger.transform);
            }
            else
            {
                _isPlayerReadyTree = false;
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
            if (_isPlayerReadyTree)
            {
                _isPlayerReadyTree = false;
                
                //시간대 체크하고 현대 시대라면 로그 하나 추가로 띄운뒤 진행.
                if (TimeTravelManager.Instance.CurrentTimeZone == TimeZoneType.Present)
                {
                    print("진엔딩");
                }
                else if (_didWarnEnding == false)
                {
                    print("가짜 엔딩 경고!");
                }
                else
                {
                    print("가짜 엔딩!");
                }
                
                //나무 아이템 삭제
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
                _isPlayerReadyTree = false;
                playerTrigger.canUseInteractionKey = true;
            }
        }
    }
}
