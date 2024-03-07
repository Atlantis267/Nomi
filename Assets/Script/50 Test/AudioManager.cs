using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip Bgm;
    public AudioClip seWalk;
    public AudioClip seRun;
    public AudioClip seWind;
    public AudioClip seWater;

    List<AudioSource> audios = new List<AudioSource>();
    private void Awake()
    {
        for (int i = 0; i < 5; i++)
        {
            var audio = this.gameObject.AddComponent<AudioSource>();
            audio.volume = 0.2f;
            audios.Add(audio);
        }
    }


    public void Play(int index,string name,bool isLoop)
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
            case "seWind":
                return seWind;
            case "seWater":
                return seWater;
        }
        return null;
    }
}
