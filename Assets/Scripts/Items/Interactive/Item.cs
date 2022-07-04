using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    //������� ������ � �������������, ��� ������������������
    protected bool isUsing = false;

    public virtual Transform UsePlaceTransform { get; set; }

    //�������������� � ���������
    //���������� true - ������� ��� ��������
    //���������� false - ������� ������ ���������
    public abstract bool Interaction();
}
