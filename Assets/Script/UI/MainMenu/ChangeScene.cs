using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeScene : MonoBehaviour
{
    private Animator flowerAnime;
    private float transWaitTime = 1.0f;
    void Start()
    {
        flowerAnime = gameObject.GetComponent<Animator>();
    }
    IEnumerator WaitAnimation() 
    {
        PlayAnimation();
        yield return new WaitForSeconds(transWaitTime);
        SceneManager.LoadScene("Loading");
    }
    public void PlayAnimation() 
    {
        flowerAnime.SetTrigger("Start");
    }
    public void ButtonClick() 
    {
        StartCoroutine(WaitAnimation());
    }
    void Update()
    {
      
    }
}
