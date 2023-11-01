using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostSystem : MonoBehaviour
{
    private bool hasAvailableBoost;
    [Header("Boost Values")]
    public float boostAmount;
    [Header("Parameters")]
    [SerializeField] float boostDrainSpeed = .1f;
    [SerializeField] float boostGainAmount = .3f;


    [Header("UI")]
    [SerializeField] Slider boostSlider;

    private Player player;
    void Start()
    {
        player = GetComponent<Player>();
        boostSlider.value = boostAmount;
    }
    void Update()
    {
        boostSlider.value = Mathf.Lerp(boostSlider.value, boostAmount, .2f);
        if (player.movementStateMachine.ReusableData.IsAirDashing || player.movementStateMachine.ReusableData.IsDashing || player.movementStateMachine.ReusableData.IsSprinting)
        {
            boostAmount -= boostDrainSpeed * Time.deltaTime;
        }    
        if (boostAmount <= 0.005f)
        {
            hasAvailableBoost = false;
            player.movementStateMachine.ReusableData.ShouldAirDash = false;
            player.movementStateMachine.ReusableData.ShouldDash = false;
            player.movementStateMachine.ReusableData.KeepSprint = false;
        }
        else if(boostAmount <= 0 && hasAvailableBoost)
        {
            hasAvailableBoost = false;
        }
        else
        {
            hasAvailableBoost = true;
            player.movementStateMachine.ReusableData.ShouldDash = true;
        }
    }
    public void AddToBoost()
    {
        float total = boostAmount + boostGainAmount;
        boostAmount = Mathf.Clamp(boostAmount + boostGainAmount, 0, 1);
    }
}
