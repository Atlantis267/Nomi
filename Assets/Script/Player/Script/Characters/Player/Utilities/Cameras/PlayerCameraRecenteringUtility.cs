using System.Collections;
using System;
using UnityEngine;
using Cinemachine;
namespace Nomimovment
{
    [Serializable]
    public class PlayerCameraRecenteringUtility
    {
        [field: SerializeField] public CinemachineFreeLook FreeLookCamera { get; private set; }
        [field: SerializeField] public float DefaultHorizontalWaitTime { get; private set; } = 0f;
        [field: SerializeField] public float DefaultHorizontalRecenteringTime { get; private set; } = 4f;

        public void EnableRecentering(float waitTime = -1f, float recenteringTime = -1f, float baseMovementSpeed = 1f, float movementSpeed = 1f)
        {
            FreeLookCamera.m_RecenterToTargetHeading.m_enabled = true;
            FreeLookCamera.m_RecenterToTargetHeading.CancelRecentering();

            if (waitTime == -1f)
            {
                waitTime = DefaultHorizontalWaitTime;
            }
            if (recenteringTime == -1f)
            {
                recenteringTime = DefaultHorizontalRecenteringTime;
            }

            recenteringTime = recenteringTime * baseMovementSpeed / movementSpeed;
            FreeLookCamera.m_RecenterToTargetHeading.m_WaitTime = waitTime;
            FreeLookCamera.m_RecenterToTargetHeading.m_RecenteringTime = recenteringTime;
        }
        public void DisableRecentering()
        {
            FreeLookCamera.m_RecenterToTargetHeading.m_enabled = false;
        }
    }

}
