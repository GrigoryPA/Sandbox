using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Button,
    Gun,
    Autogun,
    Lasergun,
    Destroyed
}

public class Item : MonoBehaviour
{
    public ItemType itemType = ItemType.Destroyed;
    public Animator controlObjectAnimator;
    private bool isInvolved = false;

    public void Interaction()
    {
        switch (itemType)
        {
            case ItemType.Button:
                isInvolved = !isInvolved;
                GetComponent<Animator>().SetBool("Active", isInvolved);
                controlObjectAnimator.SetBool("Active", isInvolved);
                break;

            case ItemType.Lasergun:
                isInvolved = !isInvolved;
                GetComponent<Animator>().SetBool("Active", isInvolved);
                controlObjectAnimator.SetBool("Active", isInvolved);
                break;

            case ItemType.Destroyed:
                Destroy(gameObject);
                break;
        }
    }
}
