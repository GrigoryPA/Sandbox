using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : Item
{
    public Animator controlledObjectAnimator;
    public new Rigidbody rigidbody;
    private Animator thisAnimator;
    private bool state = false;
    

    private void Start()
    {
        thisAnimator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public override bool Interaction()
    {
        state = !state;
        thisAnimator.SetBool("Active", state);
        controlledObjectAnimator.SetBool("Active", state);
        return false;
    }
}
