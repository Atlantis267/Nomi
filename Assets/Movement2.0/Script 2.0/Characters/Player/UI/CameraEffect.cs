using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class CameraEffect : MonoBehaviour
    {
        [SerializeField] Cyan.Blit blit;
        [SerializeField] float lerptime = 1f;
        public Player player;
        Material materialclone;
        Material orignMaterial;
        private void Awake()
        {
            player = FindObjectOfType<Player>();

            if (blit == null)
                return;

            orignMaterial = blit.settings.blitMaterial;
            materialclone = new Material(blit.settings.blitMaterial);
            blit.settings.blitMaterial = materialclone;
        }

        void Update()
        {
            if (blit == null)
                return;

            float alphaValue = player.movementStateMachine.ReusableData.IsSprinting ? 0.02f : 0;

            blit.settings.blitMaterial.SetFloat("_Alpha", Mathf.Lerp(blit.settings.blitMaterial.GetFloat("_Alpha"), alphaValue, lerptime * Time.deltaTime));
        }

        private void OnDestroy()
        {
            if (blit == null)
                return;

            blit.settings.blitMaterial = orignMaterial;
        }
    }
}
