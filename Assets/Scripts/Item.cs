using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Button,
    Destroyed
}

public class Item : MonoBehaviour
{
    public ItemType itemType = ItemType.Destroyed;
    public bool isInvolved = false;

    public void Interaction()
    {
        switch (itemType)
        {
            case ItemType.Button:
                isInvolved = !isInvolved;
                GetComponentInParent<Animator>().SetBool("Active", isInvolved);
                break;

            case ItemType.Destroyed:
                Destroy(gameObject);
                break;
        }
    }
}
