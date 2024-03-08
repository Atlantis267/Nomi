using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackArrowAnime_1 : MonoBehaviour
{
    private Animator transAnime;
    private float transWaitTime = 2.0f;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
    }
    IEnumerator WaitAnimationToMenu()
    {
        PlayAnimation();
        yield return new WaitForSeconds(transWaitTime);
        SceneManager.LoadScene("Information");
    }
    public void PlayAnimation()
    {
        transAnime.SetTrigger("Start");
    }
    public void BackToInfor()
    {
        StartCoroutine(WaitAnimationToMenu());
    }
}
