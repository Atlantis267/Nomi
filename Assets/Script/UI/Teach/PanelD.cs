using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelD : MonoBehaviour
{
    private Animator animator;
    public Animator shiftAnime;
    public Animator panelAnime;
    private float waitToShift = 5.0f;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        //shiftAnime = GameObject.Find("Shift").GetComponent<Animator>();
        //panelAnime = GameObject.Find("ShiftPanel").GetComponent<Animator>();
    }

    void Update()
    {
        if(W.isWpress && A.isApress && S.isSpress && D.isDpress) 
        {
           
        }
    }
    IEnumerator WaitTime()
    {
        animator.SetTrigger("PD");
        yield return new WaitForSeconds(waitToShift);
        //panelAnime.SetTrigger();
        //shiftAnime.SetTrigger();
    }
}
