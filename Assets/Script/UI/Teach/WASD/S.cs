using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S : MonoBehaviour
{
    private Animator transAnime;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            transAnime.SetTrigger("SD");
        }
    }
}
