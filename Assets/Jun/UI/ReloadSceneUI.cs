using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadSceneUI : MonoBehaviour
{
    [SerializeField] float timer = 0f;
    private void Awake()
    {
        StartCoroutine(TimerCor());
    }

    public IEnumerator TimerCor()
    {

        yield return new WaitForSeconds(timer);
        SceneManager.LoadScene(1);
    }
}
