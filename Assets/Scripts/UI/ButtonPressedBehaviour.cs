using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressedBehaviour : StateMachineBehaviour
{
    public static Dictionary<string, UnityEngine.Events.UnityAction> ButtonFunctionTable;

    private void Awake()
    {
        ButtonFunctionTable = new Dictionary<string, UnityEngine.Events.UnityAction>();
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UIInput.Instance.DisableAllUIInputs();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ButtonFunctionTable[animator.gameObject.name].Invoke();
    }
}
