using UnityEngine;
using UnityEngine.SceneManagement;

namespace Movement
{
    public class switchsence2 : MonoBehaviour
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
                LoadSceneGroup(sceneLoader, 4);
            }
        }
        static async void LoadSceneGroup(SceneLoader sceneLoader, int index)
        {
            await sceneLoader.LoadSceneGroup(index);
        }
    }
}

