using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Eflatun.SceneReference;

namespace Movement
{
    public class ChangeScene : MonoBehaviour , IDataPersistence
    {
        private Player player;
        [SerializeField] SceneLoader sceneLoader;
        private void Awake()
        {
            sceneLoader = GameObject.FindObjectOfType<SceneLoader>();
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        public void LoadData(GameData data)
        {
            player.transform.position = data.playerPosition;           
        //    data.pointchecked.TryGetValue(id, out ischecked);
        //    if (ischecked)
        //    {
        //        gameObject.SetActive(false);
        //    }
        }

        public void SaveData(GameData data)
        {
            data.playerPosition = player.transform.position;
            //    if (data.pointchecked.ContainsKey(id))
            //    {
            //        data.pointchecked.Remove(id);
            //    }
            //    data.pointchecked.Add(id, ischecked);
        }
        private void Update()
        {
            if (Input.GetKeyDown("p"))
            {
                DataPersistenceManager.instance.SaveGame();
                LoadSceneGroup(sceneLoader, 1);
            }
            if (Input.GetKeyDown("b"))
            {
                LoadSceneGroup(sceneLoader, 0);
            }
        }
        static async void LoadSceneGroup(SceneLoader sceneLoader, int index)
        {
            await sceneLoader.LoadSceneGroup(index);
        }
        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.CompareTag("Player"))
        //    {
        //        StartCoroutine(ProgressLoadingBar());
        //        //LoadScene();
        //    }
        //}
        //private void LoadScene()
        //{
        //    for(int i = 0; i < _levelSceneToload.Length; i++)
        //    {
        //        bool isSceneLoading = false;
        //        for(int j = 0; j< SceneManager.sceneCount; j++)
        //        {
        //            Scene loadedScene = SceneManager.GetSceneAt(j);
        //            if(loadedScene.name == _levelSceneToload[i].SceneName)
        //            {
        //                isSceneLoading = true;
        //                break;
        //            }
        //        }
        //        if (!isSceneLoading)
        //        {
        //            SceneManager.LoadSceneAsync(_levelSceneToload[i], LoadSceneMode.Additive);
        //        }
        //    }
        //}
    }
}
