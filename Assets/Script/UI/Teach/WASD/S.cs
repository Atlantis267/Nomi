using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S : MonoBehaviour
{
    private Animator transAnime;
    static public bool isSpress;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
        isSpress = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            transAnime.SetTrigger("SD");
            isSpress = true;
        }
    }
}
