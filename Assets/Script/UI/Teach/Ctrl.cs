using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl : MonoBehaviour
{
    private Animator transAnime;
    private Animator cPAnime;
    private Animator w_2transAnime;
    private bool isFristPress = true;
    private float waitTime = 5.0f;
    private float waitW_2Time = 0.5f;
    static public bool isCtrlPress;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
        cPAnime = GameObject.Find("CtrlPanel").GetComponent<Animator>();
        isCtrlPress = false;
        w_2transAnime= GameObject.Find("W_2").GetComponent<Animator>();
    }


    void Update()
    {
        if (Space.isSpacePress)
        {
            StartCoroutine(WaitTime());
        }
        /*
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(WaitW_2Time());
        }
        */
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCtrlPress = true;
        }
        if (Input.GetKey(KeyCode.W))
        {
            if (isCtrlPress)
            {
                w_2transAnime.SetTrigger("W_2D");
                if (isFristPress)
                {
                    transAnime.SetTrigger("CF");
                }
                else
                {
                    transAnime.SetTrigger("CD");
                    cPAnime.SetTrigger("CPD");
                }
                isFristPress = !isFristPress;
            }
        }
    }
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(waitTime);
        w_2transAnime.SetTrigger("StartW_2");
        transAnime.SetTrigger("StartCA");
        cPAnime.SetTrigger("StartCP");
    }
    IEnumerator WaitW_2Time()
    {
        yield return new WaitForSeconds(waitW_2Time);
        //w_2transAnime.SetTrigger("W_2D");
    }
}
