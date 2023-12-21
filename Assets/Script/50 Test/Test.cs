using Movement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("u"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        if (Input.GetKeyDown("i"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );
        }
        if (Input.GetKeyDown("o"))
        {
            SceneManager.LoadScene("Test_UI");
        }
    }
}
