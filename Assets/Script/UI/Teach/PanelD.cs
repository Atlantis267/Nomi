using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelD : MonoBehaviour
{
    private Animator animator;
    //[SerializeField] private GameObject Shift;
    //[SerializeField] private GameObject ShiftPanel;
    //private float waitToShift = 5.0f;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();    }

    void Update()
    {
        if(W.isWpress && A.isApress && S.isSpress && D.isDpress) 
        {
            animator.SetTrigger("PD");
        }
    }
    /*
    IEnumerator WaitTime()
    {
        animator.SetTrigger("PD");
        yield return new WaitForSeconds(waitToShift);
        panelAnime.SetTrigger("StartSP");
        shiftAnime.SetTrigger("StartSA");
        Shift.SetActive(true);
    }
    */
}
