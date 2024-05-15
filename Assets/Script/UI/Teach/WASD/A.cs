using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour
{
    private Animator transAnime;
    static public bool isApress;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
        isApress = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transAnime.SetTrigger("AD");
            isApress = true;
        }
    }
}
