using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip Bgm;
    public AudioClip seWalk;
    public AudioClip seRun;
    public AudioClip seWater;

    List<AudioSource> audios = new List<AudioSource>();
    void Start()
    {
        for (int i = 0;i<3;i++)
        {
            var audio =this.gameObject.AddComponent<AudioSource>();
            audios.Add(audio);
        }
    }

    void Play(int index,string name,bool isLoop)
    {
        var clip = GetAudioClip(name);
        if (clip !=null)
        {
            var audio = audios[index];
            audio.clip = clip;
            audio.loop = isLoop;
            audio.Play();
        }
    }

    AudioClip GetAudioClip(string name)
    {
        switch (name)
        {
            case "Bgm":
                return Bgm;
            case "seWalk":
                return seWalk;
            case "seRun":
                return seRun;
            case "seWater":
                return seWater;
        }
        return null;
    }
}
