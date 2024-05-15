using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class D : MonoBehaviour
{
    private Animator transAnime;
    static public bool isDpress;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
        isDpress = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            transAnime.SetTrigger("DD");
            isDpress = true;
        }
    }
}
