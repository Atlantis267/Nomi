using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Movement
{
    public class ScreenFade : MonoBehaviour
    {
        public Animator animator;
        public Player player;

        private void Update()
        {
            if (player.movementStateMachine.ReusableData.IsRespawning)
            {
                StartCoroutine(Fade());
            }
        }
        public IEnumerator Fade()
        {
            animator.SetBool("FadeOut", true);
            animator.SetBool("FadeIn", false);
            yield return new WaitForSeconds(1);
            animator.SetBool("FadeIn", true);
            animator.SetBool("FadeOut", false);
            player.movementStateMachine.ReusableData.IsRespawning = false;

        }
    }
}
