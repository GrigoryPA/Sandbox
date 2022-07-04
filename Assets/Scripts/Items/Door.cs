using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private bool state = false;
    private Animator thisAnimator;
    public void Start()
    {
        thisAnimator = GetComponent<Animator>();
    }
    public void ChangeDoorState()
    {
        state = !state;
        thisAnimator.SetBool("Active", state);
    }
}
