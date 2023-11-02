using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveBehavier : StateMachineBehaviour
{
    PlayerMovement _player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _player = animator.GetComponent<PlayerMovement>();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if  (_player.IsSprintPressed && _player.IsMovePressed)
        {
            animator.SetFloat(_player.MoveSpeedHash, _player.CurrentMovement.magnitude * _player.SprintSpeed, 0.1f, Time.deltaTime);
        }
        else if (_player.IsMovePressed)
        {
            animator.SetFloat(_player.MoveSpeedHash, _player.CurrentMovement.magnitude * _player.Speed, 0.1f, Time.deltaTime);
            
        }
        else if (!_player.IsMovePressed)
        {
            animator.SetBool(_player.MoveboolHash, false);
            animator.SetFloat(_player.MoveSpeedHash, _player.CurrentMovement.magnitude * _player.Speed, 0.1f, Time.deltaTime);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    if (_player.IsJumpPressed)
    //    {
    //        _player.VerticalVelocity = _player.InitialJumpVelocity * _player.Jumpspeed;
    //    }
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
      
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
