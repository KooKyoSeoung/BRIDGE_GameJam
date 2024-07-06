using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DinoMeetSFXTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D()
    {
        Managers.Sound.PlaySFX("DinoMeet", true);
        gameObject.SetActive(false);
    }
}
