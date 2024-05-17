using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    private Animator transAnime;
    private Animator spPAnime;
    static public bool isSpacePress;
    private bool isFirstPress = true;
    private float waitTime = 5.0f;
    private float waitJumpTime = 0.03f;
    private float waitAnime = 2.0f;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
        spPAnime = GameObject.Find("SpacePanel").GetComponent<Animator>();
        isSpacePress = false;
    }

    
    void Update()
    {
        if (Shift.isShiftPress && Shift.isW_1Press)
        {
            StartCoroutine(WaitTime());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isSpacePress = true;

            if (isFirstPress)
            {
                StartCoroutine(WaitJumpTime());
                transAnime.SetTrigger("SSp_1");
            }
            else
            {
                Time.timeScale = 1f;
                transAnime.SetTrigger("SpD");
                spPAnime.SetTrigger("SpPD");
                StartCoroutine(WaitAnime());
            }

            isFirstPress = !isFirstPress;
        }
    }
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(waitTime);
        transAnime.SetTrigger("StartSpace");
        spPAnime.SetTrigger("StartSpP");
    }
    IEnumerator WaitJumpTime()
    {
        yield return new WaitForSeconds(waitJumpTime);
        Time.timeScale = 0f;
    }
    IEnumerator WaitAnime()
    {
        yield return new WaitForSeconds(waitAnime);
        gameObject.SetActive(false);
    }
}
