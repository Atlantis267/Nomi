using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Shift : MonoBehaviour
{
    private Animator transAnime;
    private Animator w_1transAnime;
    private Animator panelAnime;
    static public bool isShiftPress;
    static public bool isW_1Press;
    private float waitTime = 7.5f;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
        w_1transAnime = GameObject.Find("W_1").GetComponent<Animator>();
        panelAnime = GameObject.Find("ShiftPanel").GetComponent<Animator>();
        isShiftPress = false;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.E) && W.isWpress && A.isApress && S.isSpress && D.isDpress)
        {  
            panelAnime.SetTrigger("StartSP");
            w_1transAnime.SetTrigger("StartW_1");
            transAnime.SetTrigger("StartSA");
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            transAnime.SetTrigger("SFD");
            isShiftPress = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(WaitTime());
            isW_1Press = true;
        }
        if (isShiftPress && isW_1Press)
        {
            panelAnime.SetTrigger("SPD");
        }
    }
    
    IEnumerator WaitTime() 
    {
        yield return new WaitForSeconds(waitTime);
        w_1transAnime.SetTrigger("W_1D");
    }
    
}
