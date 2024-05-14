using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour
{
    private Animator transAnime;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transAnime.SetTrigger("AD");
        }
    }
}
