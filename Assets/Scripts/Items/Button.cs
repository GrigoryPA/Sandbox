using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Item
{
    public Animator controlledObjectAnimator;
    private Animator thisAnimator;
    

    private void Start()
    {
        thisAnimator = GetComponent<Animator>();
    }

    public override void Interaction()
    {
        isUsed = !isUsed;
        thisAnimator.SetBool("Active", isUsed);
        controlledObjectAnimator.SetBool("Active", isUsed);
    }
}
