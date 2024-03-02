using UnityEngine;
using System;

namespace Movement
{
    public class Arrow : MonoBehaviour
    {
        public float speed;
        public GameObject arrow;
        private Rigidbody rb;
        
        float mouseX  = 0f;
        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                mouseX += Input.GetAxisRaw("Mouse X") * speed;
                transform.rotation = Quaternion.Euler(0f, mouseX, 0f);
            }
            else
            {
                arrow.SetActive(false);
            }
        }    
    }
}
