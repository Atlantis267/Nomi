using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class GameMaster : MonoBehaviour
    {
        private static GameMaster instance;
        public Vector3 lastCheckPointPos;
        public Vector3 lastCheckPointRotate;

        private void Awake()
        {
            if(instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
