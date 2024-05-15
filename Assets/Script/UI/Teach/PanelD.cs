using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelD : MonoBehaviour
{
    private Animator animator;
    private Animator shiftAnime;
    private Animator panelAnime;
    [SerializeField] private GameObject Shift;
    [SerializeField] private GameObject ShiftPanel;
    private float waitToShift = 5.0f;
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        shiftAnime = GameObject.Find("Shift").GetComponent<Animator>();
        panelAnime = GameObject.Find("ShiftPanel").GetComponent<Animator>();
        Shift.SetActive(false);
        ShiftPanel.SetActive(false);
    }

    void Update()
    {
        if(W.isWpress && A.isApress && S.isSpress && D.isDpress) 
        {
            StartCoroutine(WaitTime());
        }
    }
    IEnumerator WaitTime()
    {
        animator.SetTrigger("PD");
        yield return new WaitForSeconds(waitToShift);
        Shift.SetActive(true);
        ShiftPanel.SetActive(true);
        panelAnime.SetTrigger("StartSP");
        shiftAnime.SetTrigger("StartSA");
    }
}
