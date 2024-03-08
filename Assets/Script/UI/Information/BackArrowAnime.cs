using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackArrowAnime : MonoBehaviour
{
    private Animator transAnime;
    private float transWaitTime = 1.5f;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
    }
    IEnumerator WaitAnimationToMenu()
    {
        PlayAnimation();
        yield return new WaitForSeconds(transWaitTime);
        SceneManager.LoadScene("Test_UI");
    }
    public void PlayAnimation()
    {
        transAnime.SetTrigger("Start");
    }
    public void BackToMenu()
    {
        StartCoroutine(WaitAnimationToMenu());
    }
}
