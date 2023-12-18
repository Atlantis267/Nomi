using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class CapsuleColliderData
    {
        public CharacterController characterController { get; private set; }
        public Vector3 ColliderCenterInLocalSpace { get; private set; }
        public Vector3 ColliderVerticalEvtents { get; private set; }
        public void Initialize(GameObject gameObject)
        {
            if (characterController != null)
            {
                return;
            }

            characterController = gameObject.GetComponent<CharacterController>();

            UpdateColliderData();
        }
        public void UpdateColliderData()
        {
            ColliderCenterInLocalSpace = characterController.center;

            ColliderVerticalEvtents = new Vector3(0.0f, characterController.bounds.extents.y, 0.0f);
        }
    }
}


