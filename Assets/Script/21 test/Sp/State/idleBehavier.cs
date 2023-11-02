using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idleBehavier : StateMachineBehaviour
{
    //idle animation
    [SerializeField]
    private float timeBored;

    [SerializeField]
    private int numberOfBoredAnimations;
    [SerializeField]
    private bool isBored;
    private float idleTime;
    private int boredAnimation;
    int idleHash;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        idleHash = Animator.StringToHash("idlestate");
        ResetIdle();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isBored == false)
        {
            idleTime += Time.deltaTime;
            if (idleTime > timeBored && stateInfo.normalizedTime % 1 < 0.02f)
            {
                isBored = true;
                boredAnimation = Random.Range(1, numberOfBoredAnimations + 1);
                boredAnimation = boredAnimation * 2 - 1;
                animator.SetFloat(idleHash, boredAnimation - 1);
            }     
        }
        else if (stateInfo.normalizedTime % 1 > 0.98)
        {
            ResetIdle();
        }
        animator.SetFloat("idlestate", boredAnimation, 0.1f, Time.deltaTime);
    }

    private void ResetIdle()
    {
        if (isBored)
        {
            boredAnimation--;
        }

        isBored = false;
        idleTime = 0;
    }
}
