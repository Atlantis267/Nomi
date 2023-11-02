using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_1 : MonoBehaviour
{
    private float LoadingWiatTime = 3.0f;

    private void Start()
    {

    }
    IEnumerator ChangeToMain()
    {

        yield return new WaitForSeconds(LoadingWiatTime);
        SceneManager.LoadScene("TestScene43");
    }
}
