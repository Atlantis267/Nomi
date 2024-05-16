using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ctrl : MonoBehaviour
{
    private Animator transAnime;
    private Animator cPAnime;
    private float waitTime = 8.0f;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
        cPAnime = GameObject.Find("CtrlPanel").GetComponent<Animator>();
    }


    void Update()
    {
        if (Space.isSpacePress)
        {
            StartCoroutine(WaitTime());
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
    }
}
