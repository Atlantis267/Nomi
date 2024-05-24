using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TechUI : MonoBehaviour
{
    public Animator transAnimeW;
    public Animator transAnimeA;
    public Animator transAnimeS;
    public Animator transAnimeD;
    public GameObject TeachUI;

    private void Start()
    {
        StartCoroutine(PlayAnimation());
    }
    IEnumerator PlayAnimation()
    {
        TeachUI.SetActive(true);
        yield return new WaitForSeconds(2f);
        transAnimeW.SetTrigger("WD");
        yield return new WaitForSeconds(2f);
        transAnimeA.SetTrigger("AD");
        yield return new WaitForSeconds(2f);
        transAnimeS.SetTrigger("SD");
        yield return new WaitForSeconds(2f);
        transAnimeD.SetTrigger("DD");
        yield return new WaitForSeconds(2f);
        TeachUI.SetActive(false);
        this.gameObject.SetActive(false);
    }

}
