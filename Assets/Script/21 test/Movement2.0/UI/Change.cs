using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            SceneManager.LoadSceneAsync("Test_UI", LoadSceneMode.Single);
        }
        if (Input.GetKeyDown(KeyCode.F2))
        {
            SceneManager.LoadSceneAsync("MainMeau", LoadSceneMode.Single);
        }
    }
}
