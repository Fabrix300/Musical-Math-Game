using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnswerHUDTransitions : MonoBehaviour
{
    public Animator transition;

    public void ActivateAnimatorAndStartAnimation() 
    {
        transition.enabled = true;  
    }    

    public void TriggerStartAnimation()
    {
        transition.SetInteger("state", 1);
    }

    public void TriggerEndAnimation()
    {
        transition.SetInteger("state", 2);
    }
}
