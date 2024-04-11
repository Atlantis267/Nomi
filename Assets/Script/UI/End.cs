using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class End : MonoBehaviour
{
    private Animator victoryTrans;
    private Animator endPanelTrans;
    private Animator leftTreeTrans;
    private Animator rightTreeTrans;
    private Animator grassTrans;
    [SerializeField] private bool checking;
    public CinemachineInputProvider cinemachineInput;
    void Start()
    {
        
        victoryTrans = GameObject.Find("Victory").GetComponent<Animator>();
        endPanelTrans = GameObject.Find("EndPanel").GetComponent<Animator>();
        leftTreeTrans = GameObject.Find("LeftTree").GetComponent<Animator>();
        rightTreeTrans = GameObject.Find("RightTree").GetComponent<Animator>();
        grassTrans = GameObject.Find("Grass").GetComponent<Animator>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            victoryTrans.SetTrigger("Start");
            endPanelTrans.SetTrigger("Start");
            leftTreeTrans.SetTrigger("Start");
            rightTreeTrans.SetTrigger("Start");
            grassTrans.SetTrigger("Start");
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            cinemachineInput.enabled = false;
        }
    }
    /*
    private void Updat()
    {
        if (checking)
        {
            victoryTrans.SetTrigger("Start");
            endPanelTrans.SetTrigger("Start");
            leftTreeTrans.SetTrigger("Start");
            rightTreeTrans.SetTrigger("Start");
            grassTrans.SetTrigger("Start");
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            cinemachineInput.enabled = false;
        }
    }
    */
}
