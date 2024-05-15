using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W : MonoBehaviour
{
    private Animator transAnime;
    static public bool isWpress;
    void Start()
    {
        transAnime = gameObject.GetComponent<Animator>();
        isWpress = false;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transAnime.SetTrigger("WD");
            isWpress = true;
        }
    }
}
