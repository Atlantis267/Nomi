using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class VideoController : MonoBehaviour
{
    [SerializeField] VideoPlayer video_1;
    void Start()
    {
        video_1.loopPointReached += PlayVideo_2;
    }
    void PlayVideo_2(VideoPlayer vp) 
    {
        SceneManager.LoadScene("Test_UI");
    }
}
