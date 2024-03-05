using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_2 : MonoBehaviour
{
    private Animator transAnime;
    private float transWaitTime = 1.8f;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
    }
    IEnumerator WaitAnimation()
    {
        PlayAnimation();
        yield return new WaitForSeconds(transWaitTime);
        SceneManager.LoadScene("Test_UI");
    }
    public void PlayAnimation()
    {
        transAnime.SetTrigger("Start");
    }
    public void Menu()
    {
        Time.timeScale = 1f;
        StartCoroutine(WaitAnimation());
    }
}
