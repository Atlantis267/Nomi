using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour
{
    private Animator transAnime;
    private Animator spPAnime;
    private float waitTime = 8.0f;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
        spPAnime = GameObject.Find("SpacePanel").GetComponent<Animator>();
    }

    
    void Update()
    {
        if (Shift.isShiftPress && Shift.isW_1Press)
        {
            StartCoroutine(WaitTime());
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 0f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Time.timeScale = 1f;
                transAnime.SetTrigger("SpD");
                spPAnime.SetTrigger("SpPD");
            }
        }
    }
    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(waitTime);
        transAnime.SetTrigger("StartSpace");
        spPAnime.SetTrigger("StartSpP");
    }
}
