using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene_4 : MonoBehaviour
{
    public void Credit()
    {
        SceneManager.LoadScene("Credit");
    }
    public void BackToInfor() 
    {
        SceneManager.LoadScene("Information");
    }
}
