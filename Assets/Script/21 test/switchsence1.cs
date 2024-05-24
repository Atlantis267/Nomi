using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;

namespace Movement
{
    public class switchsence1 : MonoBehaviour
    {
        [SerializeField] SceneLoader sceneLoader;
        private void Awake()
        {
            sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LoadSceneGroup(sceneLoader, 3);
            }
        }
        static async void LoadSceneGroup(SceneLoader sceneLoader, int index)
        {
            await sceneLoader.LoadSceneGroup(index);
        }
    }
}

