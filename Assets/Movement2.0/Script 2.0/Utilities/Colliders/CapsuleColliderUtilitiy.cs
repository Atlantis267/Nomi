using System;
using UnityEngine;

namespace Movement
{
    [Serializable]
    public class CapsuleColliderUtilitiy : MonoBehaviour
    {
        public CapsuleColliderData CapsuleColliderData { get; private set; }
        [field: SerializeField] public DefaultColliderData DefaultColliderData { get; private set; }
        [field: SerializeField] public SlopeData SlopeData { get; private set; }

        private void Awake()
        {
            Resize();
        }

        private void OnValidate()
        {
            Resize();
        }
        public void Resize()
        {
            Initialize(gameObject);

            CalculateCapsuleColliderDimensions();
        }
        public void Initialize(GameObject gameObject)
        {
            if (CapsuleColliderData != null)
            {
                return;
            }

            CapsuleColliderData = new CapsuleColliderData();

            CapsuleColliderData.Initialize(gameObject);

        }
        public void CalculateCapsuleColliderDimensions()
        {
            SetCapsuleColliderRadius(DefaultColliderData.Radius);
            SetCapsuleColliderHeight(DefaultColliderData.Height * (1f - SlopeData.StepHeightPercentage));

            RecalculateCapsuleColliderCenter();

            RecalculateColliderRadius();
        }
        public void SetCapsuleColliderRadius(float radius)
        {
            CapsuleColliderData.characterController.radius = radius;
        }
        public void SetCapsuleColliderHeight(float height)
        {
            CapsuleColliderData.characterController.height = height;
        }
        public void RecalculateCapsuleColliderCenter()
        {
            float colliderHeightDifference = DefaultColliderData.Height - CapsuleColliderData.characterController.height;

            Vector3 newColliderCenter = new Vector3(0f, DefaultColliderData.CenterY + (colliderHeightDifference / 2f), 0f);

            CapsuleColliderData.characterController.center = newColliderCenter;
        }
        public void RecalculateColliderRadius()
        {
            float halfColliderHeight = CapsuleColliderData.characterController.height / 2f;

            if (halfColliderHeight >= CapsuleColliderData.characterController.radius)
            {
                return;
            }

            SetCapsuleColliderRadius(halfColliderHeight);
        }

    }

}

