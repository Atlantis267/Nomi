using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_Audio : MonoBehaviour
{
    public float Timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        Game_manager.instance.audioManager.Play(0, "Bgm", true);
        Debug.Log("ho");
    }

    // Update is called once per frame
    void Update()
    {
        Timer += Time.deltaTime;
        if (Timer > 20)
        {
            Game_manager.instance.audioManager.Play(1, "seWind", false);
            Timer = 0;
        }

    }
}
