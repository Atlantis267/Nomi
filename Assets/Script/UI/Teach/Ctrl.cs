using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl : MonoBehaviour
{
    private Animator transAnime;
    private Animator cPAnime;
    private Animator w_2transAnime;
    private float waitTime = 8.0f;
    private float waitW_2Time = 7.5f;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
        cPAnime = GameObject.Find("CtrlPanel").GetComponent<Animator>();
        w_2transAnime= GameObject.Find("W_2").GetComponent<Animator>();
    }


    void Update()
    {
        if (Space.isSpacePress)
        {
            StartCoroutine(WaitTime());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(WaitW_2Time());
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            transAnime.SetTrigger("CD");
            cPAnime.SetTrigger("CPD");

        }
    }
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(waitTime);
        transAnime.SetTrigger("StartCA");
        cPAnime.SetTrigger("StartCP");
        w_2transAnime.SetTrigger("StartW_2");
    }
    IEnumerator WaitW_2Time()
    {
        yield return new WaitForSeconds(waitW_2Time);
        w_2transAnime.SetTrigger("W_2D");
    }
}
