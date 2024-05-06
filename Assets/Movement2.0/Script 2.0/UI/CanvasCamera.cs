using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class CanvasCamera : MonoBehaviour
    {
        private Transform localtransform;
        void Start()
        {
            localtransform = GetComponent<Transform>();
            Canvas canvas = GetComponent<Canvas>();
            if (canvas != null)
            {
                canvas.worldCamera = Camera.main;
            }
        }
        private void Update()
        {
            if (Camera.main)
            {
                localtransform.LookAt(2 * localtransform.position - Camera.main.transform.position);
            }
        }
    }
}

