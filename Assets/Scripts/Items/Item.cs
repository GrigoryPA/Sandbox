using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    //������� ������ � �������������, ��� ������������������
    public bool isUsing = false;

    //�������������� � ���������
    //���������� true - ������� ��� ��������
    //���������� false - ������� ������ ���������
    public abstract bool Interaction(); 
}
