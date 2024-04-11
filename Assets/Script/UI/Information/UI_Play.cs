using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Play : MonoBehaviour
{
    public void Play() 
    {
        SceneManager.LoadScene("level001");
    }
}
