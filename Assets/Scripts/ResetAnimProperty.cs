using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimProperty : StateMachineBehaviour
{
    [System.Serializable]
    public struct BoolStatus
    {
        public string targetBool;
        public bool status;
    }
    [System.Serializable]
    public struct IntStatus
    {
        public string targetInt;
        public int status;
    }

    public BoolStatus[] boolStatuses;
    public IntStatus[] intStatuses;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        foreach (BoolStatus b in boolStatuses)
        {
            animator.SetBool(b.targetBool, b.status);
        }
        foreach (IntStatus i in intStatuses)
        {
            animator.SetInteger(i.targetInt, i.status);
        }
        PlayerActionHandler.isInvul = false;
    }

}