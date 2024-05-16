using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    private Animator transAnime;
    private Animator spPAnime;
    static public bool isSpacePress;
    private bool isFirstPress = true;
    private float waitTime = 8.0f;
    private float waitJumpTime = 0.5f;
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
}
