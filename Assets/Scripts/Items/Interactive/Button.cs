using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : Item
{
    //public Animator controlledObjectAnimator;
    public UnityEvent onClick;
    private Animator thisAnimator;
    private bool state = false;
    

    private void Start()
    {
        thisAnimator = GetComponent<Animator>();
        onClick.AddListener(ChangeButtonState);
    }

    public override bool Interaction()
    {
        onClick.Invoke();
        return false;
    }

    private void ChangeButtonState()
    {
        state = !state;
        thisAnimator.SetBool("Active", state);
    }
}
