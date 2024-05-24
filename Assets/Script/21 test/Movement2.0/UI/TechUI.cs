using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TechUI : MonoBehaviour
{
    public Animator transAnime;

    private void Start()
    {
        StartCoroutine(PlayAnimation());
    }
    IEnumerator PlayAnimation()
    {
        transAnime.SetTrigger("TeachUI");
        yield return new WaitForSeconds(3.5f);
        transAnime.SetTrigger("WASD");
        yield return new WaitForSeconds(2.5f);
        transAnime.SetTrigger("Space");
    }

}
