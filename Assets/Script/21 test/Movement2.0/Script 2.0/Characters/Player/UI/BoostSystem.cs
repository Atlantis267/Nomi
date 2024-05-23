using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Movement
{
    public class BoostSystem : MonoBehaviour
    {
        private bool hasAvailableBoost;
        [Header("Boost Values")]
        public float boostAmount;
        [Header("Parameters")]
        [SerializeField] float boostDrainSpeed = .1f;
        [SerializeField] float boostGainAmount = .2f;

        [Header("FadeAnimation")]
        [SerializeField] Animator boostfade;

        private Slider boostSlider;

        private Player player;


        private void Awake()
        {
            boostSlider = GetComponent<Player>().UIData.boostSlider;    
            player = GetComponent<Player>();
        }

        void Start()
        {
            boostSlider.value = boostAmount;
        }
        void Update()
        {
            boostSlider.value = Mathf.Lerp(boostSlider.value, boostAmount, .2f);
            if (player.movementStateMachine.ReusableData.IsAirDashing || player.movementStateMachine.ReusableData.IsDashing || player.movementStateMachine.ReusableData.IsSprinting)
            {
                boostAmount -= boostDrainSpeed * Time.deltaTime;
                boostAmount = Mathf.Clamp(boostAmount, 0, 1);
            }
            if (boostAmount <= 0)
            {
                boostfade.SetBool("BoostFadeOut", true);
                hasAvailableBoost = false;
                player.movementStateMachine.ReusableData.ShouldAirDash = false;
                player.movementStateMachine.ReusableData.ShouldDash = false;
                player.movementStateMachine.ReusableData.KeepSprint = false;
            }
            else if (boostAmount <= 0 && hasAvailableBoost)
            {
                hasAvailableBoost = false;
            }
            else
            {
                boostfade.SetBool("BoostFadeOut", false);
                hasAvailableBoost = true;
                player.movementStateMachine.ReusableData.ShouldDash = true;
            }
        }
        public IEnumerator AddToBoost()
        {
            boostfade.SetBool("BoostFadeIn", true);
            yield return null;
            boostAmount += boostGainAmount * Time.deltaTime;
            boostAmount = Mathf.Clamp(boostAmount, 0, 1);
            yield return new WaitForSeconds(1f);
            boostfade.SetBool("BoostFadeIn", false);
        }
    }

}
