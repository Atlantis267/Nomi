using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelD : MonoBehaviour
{
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.S) && Input.GetKeyDown(KeyCode.D)) 
        {
            animator.SetTrigger("PD");
        }
    }
}
