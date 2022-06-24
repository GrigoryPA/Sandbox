using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    //предмет принят к использованию, был прозаимодействован
    protected bool isUsing = false;

    public virtual Transform UsePlaceTransform { get; set; }

    //взаимодействие с предметом
    //возвращает true - предмет был подобран
    //возвращает false - предмет нельзя подобрать
    public abstract bool Interaction();
}
