using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl : MonoBehaviour
{
    private Animator transAnime;
    private Animator cPAnime;
    private Animator w_2transAnime;
    private bool isFristPress;
    private float waitTime = 5.0f;
    static public bool isCtrlPress;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
        cPAnime = GameObject.Find("CtrlPanel").GetComponent<Animator>();
        isCtrlPress = false;
        w_2transAnime = GameObject.Find("W_2").GetComponent<Animator>();
        isFristPress = false;
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
        if (Input.GetKey(KeyCode.LeftControl))
        {
            isCtrlPress = true;

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

        if (Input.GetKey(KeyCode.W) && isCtrlPress)
        {
            w_2transAnime.SetTrigger("W_2D");
        }
        IEnumerator WaitTime()
        {
            yield return new WaitForSeconds(waitTime);
            w_2transAnime.SetTrigger("StartW_2");
            transAnime.SetTrigger("StartCA");
            cPAnime.SetTrigger("StartCP");
        }
        /*
        IEnumerator WaitCF()
        {           
            yield return new WaitForSeconds(waitCF);
            transAnime.SetTrigger("CF");
        }
        */
    }
}
