using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_1 : MonoBehaviour
{
    private float LoadingWaitTime = 6.0f;

    private void Start()
    {
        StartCoroutine(ChangeToMain());
    }
    IEnumerator ChangeToMain()
    {
        yield return new WaitForSeconds(LoadingWaitTime);
        SceneManager.LoadScene("level001");
    }
}
