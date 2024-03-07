using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_manager : MonoBehaviour
{
    static public Game_manager instance;
    public GameObject audioManagerPrefab;

    public AudioManager audioManager;
    void Start()
    {
        if(!instance)
        {
            instance = this;
            audioManager=Instantiate(audioManagerPrefab).GetComponent<AudioManager>();

            DontDestroyOnLoad(audioManager);
        }
        else
        {

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
