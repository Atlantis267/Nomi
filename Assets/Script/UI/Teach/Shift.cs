using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shift : MonoBehaviour
{
    private Animator transAnime;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            transAnime.SetTrigger("SFD");
        }
    }
}
