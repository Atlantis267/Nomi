using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] private Animator flowerAnime;
    [SerializeField] private Animator transAnime;
    [SerializeField] private GameObject Information;
    private float transWaitTime = 1.0f;
    private float transWaitTime2 = 1.8f;
    void Awake()
    {
        //flowerAnime = GetComponentInChildren<Animator>();
        //transAnime = gameObject.GetComponent<Animator>();
    }
    IEnumerator WaitAnimation() 
    {
        PlayFlowerAnimation();
        yield return new WaitForSeconds(transWaitTime);
        SceneManager.LoadScene("Loading");
    }
    public void PlayFlowerAnimation() 
    {
        flowerAnime.SetTrigger("Start");
    }
    public void NewGameButtonClick() 
    {
        StartCoroutine(WaitAnimation());
    }
    IEnumerator WaitTransfromAnimation()
    {
        PlayAnimation();
        yield return new WaitForSeconds(transWaitTime2);
        SceneManager.LoadScene("Information");
    }
    public void PlayAnimation()
    {
        transAnime.SetTrigger("Start");
    }
    public void SettingClick()
    {
        StartCoroutine(WaitTransfromAnimation());
    }
    public void QuitClick()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
