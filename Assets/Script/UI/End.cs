using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class End : MonoBehaviour
{
    private Animator victoryTrans;
    public CinemachineInputProvider cinemachineInput;
    void Start()
    {
        victoryTrans = gameObject.GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            victoryTrans.SetTrigger("Start");
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            cinemachineInput.enabled = false;
        }
    }
}
