using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class Pratical : MonoBehaviour
    {
        public ParticleSystem Particale1;
        public ParticleSystem Particale2;
        public ParticleSystem Particale3;
        public ParticleSystem Particale4;
        public ParticleSystem Particale5;
        public ParticleSystem Particale6;
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("µ¹§Ú©ñ");
            ParticleSystem.ShapeModule shape1 = Particale1.shape;
            shape1.radius = 1.1f;
            ParticleSystem.ShapeModule shape2 = Particale2.shape;
            shape2.radius = 1.1f;
            ParticleSystem.ShapeModule shape3 = Particale3.shape;
            shape3.radius = 1.1f;
            ParticleSystem.ShapeModule shape4 = Particale4.shape;
            shape4.radius = 1.1f;
            ParticleSystem.ShapeModule shape5 = Particale5.shape;
            shape5.radius = 1.1f;
            ParticleSystem.ShapeModule shape6 = Particale6.shape;
            shape6.radius = 1.1f;
        }
    }
}
