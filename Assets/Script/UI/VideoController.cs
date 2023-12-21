using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    [SerializeField] VideoPlayer video_1;
    // Start is called before the first frame update
    void Start()
    {
        video_1.loopPointReached += PlayVideo_2;
    }
    void PlayVideo_2(VideoPlayer vp) 
    {
        SceneManager.LoadScene("Test_UI");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
