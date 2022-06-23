using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Item
{
    public Animator controlledObjectAnimator;
    private Animator thisAnimator;
    private bool state = false;
    

    private void Start()
    {
        thisAnimator = GetComponent<Animator>();
    }

    public override bool Interaction()
    {
        state = !state;
        thisAnimator.SetBool("Active", state);
        controlledObjectAnimator.SetBool("Active", state);
        return false;
    }
}
